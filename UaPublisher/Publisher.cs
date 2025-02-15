using MQTTnet;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using Opc.Ua;
using System.Buffers;
using System.Diagnostics;
using System.Reflection;
using System.Security.Authentication;
using UaPubSubCommon;

namespace UaPublisher
{
    public class Publisher
    {
        private Configuration m_configuration;
        private MqttClientFactory m_factory;
        private IMqttClient m_client;
        private ServiceMessageContext m_messageContext;

        private PublisherConnection m_connection;
        private ResponderConnection m_responder;
        private ActionRequestProcessor m_requestProcessor;
        private List<PublisherSource> m_sources = new();

        private bool m_deleteAllTopicsOnStart = false;
        private Dictionary<string, uint> m_sequenceNumbers = new();

        private int m_counterDelayTime = 1000;
        private Dictionary<string, DateTime> m_lastDataUpdateTime = new();
        private Dictionary<string, DateTime> m_lastMetaDataUpdateTime = new();
        private Dictionary<string, DateTime> m_lastKeyFrameTime = new();
        private HashSet<string> m_requestTopics = new();

        private int m_cleanupTopicDelayTime = 10000;
        private int m_connectionRecoveryTime = 10000;

        internal string BrokerHost => m_configuration?.BrokerHost;
        internal int BrokerPort => m_configuration?.BrokerPort ?? 1883;
        internal string UserName => m_configuration?.UserName;
        internal string Password => m_configuration?.Password;
        internal string TopicPrefix => m_configuration?.TopicPrefix;
        internal string PublisherId => m_configuration?.PublisherId;
        internal bool UseNewEncodings => m_configuration?.UseNewEncodings ?? true;

        public Publisher(Configuration configuration)
        {
            m_configuration = configuration;
            m_messageContext = new ServiceMessageContext();
            m_messageContext.Factory.AddEncodeableTypes(Assembly.GetExecutingAssembly());
        }

        private async Task<ArraySegment<byte>> Encode(IEncodeable message, bool compress = false)
        {
            using (var istrm = new MemoryStream())
            {
                using (var encoder = new JsonEncoder(
                    m_messageContext,
                    UseNewEncodings ? JsonEncodingType.Compact : JsonEncodingType.Reversible,
                    stream: istrm,
                    leaveOpen: true))
                {
                    message.Encode(encoder);
                }

                istrm.Position = 0;

                if (compress)
                {
                    return await PubSubUtils.Compress(istrm);
                }

                istrm.Close();
                return istrm.ToArray();
            }
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

        private string GetContentType(bool useCompression = false)
        {
            if (useCompression)
            {
                return "application/json+gzip";
            }

            return "application/json";
        }

        private void BuildResponder()
        {
            var targets = new List<ActionTarget>();

            foreach (var source in m_sources)
            {
                targets.Add(new ActionTarget()
                {
                    Id = (ushort)(5000 + source.Id),
                    Name = source.Name,
                    Callback = ((Boiler)source).Reset
                });
            }

            List<ActionGroup> groups = new(
            [
                new ActionGroup(
                    1000,
                    "Boilers",
                    [
                        new ActionWriter(1001, "Reset", new BoilerResetAction(targets))
                        {
                            Enabled = true
                        }
                    ]
                )
                {
                    Enabled = true,
                    PublishingInterval = 1000, // publish once per second.
                    MetaDataPublishingInterval = 120000, // resend metadata every 2min.
                    Qos = BrokerTransportQualityOfService.AtLeastOnce, // MQTT Qos=1,
                    TopicForData = $"{TopicPrefix}/Site_Riverdale/ProcessCell_South/BoilerReset"
                }
              ]
            );

            m_responder = new(
                "Default",
                m_configuration.TopicPrefix,
                m_configuration.PublisherId,
                groups)
            {
                Enabled = true
            };
        }

        private void BuildConnections()
        {
            // this publisher has 2 sources:
            var boiler1 = new Boiler(1);
            boiler1.LevelController.SetPoint = 150;
            boiler1.LevelController.SetPointUpdateTime = DateTime.UtcNow;

            var boiler2 = new Boiler(2);
            boiler2.LevelController.SetPoint = 100;
            boiler2.LevelController.SetPointUpdateTime = DateTime.UtcNow;

            m_sources = new([boiler1, boiler2]);
            List<PublisherGroup> groups = new(
             [
                // publish with no headers and raw encoding + UNS topic names.
                new PublisherGroup(
                    1000,
                    "Minimal",
                    [
                        new PublisherWriter(0, boiler1.Name, boiler1)
                        {
                            Enabled = true,
                            KeyFrameCount = 30, // every 30th message has all data.
                            FieldMask = DataSetFieldContentMask.RawData,
                            TopicForData = $"{TopicPrefix}/Site_Riverdale/ProcessCell_South/Unit1_Boiler"
                        },
                        new PublisherWriter(0, boiler2.Name, boiler2)
                        {
                            Enabled = true,
                            KeyFrameCount = 30,
                            FieldMask = DataSetFieldContentMask.RawData,
                            TopicForData = $"{TopicPrefix}/Site_Riverdale/ProcessCell_South/Unit2_Boiler"
                        }
                    ]
                )
                {
                    Enabled = true,
                    HeaderLayoutUri = HeaderProfiles.JsonMinimal,
                    PublishingInterval = 1000, // publish once per second.
                    KeepAliveCount = 10, // send keep alive if no data changes after 10s
                    MetaDataPublishingCount = 120, // resend metadata every 2min.
                    Qos = BrokerTransportQualityOfService.AtLeastOnce // MQTT Qos=1
                },
                
                // publish with dataset message header and variant encoding.
                new PublisherGroup(
                    2000,
                    "Single",
                    [
                        new PublisherWriter(0, boiler1.Name, boiler1)
                        {
                            Enabled = true,
                            KeyFrameCount = 30, // every 30th message has all data.
                            FieldMask = DataSetFieldContentMask.None
                        },
                        new PublisherWriter(0, boiler2.Name, boiler2)
                        {
                            Enabled = true,
                            KeyFrameCount = 30,
                            FieldMask = DataSetFieldContentMask.None
                        }
                    ]
                )
                {
                    Enabled = true,
                    HeaderLayoutUri = HeaderProfiles.JsonDataSetMessage,
                    PublishingInterval = 1000, // publish once per second.
                    KeepAliveCount = 10, // send keep alive if no data changes after 10s
                    MetaDataPublishingCount = 120, // resend metadata every 2min.
                    Qos = BrokerTransportQualityOfService.AtLeastOnce // MQTT Qos=1
                },
                
                // publish with network message header and data value encoding.
                new PublisherGroup(
                    3000,
                    "Multiple",
                    [
                        new PublisherWriter(0, boiler1.Name, boiler1)
                        {
                            Enabled = true,
                            KeyFrameCount = 30, // every 30th message has all data.
                            FieldMask = DataSetFieldContentMask.StatusCode | DataSetFieldContentMask.SourceTimestamp
                        },
                        new PublisherWriter(0, boiler2.Name, boiler2)
                        {
                            Enabled = true,
                            KeyFrameCount = 30,
                            FieldMask = DataSetFieldContentMask.StatusCode | DataSetFieldContentMask.SourceTimestamp
                        }
                    ]
                )
                {
                    Enabled = true,
                    HeaderLayoutUri = HeaderProfiles.JsonNetworkMessage,
                    PublishingInterval = 1000, // publish once per second.
                    KeepAliveCount = 10, // send keep alive if no data changes after 10s
                    MetaDataPublishingCount = 120, // resend metadata every 2min.
                    Qos = BrokerTransportQualityOfService.AtLeastOnce // MQTT Qos=1
                }
            ]);

            m_connection = new(
                "Default",
                m_configuration.TopicPrefix,
                m_configuration.PublisherId,
                groups)
            {
                Enabled = true
            };
        }

        private TimeSpan GetMqttKeepAlivePeriod()
        {
            // impose a minimum keepalive of 60s.
            int max = 60000;

            // the MQTT keep alive is set based on enabled WriteGroup KeepAliveTime.
            foreach (var group in m_connection.Groups)
            {
                if (group.Enabled)
                {
                    var timeout = group.KeepAliveCount * group.PublishingInterval;
                    max = Math.Max(max, timeout);
                }
            }

            return TimeSpan.FromMilliseconds(max);
        }

        private async Task Run(CancellationToken ct)
        {
            BuildConnections();
            BuildResponder();

            using (m_client = m_factory.CreateMqttClient())
            {
                m_requestProcessor = new ActionRequestProcessor(m_messageContext, m_responder, m_client);

                var willTopic = new Topic()
                {
                    TopicPrefix = TopicPrefix,
                    MessageType = TopicTypes.Status,
                    PublisherId = PublisherId
                }.Build();

                JsonStatusMessage willMessage = new()
                {
                    MessageType = MessageTypes.Status,
                    MessageId = Guid.NewGuid().ToString(),
                    PublisherId = PublisherId,
                    Status = PubSubState.Error,
                    IsCyclic = false
                };

                var willPayload = await Encode(willMessage);

                var options = new MqttClientOptionsBuilder()
                    .WithProtocolVersion(MqttProtocolVersion.V500)
                    .WithTcpServer(BrokerHost, BrokerPort)
                    .WithCredentials(UserName, Password)
                    .WithWillTopic(willTopic)
                    .WithWillRetain(true)
                    .WithWillDelayInterval(60)
                    .WithWillPayload(willPayload)
                    .WithClientId($"{TopicPrefix}.{PublisherId}")
                    .WithKeepAlivePeriod(GetMqttKeepAlivePeriod())
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
                        }
                    ).Build();

                var response = await m_client.ConnectAsync(options, ct);

                if (response.ResultCode != MqttClientConnectResultCode.Success)
                {
                    Log.Error($"Connect Failed: {response.ResultCode} {response.ResultCode} {response.ReasonString}");
                }
                else
                {
                    Log.System("Publisher Connected!");

                    if (m_deleteAllTopicsOnStart)
                    {
                        await DeleteAllTopics(ct);
                    }

                    try
                    {
                        await Publish(ct);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Publishing Error: [{e.GetType().Name}] {e.Message}");
                    }
                }

                // suppress cancellation request to ensure proper cleanup.
                if (ct.IsCancellationRequested)
                {
                    ct = CancellationToken.None;
                }

                await PublishStatus(PubSubState.Disabled, ct);
                var disconnectOptions = m_factory.CreateClientDisconnectOptionsBuilder().Build();
                await m_client.DisconnectAsync(disconnectOptions, ct);
                Log.System("Publisher Disconnected!");
            }
        }

        private async Task DeleteAllTopics(CancellationToken ct)
        {
            if (m_client == null || m_factory == null) throw new InvalidOperationException();

            try
            {
                m_client.ApplicationMessageReceivedAsync += DeleteTopicOnMessage;

                await Subscribe($"{TopicPrefix}/#", ct);
                await Task.Delay(m_cleanupTopicDelayTime, ct);
                await Unsubscribe($"{TopicPrefix}/#", ct);
            }
            finally
            {
                m_client.ApplicationMessageReceivedAsync -= DeleteTopicOnMessage;
            }
        }

        private async Task DeleteTopicOnMessage(MqttApplicationMessageReceivedEventArgs args)
        {
            string topic = args.ApplicationMessage.Topic;

            if (m_deleteAllTopicsOnStart && args.ApplicationMessage.Retain)
            {
                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload("")
                    .WithRetainFlag(true)
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                    .Build();

                await m_client.PublishAsync(applicationMessage);
            }
        }

        private async Task SubscribeActionRequests(CancellationToken ct)
        {
            m_client.ApplicationMessageReceivedAsync += ProcessRequestMessage;

            foreach (var group in m_responder.Connection.WriterGroups)
            {
                if (!group.Enabled)
                {
                    continue;
                }

                string topic = null;

                if (group.TransportSettings?.Body is BrokerWriterGroupTransportDataType wgts)
                {
                    topic = wgts.QueueName;
                }

                foreach (var writer in group.DataSetWriters)
                {
                    if (!writer.Enabled)
                    {
                        continue;
                    }

                    if (writer.TransportSettings?.Body is BrokerDataSetWriterTransportDataType dsts)
                    {
                        topic = (dsts.QueueName != null) ? dsts.QueueName : topic;
                    }

                    if (!m_requestTopics.Contains(topic))
                    {
                        await Subscribe(topic, ct);
                        m_requestTopics.Add(topic);
                    }
                }
            }
        }

        private async Task ProcessRequestMessage(MqttApplicationMessageReceivedEventArgs args)
        {
            string topic = args.ApplicationMessage.Topic;

            if (m_requestTopics.Contains(topic))
            {
                var json = await PubSubUtils.ParseMessage(args.ApplicationMessage, CancellationToken.None);

                await m_requestProcessor.ProcessRequest(json, CancellationToken.None);
                return;
            }
        }

        private async Task Subscribe(string topic, CancellationToken ct)
        {
            if (topic == null) throw new ArgumentNullException(nameof(topic));
            if (m_client == null || m_factory == null) throw new InvalidOperationException();

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

        private async Task Unsubscribe(string topic, CancellationToken ct)
        {
            if (topic == null) throw new ArgumentNullException(nameof(topic));
            if (m_client == null || m_factory == null) throw new InvalidOperationException();

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

        private async Task PublishStatus(PubSubState state, CancellationToken ct)
        {
            if (m_client == null || m_factory == null) throw new InvalidOperationException();

            var topic = new Topic()
            {
                TopicPrefix = TopicPrefix,
                MessageType = TopicTypes.Status,
                PublisherId = PublisherId
            }.Build();

            JsonStatusMessage payload = new()
            {
                MessageType = MessageTypes.Status,
                MessageId = Guid.NewGuid().ToString(),
                PublisherId = PublisherId,
                Status = state,
                IsCyclic = false
            };

            var message = await Encode(payload);

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .WithRetainFlag(true)
                .WithContentType(GetContentType())
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            var result = await m_client.PublishAsync(applicationMessage, ct);

            if (!result.IsSuccess)
            {
                Log.Error($"Error: {result.ReasonCode} {result.ReasonString}");
            }
            else
            {
                Log.Info($"Status sent to '{topic}'.");
            }
        }

        private async Task PublishApplication(ApplicationDescription application, CancellationToken ct)
        {
            if (m_client == null || m_factory == null) throw new InvalidOperationException();

            var topic = new Topic()
            {
                TopicPrefix = TopicPrefix,
                MessageType = TopicTypes.ApplicationDescription,
                PublisherId = PublisherId
            }.Build();

            JsonApplicationDescriptionMessage payload = new()
            {
                MessageType = MessageTypes.ApplicationDescription,
                MessageId = Guid.NewGuid().ToString(),
                PublisherId = PublisherId,
                Description = application
            };

            var message = await Encode(payload);

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .WithRetainFlag(true)
                .WithContentType(GetContentType())
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            var result = await m_client.PublishAsync(applicationMessage, ct);

            if (!result.IsSuccess)
            {
                Log.Error($"Error: {result.ReasonCode} {result.ReasonString}");
            }
            else
            {
                Log.Info($"ApplicationDescription sent to '{topic}'.");
            }
        }

        private async Task PublishConnection(CancellationToken ct)
        {
            if (m_client == null || m_factory == null) throw new InvalidOperationException();

            var topic = new Topic()
            {
                TopicPrefix = TopicPrefix,
                MessageType = TopicTypes.PubSubConnection,
                PublisherId = PublisherId
            }.Build();

            JsonPubSubConnectionMessage payload = new()
            {
                MessageType = MessageTypes.PubSubConnection,
                MessageId = Guid.NewGuid().ToString(),
                PublisherId = PublisherId,
                Timestamp = DateTime.UtcNow,
                Connection = m_connection.Connection
            };

            var message = await Encode(payload, m_configuration.EnableCompression);

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .WithRetainFlag(true)
                .WithContentType(GetContentType(m_configuration.EnableCompression))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            var result = await m_client.PublishAsync(applicationMessage, ct);

            if (!result.IsSuccess)
            {
                Log.Error($"Error: {result.ReasonCode} {result.ReasonString}");
            }
            else
            {
                Log.Info($"Connection sent to '{topic}'.");
            }
        }

        private async Task PublishResponder(CancellationToken ct)
        {
            if (m_client == null || m_factory == null) throw new InvalidOperationException();

            var topic = new Topic()
            {
                TopicPrefix = TopicPrefix,
                MessageType = TopicTypes.ActionResponder,
                PublisherId = PublisherId
            }.Build();

            JsonActionResponderMessage payload = new()
            {
                MessageType = MessageTypes.ActionResponder,
                MessageId = Guid.NewGuid().ToString(),
                PublisherId = PublisherId,
                Timestamp = DateTime.UtcNow,
                Connection = m_responder.Connection
            };

            var message = await Encode(payload, m_configuration.EnableCompression);

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .WithRetainFlag(true)
                .WithContentType(GetContentType(m_configuration.EnableCompression))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            var result = await m_client.PublishAsync(applicationMessage, ct);

            if (!result.IsSuccess)
            {
                Log.Error($"Error: {result.ReasonCode} {result.ReasonString}");
            }
            else
            {
                Log.Info($"ActionResponder sent to '{topic}'.");
            }
        }

        private async Task PublishMetaData(string topic, JsonDataSetMetaDataMessage metadata, CancellationToken ct)
        {
            if (m_client == null || m_factory == null) throw new InvalidOperationException();

            var message = await Encode(metadata, m_configuration.EnableCompression);

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .WithRetainFlag(true)
                .WithContentType(GetContentType(m_configuration.EnableCompression))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            var result = await m_client.PublishAsync(applicationMessage, ct);

            if (!result.IsSuccess)
            {
                Log.Error($"Error: {result.ReasonCode} {result.ReasonString}");
            }
            else
            {
                Log.Info($"MetaData sent to '{topic}'.");
            }
        }

        private async Task PublishActionMetaData(string topic, JsonActionMetaDataMessage metadata, CancellationToken ct)
        {
            if (m_client == null || m_factory == null) throw new InvalidOperationException();

            var message = await Encode(metadata, m_configuration.EnableCompression);

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .WithRetainFlag(true)
                .WithContentType(GetContentType(m_configuration.EnableCompression))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            var result = await m_client.PublishAsync(applicationMessage, ct);

            if (!result.IsSuccess)
            {
                Log.Error($"Error: {result.ReasonCode} {result.ReasonString}");
            }
            else
            {
                Log.Info($"MetaData sent to '{topic}'.");
            }
        }

        private async Task PublishData(string topic, string message, MqttQualityOfServiceLevel qos, CancellationToken ct)
        {
            if (m_client == null || m_factory == null) throw new InvalidOperationException();
            
            var utf8 = PubSubUtils.UTF8.GetBytes(message, 0, message.Length);
            using var strm = new MemoryStream(utf8);
            var payload = await PubSubUtils.Compress(strm);

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithRetainFlag(true)
                .WithContentType(GetContentType(m_configuration.EnableCompression))
                .WithQualityOfServiceLevel(qos)
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

        private void WriteNetworkMessageHeader(
            JsonEncoder encoder,
            JsonNetworkMessageContentMask nmcm,
            WriterGroupDataType group)
        {
            if ((nmcm & JsonNetworkMessageContentMask.NetworkMessageHeader) != 0)
            {
                encoder.WriteString(nameof(JsonNetworkMessage.MessageId), Guid.NewGuid().ToString());
                encoder.WriteString(nameof(JsonNetworkMessage.MessageType), MessageTypes.DataSetMessage);

                if ((nmcm & JsonNetworkMessageContentMask.PublisherId) != 0)
                {
                    encoder.WriteString(nameof(JsonNetworkMessage.PublisherId), PublisherId);
                }

                if ((nmcm & JsonNetworkMessageContentMask.WriterGroupName) != 0)
                {
                    encoder.WriteString(nameof(JsonNetworkMessage.WriterGroupName), group.Name);
                }
            }
        }

        private async Task Publish(CancellationToken ct)
        {
            if (m_client == null || m_factory == null) throw new InvalidOperationException();

            await SubscribeActionRequests(ct);
            await PublishStatus(PubSubState.Operational, ct);
            await PublishApplication(m_configuration.ApplicationDescription, ct);
            await PublishConnection(ct);
            await PublishMetaData(ct);

            await PublishResponder(ct);
            await PublishActionMetaData(ct);

            var sw = Stopwatch.StartNew();

            while (!ct.IsCancellationRequested)
            {
                foreach (var source in m_sources)
                {
                    await source.Update();
                }

                await PublishData(ct);
                await PublishDataSetMetaData(ct);
                await PublishActionMetaData(ct);

                try
                {
                    sw.Stop();

                    if (sw.ElapsedMilliseconds < m_counterDelayTime - 5)
                    {
                        await Task.Delay(m_counterDelayTime - (int)sw.ElapsedMilliseconds - 5, ct);
                    }

                    sw.Restart();
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }

            Log.System("Exiting publish loop gracefully.");
        }

        private MqttQualityOfServiceLevel GetQos(BrokerTransportQualityOfService groupQos, BrokerTransportQualityOfService writerQos)
        {
            var input = groupQos;

            if (writerQos != BrokerTransportQualityOfService.NotSpecified)
            {
                input = writerQos;
            }

            switch (input)
            {
                case BrokerTransportQualityOfService.ExactlyOnce:
                {
                    return MqttQualityOfServiceLevel.ExactlyOnce;
                }

                case BrokerTransportQualityOfService.AtLeastOnce:
                {
                    return MqttQualityOfServiceLevel.AtLeastOnce;
                }

                default:
                case BrokerTransportQualityOfService.NotSpecified:
                case BrokerTransportQualityOfService.AtMostOnce:
                case BrokerTransportQualityOfService.BestEffort:
                {
                    return MqttQualityOfServiceLevel.AtMostOnce;
                }
            }
        }

        private async Task PublishMetaData(CancellationToken ct)
        {
            foreach (var group in m_connection.Connection.WriterGroups)
            {
                foreach (var writer in group.DataSetWriters)
                {
                    if (writer.DataSetName == null)
                    {
                        continue;
                    }

                    string metadataTopic = null;

                    if (writer.TransportSettings?.Body is BrokerDataSetWriterTransportDataType dsts)
                    {
                        metadataTopic = dsts.MetaDataQueueName;
                    }

                    await PublishMetaData(
                        metadataTopic,
                        new JsonDataSetMetaDataMessage()
                        {
                            MessageId = Guid.NewGuid().ToString(),
                            MessageType = MessageTypes.DataSetMetaData,
                            DataSetWriterId = writer.DataSetWriterId,
                            DataSetWriterName = writer.Name,
                            PublisherId = PublisherId,
                            Timestamp = DateTime.UtcNow,
                            MetaData = GetMetaData(writer.DataSetName)
                        },
                        ct
                    );
                }
            }
        }

        private JsonEncodingType FindEncoding(WriterGroupDataType group)
        {
            var nmcm = JsonNetworkMessageContentMask.None;
            var dscm = JsonDataSetMessageContentMask.None;

            if (group.MessageSettings?.Body is JsonWriterGroupMessageDataType wgms)
            {
                nmcm = (JsonNetworkMessageContentMask)wgms.NetworkMessageContentMask;
            }

            foreach (var writer in group.DataSetWriters)
            {
                if (!writer.Enabled)
                {
                    continue;
                }

                if (writer.MessageSettings?.Body is JsonDataSetWriterMessageDataType dwms)
                {
                    dscm = (JsonDataSetMessageContentMask)dwms.DataSetMessageContentMask;
                    break;
                }
            }

            if ((uint)(dscm & JsonDataSetMessageContentMask.FieldEncoding1) != 0)
            {
                return ((uint)(dscm & JsonDataSetMessageContentMask.FieldEncoding2) != 0) ? JsonEncodingType.Compact : JsonEncodingType.Reversible;
            }
            else
            {
                return ((uint)(dscm & JsonDataSetMessageContentMask.FieldEncoding2) != 0) ? JsonEncodingType.Verbose : JsonEncodingType.NonReversible;
            }
        }

        private DataSetMetaDataType GetMetaData(string dataSetName)
        {
            foreach (var source in m_sources)
            {
                if (source.MetaData.Name == dataSetName)
                {
                    return source.MetaData;
                }
            }

            return null;
        }

        private List<KeyValuePair<FieldMetaData, DataValue>> ReadChangedFields(ushort dataSetWriterId, bool isKeyFrame)
        {
            foreach (var group in m_connection.Groups)
            {
                foreach (var writer in group.Writers)
                {
                    if (writer.Id == dataSetWriterId)
                    {
                        return writer.ReadChangedFields(isKeyFrame);
                    }
                }
            }

            return new();
        }

        private async Task PublishActionMetaData(CancellationToken ct)
        {
            foreach (var group in m_responder.Connection.WriterGroups)
            {
                if (!group.Enabled)
                {
                    continue;
                }

                foreach (var writer in group.DataSetWriters)
                {
                    if (!writer.Enabled)
                    {
                        continue;
                    }

                    string writerKey = $"A:{PublisherId}{writer.DataSetWriterId}";

                    var source = m_responder.Groups
                        .Where(x => x.Name == group.Name)
                        .FirstOrDefault()?
                        .Writers.Where(x => x.Name == writer.Name)
                        .FirstOrDefault()?
                        .Source;

                    string metadataTopic = null;
                    var metadataUpdateTime = 120000.0;
                    var writerQos = BrokerTransportQualityOfService.AtLeastOnce;

                    if (writer.TransportSettings?.Body is BrokerDataSetWriterTransportDataType dsts)
                    {
                        metadataTopic = dsts.MetaDataQueueName;
                        metadataUpdateTime = dsts.MetaDataUpdateTime;
                        writerQos = dsts.RequestedDeliveryGuarantee;
                    }

                    if (metadataUpdateTime > 0 && metadataTopic != null)
                    {
                        if (!m_lastMetaDataUpdateTime.TryGetValue(writerKey, out var lastMetaDataUpdateTime))
                        {
                            lastMetaDataUpdateTime = DateTime.MinValue;
                        }

                        if ((DateTime.UtcNow - lastMetaDataUpdateTime).TotalMilliseconds > metadataUpdateTime)
                        {
                            await PublishActionMetaData(
                                metadataTopic,
                                new JsonActionMetaDataMessage()
                                {
                                    MessageId = Guid.NewGuid().ToString(),
                                    MessageType = MessageTypes.ActionMetaData,
                                    DataSetWriterId = writer.DataSetWriterId,
                                    DataSetWriterName = writer.Name,
                                    PublisherId = PublisherId,
                                    Timestamp = DateTime.UtcNow,
                                    Request = source.Request,
                                    Response = source.Response,
                                    ActionTargets = source.GetTargets(),
                                    ActionMethods = source.GetMethods()
                                },
                                ct);

                            m_lastMetaDataUpdateTime[writerKey] = DateTime.UtcNow;
                        }
                    }
                }
            }

        }

        private async Task PublishDataSetMetaData(CancellationToken ct)
        {
            foreach (var group in m_connection.Connection.WriterGroups)
            {
                if (!group.Enabled)
                {
                    continue;
                }

                foreach (var writer in group.DataSetWriters)
                {
                    if (!writer.Enabled)
                    {
                        continue;
                    }

                    string writerKey = $"D:{PublisherId}{writer.DataSetWriterId}";

                    var metadata = GetMetaData(writer.DataSetName);

                    string metadataTopic = null;
                    var metadataUpdateTime = 120000.0;

                    if (writer.TransportSettings?.Body is BrokerDataSetWriterTransportDataType dsts)
                    {
                        metadataTopic = dsts.MetaDataQueueName;
                        metadataUpdateTime = dsts.MetaDataUpdateTime;
                    }

                    if (metadataUpdateTime > 0 && metadataTopic != null)
                    {
                        if (!m_lastMetaDataUpdateTime.TryGetValue(writerKey, out var lastMetaDataUpdateTime))
                        {
                            lastMetaDataUpdateTime = DateTime.MinValue;
                        }

                        if ((DateTime.UtcNow - lastMetaDataUpdateTime).TotalMilliseconds > metadataUpdateTime)
                        {
                            await PublishMetaData(
                                metadataTopic,
                                new JsonDataSetMetaDataMessage()
                                {
                                    MessageId = Guid.NewGuid().ToString(),
                                    MessageType = MessageTypes.DataSetMetaData,
                                    DataSetWriterId = writer.DataSetWriterId,
                                    DataSetWriterName = writer.Name,
                                    PublisherId = PublisherId,
                                    Timestamp = DateTime.UtcNow,
                                    MetaData = metadata
                                },
                                ct);

                            m_lastMetaDataUpdateTime[writerKey] = DateTime.UtcNow;
                        }
                    }
                }
            }
        }

        private async Task PublishData(CancellationToken ct)
        {
            foreach (var group in m_connection.Connection.WriterGroups)
            {
                if (!group.Enabled)
                {
                    continue;
                }

                string groupKey = $"{PublisherId}{group.WriterGroupId}";

                var nmcm = JsonNetworkMessageContentMask.None;
                var dscm = JsonDataSetMessageContentMask.None;

                if (group.MessageSettings?.Body is JsonWriterGroupMessageDataType wgms)
                {
                    nmcm = (JsonNetworkMessageContentMask)wgms.NetworkMessageContentMask;
                }

                bool hasArrayOfMessages = (nmcm & JsonNetworkMessageContentMask.SingleDataSetMessage) == 0;

                if ((nmcm & JsonNetworkMessageContentMask.NetworkMessageHeader) != 0)
                {
                    hasArrayOfMessages = false;
                }

                var encoder = new JsonEncoder(
                    m_messageContext,
                    FindEncoding(group),
                    topLevelIsArray: hasArrayOfMessages);

                WriteNetworkMessageHeader(encoder, nmcm, group);

                string topic = null;
                string metadataTopic = null;
                var groupQos = BrokerTransportQualityOfService.NotSpecified;

                if (group.TransportSettings?.Body is BrokerWriterGroupTransportDataType wgts)
                {
                    topic = wgts.QueueName;
                    groupQos = wgts.RequestedDeliveryGuarantee;
                }

                bool first = true;
                bool popMessagesField = false;
                bool messagesToSend = false;

                foreach (var writer in group.DataSetWriters)
                {
                    if (!writer.Enabled)
                    {
                        continue;
                    }

                    string writerKey = $"D:{PublisherId}{writer.DataSetWriterId}";

                    uint sequenceNumber = 0;

                    if (!m_sequenceNumbers.TryGetValue(writerKey, out sequenceNumber))
                    {
                        m_sequenceNumbers[writerKey] = sequenceNumber = 1;
                    }

                    var writerQos = BrokerTransportQualityOfService.NotSpecified;

                    if (writer.TransportSettings?.Body is BrokerDataSetWriterTransportDataType dsts)
                    {
                        topic = (nmcm & JsonNetworkMessageContentMask.SingleDataSetMessage) != 0 ? dsts.QueueName : topic;
                        metadataTopic = dsts.MetaDataQueueName;
                        writerQos = dsts.RequestedDeliveryGuarantee;
                    }

                    // check if key frame period has lapsed.
                    bool isKeyFrame = writer.KeyFrameCount <= 1 || sequenceNumber == 1;

                    if (m_lastKeyFrameTime.TryGetValue(writerKey, out var lastKeyFrameTime))
                    {
                        if ((DateTime.UtcNow - lastKeyFrameTime).TotalMilliseconds >= writer.KeyFrameCount * group.PublishingInterval)
                        {
                            isKeyFrame = true;
                        }
                    }

                    // read changed fields.
                    var fields = ReadChangedFields(writer.DataSetWriterId, isKeyFrame);

                    // check if keep alive period has lapsed.
                    if (m_lastDataUpdateTime.TryGetValue(writerKey, out var lastUpdateTime))
                    {
                        if (fields.Count == 0 && (DateTime.UtcNow - lastUpdateTime).TotalMilliseconds < group.KeepAliveTime)
                        {
                            continue;
                        }
                    }

                    if (fields.Count > 0)
                    {
                        m_sequenceNumbers[writerKey] = sequenceNumber + 1;
                    }

                    if (writer.MessageSettings?.Body is JsonDataSetWriterMessageDataType dwms)
                    {
                        dscm = (JsonDataSetMessageContentMask)dwms.DataSetMessageContentMask;
                    }

                    var metadata = GetMetaData(writer.DataSetName);

                    if ((nmcm & JsonNetworkMessageContentMask.SingleDataSetMessage) != 0)
                    {
                        if ((nmcm & JsonNetworkMessageContentMask.NetworkMessageHeader) != 0)
                        {
                            if ((nmcm & JsonNetworkMessageContentMask.DataSetClassId) != 0)
                            {
                                if (metadata.DataSetClassId != Guid.Empty)
                                {
                                    encoder.WriteGuid(nameof(metadata.DataSetClassId), metadata.DataSetClassId);
                                }
                            }

                            if (first)
                            {
                                encoder.PushStructure(nameof(JsonNetworkMessage.Messages));
                                popMessagesField = true;
                            }
                        }
                    }
                    else
                    {
                        if ((nmcm & JsonNetworkMessageContentMask.NetworkMessageHeader) != 0)
                        {
                            if (first)
                            {
                                encoder.PushArray(nameof(JsonNetworkMessage.Messages));
                                popMessagesField = true;
                                first = false;
                            }
                        }

                        encoder.PushStructure(null);
                    }

                    if ((nmcm & JsonNetworkMessageContentMask.DataSetMessageHeader) != 0)
                    {
                        if ((dscm & JsonDataSetMessageContentMask.DataSetWriterId) != 0)
                        {
                            encoder.WriteUInt16(nameof(JsonDataSetMessage.DataSetWriterId), writer.DataSetWriterId);
                        }

                        if ((dscm & JsonDataSetMessageContentMask.DataSetWriterName) != 0)
                        {
                            encoder.WriteString(nameof(JsonDataSetMessage.DataSetWriterName), writer.Name);
                        }

                        if ((dscm & JsonDataSetMessageContentMask.PublisherId) != 0)
                        {
                            encoder.WriteString(nameof(JsonDataSetMessage.PublisherId), PublisherId);
                        }

                        if ((dscm & JsonDataSetMessageContentMask.WriterGroupName) != 0)
                        {
                            encoder.WriteString(nameof(JsonDataSetMessage.WriterGroupName), group.Name);
                        }

                        if ((dscm & JsonDataSetMessageContentMask.SequenceNumber) != 0)
                        {
                            encoder.WriteUInt32(nameof(JsonDataSetMessage.SequenceNumber), sequenceNumber);
                        }

                        if ((dscm & JsonDataSetMessageContentMask.MetaDataVersion) != 0)
                        {
                            encoder.WriteEncodeable(nameof(JsonDataSetMessage.MetaDataVersion), metadata.ConfigurationVersion, null);
                        }

                        if ((dscm & JsonDataSetMessageContentMask.MinorVersion) != 0)
                        {
                            encoder.WriteUInt32(nameof(JsonDataSetMessage.MinorVersion), metadata.ConfigurationVersion.MinorVersion);
                        }

                        if ((dscm & JsonDataSetMessageContentMask.Timestamp) != 0)
                        {
                            encoder.WriteDateTime(nameof(JsonDataSetMessage.Timestamp), DateTime.UtcNow);
                        }

                        if ((dscm & JsonDataSetMessageContentMask.Status) != 0)
                        {
                            encoder.WriteStatusCode(nameof(JsonDataSetMessage.Status), StatusCodes.Good);
                        }

                        if ((dscm & JsonDataSetMessageContentMask.MessageType) != 0)
                        {
                            encoder.WriteString(
                                nameof(JsonDataSetMessage.MessageType),
                                (isKeyFrame) ? MessageTypes.KeyFrame :
                                (fields.Count == 0) ? MessageTypes.KeepAlive :
                                MessageTypes.DeltaFrame
                            );
                        }

                        encoder.PushStructure(nameof(JsonDataSetMessage.Payload));
                    }

                    foreach (var field in fields)
                    {
                        encoder.WriteRawValue(field.Key, field.Value, (DataSetFieldContentMask)writer.DataSetFieldContentMask);
                    }

                    if ((nmcm & JsonNetworkMessageContentMask.DataSetMessageHeader) != 0)
                    {
                        encoder.PopStructure();
                    }

                    if ((nmcm & JsonNetworkMessageContentMask.SingleDataSetMessage) != 0)
                    {
                        if (popMessagesField)
                        {
                            encoder.PopStructure();
                        }

                        await PublishData(
                            topic,
                            encoder.CloseAndReturnText(),
                            GetQos(groupQos, writerQos),
                            ct);

                        encoder = new JsonEncoder(m_messageContext, FindEncoding(group));
                        WriteNetworkMessageHeader(encoder, nmcm, group);
                        first = true;
                    }
                    else
                    {
                        encoder.PopStructure();
                        messagesToSend = true;
                    }

                    m_lastDataUpdateTime[writerKey] = DateTime.UtcNow;

                    if (isKeyFrame)
                    {
                        m_lastKeyFrameTime[writerKey] = DateTime.UtcNow;
                    }
                }

                if ((nmcm & JsonNetworkMessageContentMask.SingleDataSetMessage) == 0)
                {
                    if (popMessagesField)
                    {
                        encoder.PopArray();
                    }

                    if (messagesToSend)
                    {
                        await PublishData(
                            topic,
                            encoder.CloseAndReturnText(),
                            GetQos(groupQos, BrokerTransportQualityOfService.NotSpecified),
                            ct);
                    }
                }
            }
        }
    }
}
