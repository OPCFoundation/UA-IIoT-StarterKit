using MQTTnet;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using Opc.Ua;
using System.Reflection;
using System.Security.Authentication;
using System.Text;
using UaPubSubCommon;

namespace UaSubscriber
{
    public class Subscriber
    {
        private Configuration m_configuration;
        private MqttClientFactory m_factory;
        private IMqttClient m_client;
        private ServiceMessageContext m_messageContext;
        private bool m_useResponder;
        private string m_targetPublisherId;

        private Dictionary<string, IRemotePublisher> m_topics = new();
        private Dictionary<string, RemotePublisher> m_publishers = new();
        private Dictionary<string, RemoteResponder> m_responders = new();
        private int m_connectionRecoveryTime = 10000;

        internal string BrokerHost => m_configuration?.BrokerHost;
        internal int BrokerPort => m_configuration?.BrokerPort ?? 1883;
        internal string UserName => m_configuration?.UserName;
        internal string Password => m_configuration?.Password;
        internal string TopicPrefix => m_configuration?.TopicPrefix;
        internal string PublisherId => m_configuration?.PublisherId;
        internal bool UseNewEncodings => m_configuration?.UseNewEncodings ?? true;

        public Subscriber(
            Configuration configuration,
            bool useResponder,
            string targetPublisherId)
        {
            m_configuration = configuration;
            m_messageContext = new ServiceMessageContext();
            m_messageContext.Factory.AddEncodeableTypes(Assembly.GetExecutingAssembly());
            m_useResponder = useResponder;
            m_targetPublisherId = targetPublisherId;
        }

        public async Task Connect(CancellationToken ct)
        {
            m_factory = new MqttClientFactory();

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    await Run(ct);
                    break;
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception e)
                {
                    Log.Error($"Publishing Error: [{e.GetType().Name}] {e.Message}");
                }

                Log.System($"Reconnecting in 10s...");

                try
                {
                    await Task.Delay(m_connectionRecoveryTime, ct);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        private async Task Run(CancellationToken ct)
        {
            using (m_client = m_factory.CreateMqttClient())
            {
                var options = new MqttClientOptionsBuilder()
                    .WithProtocolVersion(MqttProtocolVersion.V500)
                    .WithTcpServer(BrokerHost, BrokerPort)
                    .WithCredentials(UserName, Password)
                    .WithClientId($"{TopicPrefix}.{PublisherId}.{DateTime.Now.Ticks}")
                    .WithCleanStart()
                    .WithTimeout(TimeSpan.FromSeconds(120))
                    .WithKeepAlivePeriod(TimeSpan.FromSeconds(120))
                    .WithTlsOptions(
                        o =>
                        {
                            o.UseTls(m_configuration.UseTls);

                            o.WithCertificateValidationHandler(e =>
                            {
                                Log.Info($"Broker Certificate: '{e.Certificate.Subject}' {e.SslPolicyErrors}");
                                return true;
                            });

                            // The default value is determined by the OS. Set manually to force version.
                            o.WithSslProtocols(SslProtocols.Tls12);
                        })
                    .Build();

                var response = await m_client.ConnectAsync(options, ct);

                if (response.ResultCode != MqttClientConnectResultCode.Success)
                {
                    Log.Error($"Connect Failed: {response.ResultCode} {response.ResultCode} {response.ReasonString}");
                }
                else
                {
                    Log.System("Subscriber Connected!");

                    try
                    {
                        m_client.ApplicationMessageReceivedAsync += async delegate (MqttApplicationMessageReceivedEventArgs args)
                        {
                            try
                            {
                                Log.Info($"{args.ApplicationMessage.Topic}: {args.ApplicationMessage.Payload.Length} bytes");

                                if (m_topics.TryGetValue(args.ApplicationMessage.Topic, out var publisher) && publisher != null)
                                {
                                    var json = await PubSubUtils.ParseMessage(args.ApplicationMessage, ct);

                                    if (String.IsNullOrEmpty(json))
                                    {
                                        Log.Warning($"Ignoring empty message set to {args.ApplicationMessage.Topic}.");
                                        return;
                                    }

                                    if (await publisher.ProcessMessage(args.ApplicationMessage.Topic, json, ct))
                                    {
                                        var topics = publisher.GetTopics();

                                        foreach (var ii in topics)
                                        {
                                            if (ii.Value) await Subscribe(ii.Key, publisher, ct);
                                            else await Unsubscribe(ii.Key, ct);
                                        }
                                    }

                                    return;
                                }

                                string prefix = $"{TopicPrefix}/json/";

                                if (args.ApplicationMessage.Topic.StartsWith(prefix))
                                {
                                    var topic = Topic.Parse(args.ApplicationMessage.Topic, TopicPrefix);

                                    if (topic.MessageType == TopicTypes.Status)
                                    {
                                        await ProcessStatus(args.ApplicationMessage, ct);
                                        return;
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Log.Error($"Parsing Error [{e.GetType().Name}] {e.Message}");
                            }
                        };

                        var topic = $"{TopicPrefix}/json/{TopicTypes.Status}/";
                        RemotePublisher publisher = null;

                        if (!String.IsNullOrEmpty(m_targetPublisherId))
                        {
                            topic += m_targetPublisherId;

                            publisher = m_publishers[m_targetPublisherId] = new RemotePublisher(m_messageContext, TopicPrefix)
                            {
                                PublisherId = m_targetPublisherId
                            };

                            await Subscribe($"{TopicPrefix}/json/{TopicTypes.PubSubConnection}/{m_targetPublisherId}", publisher, ct);
                        }
                        else
                        {
                            topic += "#";
                        }

                        await Subscribe($"{topic}", null, ct);

                        try
                        {
                            Log.System("Press any key to enter the action menu.");

                            while (true)
                            {
                                if (Console.KeyAvailable && m_responders.Count > 0)
                                {
                                    while (Console.KeyAvailable) Console.ReadKey(true);
                                    await SendRequests(ct);
                                }

                                await Task.Delay(100, ct);
                            }
                        }
                        catch (TaskCanceledException)
                        {
                            Log.System($"Subscriber Stopping...");
                        }

                        Log.LogLevel = LogLevel.Debug;
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Subscriber Error: [{e.GetType().Name}] {e.Message}");
                    }
                }

                // suppress cancellation request to ensure proper cleanup.
                if (ct.IsCancellationRequested)
                {
                    ct = CancellationToken.None;
                }

                var disconnectOptions = m_factory.CreateClientDisconnectOptionsBuilder().Build();
                await m_client.DisconnectAsync(disconnectOptions, ct);
                Log.System("Subscriber Disconnected!");
            }
        }

        private async Task SendRequests(CancellationToken ct)
        {
            try
            {
                Log.EnableBuffering = true;

                while (true)
                {
                    int count = 1;
                    var responder = m_responders.Values.FirstOrDefault();

                    if (m_responders.Count > 1)
                    {
                        Log.Prompt("Please choose a responder:");

                        var list = m_responders.Values.ToList();

                        foreach (var ii in list)
                        {
                            Log.Prompt($"   {count++}) {ii.PublisherId}");
                        }

                        Log.Prompt("Press 'X' to cancel.");
                        responder = await PubSubUtils.ReadChoice(list, ct);

                        if (responder == null)
                        {
                            return;
                        }
                    }

                    Log.Prompt("Please choose a target:");

                    count = 1;
                    List<dynamic> targets = new();

                    foreach (var ii in m_responders.Values.First().Writers)
                    {
                        if (ii.Value?.ActionTargets != null)
                        {
                            foreach (var jj in ii.Value.ActionTargets)
                            {
                                Log.Prompt($"   {count++}) {ii.Value.GroupName}.{ii.Value.Name}.{jj.Name}");
                                targets.Add(new { Target = jj, Writer = ii.Value });
                            }
                        }
                    }

                    Log.Prompt("Press 'X' to cancel.");
                    var choice = await PubSubUtils.ReadChoice(targets, ct);

                    if (choice == null)
                    {
                        return;
                    }

                    ActionWriter writer = choice.Writer;
                    ActionTargetDataType target = choice.Target;

                    Log.Prompt("Please enter arguments:");
                    List<Variant> inputArguments = new();

                    foreach (var ii in writer.Request.Fields)
                    {
                        var sb = new StringBuilder();
                        sb.Append($"  {ii.Name} [{(BuiltInType)ii.BuiltInType}]");
                        sb.Append($"{((LocalizedText.IsNullOrEmpty(ii.Description.Text)) ? "" : ":")}");
                        sb.Append($" {ii.Description}");
                        Log.Prompt(sb.ToString());

                        var value = await PubSubUtils.ReadValue(ii, ct);
                        inputArguments.Add(value);
                    }

                    Log.Prompt("Sending action request.");
                    var message = await responder.BuildActionRequest(responder, writer, target, inputArguments, ct);
                    await PublishActionRequest(responder, writer.DataTopic, message, ct);
                    break;
                }
            }
            finally
            {
                Log.EnableBuffering = false;
            }
        }

        private async Task PublishActionRequest(RemoteResponder responder, string topic, string message, CancellationToken ct)
        {
            if (m_client == null || m_factory == null) throw new InvalidOperationException();

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .WithRetainFlag(true)
                .WithContentType("application/json")
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .WithResponseTopic(responder.ResponseTopic)
                .WithCorrelationData(responder.CorrelationData)
                .Build();

            var result = await m_client.PublishAsync(applicationMessage, ct);

            if (!result.IsSuccess)
            {
                Log.Error($"Error: {result.ReasonCode} {result.ReasonString}");
            }
            else
            {
                Log.Info($"Data sent to '{topic}'.");
            }
        }

        private async Task ProcessStatus(MqttApplicationMessage message, CancellationToken ct)
        {
            var json = await PubSubUtils.ParseMessage(message, ct);

            JsonStatusMessage status = new();

            using (var decoder = new JsonDecoder(json, m_messageContext))
            {
                status.Decode(decoder);
            }

            if (String.IsNullOrEmpty(status.PublisherId))
            {
                Log.Error($"Status Message with no PublisherId: '{message.Topic}'.");
                return;
            }

            if (!m_publishers.TryGetValue(status.PublisherId, out var publisher))
            {
                // ignoring publishers that have never been operational.
                if (status.Status != PubSubState.Operational)
                {
                    return;
                }

                m_publishers[status.PublisherId] = publisher = new RemotePublisher(m_messageContext, TopicPrefix)
                {
                    PublisherId = status.PublisherId
                };

                m_responders[status.PublisherId] = new RemoteResponder(m_messageContext, TopicPrefix)
                {
                    PublisherId = status.PublisherId,
                    RequestorId = PublisherId
                };
            }

            publisher.PubSubState = status.Status;
            Log.Info($"Publisher '{publisher.PublisherId}' in state '{publisher.PubSubState}'.");

            // subscribe to all publisher topics.
            if (!m_useResponder)
            {
                await Subscribe($"{TopicPrefix}/json/{TopicTypes.PubSubConnection}/{publisher.PublisherId}", publisher, ct);
                await Subscribe($"{TopicPrefix}/json/{TopicTypes.ApplicationDescription}/{publisher.PublisherId}", publisher, ct);
            }
            else
            {
                if (!m_responders.TryGetValue(status.PublisherId, out var responder))
                {
                    m_responders[status.PublisherId] = responder = new RemoteResponder(m_messageContext, TopicPrefix)
                    {
                        PublisherId = status.PublisherId,
                        RequestorId = PublisherId
                    };
                }

                await Subscribe($"{TopicPrefix}/json/{TopicTypes.ActionResponder}/{publisher.PublisherId}", responder, ct);
            }
        }

        private async Task Subscribe(string topic, IRemotePublisher publisher, CancellationToken ct)
        {
            if (topic == null) throw new ArgumentNullException(nameof(topic));
            if (m_client == null || m_factory == null) throw new InvalidOperationException();

            if (!m_topics.ContainsKey(topic))
            {
                var options = m_factory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(f => { f.WithTopic(topic); })
                    .Build();

                var response = await m_client.SubscribeAsync(options, ct);

                if (!String.IsNullOrEmpty(response?.ReasonString))
                {
                    Log.Error($"Subscribe Failed:'{response?.ReasonString}'.");
                }
                else
                {
                    Log.Info($"Subscribed: '{topic}'.");
                }
            }

            m_topics[topic] = publisher;
        }

        private async Task Unsubscribe(string topic, CancellationToken ct)
        {
            if (topic == null) throw new ArgumentNullException(nameof(topic));
            if (m_client == null || m_factory == null) throw new InvalidOperationException();

            if (m_topics.Remove(topic))
            {
                var options = m_factory.CreateUnsubscribeOptionsBuilder()
                    .WithTopicFilter(topic)
                    .Build();

                var response = await m_client.UnsubscribeAsync(options, ct);

                if (!String.IsNullOrEmpty(response?.ReasonString))
                {
                    Log.Error($"Unsubscribe Failed: '{response?.ReasonString}'.");
                }
                else
                {
                    Log.Info($"Unsubscribed: '{topic}'.");
                }
            }
        }
    }
}
