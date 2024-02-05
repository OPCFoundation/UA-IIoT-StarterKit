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
using System.Text.Json.Serialization;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;
using Opc.Ua;
using UaMqttCommon;
using UaMqttPublisher.Server;

namespace UaMqttPublisher
{
    internal class Publisher : IDisposable
    {
        private string TopicPrefix => m_broker.TopicPrefix;
        private string PublisherId => m_connection.PublisherId;

        private MqttFactory m_mqttFactory;
        private IMqttClient m_mqttClient;
        private uint m_metadataVersion;
        private UAClient m_uaClient;
        private BrokerConfiguration m_broker;
        private ConnectionConfiguration m_connection;
        private bool m_disposed;
        private readonly Dictionary<string, SubscribedValue> m_cache = new();
        private readonly Queue<MqttApplicationMessage> m_messageQueue = new();
        private readonly CancellationTokenSource m_shutdown = new();
        private readonly HashSet<string> m_retainedTopics = new();

        private IServiceMessageContext MessageContext => m_uaClient?.Session?.MessageContext ?? ServiceMessageContext.GlobalContext;

        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                    m_mqttClient?.Dispose();
                    m_uaClient?.Dispose();
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

        private async Task LoadConfiguration(string configurationFile, string brokerName, string connectionName, string port)
        {
            if (configurationFile == null)
            {
                string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                configurationFile = Path.Combine(folder, "config", "uapublisher-config.json");
            }

            var json = File.ReadAllText(configurationFile);

            if (!String.IsNullOrEmpty(port))
            {
                json = json.Replace("48040", port);
            }

            using (var istrm = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var configuration = await JsonSerializer.DeserializeAsync<PublisherConfiguration>(istrm);

                m_broker = configuration?.Brokers?
                    .Where(x => brokerName == null || x.Name == brokerName)
                    .FirstOrDefault();

                if (m_broker == null)
                {
                    throw new InvalidDataException($"Broker '{brokerName}' configuration is missing.");
                }

                m_connection = configuration?.Connections?
                    .Where(x => connectionName == null || x.Name == connectionName)
                    .FirstOrDefault();

                if (m_connection == null)
                {
                    throw new InvalidDataException($"Connection '{connectionName}' configuration is missing.");
                }

                HashSet<ushort> assignedIds = new();

                foreach (var group in m_connection.Groups)
                {
                    foreach (var writer in group.Writers)
                    {
                        writer.WriterId = GetWriterId(assignedIds, writer);
                        writer.SequenceNumber = 1;
                    }
                };
            }
        }

        private async Task ConnectToBroker()
        {
            m_mqttClient = m_mqttFactory.CreateMqttClient();

            var willTopic = new Topic()
            {
                TopicPrefix = TopicPrefix,
                MessageType = MessageTypes.Status,
                PublisherId = PublisherId
            }.Build();

            JsonStatusMessage willPayload = new()
            {
                MessageId = Guid.NewGuid().ToString(),
                PublisherId = PublisherId,
                Status = PubSubState.Error,
                IsCyclic = false
            };

            var json = JsonSerializer.Serialize(willPayload, new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault });

            var url = new Uri("mqtt://" + m_broker.BrokerUrl);

            var options = new MqttClientOptionsBuilder()
                .WithProtocolVersion((m_broker.UseMqtt311 ?? false) ? MqttProtocolVersion.V311 : MqttProtocolVersion.V500)
                .WithTcpServer(url.Host, url.Port > 0 ? url.Port : null)
                .WithWillTopic(willTopic)
                .WithWillRetain(true)
                .WithWillDelayInterval(60)
                .WithWillPayload(json)
                .WithClientId($"{TopicPrefix}.{PublisherId}");

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

        public async Task Start(string configurationFile, string brokerName, string connectionName, string port)
        {
            m_mqttFactory = new MqttFactory();
            m_metadataVersion = GetVersionTime();

            Console.WriteLine("Starting OPC UA Server.");
            var server = new GPIO();
            await server.Start(false, port).ConfigureAwait(false);

            await LoadConfiguration(configurationFile, brokerName, connectionName, port);
            m_uaClient = await UAClient.Run(m_connection, m_cache);

            var task = Task.Run(() => ProcessQueuedMessages());

            await Publish();
            var statusTopic = await PublishStatus(PubSubState.Paused, sendNow: true);

            // clear out all retained messages except for the status message.
            foreach (var topic in m_retainedTopics)
            {
                if (statusTopic != topic)
                {
                    await SendMessage(topic, "", true, true);
                }
            }

            m_shutdown.Cancel();
            await task;

            m_uaClient?.Disconnect();

            if (m_mqttClient != null)
            {
                var disconnectOptions = m_mqttFactory.CreateClientDisconnectOptionsBuilder().Build();
                await m_mqttClient.DisconnectAsync(disconnectOptions, CancellationToken.None);
                Log.Info("Publisher Disconnected!");
            }

            Console.WriteLine("Stopping OPC UA Server.");
            await server.Stop().ConfigureAwait(false);
        }

        private async Task SendMessage(string topic, string data, bool retain = false, bool sendNow = false, int? qosForData = (int)BrokerTransportQualityOfService.AtLeastOnce)
        {
            var builder = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(data);

            if (retain)
            {
                if (!String.IsNullOrEmpty(topic))
                {
                    m_retainedTopics.Add(topic);
                }
                else
                {
                    m_retainedTopics.Remove(topic);
                }

                builder = builder
                    .WithRetainFlag(true)
                    .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce);
            }
            else
            {
                switch ((BrokerTransportQualityOfService?)qosForData)
                {
                    default:
                    case BrokerTransportQualityOfService.AtMostOnce:
                        builder = builder.WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce);
                        break;
                    case BrokerTransportQualityOfService.AtLeastOnce:
                        builder = builder.WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce);
                        break;
                    case BrokerTransportQualityOfService.ExactlyOnce:
                        builder = builder.WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce);
                        break;
                }
            }

            var message = builder.Build();

            if (sendNow && m_mqttClient?.IsConnected == true)
            {
                await m_mqttClient.PublishAsync(message, m_shutdown.Token);
                return;
            }

            lock (m_messageQueue)
            {
                m_messageQueue.Enqueue(message);
            }
        }

        private async Task ProcessQueuedMessages()
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
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Broker Connect Error: [{e.GetType().Name}] {e.Message}");
                        m_mqttClient.Dispose();
                        m_mqttClient = null;
                        continue;
                    }
                }

                MqttApplicationMessage message = null;

                lock (m_messageQueue)
                {
                    if (m_messageQueue.Count > 0)
                    {
                        message = m_messageQueue.Dequeue();
                    }
                }

                if (message != null)
                {
                    try
                    {
                        var result = await m_mqttClient.PublishAsync(message, m_shutdown.Token);

                        if (!result.IsSuccess)
                        {
                            Log.Error($"Error: {result.ReasonCode} {result.ReasonString}");
                        }
                        else
                        {
                            Log.Info("{0} bytes sent to '{1}'!", message.PayloadSegment.Count, message.Topic);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Publish Error: [{e.GetType().Name}] {e.Message}");
                    }
                }
            }
            while (!m_shutdown.Token.WaitHandle.WaitOne(250));
        }

        private static uint GetVersionTime()
        {
            return (uint)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        private async Task<string> PublishStatus(PubSubState state, bool sendNow = false)
        {
            var topic = new Topic()
            {
                TopicPrefix = TopicPrefix,
                MessageType = MessageTypes.Status,
                PublisherId = PublisherId
            }.Build();

            JsonStatusMessage payload = new()
            {
                MessageId = Guid.NewGuid().ToString(),
                PublisherId = PublisherId,
                Status = state,
                IsCyclic = false
            };

            var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault });

            await SendMessage(topic, json, true, sendNow);

            return topic;
        }

        private async Task PublishDataSetMetaData(
            GroupConfiguration group,
            WriterConfiguration writer,
            uint sequenceNumber)
        {
            FieldMetaDataCollection fields = new();
            bool updateVersion = false;

            foreach (var value in writer.Fields)
            {
                var field = new FieldMetaData()
                {
                    Name = value.Name,
                    BuiltInType = (byte)(value.BuiltInType ?? BuiltInType.Variant),
                    Description = new LocalizedText(value.Description),
                    ValueRank = value.ValueRank ?? ValueRanks.Scalar
                };

                var id = $"{group.Name}.{writer.Name}.{value.Name}";

                lock (m_cache)
                {
                    if (m_cache.TryGetValue(id, out var cachedValue))
                    {
                        if (cachedValue.Value.TypeInfo != null)
                        {
                            field.BuiltInType = (byte)cachedValue.Value.TypeInfo.BuiltInType;
                            field.ValueRank = cachedValue.Value.TypeInfo.ValueRank;
                        }
                    }
                }

                field.DataType = (value.DataType != null)
                    ? ExpandedNodeId.Parse(value.DataType, MessageContext.NamespaceUris)
                    : new NodeId(field.BuiltInType);

                if (value.Properties?.Count > 0)
                {
                    field.Properties = new();

                    foreach (var property in value.Properties)
                    {
                        var propertyValue = new Opc.Ua.KeyValuePair()
                        {
                            Key = new QualifiedName(property.Name)
                        };

                        id = $"{group.Name}.{writer.Name}.{value.Name}.{property.Name}";

                        lock (m_cache)
                        {
                            if (m_cache.TryGetValue(id, out var cachedValue))
                            {
                                propertyValue.Value = cachedValue.Value;

                                if (cachedValue.IsDirty)
                                {
                                    updateVersion = true;
                                    cachedValue.IsDirty = false;
                                }
                            }
                        }

                        field.Properties.Add(propertyValue);
                    }
                }

                fields.Add(field);
            }

            if (sequenceNumber % group.MetaDataPublishingCount != 1 && !updateVersion)
            {
                return;
            }

            var metadata = new DataSetMetaDataType()
            {
                Name = writer.DataSetName,
                Description = new LocalizedText("A set of energy consumption metrics for a device."),
                Fields = fields
            };

            if (updateVersion)
            {
                m_metadataVersion = GetVersionTime();
            }

            metadata.ConfigurationVersion = new ConfigurationVersionDataType()
            {
                MajorVersion = m_metadataVersion,
                MinorVersion = m_metadataVersion
            };

            var topic = (writer.TopicForMetaData != null)
                ? writer.TopicForMetaData
                : new Topic()
                {
                    TopicPrefix = TopicPrefix,
                    MessageType = MessageTypes.DataSetMetaData,
                    PublisherId = PublisherId,
                    GroupName = group.Name,
                    WriterName = writer.Name
                }.Build();

            JsonDataSetMetaDataMessage message = new()
            {
                MessageId = Guid.NewGuid().ToString(),
                PublisherId = PublisherId,
                DataSetWriterId = writer.WriterId,
                Timestamp = DateTime.UtcNow,
                MetaData = metadata
            };

            var json = message.Encode(MessageContext);

            await SendMessage(topic, json, true);
        }

        private JsonNetworkMessageContentMask GetNetworkMessageContentMask(GroupConfiguration group)
        {
            switch (group.HeaderProfile)
            {
                default:
                case HeaderProfiles.JsonMinimal:
                    return JsonNetworkMessageContentMask.SingleDataSetMessage;
                case HeaderProfiles.JsonDataSetMessage:
                    return (
                        JsonNetworkMessageContentMask.DataSetMessageHeader |
                        JsonNetworkMessageContentMask.SingleDataSetMessage
                    );
                case HeaderProfiles.JsonNetworkMessage:
                    return (
                        JsonNetworkMessageContentMask.NetworkMessageHeader |
                        JsonNetworkMessageContentMask.DataSetMessageHeader |
                        JsonNetworkMessageContentMask.SingleDataSetMessage |
                        JsonNetworkMessageContentMask.PublisherId
                    );
            }
        }

        private JsonDataSetMessageContentMask GetDataSetMessageContentMask(GroupConfiguration group, WriterConfiguration writer)
        {
            JsonDataSetMessageContentMask contentMask = 0;

            switch (group.HeaderProfile)
            {
                default:
                case HeaderProfiles.JsonMinimal:
                    contentMask = JsonDataSetMessageContentMask.None;
                    break;
                case HeaderProfiles.JsonDataSetMessage:
                case HeaderProfiles.JsonNetworkMessage:
                    contentMask = (
                        JsonDataSetMessageContentMask.DataSetWriterId |
                        JsonDataSetMessageContentMask.SequenceNumber |
                        JsonDataSetMessageContentMask.Timestamp |
                        JsonDataSetMessageContentMask.Status |
                        (JsonDataSetMessageContentMask)0x100 | // JsonDataSetMessageContentMask.PublisherId
                        (JsonDataSetMessageContentMask)0x400   // JsonDataSetMessageContentMask.MinorVersion
                    );
                    break;
            }

            if ((writer.KeyFrameCount ?? 1) != 1)
            {
                contentMask |= JsonDataSetMessageContentMask.MessageType;
            }

            return contentMask;
        }

        private DataSetFieldContentMask GetFieldContent(WriterConfiguration writer)
        {
            DataSetFieldContentMask fieldMasks = 0;

            switch (writer.FieldMasks ?? FieldMasks.Value)
            {
                case FieldMasks.Raw:
                    fieldMasks = DataSetFieldContentMask.RawData;
                    break;
                case FieldMasks.Value:
                    fieldMasks = DataSetFieldContentMask.None;
                    break;
                case FieldMasks.ValueStatus:
                    fieldMasks = DataSetFieldContentMask.StatusCode;
                    break;
                case FieldMasks.ValueStatusTimestamp:
                    fieldMasks = (DataSetFieldContentMask.StatusCode | DataSetFieldContentMask.SourceTimestamp);
                    break;
                case FieldMasks.All:
                    fieldMasks = (
                        DataSetFieldContentMask.StatusCode |
                        DataSetFieldContentMask.SourceTimestamp |
                        DataSetFieldContentMask.SourcePicoSeconds |
                        DataSetFieldContentMask.ServerTimestamp |
                        DataSetFieldContentMask.ServerPicoSeconds
                    );
                    break;
            }

            return fieldMasks;
        }

        private ushort GetWriterId(HashSet<ushort> assignedIds, WriterConfiguration writer)
        {
            ushort id = 101;

            if (writer.WriterId != null && writer.WriterId > 0)
            {
                id = writer.WriterId ?? 1;

                if (!assignedIds.Contains(id))
                {
                    assignedIds.Add(id);
                    return id;
                }

                Log.Warning($"Duplicate WriterId '{writer.WriterId}' reassigned.");
            }

            while (assignedIds.Contains(id))
            {
                id++;
            }

            assignedIds.Add(id);
            return id;
        }

        private async Task PublishConnection()
        {
            if (m_mqttClient == null || m_mqttFactory == null) throw new InvalidOperationException();

            var connection = new PubSubConnectionDataType()
            {
                Name = m_connection.Name,
                PublisherId = PublisherId,
                Enabled = true,
                WriterGroups = new()
            };

            HashSet<ushort> assignedIds = new();

            foreach (var group in m_connection.Groups)
            {
                var wg = new WriterGroupDataType()
                {
                    Name = group.Name,
                    PublishingInterval = group.PublishingInterval ?? 1000,
                    KeepAliveTime = (group.KeepAliveCount ?? 10) * (group.PublishingInterval ?? 1000),
                    HeaderLayoutUri = group.HeaderProfile,
                    Enabled = group.Enabled ?? true,
                    MessageSettings = new ExtensionObject(new JsonWriterGroupMessageDataType()
                    {
                        NetworkMessageContentMask = (uint)GetNetworkMessageContentMask(group)
                    }),
                    TransportSettings = new ExtensionObject(new BrokerWriterGroupTransportDataType()
                    {
                        QueueName = (group.TopicForData != null) ? group.TopicForData : null
                    }),
                    DataSetWriters = new()
                };

                connection.WriterGroups.Add(wg);

                foreach (var writer in group.Writers)
                {
                    var dataTopic = (writer.TopicForData != null)
                        ? writer.TopicForData
                        : new Topic()
                        {
                            TopicPrefix = TopicPrefix,
                            MessageType = MessageTypes.Data,
                            PublisherId = PublisherId,
                            GroupName = group.Name,
                            WriterName = writer.Name
                        }.Build();

                    var metaDataTopic = (writer.TopicForMetaData != null)
                        ? writer.TopicForMetaData
                        : new Topic()
                        {
                            TopicPrefix = TopicPrefix,
                            MessageType = MessageTypes.DataSetMetaData,
                            PublisherId = PublisherId,
                            GroupName = group.Name,
                            WriterName = writer.Name
                        }.Build();

                    var dsw = new DataSetWriterDataType()
                    {
                        Name = writer.Name,
                        DataSetWriterId = writer.WriterId ?? 1,
                        DataSetFieldContentMask = (uint)GetFieldContent(writer),
                        KeyFrameCount = writer.KeyFrameCount ?? 1,
                        Enabled = writer.Enabled ?? true,
                        DataSetName = writer.DataSetName ?? writer.Name,
                        MessageSettings = new ExtensionObject(new JsonDataSetWriterMessageDataType()
                        {
                            DataSetMessageContentMask = (uint)GetDataSetMessageContentMask(group, writer)
                        }),
                        TransportSettings = new ExtensionObject(new BrokerDataSetWriterTransportDataType()
                        {
                            QueueName = dataTopic,
                            MetaDataQueueName = metaDataTopic,
                            MetaDataUpdateTime = (double)(group.MetaDataPublishingCount ?? 1) * (group.PublishingInterval ?? 1000)
                        })
                    };

                    wg.DataSetWriters.Add(dsw);
                }
            };

            JsonPubSubConnectionMessage message = new()
            {
                MessageId = Guid.NewGuid().ToString(),
                PublisherId = PublisherId,
                Timestamp = DateTime.UtcNow,
                Connection = connection
            };

            var json = message.Encode(MessageContext);

            var topic = new Topic()
            {
                TopicPrefix = TopicPrefix,
                MessageType = MessageTypes.Connection,
                PublisherId = PublisherId
            }.Build();

            await SendMessage(topic, json, true);
        }

        private JsonNode EncodeValue(WriterConfiguration writer, DataValue dv)
        {
            string json = null;

            if (writer.FieldMasks == FieldMasks.Raw)
            {
                using (var encoder = new JsonEncoder(MessageContext, false))
                {
                    encoder.WriteVariantContents(dv.WrappedValue.Value, dv.WrappedValue.TypeInfo);
                    json = encoder.CloseAndReturnText()[1..^1];
                }
            }
            else if (writer.FieldMasks == FieldMasks.Value)
            {
                using (var encoder = new JsonEncoder(MessageContext, true))
                {
                    if (StatusCode.IsBad(dv.StatusCode))
                    {
                        encoder.WriteVariant(null, new Variant(dv.StatusCode));
                    }
                    else
                    {
                        encoder.WriteVariant(null, dv.WrappedValue);
                    }

                    json = encoder.CloseAndReturnText()[1..^1];
                }
            }
            else
            {
                using (var encoder = new JsonEncoder(MessageContext, true))
                {
                    if (writer.FieldMasks == FieldMasks.ValueStatusTimestamp)
                    {
                        encoder.WriteVariant(nameof(DataValue.Value), dv.WrappedValue);
                        encoder.WriteStatusCode(nameof(DataValue.StatusCode), dv.StatusCode);
                        encoder.WriteDateTime(nameof(DataValue.SourceTimestamp), dv.SourceTimestamp);
                    }
                    else if (writer.FieldMasks == FieldMasks.ValueStatus)
                    {
                        encoder.WriteVariant(nameof(DataValue.Value), dv.WrappedValue);
                        encoder.WriteStatusCode(nameof(DataValue.StatusCode), dv.StatusCode);
                    }
                    else
                    {
                        encoder.WriteDataValue(null, dv);
                    }

                    json = encoder.CloseAndReturnText();
                }
            }

            return JsonNode.Parse(json);
        }

        private async Task<JsonDataSetMessage> PublishData(
            GroupConfiguration group,
            WriterConfiguration writer,
            JsonNetworkMessageContentMask nmMask,
            JsonDataSetMessageContentMask dmMask)
        {
            uint sequenceNumber = (writer.SequenceNumber ?? 1);

            await PublishDataSetMetaData(group, writer, writer.MessageCount);

            JsonDataSetMessage message = new()
            {
                PublisherId = (dmMask & JsonDataSetMessageContentMask.DataSetWriterId) != 0 ? PublisherId : null,
                DataSetWriterId = (dmMask & JsonDataSetMessageContentMask.DataSetWriterId) != 0 ? writer.WriterId : null,
                Timestamp = (dmMask & JsonDataSetMessageContentMask.Timestamp) != 0 ? DateTime.UtcNow : null,
                SequenceNumber = (dmMask & JsonDataSetMessageContentMask.SequenceNumber) != 0 ? sequenceNumber : null,
                MinorVersion = (dmMask & (JsonDataSetMessageContentMask)0x400 /* MinorVersion */) != 0 ? m_metadataVersion : null,
                Payload = new(),
                ExcludeHeader = (nmMask & JsonNetworkMessageContentMask.DataSetMessageHeader) == 0
            };

            if ((nmMask & JsonNetworkMessageContentMask.PublisherId) != 0)
            {
                message.PublisherId = null;
            }

            if ((nmMask & (JsonNetworkMessageContentMask)0x020 /*WriterGroupName*/) != 0)
            {
                message.WriterGroupName = null;
            }

            if ((dmMask & JsonDataSetMessageContentMask.MetaDataVersion) != 0)
            {
                message.MetaDataVersion = new ConfigurationVersionDataType()
                {
                    MajorVersion = m_metadataVersion,
                    MinorVersion = m_metadataVersion
                };

                message.MinorVersion = null;
            }

            if ((dmMask & JsonDataSetMessageContentMask.MessageType) != 0)
            {
                message.MessageType = (writer.MessageCount % writer.KeyFrameCount == 0) ? DataSetMessageTypes.KeyFrame : DataSetMessageTypes.DeltaFrame;
            }

            foreach (var field in writer.Fields)
            {
                var dv = new DataValue()
                {
                    StatusCode = StatusCodes.BadNotFound,
                    ServerTimestamp = DateTime.MinValue,
                    SourceTimestamp = DateTime.MinValue
                };

                lock (m_cache)
                {
                    if (m_cache.TryGetValue($"{group.Name}.{writer.Name}.{field.Name}", out var value))
                    {
                        if (writer.MessageCount % writer.KeyFrameCount == 0 || value.IsDirty)
                        {
                            dv.WrappedValue = value.Value;
                            dv.SourceTimestamp = value.Timestamp;
                            dv.StatusCode = value.StatusCode;
                            value.IsDirty = false;

                            message.Payload[field.Name] = EncodeValue(writer, dv);
                        }
                    }
                }
            }

            if (message.Payload.Count == 0)
            {
                if (group.KeepAliveCount == null || (writer.MessageCount - sequenceNumber) % group.KeepAliveCount != 0)
                {
                    writer.MessageCount++;
                    return null;
                }

                if ((nmMask & JsonNetworkMessageContentMask.DataSetMessageHeader) == 0)
                {
                    writer.MessageCount++;
                    return null;
                }

                if ((dmMask & JsonDataSetMessageContentMask.MessageType) != 0)
                {
                    message.MessageType = DataSetMessageTypes.KeepAlive;
                }

                message.Payload = null;
            }
            else
            {
                writer.SequenceNumber = sequenceNumber + 1;
            }

            writer.MessageCount++;
            return message;
        }

        private async Task Publish()
        {
            await PublishStatus(PubSubState.Operational);
            await PublishConnection();

            Log.Control("Press enter to exit.");

            ulong counter = 0;

            while (true)
            {
                foreach (var group in m_connection.Groups)
                {
                    if (counter % (ulong)(group.PublishingInterval ?? 1000) != 0)
                    {
                        continue;
                    }

                    var nmMask = GetNetworkMessageContentMask(group);

                    var topic = (group.TopicForData != null)
                        ? group.TopicForData
                        : new Topic()
                        {
                            TopicPrefix = TopicPrefix,
                            MessageType = MessageTypes.Data,
                            PublisherId = PublisherId,
                            GroupName = group.Name
                        }.Build();

                    var nm = new JsonNetworkMessage()
                    {
                        MessageId = Guid.NewGuid().ToString(),
                        PublisherId = (nmMask & JsonNetworkMessageContentMask.PublisherId) != 0 ? PublisherId : null,
                        DataSetClassId = (nmMask & JsonNetworkMessageContentMask.DataSetClassId) != 0 ? group.DataSetClassId : null,
                        WriterGroupName = (nmMask & (JsonNetworkMessageContentMask)0x020 /*WriterGroupName*/) != 0 ? group.Name : null,
                        SingleMessage = (nmMask & JsonNetworkMessageContentMask.SingleDataSetMessage) != 0,
                        ExcludeHeader = (nmMask & JsonNetworkMessageContentMask.NetworkMessageHeader) == 0,
                        Messages = new()
                    };

                    foreach (var writer in group.Writers)
                    {
                        var dmMask = GetDataSetMessageContentMask(group, writer);
                        var dm = await PublishData(group, writer, nmMask, dmMask);

                        if (dm?.Payload?.Count > 0 || dm?.MessageType == DataSetMessageTypes.KeepAlive)
                        {
                            nm.Messages.Add(dm);

                            if (nm.SingleMessage)
                            {
                                topic = (writer.TopicForData != null)
                                    ? writer.TopicForData
                                    : new Topic()
                                    {
                                        TopicPrefix = TopicPrefix,
                                        MessageType = MessageTypes.Data,
                                        PublisherId = PublisherId,
                                        GroupName = group.Name,
                                        WriterName = writer.Name
                                    }.Build();

                                var json = nm.Encode(MessageContext);
                                await SendMessage(topic, json, qosForData: writer.QoSForData);
                            }
                        }
                    }
                }

                if (await Log.CheckForKeyPress(1000, () =>
                {
                    counter = (counter > UInt32.MaxValue) ? counter = 0 : counter + 1000;
                    return false;
                }))
                {
                    break;
                }
            }
        }
    }
}
