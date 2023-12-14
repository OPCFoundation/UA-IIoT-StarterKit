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
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;
using Opc.Ua;
using UaMqttCommon;
using UaMqttPublisher;
using BrokerDataSetWriterTransportDataType = Opc.Ua.BrokerDataSetWriterTransportDataType;
using BrokerWriterGroupTransportDataType = Opc.Ua.BrokerWriterGroupTransportDataType;
using BuiltInType = Opc.Ua.BuiltInType;
using ConfigurationVersionDataType = Opc.Ua.ConfigurationVersionDataType;
using DataSetMetaDataType = Opc.Ua.DataSetMetaDataType;
using DataSetWriterDataType = Opc.Ua.DataSetWriterDataType;
using FieldMetaData = Opc.Ua.FieldMetaData;
using JsonWriterGroupMessageDataType = Opc.Ua.JsonWriterGroupMessageDataType;
using LocalizedText = Opc.Ua.LocalizedText;
using NodeId = Opc.Ua.NodeId;
using PubSubConnectionDataType = Opc.Ua.PubSubConnectionDataType;
using PubSubState = Opc.Ua.PubSubState;
using QualifiedName = Opc.Ua.QualifiedName;
using WriterGroupDataType = Opc.Ua.WriterGroupDataType;

await new Publisher().Connect();

internal class Publisher
{
    const string BrokerUrl = "broker.hivemq.com";
    const string TopicPrefix = "opcua";
    const string PublisherId = "(Quickstart004)";
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

    private MqttFactory m_factory;
    private IMqttClient m_client;
    private readonly object m_lock = new();
    private Dictionary<string, CachedValue> m_cache;
    private uint m_metadataVersion;
    private UAClient m_datasource;

    private IServiceMessageContext MessageContext => m_datasource.Session?.MessageContext ?? Opc.Ua.ServiceMessageContext.GlobalContext;

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
                Status = (int)PubSubState.Error
            };

            var json = JsonSerializer.Serialize(willPayload);

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

                m_cache = new()
                {
                    [nameof(EnergyMetrics.Consumption)] = new(m_lock)
                    {
                        Name = nameof(EnergyMetrics.Consumption),
                        Description = "The energy consumed by the device during the calculation period.",
                        NodeId = "nsu=http://opcfoundation.org/UA/Boiler/;i=1272",
                        StatusCode = Opc.Ua.StatusCodes.BadWaitingForInitialData,
                        Timestamp = DateTime.UtcNow,
                        Properties = new()
                        {
                            new CachedValue(m_lock) {
                                Name = Opc.Ua.BrowseNames.EngineeringUnits,
                                NodeId = "nsu=http://opcfoundation.org/Quickstarts/ReferenceServer;s=DataAccess_AnalogType_DataAccess_AnalogType_Double_EngineeringUnits",
                                StatusCode = Opc.Ua.StatusCodes.BadWaitingForInitialData,
                                Timestamp = DateTime.UtcNow
                            }
                        }
                    },
                    [nameof(EnergyMetrics.DutyCycle)] = new(m_lock)
                    {
                        Name = nameof(EnergyMetrics.DutyCycle),
                        Description = "The fraction of the calulation period where the device is consuming power.",
                        NodeId = "nsu=http://opcfoundation.org/UA/Boiler/;i=1242",
                        StatusCode = Opc.Ua.StatusCodes.BadWaitingForInitialData,
                        Timestamp = DateTime.UtcNow
                    },
                    [nameof(EnergyMetrics.CalculationPeriod)] = new(m_lock)
                    {
                        Name = nameof(EnergyMetrics.CalculationPeriod),
                        Description = "The period, in ms, over which power calculations are computed.",
                        NodeId = "nsu=http://opcfoundation.org/Quickstarts/ReferenceServer;s=DataAccess_AnalogType_Double",
                        StatusCode = Opc.Ua.StatusCodes.BadWaitingForInitialData,
                        Timestamp = DateTime.UtcNow
                    }
                };

                m_metadataVersion = GetVersionTime();

                m_datasource = await UAClient.Run(
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

            m_datasource?.Disconnect();
            await PublishStatus(PubSubState.Paused);
            var disconnectOptions = m_factory.CreateClientDisconnectOptionsBuilder().Build();
            await m_client.DisconnectAsync(disconnectOptions, CancellationToken.None);
            Console.WriteLine("Publisher Disconnected!");
        }
    }

    private static uint GetVersionTime()
    {
        return (uint)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
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
            Status = (int)state
        };

        var json = JsonSerializer.Serialize(payload);

        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(json)
            .WithMessageExpiryInterval(7200)
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

        var topic = new Topic()
        {
            TopicPrefix = TopicPrefix,
            MessageType = MessageTypes.DataSetMetaData,
            PublisherId = PublisherId,
            GroupName = GroupName,
            WriterName = WriterName
        }.Build();

        JsonEncoder encoder = new(MessageContext, true);

        encoder.WriteGuid("MessageId", Guid.NewGuid());
        encoder.WriteString("PublisherId", PublisherId);
        encoder.WriteUInt16("DataSetWriterId", DataSetWriterId);
        encoder.WriteEncodeable("MetaData", metadata, typeof(DataSetMetaDataType));

        var json = encoder.CloseAndReturnText();

        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(json)
            .WithMessageExpiryInterval(7200)
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
        }
    }

    private async Task PublishConnection()
    {
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

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
                                    }.Build()
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

        JsonEncoder encoder = new(MessageContext, true);

        encoder.WriteGuid("MessageId", Guid.NewGuid());
        encoder.WriteString("PublisherId", PublisherId);
        encoder.WriteDateTime("Timestamp", DateTime.UtcNow);
        encoder.WriteEncodeable("Connection", connection, typeof(PubSubConnectionDataType));

        var json = encoder.CloseAndReturnText();

        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(json)
            .WithMessageExpiryInterval(7200)
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
        }
    }

    private async Task Publish()
    {
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

        await PublishStatus(PubSubState.Operational);
        await PublishConnection();

        Console.WriteLine("Press enter to exit.");
        int count = 0;
        int lastDataCount = 0;

        while (true)
        {
            await PublishDataSetMetaData(count);

            JsonEncoder encoder = new(MessageContext, true);

            encoder.WriteUInt16("DataSetWriterId", DataSetWriterId);
            encoder.WriteString("PublisherId", PublisherId);
            encoder.WriteInt32("SequenceNumber", count + 1);
            encoder.WriteDateTime("Timestamp", DateTime.UtcNow);
            encoder.WriteUInt32("MinorVersion", m_metadataVersion);

            lock (m_lock)
            {
                var values = m_cache.Values.ToArray();

                if (count % KeyFrameCount == 0 || values.Where(x => x.IsDirty).Any())
                {
                    encoder.WriteString("MessageType", (count % KeyFrameCount == 0) ? "ua-keyframe" : "ua-deltaframe");
                    encoder.PushStructure("Payload");

                    foreach (var value in values)
                    {
                        if (count % KeyFrameCount == 0 || value.IsDirty)
                        {
                            encoder.WriteDataValue(value.Name, new Opc.Ua.DataValue()
                            {
                                WrappedValue = value.Value,
                                SourceTimestamp = value.Timestamp,
                                ServerTimestamp = DateTime.MinValue,
                                StatusCode = value.StatusCode
                            });

                            value.IsDirty = false;
                        }
                    }

                    encoder.PopStructure();
                    lastDataCount = count;
                }
                else
                {
                    encoder.WriteString("MessageType", "ua-keepalive");

                    if ((count - lastDataCount) % KeepAliveCount != 0)
                    {
                        count++;
                        continue;
                    }
                }
            }

            var json = encoder.CloseAndReturnText();

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
