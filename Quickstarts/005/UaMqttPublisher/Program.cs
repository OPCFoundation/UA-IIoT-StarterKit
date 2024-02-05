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
using System.Security.Authentication;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;
using Opc.Ua;
using UaMqttCommon;
using UaMqttPublisher;

await new Publisher().Connect();

internal class Publisher
{
    const string BrokerUrl = "broker.hivemq.com";
    const string TopicPrefix = "opcua";
    const string PublisherId = "(Quickstart005)";
    const string GroupName = "Conveyor";
    const string WriterName = "Motor";
    const string DataSetName = "EnergyMetrics";
    const int DataSetWriterId = 101;
    const string ServerUrl = "opc.tcp://localhost:62541/Quickstarts/ReferenceServer";
    const bool NoSecurity = true;
    const string UserName = "user1";
    const string Password = "password";
    const int PublishingInterval = 5000;
    const int MetaDataPublishingCount = 10;
    const int KeepAliveCount = 3;
    const int KeyFrameCount = 12;

    private MqttFactory m_mqttFactory;
    private IMqttClient m_mqttClient;
    private readonly object m_lock = new();
    private Dictionary<string, SubscribedValue> m_cache;
    private uint m_metadataVersion;
    private UAClient m_uaClient;
    private readonly HashSet<string> m_retainedTopics = new();

    private IServiceMessageContext MessageContext => m_uaClient?.Session?.MessageContext ?? ServiceMessageContext.GlobalContext;

    public async Task Connect()
    {
        m_mqttFactory = new MqttFactory();

        using (m_mqttClient = m_mqttFactory.CreateMqttClient())
        {
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

            var options = new MqttClientOptionsBuilder()
                .WithProtocolVersion(MqttProtocolVersion.V500)
                .WithTcpServer(BrokerUrl)
                .WithWillTopic(willTopic)
                .WithWillRetain(true)
                .WithWillDelayInterval(60)
                .WithWillPayload(json)
                .WithClientId($"{TopicPrefix}.{PublisherId}")
                .WithTlsOptions(
                    o =>
                    {
                        o.WithCertificateValidationHandler(e =>
                        {
                            Console.WriteLine($"Broker Certificate: '{e.Certificate.Subject}' {e.SslPolicyErrors}");
                            return true;
                        });

                        // The default value is determined by the OS. Set manually to force version.
                        o.WithSslProtocols(SslProtocols.Tls12);
                    })
                .Build();

            var response = await m_mqttClient.ConnectAsync(options, CancellationToken.None);

            if (response.ResultCode != MqttClientConnectResultCode.Success)
            {
                Console.WriteLine($"Connect Failed: {response.ResultCode} {response.ResultCode} {response.ReasonString}");
            }
            else
            {
                Console.WriteLine("Publisher Connected!");

                m_cache = new()
                {
                    [nameof(EnergyMetrics.Consumption)] = new(m_lock)
                    {
                        Name = nameof(EnergyMetrics.Consumption),
                        Description = "The energy consumed by the device during the calculation period.",
                        NodeId = "nsu=http://opcfoundation.org/UA/Boiler/;i=1272",
                        StatusCode = Opc.Ua.StatusCodes.BadWaitingForInitialData,
                        Properties = new()
                        {
                            new SubscribedValue(m_lock) {
                                Name = Opc.Ua.BrowseNames.EngineeringUnits,
                                NodeId = "nsu=http://opcfoundation.org/Quickstarts/ReferenceServer;s=DataAccess_AnalogType_DataAccess_AnalogType_Double_EngineeringUnits",
                                StatusCode = Opc.Ua.StatusCodes.BadWaitingForInitialData
                            }
                        }
                    },
                    [nameof(EnergyMetrics.DutyCycle)] = new(m_lock)
                    {
                        Name = nameof(EnergyMetrics.DutyCycle),
                        Description = "The fraction of the calulation period where the device is consuming power.",
                        NodeId = "nsu=http://opcfoundation.org/UA/Boiler/;i=1242",
                        StatusCode = Opc.Ua.StatusCodes.BadWaitingForInitialData
                    },
                    [nameof(EnergyMetrics.CalculationPeriod)] = new(m_lock)
                    {
                        Name = nameof(EnergyMetrics.CalculationPeriod),
                        Description = "The period, in ms, over which power calculations are computed.",
                        NodeId = "nsu=http://opcfoundation.org/Quickstarts/ReferenceServer;s=DataAccess_AnalogType_Double",
                        StatusCode = Opc.Ua.StatusCodes.BadWaitingForInitialData
                    }
                };

                m_metadataVersion = GetVersionTime();

                m_uaClient = await UAClient.Run(
                    new UAClientSettings()
                    {
                        ServerUrl = ServerUrl,
                        NoSecurity = NoSecurity,
                        UserName = UserName,
                        Password = Password,
                        AutoAccept = true
                    },
                    m_cache.Values
                );

                await Publish();
            }

            m_uaClient?.Disconnect();
            await PublishStatus(PubSubState.Paused);
            await CleanupRetainedMessages();

            var disconnectOptions = m_mqttFactory.CreateClientDisconnectOptionsBuilder().Build();
            await m_mqttClient.DisconnectAsync(disconnectOptions, CancellationToken.None);
            Console.WriteLine("Publisher Disconnected!");
        }
    }

    private async Task CleanupRetainedMessages()
    {
        if (m_mqttClient == null || m_mqttFactory == null) throw new InvalidOperationException();

        foreach (var topic in m_retainedTopics)
        {
            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload("")
                .WithRetainFlag(true)
                .Build();

            var result = await m_mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

            if (!result.IsSuccess)
            {
                Console.WriteLine($"Error: {result.ReasonCode} {result.ReasonString}");
            }
            else
            {
                Console.WriteLine($"Retained Message Removed from '{topic}'.");
            }
        }
    }

    private static uint GetVersionTime()
    {
        return (uint)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
    }

    private async Task PublishStatus(PubSubState state)
    {
        if (m_mqttClient == null || m_mqttFactory == null) throw new InvalidOperationException();

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

        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(json)
            .WithRetainFlag(true)
            .Build();

        var result = await m_mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

        if (!result.IsSuccess)
        {
            Console.WriteLine($"Error: {result.ReasonCode} {result.ReasonString}");
        }
        else
        {
            Console.WriteLine("Status Message Sent!");
        }
    }

    private async Task PublishDataSetMetaData(int count)
    {
        FieldMetaDataCollection fields = new();
        bool updateVersion = false;

        lock (m_lock)
        {
            foreach (var value in m_cache.Values)
            {
                var field = new FieldMetaData()
                {
                    Name = value.Name,
                    BuiltInType = (int)BuiltInType.Double,
                    DataType = new NodeId((int)BuiltInType.Double),
                    Description = new LocalizedText(value.Description),
                    ValueRank = -1,
                };

                if (value.Properties?.Count > 0)
                {
                    field.Properties = new();

                    foreach (var property in value.Properties)
                    {
                        field.Properties.Add(new Opc.Ua.KeyValuePair()
                        {
                            Key = new QualifiedName(property.Name),
                            Value = property.Value
                        });

                        if (property.IsDirty)
                        {
                            updateVersion = true;
                            property.IsDirty = false;
                        }
                    }
                }

                fields.Add(field);
            }
        }

        if (count % MetaDataPublishingCount != 0 && !updateVersion)
        {
            return;
        }

        var metadata = new DataSetMetaDataType()
        {
            Name = DataSetName, // same name appears in the DataSetWriter.
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

        JsonDataSetMetaDataMessage message = new()
        {
            MessageId = Guid.NewGuid().ToString(),
            PublisherId = PublisherId,
            DataSetWriterId = DataSetWriterId,
            Timestamp = DateTime.UtcNow,
            MetaData = metadata
        };

        var json = message.Encode(MessageContext);

        var topic = new Topic()
        {
            TopicPrefix = TopicPrefix,
            MessageType = MessageTypes.DataSetMetaData,
            PublisherId = PublisherId,
            GroupName = GroupName,
            WriterName = WriterName
        }.Build();

        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(json)
            .WithRetainFlag(true)
            .Build();

        var result = await m_mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

        if (!result.IsSuccess)
        {
            Console.WriteLine($"Error: {result.ReasonCode} {result.ReasonString}");
        }
        else
        {
            Console.WriteLine("DataSetMetaData Message Sent.");
            m_retainedTopics.Add(topic);
        }
    }

    private async Task PublishConnection()
    {
        if (m_mqttClient == null || m_mqttFactory == null) throw new InvalidOperationException();

        var connection = new PubSubConnectionDataType()
        {
            Name = PublisherId,
            PublisherId = PublisherId, // Used in the metadata topic names
            Enabled = true,
            WriterGroups = new()
            {
                new WriterGroupDataType()
                {
                    Name = GroupName, // Used in the metadata topic names
                    PublishingInterval = PublishingInterval,
                    KeepAliveTime = KeepAliveCount*KeepAliveCount,
                    HeaderLayoutUri = "http://opcfoundation.org/UA/PubSub-Layouts/JSON-DataSetMessage",
                    Enabled = true,
                    MessageSettings = new ExtensionObject(
                        new JsonWriterGroupMessageDataType()
                        {
                             // DataSetMessageHeader | SingleDataSetMessage 
                            NetworkMessageContentMask = 0x02 | 0x04 |0x08
                        }
                    ),
                    TransportSettings = new ExtensionObject(
                        new BrokerWriterGroupTransportDataType()
                        {
                            // have to publish the Data topic name even if the standard topic is used
                            // since the subscriber is expected to use this field to find the data.
                            // This value may be overridden at the DataSetWriter level.
                            QueueName = new Topic()
                            {
                                TopicPrefix = TopicPrefix,
                                MessageType = MessageTypes.Data,
                                PublisherId = PublisherId,
                                GroupName = GroupName
                            }.Build()
                        }
                    ),
                    DataSetWriters = new()
                    {
                        new DataSetWriterDataType()
                        {
                            Name = WriterName, // Used in the metadata topic names.
                            DataSetFieldContentMask = 0x01 | 0x02, // StatusCode | SourceTimestamp
                            KeyFrameCount = KeyFrameCount,
                            Enabled = true,
                            DataSetName = DataSetName,
                            DataSetWriterId = 101, // Unique across all Writers which are part of the Connection.
                            MessageSettings = new ExtensionObject(
                                new Opc.Ua.JsonDataSetWriterMessageDataType()
                                {
                                     // DataSetWriterId | SequenceNumber | Timestamp | Status | PublisherId | MinorVersion   
                                     DataSetMessageContentMask = 0x01 | 0x04 | 0x08 | 0x10 | 0x100 | 0x400
                                }
                            ),
                            TransportSettings = new ExtensionObject(
                                new BrokerDataSetWriterTransportDataType()
                                {
                                    // have to publish the MetaData topic name even if the standard topic is used
                                    // since the subscriber is expected to use this field to find the metadata. 
                                    MetaDataQueueName = new Topic()
                                    {
                                        TopicPrefix = TopicPrefix,
                                        MessageType = MessageTypes.DataSetMetaData,
                                        PublisherId = PublisherId,
                                        GroupName = GroupName,
                                        WriterName = WriterName
                                    }.Build(),
                                    MetaDataUpdateTime = MetaDataPublishingCount*PublishingInterval
                                }
                            )
                        }
                    }
                }
            }
        };

        var topic = new Topic()
        {
            TopicPrefix = TopicPrefix,
            MessageType = MessageTypes.Connection,
            PublisherId = PublisherId
        }.Build();

        JsonPubSubConnectionMessage message = new()
        {
            MessageId = Guid.NewGuid().ToString(),
            PublisherId = PublisherId,
            Timestamp = DateTime.UtcNow,
            Connection = connection
        };

        var json = message.Encode(MessageContext);

        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(json)
            .WithRetainFlag(true)
            .Build();

        var result = await m_mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

        if (!result.IsSuccess)
        {
            Console.WriteLine($"Error: {result.ReasonCode} {result.ReasonString}");
        }
        else
        {
            Console.WriteLine("Connection Message Sent!");
            m_retainedTopics.Add(topic);
        }
    }

    private async Task Publish()
    {
        if (m_mqttClient == null || m_mqttFactory == null) throw new InvalidOperationException();

        await PublishStatus(PubSubState.Operational);
        await PublishConnection();

        Console.WriteLine("Press enter to exit.");
        int count = 0;
        int lastDataCount = 0;

        while (true)
        {
            await PublishDataSetMetaData(count);

            JsonDataSetMessage message = new()
            {
                PublisherId = PublisherId,
                DataSetWriterId = DataSetWriterId,
                Timestamp = DateTime.UtcNow,
                SequenceNumber = (uint)(count + 1),
                MinorVersion = m_metadataVersion,
                MessageType = (count % KeyFrameCount == 0) ? "ua-keyframe" : "ua-deltaframe",
                Payload = new()
            };

            lock (m_lock)
            {
                var values = m_cache.Values.ToArray();

                if (!values.Where(v => v.IsDirty).Any())
                {
                    if ((count - lastDataCount) % KeepAliveCount != 0)
                    {
                        count++;
                        continue;
                    }

                    message.MessageType = "ua-keepalive";
                }
                else
                {
                    foreach (var value in values)
                    {
                        if (count % KeyFrameCount == 0 || value.IsDirty)
                        {
                            using (var encoder = new JsonEncoder(MessageContext, true))
                            {
                                encoder.WriteDataValue(null, new DataValue()
                                {
                                    WrappedValue = value.Value,
                                    SourceTimestamp = value.Timestamp,
                                    ServerTimestamp = DateTime.MinValue,
                                    StatusCode = value.StatusCode
                                });

                                message.Payload[value.Name] = JsonNode.Parse(encoder.CloseAndReturnText());
                            }

                            value.IsDirty = false;
                        }
                    }

                    lastDataCount++;
                }
            }

            var json = message.Encode(MessageContext);

            var topic = new Topic()
            {
                TopicPrefix = TopicPrefix,
                MessageType = MessageTypes.Data,
                PublisherId = PublisherId,
                GroupName = GroupName
            }.Build();

            var dataMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(json)
                .Build();

            var result = await m_mqttClient.PublishAsync(dataMessage, CancellationToken.None);

            if (!result.IsSuccess)
            {
                Console.WriteLine($"Error: {result.ReasonCode} {result.ReasonString}");
            }
            else
            {
                Console.WriteLine($"DataSetMessage Sent!");
                count++;
            }

            for (int ii = 0; ii < PublishingInterval / 100; ii++)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(false);

                    if (key.Key == ConsoleKey.Enter)
                    {
                        return;
                    }
                }

                await Task.Delay(100);
            }
        }
    }
}
