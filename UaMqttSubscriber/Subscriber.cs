/* ========================================================================
 * Copyright (c) 2005-2024 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 * 
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/
using System.Net;
using System.Reflection;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;
using Opc.Ua;
using UaMqttCommon;
using KeyValuePair = Opc.Ua.KeyValuePair;

namespace UaMqttSubscriber
{
    internal class Subscriber
    {
        private string BrokerUrl => m_broker.BrokerUrl;
        private string TopicPrefix => m_broker.TopicPrefix;

        private MqttFactory m_mqttFactory;
        private IMqttClient m_mqttClient;
        private BrokerConfiguration m_broker;
        private bool m_disposed;
        private readonly CancellationTokenSource m_shutdown = new();
        private readonly Dictionary<string, Writer> m_writers = new();
        private readonly Dictionary<string, Group> m_groups = new();
        private readonly Queue<SubscribeRequest> m_subscribeQueue = new();
        private readonly HashSet<string> m_subscribedTopics = new();

        private class SubscribeRequest
        {
            public string Topic { get; set; }
            public bool Unsubscribe { get; set; }
        }

        private class Group
        {
            public string PublisherId { get; set; }
            public string GroupName { get; set; }
            public bool HasNetworkMessageHeader { get; set; }
            public bool HasDataSetMessageHeader { get; set; }
            public bool HasMultipleDataSetMessages { get; set; }
            public List<Writer> Writers { get; set; } = new();
        }

        private class Writer
        {
            public string PublisherId { get; set; }
            public string WriterName { get; set; }
            public int? DataSetWriterId { get; set; }
            public WriterGroupDataType WriterGroup { get; set; }
            public DataSetWriterDataType DataSetWriter { get; set; }
            public DataSetMetaDataType DataSetMetaData { get; set; }
            public string DataTopic { get; set; }
            public string MetaDataTopic { get; set; }
            public Dictionary<string, List<KeyValuePair>> Properties { get; set; } = new();
        }

        private IServiceMessageContext MessageContext => ServiceMessageContext.GlobalContext;

        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                    m_mqttClient?.Dispose();
                    m_shutdown?.Dispose();
                }

                m_disposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        private async Task LoadConfiguration(string configurationFile, string brokerName)
        {
            if (configurationFile == null)
            {
                string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                configurationFile = Path.Combine(folder, "Config", "uasubscriber-config.json");
            }

            using (var istrm = File.OpenRead(configurationFile))
            {
                var configuration = await JsonSerializer.DeserializeAsync<SubscriberConfiguration>(istrm);

                m_broker = configuration?.Brokers?
                    .Where(x => brokerName == null || x.Name == brokerName)
                    .FirstOrDefault();

                if (m_broker == null)
                {
                    throw new InvalidDataException($"Broker '{brokerName}' configuration is missing.");
                }
            }
        }

        private async Task ConnectToBroker()
        {
            m_mqttClient = m_mqttFactory.CreateMqttClient();

            var url = new Uri("mqtt://" + m_broker.BrokerUrl);

            var options = new MqttClientOptionsBuilder()
                .WithProtocolVersion((m_broker.UseMqtt311 ?? false) ? MqttProtocolVersion.V311 : MqttProtocolVersion.V500)
                .WithTcpServer(url.Host, url.Port > 0 ? url.Port : null);

            if (!String.IsNullOrEmpty(m_broker.UserName))
            {
                options = options.WithCredentials(
                    new NetworkCredential(String.Empty, m_broker.UserName).Password,
                    new NetworkCredential(String.Empty, m_broker.Password).Password);
            }

            if (!m_broker.DoNotUseTls ?? true)
            {
                options = options.WithTlsOptions(
                    o =>
                    {
                        o.WithCertificateValidationHandler(e =>
                        {
                            Log.Error($"Broker Certificate: '{e.Certificate.Subject}' {e.SslPolicyErrors}");
                            return m_broker.IgnoreCertificateErrors ?? false;
                        });

                        // The default value is determined by the OS. Set manually to force version.
                        o.WithSslProtocols(SslProtocols.Tls12);
                    });
            }

            var response = await m_mqttClient.ConnectAsync(options.Build(), m_shutdown.Token);

            if (response.ResultCode != MqttClientConnectResultCode.Success)
            {
                Log.Error($"Connect Failed: {response.ResultCode} {response.ResultCode} {response.ReasonString}");
            }
            else
            {
                Log.Info($"Publisher Connected to '{url}'");
            }
        }

        public async Task Connect(string configurationFile, string brokerName)
        {
            m_mqttFactory = new MqttFactory();

            await LoadConfiguration(configurationFile, brokerName);

            await ConnectToBroker();

            m_mqttClient.ApplicationMessageReceivedAsync += async delegate (MqttApplicationMessageReceivedEventArgs args)
            {
                string topic = args.ApplicationMessage.Topic;

                Log.Info($"Received on Topic: {topic}");

                if (topic.StartsWith($"{TopicPrefix}/json/{MessageTypes.Status}"))
                {
                    await HandleStatus(args.ApplicationMessage);
                }
                else if (topic.StartsWith($"{TopicPrefix}/json/{MessageTypes.Connection}"))
                {
                    await HandleConnection(args.ApplicationMessage);
                }
                else if (topic.StartsWith($"{TopicPrefix}/json/{MessageTypes.DataSetMetaData}"))
                {
                    await HandleDataSetMetaData(args.ApplicationMessage);
                }
                else
                {
                    await HandleData(args.ApplicationMessage);
                }
            };

            Subscribe(new Topic()
            {
                TopicPrefix = TopicPrefix,
                MessageType = MessageTypes.Status,
                PublisherId = "#"
            }.Build());

            var task = Task.Run(() => ProcessSubscribeRequests());

            Log.Control("Press enter to exit.");

            do
            {
                if (await Log.CheckForKeyPress(1000, () => { return false; }))
                {
                    break;
                }
            }
            while (true);

            var disconnectOptions = m_mqttFactory.CreateClientDisconnectOptionsBuilder().Build();
            await m_mqttClient.DisconnectAsync(disconnectOptions, CancellationToken.None);

            m_shutdown.Cancel();
            await task;

            Log.Info("Subscriber Disconnected!");
        }

        private async Task ProcessSubscribeRequests()
        {
            do
            {
                if (m_mqttClient == null || !m_mqttClient.IsConnected)
                {
                    if (m_mqttClient != null)
                    {
                        try
                        {
                            await m_mqttClient.DisconnectAsync(new MqttClientDisconnectOptions() { }, m_shutdown.Token);
                        }
                        catch (Exception e)
                        {
                            Log.Error($"Broker Disconnect Error: [{e.GetType().Name}] {e.Message}");
                            continue;
                        }

                        m_mqttClient.Dispose();
                        m_mqttClient = null;
                    }

                    try
                    {
                        await ConnectToBroker();

                        lock (m_subscribeQueue)
                        {
                            foreach (var topic in m_subscribedTopics)
                            {
                                Subscribe(topic);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Broker Connect Error: [{e.GetType().Name}] {e.Message}");
                        m_mqttClient.Dispose();
                        m_mqttClient = null;
                        continue;
                    }
                }

                SubscribeRequest request;

                do
                {
                    request = null;

                    lock (m_subscribeQueue)
                    {
                        if (m_subscribeQueue.Count > 0)
                        {
                            request = m_subscribeQueue.Dequeue();
                        }
                    }

                    if (request != null)
                    {
                        try
                        {
                            if (request.Unsubscribe)
                            {
                                lock (m_subscribeQueue)
                                {
                                    m_subscribedTopics.Remove(request.Topic);
                                }

                                var options = m_mqttFactory.CreateUnsubscribeOptionsBuilder()
                                    .WithTopicFilter(request.Topic)
                                    .Build();

                                var response = await m_mqttClient.UnsubscribeAsync(options, m_shutdown.Token);

                                if (!String.IsNullOrEmpty(response?.ReasonString))
                                {
                                    Log.Error($"Unsubscribe Failed:'{response?.ReasonString}'.");
                                }
                                else
                                {
                                    Log.Info($"Unsubscribed: '{request.Topic}'.");
                                }
                            }
                            else
                            {
                                lock (m_subscribeQueue)
                                {
                                    if (m_subscribedTopics.Contains(request.Topic))
                                    {
                                        Log.Info($"Already subscribed: '{request.Topic}'.");
                                        continue;
                                    }

                                    m_subscribedTopics.Add(request.Topic);
                                }

                                var options = m_mqttFactory.CreateSubscribeOptionsBuilder()
                                    .WithTopicFilter(f => { f.WithTopic(request.Topic); })
                                    .Build();

                                var response = await m_mqttClient.SubscribeAsync(options, m_shutdown.Token);

                                if (!String.IsNullOrEmpty(response?.ReasonString))
                                {
                                    Log.Error($"Subscribe Failed:'{response?.ReasonString}'.");

                                    lock (m_subscribeQueue)
                                    {
                                        m_subscribedTopics.Remove(request.Topic);
                                    }
                                }
                                else
                                {
                                    Log.Info($"Subscribed: '{request.Topic}'.");
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Error($"Subscribe/Unsubscribe Error: [{e.GetType().Name}] {e.Message}");
                        }
                    }
                }
                while (request != null);
            }
            while (!m_shutdown.Token.WaitHandle.WaitOne(250));
        }

        private void Subscribe(string topic)
        {
            lock (m_subscribeQueue)
            {
                m_subscribeQueue.Enqueue(new SubscribeRequest()
                {
                    Topic = topic,
                    Unsubscribe = false
                });
            }
        }

        private void Unsubscribe(string topic)
        {
            lock (m_subscribeQueue)
            {
                if (m_subscribedTopics.Contains(topic))
                {
                    m_subscribeQueue.Enqueue(new SubscribeRequest()
                    {
                        Topic = topic,
                        Unsubscribe = true
                    });
                }
            }
        }

        private void WriteFieldValue(string name, DataSetField field, Writer writer)
        {
            StringBuilder sb = new();

            if (field?.SourceTimestamp != null)
            {
                sb.Append($"[{field?.SourceTimestamp:HH:mm:ss}] ");
            }

            if (StatusCode.IsNotGood(field.StatusCode ?? 0))
            {
                sb.Append($"{name}=[{field?.StatusCode}] ");
            }

            JsonNode value = null;
            BuiltInType builtInType = BuiltInType.Null;

            if (field?.Value is JsonObject @object)
            {
                if (@object.TryGetPropertyValue("Body", out var body))
                {
                    if (@object.TryGetPropertyValue("Type", out var type))
                    {
                        builtInType = (BuiltInType)(int)type.AsValue();

                        if (builtInType == BuiltInType.StatusCode)
                        {
                            value = $"{StatusCodes.GetBrowseName((uint)body.AsValue())}";
                        }
                        else
                        {
                            value = $"{body}";
                        }
                    }
                    else
                    {
                        value = body;
                    }
                }
                else
                {
                    value = @object;
                }
            }
            else
            {
                value = $"{field?.Value}";
            }

            sb.Append($"{name}: ");

            if (writer != null)
            {
                if (writer.Properties.TryGetValue(name, out var properties))
                {
                    foreach (var property in properties)
                    {
                        if (property.Key?.Name == BrowseNames.EngineeringUnits)
                        {
                            var eu = ExtensionObject.ToEncodeable((ExtensionObject)property.Value.Value) as EUInformation;

                            if (eu != null)
                            {
                                sb.Append($"[{builtInType}] {value} {eu.Description.Text}");
                            }
                        }

                        if (property.Key?.Name == BrowseNames.TrueState)
                        {
                            var text = property.Value.Value as LocalizedText;

                            if (text != null)
                            {
                                if (Boolean.TryParse(value.ToString(), out var boolean))
                                {
                                    if (boolean)
                                    {
                                        sb.Append($"[{builtInType}] {text.Text}");
                                    }
                                }
                            }
                        }

                        if (property.Key?.Name == BrowseNames.FalseState)
                        {
                            var text = property.Value.Value as LocalizedText;

                            if (text != null)
                            {
                                if (Boolean.TryParse(value.ToString(), out var boolean))
                                {
                                    if (!boolean)
                                    {
                                        sb.Append($"[{builtInType}] {text.Text}");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    sb.Append($"[{builtInType}] {value}");
                }
            }
            else
            {
                sb.Append($"[{builtInType}] {value}");
            }

            Log.Info(sb.ToString());
        }

        private void HandleDataSetMessage(string topic, JsonDataSetMessage dm)
        {
            var publisherId = $"{dm?.PublisherId}";

            Log.Info(new string('=', 60));

            if (!dm.ExcludeHeader)
            {
                Log.Info(new string('-', 60));
                if (dm?.PublisherId != null) Log.Info($"DataSetWriterId: {dm?.DataSetWriterId}");
                if (dm?.DataSetWriterName != null) Log.Info($"DataSetWriterName: {dm?.DataSetWriterName}");
                if (dm?.PublisherId != null) Log.Info($"PublisherId: {publisherId}");
                if (dm?.WriterGroupName != null) Log.Info($"DataSetWriterName: {dm?.WriterGroupName}");
                if (dm?.SequenceNumber != null) Log.Info($"SequenceNumber: {dm?.SequenceNumber}");
                if (dm?.MetaDataVersion != null) Log.Info($"MajorVersion: {dm?.MetaDataVersion?.MajorVersion}");
                if (dm?.MetaDataVersion != null) Log.Info($"MinorVersion: {dm?.MetaDataVersion?.MinorVersion}");
                if (dm?.MinorVersion != null) Log.Info($"MinorVersion: {dm?.MinorVersion}");
                if (dm?.Timestamp != null) Log.Info($"Timestamp: {dm?.Timestamp:HH:mm:ss.fff}");
                if (dm?.Status != null) Log.Info($"Status: {dm?.Status}");
                if (dm?.MessageType != null) Log.Info($"MessageType: {dm?.MessageType}");
                Log.Info(new string('-', 60));
            }

            if (dm.Payload != null)
            {
                Writer writer = null;

                // find the writer using information in the header.
                if (!dm.ExcludeHeader)
                {
                    var writerId = $"{publisherId}.{dm?.DataSetWriterId}";

                    lock (m_writers)
                    {
                        if (!m_writers.TryGetValue(writerId, out writer))
                        {
                            Log.Info($"[Writer for Data message not found: {writerId}]");
                        }
                    }
                }

                // find the writer using information in the topic.
                else if (topic != null)
                {
                    lock (m_writers)
                    {
                        writer = m_writers.Values.Where(x => x.DataTopic == topic).FirstOrDefault();
                    }

                    if (writer == null)
                    {
                        Log.Error($"[Writer for Data message not found for topic: {topic}]");
                    }
                }

                foreach (var item in dm.Payload)
                {
                    var field = DataSetField.Decode(MessageContext, item.Value.ToJsonString());
                    WriteFieldValue(item.Key, field, writer);
                }
            }

            Log.Info(new string('=', 60));
        }

        private Task HandleData(MqttApplicationMessage message)
        {
            byte[] payload = message.PayloadSegment.Array;

            if (payload != null)
            {
                var json = Encoding.UTF8.GetString(payload);

                try
                {
                    var nm = JsonNetworkMessage.Decode(MessageContext, json);

                    Log.Info(new string('>', 60));

                    if (!nm.ExcludeHeader)
                    {
                        Log.Info(new string('-', 60));
                        if (nm.MessageId != null) Log.Info($"MessageId: {nm.MessageId}");
                        if (nm.MessageType != null) Log.Info($"MessageType: {nm.MessageType}");
                        if (nm.PublisherId != null) Log.Info($"PublisherId: {nm.PublisherId}");
                        if (nm.WriterGroupName != null) Log.Info($"WriterGroupName: {nm.WriterGroupName}");
                        if (nm.DataSetClassId != null) Log.Info($"DataSetClassId: {nm.DataSetClassId}");
                        Log.Info(new string('-', 60));
                    }

                    if (nm?.Messages != null)
                    {
                        foreach (var dm in nm.Messages)
                        {
                            HandleDataSetMessage(message.Topic, dm);
                        }
                    }

                    Log.Info(new string('<', 60));
                }
                catch (Exception e)
                {
                    Log.Error($"Parsing Failed: '{e.Message}' [{json}]");
                }
            }

            return Task.CompletedTask;
        }

        private Task HandleStatus(MqttApplicationMessage message)
        {
            byte[] payload = message.PayloadSegment.Array;

            if (payload != null)
            {
                var json = Encoding.UTF8.GetString(payload);

                try
                {
                    var status = JsonStatusMessage.Decode(MessageContext, json);
                    Log.Info($"{status?.PublisherId}: Status={((status?.Status != null) ? status.Status.Value : "")}");

                    var connectionTopic = new Topic()
                    {
                        TopicPrefix = TopicPrefix,
                        MessageType = MessageTypes.Connection,
                        PublisherId = status?.PublisherId
                    }.Build();

                    if (status?.Status == PubSubState.Operational)
                    {
                        Subscribe(connectionTopic);
                    }
                    else
                    {
                        Unsubscribe(connectionTopic);

                        // unsubscribe from all data topics for this publisher.
                        lock (m_writers)
                        {
                            foreach (var group in m_groups.Values)
                            {
                                if (group.PublisherId == status?.PublisherId)
                                {
                                    foreach (var writer in group.Writers)
                                    {
                                        if (writer.DataTopic != null)
                                        {
                                            Unsubscribe(writer.DataTopic);
                                        }

                                        if (writer.MetaDataTopic != null)
                                        {
                                            Unsubscribe(writer.MetaDataTopic);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"Parsing Failed: '{e.Message}' [{json}]");
                }
            }

            return Task.CompletedTask;
        }

        private Task HandleDataSetMetaData(MqttApplicationMessage message)
        {
            byte[] payload = message.PayloadSegment.Array;

            if (payload != null)
            {
                var json = Encoding.UTF8.GetString(payload);

                try
                {
                    var metadata = JsonDataSetMetaDataMessage.Decode(MessageContext, json);
                    var writerId = $"{metadata?.PublisherId}.{metadata?.DataSetWriterId}";
                    var source = metadata?.MetaData?.Fields;

                    Log.Info($"DataSetMetaData Message: '{writerId}'");

                    if (source != null)
                    {
                        lock (m_writers)
                        {
                            if (!m_writers.TryGetValue(writerId, out var writer))
                            {
                                writer = new Writer()
                                {
                                    PublisherId = metadata?.PublisherId,
                                    DataSetMetaData = metadata?.MetaData
                                };

                                m_writers[writerId] = writer;
                            }

                            foreach (var field in source)
                            {
                                if (field.Name == null || field.Properties == null)
                                {
                                    continue;
                                }

                                var properties = new List<Opc.Ua.KeyValuePair>();

                                foreach (var property in field.Properties)
                                {
                                    if (property.Value != Variant.Null)
                                    {
                                        properties.Add(property);
                                    }
                                }

                                if (properties.Count > 0)
                                {
                                    writer.Properties[field.Name] = properties;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"Parsing Failed: '{e.Message}' [{json}]");
                }
            }

            return Task.CompletedTask;
        }

        public Task HandleConnection(MqttApplicationMessage message)
        {
            byte[] payload = message.PayloadSegment.Array;

            if (payload == null)
            {
                return Task.CompletedTask;
            }

            JsonPubSubConnectionMessage connection;
            var json = Encoding.UTF8.GetString(payload);

            try
            {
                connection = JsonPubSubConnectionMessage.Decode(MessageContext, json);
            }
            catch (Exception e)
            {
                Log.Info($"Parsing Failed: '{e.Message}' [{json}]");
                return Task.CompletedTask;
            }

            if (connection?.Connection?.WriterGroups != null)
            {
                foreach (var ii in connection.Connection.WriterGroups)
                {
                    var groupId = $"{connection?.PublisherId}.{ii?.Name}";

                    var groupTransportSettings = ExtensionObject.ToEncodeable(ii.TransportSettings) as BrokerWriterGroupTransportDataType;
                    var dataTopicName = groupTransportSettings?.QueueName;

                    if (dataTopicName == null)
                    {
                        dataTopicName = new Topic()
                        {
                            TopicPrefix = TopicPrefix,
                            MessageType = MessageTypes.Data,
                            PublisherId = connection?.PublisherId,
                            GroupName = ii?.Name,
                            WriterName = "#"
                        }.Build();
                    }

                    Group group = null;

                    lock (m_writers)
                    {
                        if (!m_groups.TryGetValue(groupId, out group))
                        {
                            group = new Group()
                            {
                                PublisherId = connection?.PublisherId,
                                GroupName = ii?.Name
                            };

                            m_groups[groupId] = group;
                        }
                    }

                    group.HasNetworkMessageHeader = false;
                    group.HasDataSetMessageHeader = false;
                    group.HasMultipleDataSetMessages = false;

                    if (ii?.MessageSettings?.Body is JsonWriterGroupMessageDataType wgm)
                    {
                        var mask = wgm.NetworkMessageContentMask;
                        group.HasNetworkMessageHeader = (mask & 0x01) != 0;
                        group.HasDataSetMessageHeader = (mask & 0x02) != 0;
                        group.HasMultipleDataSetMessages = (mask & 0x04) == 0;
                    }

                    if (ii?.DataSetWriters != null)
                    {
                        foreach (var jj in ii.DataSetWriters)
                        {
                            var writerTransportSettings = ExtensionObject.ToEncodeable(jj.TransportSettings) as BrokerDataSetWriterTransportDataType;
                            var metaDataTopicName = writerTransportSettings?.MetaDataQueueName;

                            lock (m_writers)
                            {
                                var writerId = $"{connection?.PublisherId}.{jj?.DataSetWriterId}";

                                if (!m_writers.TryGetValue(writerId, out var writer))
                                {
                                    writer = new Writer()
                                    {
                                        PublisherId = connection?.PublisherId,
                                        DataSetWriterId = jj?.DataSetWriterId,
                                        WriterName = jj?.Name
                                    };

                                    m_writers[writerId] = writer;
                                    group.Writers.Add(writer);
                                }

                                writer.WriterGroup = ii;
                                writer.DataSetWriter = jj;

                                dataTopicName = writer.DataTopic = (writerTransportSettings?.QueueName != null)
                                    ? writerTransportSettings?.QueueName
                                    : new Topic()
                                    {
                                        TopicPrefix = TopicPrefix,
                                        MessageType = MessageTypes.Data,
                                        PublisherId = connection?.PublisherId,
                                        GroupName = ii?.Name,
                                        WriterName = ii?.Name
                                    }.Build();

                                metaDataTopicName = writer.MetaDataTopic = (metaDataTopicName != null)
                                    ? metaDataTopicName
                                    : new Topic()
                                    {
                                        TopicPrefix = TopicPrefix,
                                        MessageType = MessageTypes.DataSetMetaData,
                                        PublisherId = connection?.PublisherId,
                                        GroupName = ii?.Name,
                                        WriterName = ii?.Name
                                    }.Build();
                            }

                            if (dataTopicName != null)
                            {
                                Subscribe(dataTopicName);
                            }

                            if (metaDataTopicName != null)
                            {
                                Subscribe(metaDataTopicName);
                            }
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
