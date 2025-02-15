using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Opc.Ua;
using System.Text;
using UaPubSubCommon;

namespace UaSubscriber
{
    public class RemotePublisher : IRemotePublisher
    {
        private readonly object m_lock = new();
        private readonly string m_topicPrefix;
        private readonly ServiceMessageContext m_messageContext;
        private Dictionary<ushort, Writer> m_writers = new();
        private Dictionary<string, bool> m_topics = new();

        private enum HeaderProfile
        {
            NetworkMessage = 1,
            DataSetMessage = 2,
            Minimal = 3
        }

        private class Writer
        {
            public ushort Id;
            public string Name;
            public string GroupName;
            public string DataSetName;
            public string DataTopic;
            public string MetaDataTopic;
            public DataSetMetaDataType MetaData;
            public List<PubSubField> Fields;
        }

        public RemotePublisher(ServiceMessageContext messageContext, string topicPrefix)
        {
            m_messageContext = messageContext;
            m_topicPrefix = topicPrefix;
        }

        public string PublisherId { get; set; }

        public PubSubState PubSubState { get; set; }

        public PubSubConnectionDataType Connection { get; set; }

        public ApplicationDescription ApplicationDescription { get; set; }

        public Dictionary<string, bool> GetTopics()
        {
            lock (m_lock)
            {
                Dictionary<string, bool> topics = new(m_topics);

                m_topics = new();

                foreach (var writer in m_writers)
                {
                    topics[writer.Value.DataTopic] = true;
                    topics[writer.Value.MetaDataTopic] = true;
                }

                return topics;
            }
        }

        public async Task<bool> ProcessMessage(string topic, string json, CancellationToken ct)
        {
            Writer[] writers;

            lock (m_lock)
            {
                writers = m_writers.Values.ToArray();
            }

            foreach (var writer in writers)
            {
                if (writer.DataTopic == topic)
                {
                    await ProcessDataMessage(topic, json, ct);
                    return false;
                }

                if (writer.MetaDataTopic == topic)
                {
                    await ProcessMetaDataMessage(writer, json, ct);
                    return false;
                }
            }

            var pt = Topic.Parse(topic, m_topicPrefix);

            if (pt.MessageType == TopicTypes.ApplicationDescription)
            {
                await ProcessApplicationDescription(json, ct);
                return false;
            }

            if (pt.MessageType == TopicTypes.PubSubConnection)
            {
                await ProcessPubSubConnection(json, ct);
                return true;
            }

            return false;
        }

        private async Task ProcessDataMessage(string topic, string json, CancellationToken ct)
        {
            var nm = await ReadNetworkMessage(json, ct);

            if (!String.IsNullOrEmpty(nm.MessageType))
            {
                Log.System(new string('=', 80));
                Log.Info($"MessageId: '{nm.MessageId}'");
                Log.Info($"MessageType: '{nm.MessageType}'");
                Log.Info($"PublisherId: '{nm.PublisherId}'");
                if (!String.IsNullOrEmpty(nm.DataSetClassId)) Log.Info($"DataSetClassId: '{nm.DataSetClassId}'");
                if (!String.IsNullOrEmpty(nm.WriterGroupName)) Log.Info($"WriterGroupName: '{nm.WriterGroupName}'");
            }

            foreach (var ii in nm.Messages.Value as IList<ExtensionObject>)
            {
                var dm = ii.Body as JsonDataSetMessage;

                if (dm.DataSetWriterId != 0)
                {
                    Log.System(new string('=', 80));
                    Log.Info($"DataSetWriterId: '{dm.DataSetWriterId}'");
                    if (!String.IsNullOrEmpty(dm.PublisherId)) Log.Info($"PublisherId: '{dm.PublisherId}'");
                    if (!String.IsNullOrEmpty(dm.WriterGroupName)) Log.Info($"WriterGroupName: '{dm.WriterGroupName}'");
                    if (!String.IsNullOrEmpty(dm.DataSetWriterName)) Log.Info($"DataSetWriterName: '{dm.DataSetWriterName}'");
                    if (dm.SequenceNumber != 0) Log.Info($"SequenceNumber: '{dm.SequenceNumber}'");
                    if (!String.IsNullOrEmpty(dm.MessageType)) Log.Info($"MessageType: '{dm.MessageType}'");
                    if (dm.MetaDataVersion?.MajorVersion != 0) Log.Info($"MetaDataVersion: '{dm.MetaDataVersion.MajorVersion}.{dm.MetaDataVersion.MinorVersion}'");
                    if (dm.MinorVersion != 0) Log.Info($"MinorVersion: '{dm.MinorVersion}'");
                    Log.Info($"Status: '{dm.Status}'");
                    Log.Info($"Timestamp: '{dm.Timestamp.ToString("HH:mm:ss.fff")}'");
                }

                if (dm.Payload.Body is JObject jobject)
                {
                    using (var reader = jobject.CreateReader())
                    {
                        Writer writer;
                        
                        if (!m_writers.TryGetValue(dm.DataSetWriterId, out writer))
                        {
                            writer = m_writers.Where(x => x.Value.DataTopic == topic).Select(x => x.Value).FirstOrDefault();
                        }

                        if (writer == null)
                        {
                            Log.Error($"Missing writer for incoming message on '{topic}'!");
                            continue;
                        }

                        var fields = await PubSubUtils.ReadFieldValues(m_messageContext, reader, writer.Fields, ct);

                        Log.System(new string('=', 80));

                        foreach (var field in fields)
                        {
                            var dv = field.Value.Value;

                            StringBuilder sb = new();
                            sb.Append(StatusCode.IsBad(dv.StatusCode) ? dv.StatusCode : dv.WrappedValue);

                            var eu = field.Value.Properties.Where(x => x.Key == BrowseNames.EngineeringUnits).Select(x => x.Value).FirstOrDefault();

                            if (eu.Value is ExtensionObject eo && eo.Body is EUInformation info)
                            {
                                sb.Append(' ');
                                sb.Append(info.DisplayName);
                            }

                            Log.Info($"{field.Key}: {sb}");
                        }
                    }
                }
            }

            Log.System(new string('=', 80));
        }

        private Task ProcessMetaDataMessage(Writer writer, string json, CancellationToken ct)
        {
            JsonDataSetMetaDataMessage message = new();

            using (var decoder = new JsonDecoder(json, m_messageContext))
            {
                message.Decode(decoder);
            }

            if (String.IsNullOrEmpty(message.PublisherId) || message.PublisherId != PublisherId)
            {
                Log.Error($"[{PublisherId}] DataSetMetaData Message with invalid PublisherId: '{message.PublisherId}'.");
                return Task.CompletedTask;
            }

            Log.Info($"[{PublisherId}] DataSetMetaData{Environment.NewLine}   Name: '{message?.MetaData?.Name}'{Environment.NewLine}   Description: '{message?.MetaData?.Description}'");

            if (writer.MetaData?.ConfigurationVersion == null || !writer.MetaData.ConfigurationVersion.Equals(message.MetaData.ConfigurationVersion))
            {
                if (message.MetaData.Namespaces != null)
                {
                    foreach (var ns in message.MetaData.Namespaces)
                    {
                        m_messageContext.NamespaceUris.GetIndexOrAppend(ns);
                    }
                }

                writer.Fields = new();

                if (message.MetaData?.Fields != null)
                {
                    foreach (var ii in message.MetaData.Fields)
                    {
                        var field = new PubSubField()
                        {
                            Name = ii.Name,
                            BuiltInType = (BuiltInType)ii.BuiltInType,
                            ValueRank = ii.ValueRank,
                            Value = new DataValue() { StatusCode = StatusCodes.BadWaitingForInitialData },
                            Properties = new()
                        };

                        foreach (var property in ii?.Properties)
                        {
                            field.Properties[property.Key.Name] = property.Value;
                        }

                        writer.Fields.Add(field);
                    }
                }
            }

            writer.MetaData = message.MetaData;

            return Task.CompletedTask;
        }

        private Task ProcessApplicationDescription(string json, CancellationToken ct)
        {
            JsonApplicationDescriptionMessage message = new();

            using (var decoder = new JsonDecoder(json, m_messageContext))
            {
                message.Decode(decoder);
            }

            if (String.IsNullOrEmpty(message.PublisherId) || message.PublisherId != PublisherId)
            {
                Log.Error($"[{PublisherId}] ApplicationDescription Message with invalid PublisherId: '{message.PublisherId}'.");
                return Task.CompletedTask;
            }

            ApplicationDescription = message.Description;
            Log.Info($"[{PublisherId}] ApplicationDescription{Environment.NewLine}   ApplicationName: '{ApplicationDescription?.ApplicationName?.Text}'{Environment.NewLine}   ApplicationUri: '{ApplicationDescription?.ApplicationUri}'");

            return Task.CompletedTask;
        }

        private Task ProcessPubSubConnection(string json, CancellationToken ct)
        {
            JsonPubSubConnectionMessage message = new();

            using (var decoder = new JsonDecoder(json, m_messageContext))
            {
                message.Decode(decoder);
            }

            if (String.IsNullOrEmpty(message.PublisherId) || message.PublisherId != PublisherId)
            {
                Log.Error($"[{PublisherId}] PubSubConnection Message with invalid PublisherId: '{message.PublisherId}'.");
                return Task.CompletedTask;
            }

            lock (m_lock)
            {
                foreach (var topic in m_topics.Keys)
                {
                    m_topics[topic] = false;
                }
            }

            Connection = message.Connection;
            StringBuilder sb = new();
            sb.AppendLine("DataSetWriters:");

            lock (m_lock)
            {
                foreach (var group in Connection.WriterGroups)
                {
                    var groupSettings = group.TransportSettings.Body as BrokerWriterGroupTransportDataType;

                    foreach (var writer in group.DataSetWriters)
                    {
                        var writerSettings = writer.TransportSettings.Body as BrokerDataSetWriterTransportDataType;

                        if (!m_writers.TryGetValue(writer.DataSetWriterId, out var writer2))
                        {
                            m_writers[writer.DataSetWriterId] = writer2 = new Writer()
                            {
                                Id = writer.DataSetWriterId,
                                Fields = new()
                            };
                        }

                        writer2.Name = writer.Name;
                        writer2.GroupName = group.Name;
                        writer2.DataSetName = writer.DataSetName;
                        writer2.DataTopic = (!String.IsNullOrWhiteSpace(writerSettings.QueueName)) ? writerSettings?.QueueName : groupSettings?.QueueName;
                        writer2.MetaDataTopic = writerSettings?.MetaDataQueueName;

                        m_topics[writer2.DataTopic] = !String.IsNullOrEmpty(writer2.DataTopic);
                        m_topics[writer2.MetaDataTopic] = !String.IsNullOrEmpty(writer2.MetaDataTopic);

                        sb.AppendLine($"   {writer2.GroupName}.{writer2.Name}");
                    }
                }
            }

            Log.Info($"[{PublisherId}] PubSubConnection {sb.ToString()}");

            return Task.CompletedTask;
        }

        private async Task<JsonDataSetMessage> ReadDataSetMessage(JsonReader reader, CancellationToken ct)
        {
            JsonDataSetMessage dm = new();

            if (reader.TokenType == JsonToken.StartObject)
            {
                while (await reader.ReadAsync(ct) && reader.TokenType == JsonToken.PropertyName)
                {
                    switch (reader.Value)
                    {
                        case nameof(JsonDataSetMessage.DataSetWriterId):
                        {
                            dm.DataSetWriterId = (ushort)((await reader.ReadAsInt32Async(ct)) ?? 0);
                            break;
                        }

                        case nameof(JsonDataSetMessage.SequenceNumber):
                        {
                            dm.SequenceNumber = (uint)((await reader.ReadAsDecimalAsync(ct)) ?? 0);
                            break;
                        }

                        case nameof(JsonDataSetMessage.PublisherId):
                        {
                            dm.PublisherId = await reader.ReadAsStringAsync(ct);
                            break;
                        }

                        case nameof(JsonDataSetMessage.WriterGroupName):
                        {
                            dm.WriterGroupName = await reader.ReadAsStringAsync(ct);
                            break;
                        }

                        case nameof(JsonDataSetMessage.DataSetWriterName):
                        {
                            dm.DataSetWriterName = await reader.ReadAsStringAsync(ct);
                            break;
                        }

                        case nameof(JsonDataSetMessage.MessageType):
                        {
                            dm.MessageType = await reader.ReadAsStringAsync(ct);
                            break;
                        }

                        case nameof(JsonDataSetMessage.MetaDataVersion):
                        {
                            dm.MetaDataVersion = await ReadConfigurationVersion(reader, ct);
                            break;
                        }

                        case nameof(JsonDataSetMessage.MinorVersion):
                        {
                            dm.MinorVersion = (uint)((await reader.ReadAsDecimalAsync(ct)) ?? 0);
                            break;
                        }

                        case nameof(JsonDataSetMessage.Status):
                        {
                            if ((await reader.ReadAsync(ct) && reader.TokenType == JsonToken.StartObject))
                            {
                                var jobject = await JObject.LoadAsync(reader, ct);
                                dm.Status = PubSubUtils.ReadStatusCode(jobject);
                            }

                            break;
                        }

                        case nameof(JsonDataSetMessage.Timestamp):
                        {
                            dm.Timestamp = await reader.ReadAsDateTimeAsync(ct) ?? DateTime.MinValue;
                            break;
                        }

                        case nameof(JsonDataSetMessage.Payload):
                        {
                            if (await reader.ReadAsync(ct) && reader.TokenType == JsonToken.StartObject)
                            {
                                dm.Payload = new ExtensionObject(JObject.Load(reader));
                            }

                            break;
                        }
                    }
                }
            }

            return dm;
        }

        private async Task<JsonNetworkMessage> ReadNetworkMessage(string json, CancellationToken ct)
        {
            JsonNetworkMessage nm = new();

            var style = await DetectHeaderProfile(json, ct);

            using (var istrm = new StringReader(json))
            {
                using (var reader = new JsonTextReader(istrm))
                {
                    await reader.ReadAsync(ct);

                    if (style == HeaderProfile.Minimal)
                    {
                        var dm = new JsonDataSetMessage();
                        dm.Payload = new ExtensionObject(JObject.Load(reader));
                        nm.Messages = new Variant(new ExtensionObject[] { new ExtensionObject(dm) });
                        return nm;
                    }

                    if (style == HeaderProfile.DataSetMessage)
                    {
                        var dm = await ReadDataSetMessage(reader, ct);
                        nm.Messages = new Variant(new ExtensionObject[] { new ExtensionObject(dm) });
                        return nm;
                    }

                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        while (await reader.ReadAsync(ct))
                        {
                            if (reader.TokenType != JsonToken.PropertyName)
                            {
                                await reader.SkipAsync(ct);
                                continue;
                            }

                            switch (reader.Value)
                            {
                                case nameof(JsonNetworkMessage.MessageId):
                                {
                                    nm.MessageId = await reader.ReadAsStringAsync(ct);
                                    break;
                                }

                                case nameof(JsonNetworkMessage.MessageType):
                                {
                                    nm.MessageType = await reader.ReadAsStringAsync(ct);
                                    break;
                                }

                                case nameof(JsonNetworkMessage.PublisherId):
                                {
                                    nm.PublisherId = await reader.ReadAsStringAsync(ct);
                                    break;
                                }

                                case nameof(JsonNetworkMessage.WriterGroupName):
                                {
                                    nm.WriterGroupName = await reader.ReadAsStringAsync(ct);
                                    break;
                                }

                                case nameof(JsonNetworkMessage.DataSetClassId):
                                {
                                    nm.DataSetClassId = await reader.ReadAsStringAsync(ct);
                                    break;
                                }

                                case nameof(JsonNetworkMessage.Messages):
                                {
                                    List<ExtensionObject> messages = new();

                                    if (await reader.ReadAsync(ct) && reader.TokenType == JsonToken.StartObject)
                                    {
                                        messages.Add(new ExtensionObject(await ReadDataSetMessage(reader, ct)));
                                    }
                                    else if (reader.TokenType == JsonToken.StartArray)
                                    {
                                        while (await reader.ReadAsync(ct) && reader.TokenType != JsonToken.EndArray)
                                        {
                                            messages.Add(new ExtensionObject(await ReadDataSetMessage(reader, ct)));
                                        }
                                    }

                                    nm.Messages = new Variant(messages.ToArray());
                                    break;
                                }

                                default:
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return nm;
        }

        private async Task<HeaderProfile> DetectHeaderProfile(string json, CancellationToken ct)
        {
            bool hasMessageId = false;
            bool hasMessages = false;

            using (var istrm = new StringReader(json))
            {
                using (var reader = new JsonTextReader(istrm))
                {
                    if (await reader.ReadAsync(ct) && reader.TokenType == JsonToken.StartObject)
                    {
                        while (await reader.ReadAsync(ct) && reader.TokenType == JsonToken.PropertyName)
                        {
                            switch (reader.Value)
                            {
                                case nameof(JsonNetworkMessage.MessageId):
                                {
                                    hasMessageId = true;
                                    if (hasMessageId && hasMessages) return HeaderProfile.NetworkMessage;
                                    break;
                                }

                                case nameof(JsonNetworkMessage.Messages):
                                {
                                    hasMessages = true;
                                    if (hasMessageId && hasMessages) return HeaderProfile.NetworkMessage;
                                    break;
                                }

                                case nameof(JsonDataSetMessage.Payload):
                                {
                                    return HeaderProfile.DataSetMessage;
                                }

                                default:
                                {
                                    break;
                                }
                            }

                            await reader.SkipAsync(ct);
                        }
                    }
                }
            }

            return HeaderProfile.Minimal;
        }

        private async Task<ConfigurationVersionDataType> ReadConfigurationVersion(JsonReader reader, CancellationToken ct)
        {
            var version = new ConfigurationVersionDataType();

            if (await reader.ReadAsync(ct) && reader.TokenType == JsonToken.StartObject)
            {
                while (await reader.ReadAsync(ct) && reader.TokenType == JsonToken.PropertyName)
                {
                    switch (reader.Value)
                    {
                        case nameof(ConfigurationVersionDataType.MajorVersion):
                        {
                            version.MajorVersion = (uint)((await reader.ReadAsDecimalAsync(ct)) ?? 0);
                            break;
                        }

                        case nameof(ConfigurationVersionDataType.MinorVersion):
                        {
                            version.MinorVersion = (uint)((await reader.ReadAsDecimalAsync(ct)) ?? 0);
                            break;
                        }
                    }
                }
            }

            return version;
        }
    }
}
