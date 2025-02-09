using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Opc.Ua;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using UaPubSubCommon;

namespace UaSubscriber
{
    public class RemoteResponder : IRemotePublisher
    {
        private readonly object m_lock = new();
        private readonly string m_topicPrefix;
        private readonly ServiceMessageContext m_messageContext;
        private Dictionary<ushort, ActionWriter> m_writers = new();
        private Dictionary<string, bool> m_topics = new();
        private byte[] m_correlationData;
        private Dictionary<ushort, ActionRequest> m_requests = new();
        private ushort m_nextRequestId = 0;

        public RemoteResponder(ServiceMessageContext messageContext, string topicPrefix)
        {
            m_messageContext = messageContext;
            m_topicPrefix = topicPrefix;
            m_correlationData = Guid.NewGuid().ToByteArray();
        }

        public string PublisherId { get; set; }

        public string RequestorId { get; set; }

        public PubSubState PubSubState { get; set; }

        public PubSubConnectionDataType Connection { get; set; }

        public ApplicationDescription ApplicationDescription { get; set; }

        public Dictionary<ushort, ActionWriter> Writers => m_writers;

        public string ResponseTopic { get; set; }

        public byte[] CorrelationData => m_correlationData;

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
            ActionWriter[] writers;

            lock (m_lock)
            {
                writers = m_writers.Values.ToArray();
            }

            if (ResponseTopic == topic)
            {
                await ProcessResponseMessage(json, ct);
                return false;
            }

            foreach (var writer in writers)
            {
                if (writer.MetaDataTopic == topic)
                {
                    await ProcessMetaDataMessage(writer, json, ct);
                    return false;
                }
            }

            var pt = Topic.Parse(topic, m_topicPrefix);

            if (pt.MessageType == TopicTypes.ActionResponder)
            {
                await ProcessActionResponder(json, ct);
                return true;
            }

            return false;
        }

        private Task ProcessMetaDataMessage(ActionWriter writer, string json, CancellationToken ct)
        {
            JsonActionMetaDataMessage message = new();

            using (var decoder = new JsonDecoder(json, m_messageContext))
            {
                message.Decode(decoder);
            }

            if (String.IsNullOrEmpty(message.PublisherId) || message.PublisherId != PublisherId)
            {
                Log.Error($"[{PublisherId}] DataSetMetaData Message with invalid PublisherId: '{message.PublisherId}'.");
                return Task.CompletedTask;
            }

            Log.Info($"[{PublisherId}] DataSetMetaData{Environment.NewLine}   Name: '{message?.Request.Name}'{Environment.NewLine}   Description: '{message?.Request?.Description}'");

            writer.ConfigurationVersion = message.Request.ConfigurationVersion;
            writer.ActionTargets = message.ActionTargets;
            writer.Request = message.Request;
            writer.Response = message.Response;

            return Task.CompletedTask;
        }

        private async Task ProcessResponseMessage(string json, CancellationToken ct)
        {
            JsonActionNetworkMessage message = new();

            var root = JObject.Parse(json);

            if (root.TryGetValue(nameof(JsonActionNetworkMessage.Messages), out var messages))
            {
                root.Remove(nameof(JsonActionNetworkMessage.Messages));
            }

            using (var decoder = new JsonDecoder(json, m_messageContext))
            {
                message.Decode(decoder);
            }

            if (String.IsNullOrEmpty(message.RequestorId) || message.RequestorId != RequestorId)
            {
                Log.Error($"[{PublisherId}] ActionResponse Message with invalid RequestorId: '{message.RequestorId}'.");
                return;
            }

            var list = messages as JArray;

            if (list != null)
            {
                foreach (var item in list)
                {
                    var jobject = item as JObject;

                    if (jobject == null)
                    {
                        continue;
                    }

                    if (jobject.TryGetValue(nameof(JsonActionResponseMessage.Payload), out var payload))
                    {
                        jobject.Remove(nameof(JsonActionResponseMessage.Payload));
                    }

                    var response = new JsonActionResponseMessage();

                    using (var decoder = new JsonDecoder(jobject.ToString(), m_messageContext))
                    {
                        response.Decode(decoder);
                    }

                    if (!m_requests.TryGetValue(response.RequestId, out var request))
                    {
                        Log.Error($"[{PublisherId}] ActionResponse Message with invalid RequestId: {response.RequestId}");
                        continue;
                    }

                    Log.Info($"[{PublisherId}] ActionResponse: RequestId={response.RequestId} ActionState={response.ActionState}.");

                    List<PubSubField> fields = request.Writer.Response.Fields.Select(x => new PubSubField()
                    {
                        Name = x.Name,
                        BuiltInType = (BuiltInType)x.BuiltInType,
                        DataTypeId = x.DataType,                           
                        ValueRank = x.ValueRank    
                    }).ToList();

                    using (var reader = payload.CreateReader())
                    {
                        var outputArguments = await PubSubUtils.ReadFieldValues(m_messageContext, reader, fields, ct);

                        Log.System("Output Arguments:");

                        foreach (var argument in outputArguments)
                        {
                            var dv = argument.Value.Value;
                            StringBuilder sb = new();
                            sb.Append(StatusCode.IsBad(dv.StatusCode) ? dv.StatusCode : dv.WrappedValue);
                            Log.System($"   {argument.Key} [{dv.WrappedValue.TypeInfo.BuiltInType}]: {sb}");
                        }
                    }
                }
            }
        }

        private Task ProcessActionResponder(string json, CancellationToken ct)
        {
            JsonPubSubConnectionMessage message = new();

            using (var decoder = new JsonDecoder(json, m_messageContext))
            {
                message.Decode(decoder);
            }

            if (String.IsNullOrEmpty(message.MessageType) || message.PublisherId != PublisherId)
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
                ResponseTopic = new Topic()
                {
                    TopicPrefix = m_topicPrefix,
                    PublisherId = RequestorId,
                    MessageType = MessageTypes.ActionResponse
                }.Build();

                m_topics[ResponseTopic] = true;

                foreach (var group in Connection.WriterGroups)
                {
                    var groupSettings = group.TransportSettings.Body as BrokerWriterGroupTransportDataType;

                    foreach (var writer in group.DataSetWriters)
                    {
                        var writerSettings = writer.TransportSettings.Body as BrokerDataSetWriterTransportDataType;

                        if (!m_writers.TryGetValue(writer.DataSetWriterId, out var writer2))
                        {
                            m_writers[writer.DataSetWriterId] = writer2 = new ActionWriter()
                            {
                                Id = writer.DataSetWriterId
                            };
                        }

                        writer2.Name = writer.Name;
                        writer2.GroupName = group.Name;
                        writer2.DataSetName = writer.DataSetName;
                        writer2.DataTopic = writerSettings?.QueueName ?? groupSettings?.QueueName;
                        writer2.MetaDataTopic = writerSettings?.MetaDataQueueName;

                        m_topics[writer2.MetaDataTopic] = true;

                        sb.AppendLine($"   {writer2.GroupName}.{writer2.Name}");
                    }
                }
            }

            Log.Info($"[{PublisherId}] ActionResponder {sb.ToString()}");

            return Task.CompletedTask;
        }

        public Task<string> BuildActionRequest(
            RemoteResponder responder,
            ActionWriter writer,
            ActionTargetDataType target,
            List<Variant> inputArguments,
            CancellationToken ct)
        {
            ActionRequest request = new()
            {
                Writer = writer,
                Target = target,
                InputArguments = inputArguments,
                RequestId = ++m_nextRequestId
            };

            m_requests[request.RequestId] = request;

            using (var encoder = new JsonEncoder(m_messageContext, JsonEncodingType.Compact))
            {
                encoder.WriteString(nameof(JsonActionNetworkMessage.MessageId), Guid.NewGuid().ToString());
                encoder.WriteString(nameof(JsonActionNetworkMessage.MessageType), MessageTypes.ActionRequest);
                encoder.WriteString(nameof(JsonActionNetworkMessage.PublisherId), responder.PublisherId);
                encoder.WriteString(nameof(JsonActionNetworkMessage.RequestorId), RequestorId);
                encoder.WriteString(nameof(JsonActionNetworkMessage.ResponseAddress), responder.ResponseTopic);
                encoder.WriteByteString(nameof(JsonActionNetworkMessage.CorrelationData), responder.CorrelationData);
                encoder.WriteDouble(nameof(JsonActionNetworkMessage.TimeoutHint), 120000);

                encoder.PushArray(nameof(JsonActionNetworkMessage.Messages));
                encoder.PushStructure(null);

                encoder.WriteUInt16(nameof(JsonActionRequestMessage.DataSetWriterId), writer.Id);
                encoder.WriteUInt16(nameof(JsonActionRequestMessage.ActionTargetId), target.ActionTargetId);
                encoder.WriteDateTime(nameof(JsonActionRequestMessage.Timestamp), DateTime.UtcNow);
                encoder.WriteUInt16(nameof(JsonActionRequestMessage.RequestId), request.RequestId);
                encoder.WriteEnumerated(nameof(JsonActionRequestMessage.ActionState), ActionState.Executing);

                encoder.PushStructure(nameof(JsonActionRequestMessage.Payload));

                for (int ii = 0; ii < inputArguments.Count; ii++)
                {
                    encoder.WriteRawValue(
                        request.Writer.Request.Fields[ii],
                        new DataValue() { WrappedValue = inputArguments[ii] },
                        DataSetFieldContentMask.None);
                }

                encoder.PopStructure();

                encoder.PopStructure();
                encoder.PopArray();

                return Task.FromResult(encoder.CloseAndReturnText());
            }
        }
    }

    public class ActionWriter
    {
        public ushort Id;
        public string Name;
        public string GroupName;
        public string DataSetName;
        public string DataTopic;
        public string MetaDataTopic;
        public ConfigurationVersionDataType ConfigurationVersion;
        public DataSetMetaDataType Request;
        public DataSetMetaDataType Response;
        public List<ActionTargetDataType> ActionTargets;
    }

    public class ActionRequest
    {
        public ushort RequestId;
        public ActionWriter Writer;
        public ActionTargetDataType Target;
        public List<Variant> InputArguments;
    }
}
