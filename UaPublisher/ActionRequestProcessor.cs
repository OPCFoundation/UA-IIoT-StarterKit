using MQTTnet;
using MQTTnet.Protocol;
using Newtonsoft.Json.Linq;
using Opc.Ua;
using UaPubSubCommon;

namespace UaPublisher
{
    internal class ActionRequestProcessor
    {
        private readonly ServiceMessageContext m_messageContext;
        private readonly ResponderConnection m_responder;
        private readonly IMqttClient m_client;

        public ActionRequestProcessor(ServiceMessageContext messageContext, ResponderConnection responder, IMqttClient client)
        {
            m_messageContext = messageContext;
            m_responder = responder;
            m_client = client;
        }
        public async Task ProcessRequest(string json, CancellationToken ct)
        {
            Opc.Ua.JsonActionNetworkMessage incoming = new();

            var root = JObject.Parse(json);

            if (root.TryGetValue(nameof(JsonActionNetworkMessage.Messages), out var messages))
            {
                root.Remove(nameof(JsonActionNetworkMessage.Messages));
            }

            using (var decoder = new JsonDecoder(root.ToString(), m_messageContext))
            {
                incoming.Decode(decoder);
            }

            List<JObject> responses = new();

            if (messages is JArray array)
            {
                foreach (var ii in messages)
                {
                    if (ii is JObject jobject)
                    {
                        if (jobject.TryGetValue(nameof(JsonActionRequestMessage.Payload), out var payload))
                        {
                            jobject.Remove(nameof(JsonActionRequestMessage.Payload));
                        }

                        JsonActionRequestMessage request = new();

                        using (var decoder = new JsonDecoder(jobject.ToString(), m_messageContext))
                        {
                            request.Decode(decoder);
                        }

                        var response = await ProcessRequest(request, payload as JObject, ct);
                        responses.Add(response);
                    }
                }
            }

            JsonActionNetworkMessage outgoing = new()
            {
                CorrelationData = incoming.CorrelationData,
                MessageId = Guid.NewGuid().ToString(),
                MessageType = MessageTypes.ActionResponse,
                PublisherId = m_responder.PublisherId,
                RequestorId = incoming.RequestorId,
                ResponseAddress = incoming.ResponseAddress,
                Timestamp = DateTime.UtcNow,
                Messages = responses.Select(x => new ExtensionObject(x)).ToArray()
            };

            using (var encoder = new JsonEncoder(m_messageContext, JsonEncodingType.Compact))
            {
                outgoing.Encode(encoder);

                await PublishResponse(
                    incoming.ResponseAddress,
                    encoder.CloseAndReturnText(), 
                    MqttQualityOfServiceLevel.AtLeastOnce, 
                    ct);
            }
        }

        private async Task PublishResponse(string topic, string message, MqttQualityOfServiceLevel qos, CancellationToken ct)
        {
            if (m_client == null) throw new InvalidOperationException();

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .WithRetainFlag(true)
                .WithContentType("application/json")
                .WithQualityOfServiceLevel(qos)
                .Build();

            var result = await m_client.PublishAsync(applicationMessage, ct);

            if (!result.IsSuccess)
            {
                Log.Error($"Error: {result.ReasonCode} {result.ReasonString}");
            }
            else
            {
                Log.Info($"Action Response sent to '{topic}'.");
            }
        }

        private JObject BuildResponse(
            JsonActionRequestMessage request,
            ActionSource source,
            ActionTarget target,
            StatusCode result,
            List<Variant> outputArguments = null)
        {
            using (var encoder = new JsonEncoder(m_messageContext, JsonEncodingType.Compact))
            {
                encoder.WriteUInt16(nameof(JsonActionResponseMessage.DataSetWriterId), request.DataSetWriterId);
                encoder.WriteUInt16(nameof(JsonActionResponseMessage.ActionTargetId), request.ActionTargetId);
                encoder.WriteUInt16(nameof(JsonActionResponseMessage.RequestId), request.RequestId);
                encoder.WriteEnumerated(nameof(JsonActionResponseMessage.ActionState), request.ActionState);
                encoder.WriteStatusCode(nameof(JsonActionResponseMessage.Status), result);

                if (outputArguments?.Count > 0)
                {
                    encoder.PushStructure(nameof(JsonActionResponseMessage.Payload));

                    for (int ii = 0; ii < source.Response.Fields.Count; ii++)
                    {
                        encoder.WriteRawValue(
                            source.Response.Fields[ii],
                            new DataValue() { WrappedValue = outputArguments[ii] },
                            DataSetFieldContentMask.None);
                    }

                    encoder.PopStructure();
                }

                return JObject.Parse(encoder.CloseAndReturnText());
            }
        }

        private async Task<JObject> ProcessRequest(
            JsonActionRequestMessage request, 
            JObject payload, 
            CancellationToken ct)
        {
            ActionWriter writer = null;
            ActionTarget target = null;

            try
            {
                writer = m_responder.FindWriter(request.DataSetWriterId);

                if (writer == null)
                {
                    throw new ServiceResultException(StatusCodes.BadNotFound);
                }

                target = writer.Source.Targets.Where(x => x.Id == request.ActionTargetId).FirstOrDefault();

                if (target == null)
                {
                    throw new ServiceResultException(StatusCodes.BadNotFound);
                }

                var fields = PubSubUtils.ReadFields(writer.Source.Request);
                List<Variant> inputArguments = new();

                if (payload != null)
                {
                    using (var reader = payload.CreateReader())
                    {
                        var map = await PubSubUtils.ReadFieldValues(m_messageContext, reader, fields, ct);

                        foreach (var jj in map.Values)
                        {
                            if (StatusCode.IsBad(jj.Value.StatusCode))
                            {
                                throw new ServiceResultException(StatusCodes.BadArgumentsMissing);
                            }

                            inputArguments.Add(jj.Value.WrappedValue);
                        }

                        if (map.Count > fields.Count)
                        {
                            throw new ServiceResultException(StatusCodes.BadTooManyArguments);
                        }

                        if (map.Count < fields.Count)
                        {
                            throw new ServiceResultException(StatusCodes.BadArgumentsMissing);
                        }
                    }
                }

                var outputArguments = target.Callback(target, inputArguments);

                var response = BuildResponse(
                    request,
                    writer.Source,
                    target,
                    StatusCodes.Good,
                    outputArguments);

                return response;
            }
            catch (Exception e)
            {
                var se = e as ServiceResultException;

                var response = BuildResponse(
                    request,
                    writer?.Source,
                    target,
                    (se != null) ? se.StatusCode : StatusCodes.BadUnexpectedError);

                return response;
            }
        }
    }
}