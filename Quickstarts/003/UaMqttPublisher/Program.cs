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
using System.Text.Json.Serialization;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;
using UaMqttCommon;
using UaMqttPublisher;

await new Publisher().Connect();

internal class Publisher
{
    const string BrokerUrl = "broker.hivemq.com";
    const string TopicPrefix = "opcua";
    const string PublisherId = "(Quickstart003)";
    const string GroupName = "Conveyor";
    const string Writer1Name = "Motor1";
    const string Writer2Name = "Motor2";
    const string DataSetName = "EnergyMetrics";
    const int DataSetWriter1Id = 101;
    const int DataSetWriter2Id = 201;

    private static readonly EUInformation[] UnitsOfPower = new EUInformation[]
    {
        new EUInformation()
        {
            NamespaceUri = "http://www.opcfoundation.org/UA/units/un/cefact",
            UnitId = 4937544,
            DisplayName = new LocalizedText() { Text = "kW·h" },
            Description = new LocalizedText() { Text = "kilowatt hour" }
        },
        new EUInformation()
        {
            NamespaceUri = "http://www.opcfoundation.org/UA/units/un/cefact",
            UnitId = 4928563,
            DisplayName = new LocalizedText() { Text = "electric hp" },
            Description = new LocalizedText() { Text = "horsepower (electric)" }
        }
    };

    private MqttFactory? m_factory;
    private IMqttClient? m_client;
    private readonly HashSet<string> m_retainedTopics = new();

    public async Task Connect()
    {
        m_factory = new MqttFactory();

        using (m_client = m_factory.CreateMqttClient())
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
                Status = (int)PubSubState.Error,
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

            var response = await m_client.ConnectAsync(options, CancellationToken.None);

            if (response.ResultCode != MqttClientConnectResultCode.Success)
            {
                Console.WriteLine($"Connect Failed: {response.ResultCode} {response.ResultCode} {response.ReasonString}");
            }
            else
            {
                Console.WriteLine("Publisher Connected!");
                await Publish();
            }

            await PublishStatus(PubSubState.Paused);
            await CleanupRetainedMessages();

            var disconnectOptions = m_factory.CreateClientDisconnectOptionsBuilder().Build();
            await m_client.DisconnectAsync(disconnectOptions, CancellationToken.None);
            Console.WriteLine("Publisher Disconnected!");
        }
    }

    private async Task CleanupRetainedMessages()
    {
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

        foreach (var topic in m_retainedTopics)
        {
            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload("")
                .WithRetainFlag(true)
                .Build();

            var result = await m_client.PublishAsync(applicationMessage, CancellationToken.None);

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

    private static int GetVersionTime()
    {
        return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
    }

    private async Task PublishStatus(PubSubState state)
    {
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

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
            Status = (int)state,
            IsCyclic = false
        };

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault });

        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(json)
            .WithRetainFlag(true)
            .Build();

        var result = await m_client.PublishAsync(applicationMessage, CancellationToken.None);

        if (!result.IsSuccess)
        {
            Console.WriteLine($"Error: {result.ReasonCode} {result.ReasonString}");
        }
        else
        {
            Console.WriteLine("Status Message Sent!");
        }
    }

    private async Task PublishDataSetMetaData(
        string writerName,
        int dataSetWriterId,
        EUInformation unitOfPower,
        ConfigurationVersionDataType version)
    {
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

        var metadata = new DataSetMetaDataType()
        {
            Name = DataSetName, // same name appears in the DataSetWriter.
            ConfigurationVersion = version,
            DataSetClassId = EnergyMetrics.DataSetClassId.ToString(), // Used to indicate that many DataSetWriters report the same data.
            Description = new LocalizedText() { Text = "A set of energy consumption metrics for a device." },
            Fields = new List<FieldMetaData>()
            {
                new FieldMetaData()
                {
                    Name = "Consumption",
                    BuiltInType = (int)BuiltInType.Double,
                    DataType = new NodeId((int)BuiltInType.Double),
                    Description = new LocalizedText() { Text = "The energy consumed by the device during the calculation period." },
                    ValueRank = -1,
                    Properties = new List<UaMqttCommon.KeyValuePair>()
                    {
                        new UaMqttCommon.KeyValuePair()
                        {
                            Key = new QualifiedName() { Name = "EngineeringUnits" },
                            Value = new Variant()
                            {
                                Type = (int)BuiltInType.ExtensionObject,
                                Body = new ExtensionObject<EUInformation>()
                                {
                                    TypeId = EUInformation.TypeId,
                                    Body = unitOfPower
                                }
                            }
                        }
                    }
                },
                new FieldMetaData()
                {
                    Name = "DutyCycle",
                    BuiltInType = (int)BuiltInType.Float,
                    DataType = new NodeId((int)BuiltInType.Float),
                    Description = new LocalizedText() { Text = "The fraction of the calulation period where the device is consuming power." },
                    ValueRank = -1,
                    Properties = new List<UaMqttCommon.KeyValuePair>()
                    {
                        new UaMqttCommon.KeyValuePair()
                        {
                            Key = new QualifiedName() { Name = "EngineeringUnits" },
                            Value = new Variant()
                            {
                                Type = (int)BuiltInType.ExtensionObject,
                                Body = new ExtensionObject<EUInformation>()
                                {
                                    TypeId = EUInformation.TypeId,
                                    Body = new EUInformation()
                                    {
                                        NamespaceUri = "http://www.opcfoundation.org/UA/units/un/cefact",
                                        UnitId = 20529,
                                        DisplayName = new LocalizedText() { Text = "%" },
                                        Description = new LocalizedText() { Text = "percent" }
                                    }
                                }
                            }
                        }
                    }
                },
                new FieldMetaData()
                {
                    Name = "CalculationPeriod",
                    BuiltInType = (int)BuiltInType.Double,
                    DataType = new NodeId((int)BuiltInType.Double),
                    Description = new LocalizedText() { Text = "The period, in ms, over which power calculations are computed." },
                    ValueRank = -1,
                    Properties = new List<UaMqttCommon.KeyValuePair>()
                    {
                        new UaMqttCommon.KeyValuePair()
                        {
                            Key = new QualifiedName() { Name = "EngineeringUnits" },
                            Value = new Variant()
                            {
                                Type = (int)BuiltInType.ExtensionObject,
                                Body = new ExtensionObject<EUInformation>()
                                {
                                    TypeId = EUInformation.TypeId,
                                    Body = new EUInformation()
                                    {
                                        NamespaceUri = "http://www.opcfoundation.org/UA/units/un/cefact",
                                        UnitId = 4403766,
                                        DisplayName = new LocalizedText() { Text = "ms" },
                                        Description = new LocalizedText() { Text = "millisecond" }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        var topic = new Topic()
        {
            TopicPrefix = TopicPrefix,
            MessageType = MessageTypes.DataSetMetaData,
            PublisherId = PublisherId,
            GroupName = GroupName,
            WriterName = writerName
        }.Build();

        var payload = new JsonDataSetMetaDataMessage()
        {
            MessageId = Guid.NewGuid().ToString(),
            MessageType = "ua-metadata",
            PublisherId = PublisherId,
            DataSetWriterId = dataSetWriterId,
            MetaData = metadata
        };

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault });

        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(json)
            .WithRetainFlag(true)
            .Build();

        var result = await m_client.PublishAsync(applicationMessage, CancellationToken.None);

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
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

        var connection = new PubSubConnectionDataType()
        {
            Name = PublisherId,
            PublisherId = new Variant() { Type = (int)BuiltInType.String, Body = PublisherId }, // Used in the metadata topic names
            Enabled = true,
            WriterGroups = new List<WriterGroupDataType>()
            {
                new WriterGroupDataType()
                {
                    Name = GroupName, // Used in the metadata topic names
                    HeaderLayoutUri = "http://opcfoundation.org/UA/PubSub-Layouts/JSON-NetworkMessage",
                    Enabled = true,
                    MessageSettings = new ExtensionObject<JsonWriterGroupMessageDataType>()
                    {
                        TypeId = JsonWriterGroupMessageDataType.TypeId,
                        Body = new JsonWriterGroupMessageDataType()
                        {
                             // NetworkMessageHeader | DataSetMessageHeader | PublisherId 
                            NetworkMessageContentMask = 0x01 | 0x02 | 0x08
                        }
                    },
                    TransportSettings = new ExtensionObject<BrokerWriterGroupTransportDataType>()
                    {
                        TypeId = BrokerWriterGroupTransportDataType.TypeId,
                        Body = new BrokerWriterGroupTransportDataType()
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
                    },
                    DataSetWriters = new List<DataSetWriterDataType>()
                    {
                        new DataSetWriterDataType()
                        {
                            Name = Writer1Name, // Used in the metadata topic names.
                            DataSetFieldContentMask = 0x20, // RawData (i.e. no timestamps or status and simplified encoding)
                            KeyFrameCount = 1, // Each message has all fields.
                            Enabled = true,
                            DataSetName = DataSetName,
                            DataSetWriterId = 101, // Unique across all Writers which are part of the Connection.
                            MessageSettings = new ExtensionObject<JsonDataSetWriterMessageDataType>()
                            {
                                TypeId = JsonDataSetWriterMessageDataType.TypeId,
                                Body = new JsonDataSetWriterMessageDataType()
                                {
                                     // DataSetWriterId | SequenceNumber | Timestamp | Status | MinorVersion
                                     DataSetMessageContentMask = 0x01 | 0x04 | 0x08 | 0x10 | 0x400
                                }
                            },
                            TransportSettings = new ExtensionObject<BrokerDataSetWriterTransportDataType>()
                            {
                                TypeId = BrokerDataSetWriterTransportDataType.TypeId,
                                Body = new BrokerDataSetWriterTransportDataType()
                                {
                                    // have to publish the MetaData topic name even if the standard topic is used
                                    // since the subscriber is expected to use this field to find the metadata. 
                                    MetaDataQueueName = new Topic()
                                    {
                                        TopicPrefix = TopicPrefix,
                                        MessageType = MessageTypes.DataSetMetaData,
                                        PublisherId = PublisherId,
                                        GroupName = GroupName,
                                        WriterName = Writer1Name
                                    }.Build()
                                }
                            }
                        },
                        new DataSetWriterDataType()
                        {
                            Name = Writer2Name, // Used in the metadata topic names.
                            DataSetFieldContentMask = 0x20, // RawData (i.e. no timestamps or status and simplified encoding)
                            KeyFrameCount = 1, // Each message has all fields.
                            Enabled = true,
                            DataSetName = DataSetName,
                            DataSetWriterId = 201, // Unique across all Writers which are part of the Connection.
                            MessageSettings = new ExtensionObject<JsonDataSetWriterMessageDataType>()
                            {
                                TypeId = JsonDataSetWriterMessageDataType.TypeId,
                                Body = new JsonDataSetWriterMessageDataType()
                                {
                                     // DataSetWriterId | SequenceNumber | Timestamp | Status | MinorVersion
                                     DataSetMessageContentMask = 0x01 | 0x04 | 0x08 | 0x10 | 0x400
                                }
                            },
                            TransportSettings = new ExtensionObject<BrokerDataSetWriterTransportDataType>()
                            {
                                TypeId = BrokerDataSetWriterTransportDataType.TypeId,
                                Body = new BrokerDataSetWriterTransportDataType()
                                {
                                    // have to publish the MetaData topic name even if the standard topic is used
                                    // since the subscriber is expected to use this field to find the metadata. 
                                    MetaDataQueueName = new Topic()
                                    {
                                        TopicPrefix = TopicPrefix,
                                        MessageType = MessageTypes.DataSetMetaData,
                                        PublisherId = PublisherId,
                                        GroupName = GroupName,
                                        WriterName = Writer2Name
                                    }.Build()
                                }
                            }
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

        JsonPubSubConnectionMessage payload = new()
        {
            MessageId = Guid.NewGuid().ToString(),
            MessageType = "ua-connection",
            PublisherId = PublisherId,
            Timestamp = DateTime.UtcNow,
            Connection = connection
        };

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault });

        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(json)
            .WithRetainFlag(true)
            .Build();

        var result = await m_client.PublishAsync(applicationMessage, CancellationToken.None);

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
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

        await PublishStatus(PubSubState.Operational);
        await PublishConnection();

        var random = new Random();
        int unitOfPower = 0;
        ConfigurationVersionDataType version = new();

        Console.WriteLine("Press enter to exit.");
        int count1 = 0;
        int count2 = 0;

        while (true)
        {
            // every 10th message, change the units of power.
            if (count1 % 10 == 0)
            {
                version.MajorVersion = version.MinorVersion = GetVersionTime();
                await PublishDataSetMetaData(Writer1Name, DataSetWriter1Id, UnitsOfPower[(unitOfPower++) % 2], version);
            }

            if (count2 % 10 == 0)
            {
                version.MajorVersion = version.MinorVersion = GetVersionTime();
                await PublishDataSetMetaData(Writer2Name, DataSetWriter2Id, UnitsOfPower[(unitOfPower++) % 2], version);
            }

            JsonDataSetMessage message1 = new()
            {
                DataSetWriterId = DataSetWriter1Id,
                SequenceNumber = count1 + 1,
                MinorVersion = version.MinorVersion,
                Timestamp = DateTime.UtcNow,
                Payload = new EnergyMetrics()
                {
                    CalculationPeriod = 3600,
                    Consumption = 42 + random.NextDouble(),
                    DutyCycle = random.NextDouble()
                }
            };

            JsonDataSetMessage message2 = new()
            {
                DataSetWriterId = DataSetWriter2Id,
                SequenceNumber = count2 + 1,
                MinorVersion = version.MinorVersion,
                Timestamp = DateTime.UtcNow,
                Payload = new EnergyMetrics()
                {
                    CalculationPeriod = 3600,
                    Consumption = 684 + random.NextDouble(),
                    DutyCycle = random.NextDouble()
                }
            };

            NetworkMessage nm = new()
            {
                MessageId = Guid.NewGuid().ToString(),
                PublisherId = PublisherId
            };

            // randomly send 1 or 2 messages to simulate real-world behavior.
            switch ((count1 + count2) % 3)
            {
                case 0: { nm.Messages = new List<JsonDataSetMessage>() { message1, message2 }; count1++; count2++; break; }
                case 1: { nm.Messages = new List<JsonDataSetMessage>() { message1 }; count1++; break; }
                case 2: { nm.Messages = new List<JsonDataSetMessage>() { message2 }; count2++; break; }
            }

            var json = JsonSerializer.Serialize(nm, new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault });

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

            var result = await m_client.PublishAsync(dataMessage, CancellationToken.None);

            if (!result.IsSuccess)
            {
                Console.WriteLine($"Error: {result.ReasonCode} {result.ReasonString}");
            }
            else
            {
                Console.WriteLine($"Network ({((List<JsonDataSetMessage>)nm.Messages!)?.Count}) Messages Sent!");
            }

            for (int ii = 0; ii < 50; ii++)
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
