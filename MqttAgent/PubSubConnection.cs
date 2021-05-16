using Opc.Ua;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace MqttAgent
{
    public class PubSubConnection
    {
        public static string Encode()
        {
            DataSetMetaDataType dataset = new DataSetMetaDataType()
            {
                Name = "Test1",
                Description = "This is a test dataset",
                DataSetClassId = (Uuid)Guid.NewGuid(),
                ConfigurationVersion = new ConfigurationVersionDataType()
                {
                    MajorVersion = 1,
                    MinorVersion = 0
                },
                Fields = new FieldMetaData[]
                {
                    new FieldMetaData()
                    {
                        Name = "Sensor",
                        Description = "A sensor",
                        DataType = DataTypeIds.Double,
                        ValueRank = ValueRanks.Scalar,
                        BuiltInType = (byte)BuiltInType.Double,
                        DataSetFieldId= (Uuid)Guid.NewGuid()
                    },

                    new FieldMetaData()
                    {
                        Name = "Switch",
                        Description = "A switch",
                        DataType = DataTypeIds.Boolean,
                        ValueRank = ValueRanks.Scalar,
                        BuiltInType = (byte)BuiltInType.Boolean,
                        DataSetFieldId = (Uuid)Guid.NewGuid()
                    }
                }
            };

            using (StreamWriter writer = new StreamWriter("dataset.json"))
            {
                using (JsonEncoder encoder = new JsonEncoder(ServiceMessageContext.GlobalContext, true, writer, false))
                {
                    dataset.Encode(encoder);
                    return encoder.CloseAndReturnText();
                }
            }
        }

        public static PubSubConnectionDataType Decode(string fileName)
        {
            string json = File.ReadAllText(fileName);

            PubSubConnectionDataType configuration = new PubSubConnectionDataType();

            using (var decoder = new JsonDecoder(json, ServiceMessageContext.GlobalContext))
            {
                configuration.Decode(decoder);
            }

            return configuration;
        }

        public static PubSubConnectionDataType Encode(string fileName, PubSubConnectionDataType configuration)
        {
            using (var ostrm = File.Open(fileName, FileMode.Create))
            {
                using (var writer = new StreamWriter(ostrm, new UTF8Encoding(false)))
                {
                    using (var encoder = new JsonEncoder(ServiceMessageContext.GlobalContext, true, writer))
                    {
                        configuration.Encode(encoder);
                        encoder.Close();
                    }
                    writer.Close();
                }
            }

            return configuration;
        }

        public static PubSubConnectionDataType Create()
        {
            PubSubConnectionDataType connection = new PubSubConnectionDataType();

            connection.Address = new ExtensionObject(DataTypeIds.NetworkAddressUrlDataType, new NetworkAddressUrlDataType()
            {
                Url = "mqtt://localhost"
            });

            DataSetMetaDataType metadata = new DataSetMetaDataType()
            {
                Name = "boiler01",
                ConfigurationVersion = new ConfigurationVersionDataType()
                {
                    MajorVersion = 1,
                    MinorVersion = 1
                },
                DataSetClassId = new Uuid(Guid.NewGuid()),
                Description = "A boiler control loop.",
                Fields = new FieldMetaDataCollection()
                {
                    new FieldMetaData()
                    {
                        Name = "Temperature",
                        BuiltInType = (byte)BuiltInType.Double,
                        DataSetFieldId = new Uuid(Guid.NewGuid())
                    },
                    new FieldMetaData()
                    {
                        Name = "Setpoint",
                        BuiltInType = (byte)BuiltInType.Double,
                        DataSetFieldId = new Uuid(Guid.NewGuid())
                    }
                }
            };

            connection.Name = "Local Broker";
            connection.Enabled = true;
            connection.PublisherId = $"urn:{Dns.GetHostName()}:opcfoundation:iot-toolkit:publisher";
            connection.TransportProfileUri = "http://opcfoundation.org/UA-Profile/Transport/pubsub-mqtt-json";
            connection.WriterGroups = new WriterGroupDataTypeCollection()
            {
                new WriterGroupDataType()
                {
                    Name = "MinimalGroup",
                    WriterGroupId = 100,
                    Enabled = true,
                    MaxNetworkMessageSize = 10000,
                    Priority = 1,
                    SecurityGroupId  = "1",
                    PublishingInterval = 1000,
                    SecurityMode = MessageSecurityMode.None,
                    MessageSettings = new ExtensionObject(DataTypeIds.JsonWriterGroupMessageDataType, new JsonWriterGroupMessageDataType()
                    {
                        NetworkMessageContentMask = (uint)(
                            JsonNetworkMessageContentMask.DataSetMessageHeader |
                            JsonNetworkMessageContentMask.SingleDataSetMessage
                        )
                    }),
                    TransportSettings = new ExtensionObject(DataTypeIds.BrokerWriterGroupTransportDataType, new BrokerWriterGroupTransportDataType()
                    {
                        RequestedDeliveryGuarantee = BrokerTransportQualityOfService.AtLeastOnce
                    }),
                    DataSetWriters = new DataSetWriterDataTypeCollection()
                    {
                         new DataSetWriterDataType()
                         {
                            Name = "boiler01.minimal",
                            DataSetName = metadata.Name,
                            Enabled = true,
                            DataSetFieldContentMask = (uint)DataSetFieldContentMask.RawData,
                            DataSetWriterId = 1,
                            TransportSettings = new ExtensionObject(DataTypeIds.BrokerDataSetWriterTransportDataType, new BrokerDataSetWriterTransportDataType()
                            {
                                QueueName = "device01/boiler01",
                                RequestedDeliveryGuarantee = BrokerTransportQualityOfService.ExactlyOnce
                            }),
                            MessageSettings = new ExtensionObject(DataTypeIds.JsonDataSetWriterMessageDataType, new JsonDataSetWriterMessageDataType()
                            {
                                DataSetMessageContentMask = (uint)(
                                    JsonDataSetMessageContentMask.Status |
                                    JsonDataSetMessageContentMask.Timestamp
                                )
                            })
                         }
                     }
                },
                new WriterGroupDataType()
                {
                    Name = "FullGroup",
                    WriterGroupId = 200,
                    Enabled = true,
                    MaxNetworkMessageSize = 10000,
                    Priority = 1,
                    SecurityGroupId  = "1",
                    PublishingInterval = 1000,
                    SecurityMode = MessageSecurityMode.None,
                    MessageSettings = new ExtensionObject(DataTypeIds.JsonWriterGroupMessageDataType, new JsonWriterGroupMessageDataType()
                    {
                        NetworkMessageContentMask = (uint)(
                            JsonNetworkMessageContentMask.PublisherId |
                            JsonNetworkMessageContentMask.NetworkMessageHeader |
                            JsonNetworkMessageContentMask.DataSetMessageHeader |
                            JsonNetworkMessageContentMask.DataSetClassId |
                            JsonNetworkMessageContentMask.SingleDataSetMessage
                        )
                    }),
                    TransportSettings = new ExtensionObject(DataTypeIds.BrokerWriterGroupTransportDataType, new BrokerWriterGroupTransportDataType()
                    {
                        RequestedDeliveryGuarantee = BrokerTransportQualityOfService.ExactlyOnce
                    }),
                    DataSetWriters = new DataSetWriterDataTypeCollection()
                    {
                         new DataSetWriterDataType()
                         {
                            Name = "boiler01.full",
                            DataSetName = metadata.Name,
                            Enabled = true,
                            DataSetWriterId = 1,
                            DataSetFieldContentMask = (uint)(DataSetFieldContentMask.SourceTimestamp | DataSetFieldContentMask.StatusCode),
                            TransportSettings = new ExtensionObject(DataTypeIds.BrokerDataSetWriterTransportDataType, new BrokerDataSetWriterTransportDataType()
                            {
                                QueueName = "device01/boiler01",
                                RequestedDeliveryGuarantee = BrokerTransportQualityOfService.ExactlyOnce,
                                MetaDataQueueName = "device01/boiler01/metadata"
                            }),
                            MessageSettings = new ExtensionObject(DataTypeIds.JsonDataSetWriterMessageDataType, new JsonDataSetWriterMessageDataType()
                            {
                                DataSetMessageContentMask = (uint)(
                                    JsonDataSetMessageContentMask.SequenceNumber |
                                    JsonDataSetMessageContentMask.DataSetWriterId |
                                    JsonDataSetMessageContentMask.MetaDataVersion
                                )
                            })
                         },
                     }
                }
            };
            connection.ReaderGroups = new ReaderGroupDataTypeCollection()
            {
                new ReaderGroupDataType()
                {
                    Name = "MinimalGroup",
                    Enabled = true,
                    MaxNetworkMessageSize = 10000,
                    SecurityGroupId  = "1",
                    SecurityMode = MessageSecurityMode.None,
                    DataSetReaders = new DataSetReaderDataTypeCollection()
                    {
                        new DataSetReaderDataType()
                        {
                            Name = "boiler01.minimal",
                            Enabled = true,
                            PublisherId = connection.PublisherId,
                            WriterGroupId = 100,
                            DataSetWriterId = 1,
                            DataSetMetaData = metadata,
                            DataSetFieldContentMask = (uint)DataSetFieldContentMask.RawData,
                            MessageReceiveTimeout = 60000,
                            TransportSettings = new ExtensionObject(DataTypeIds.BrokerDataSetReaderTransportDataType, new BrokerDataSetReaderTransportDataType()
                            {
                                QueueName = "device01/boiler01",
                                RequestedDeliveryGuarantee = BrokerTransportQualityOfService.ExactlyOnce
                            }),
                            MessageSettings = new ExtensionObject(DataTypeIds.JsonDataSetReaderMessageDataType, new JsonDataSetReaderMessageDataType()
                            {
                                NetworkMessageContentMask = (uint)(
                                    JsonNetworkMessageContentMask.DataSetMessageHeader |
                                    JsonNetworkMessageContentMask.SingleDataSetMessage
                                ),
                                DataSetMessageContentMask = (uint)(
                                    JsonDataSetMessageContentMask.Status |
                                    JsonDataSetMessageContentMask.Timestamp
                                )
                            })
                        }
                    }
                },

                new ReaderGroupDataType()
                {
                    Name = "FullGroup",
                    Enabled = true,
                    MaxNetworkMessageSize = 10000,
                    SecurityGroupId  = "1",
                    SecurityMode = MessageSecurityMode.None,
                    DataSetReaders = new DataSetReaderDataTypeCollection()
                    {
                        new DataSetReaderDataType()
                        {
                            Name = "boiler01.full",
                            Enabled = true,
                            PublisherId = connection.PublisherId,
                            WriterGroupId = 200,
                            DataSetWriterId = 1,
                            DataSetMetaData = metadata,
                            DataSetFieldContentMask = (uint)(DataSetFieldContentMask.StatusCode | DataSetFieldContentMask.SourceTimestamp),
                            MessageReceiveTimeout = 60000,
                            TransportSettings = new ExtensionObject(DataTypeIds.BrokerDataSetReaderTransportDataType, new BrokerDataSetReaderTransportDataType()
                            {
                                QueueName = "device01/boiler01/full",
                                RequestedDeliveryGuarantee = BrokerTransportQualityOfService.ExactlyOnce,
                                MetaDataQueueName = "device01/boiler01/full/metadata"
                            }),
                            MessageSettings = new ExtensionObject(DataTypeIds.JsonDataSetReaderMessageDataType, new JsonDataSetReaderMessageDataType()
                            {
                                NetworkMessageContentMask = (uint)(
                                    JsonNetworkMessageContentMask.PublisherId |
                                    JsonNetworkMessageContentMask.NetworkMessageHeader |
                                    JsonNetworkMessageContentMask.DataSetMessageHeader |
                                    JsonNetworkMessageContentMask.DataSetClassId|
                                    JsonNetworkMessageContentMask.SingleDataSetMessage
                                ),
                                DataSetMessageContentMask = (uint)(
                                    JsonDataSetMessageContentMask.SequenceNumber |
                                    JsonDataSetMessageContentMask.DataSetWriterId |
                                    JsonDataSetMessageContentMask.MetaDataVersion
                                )
                            })
                        }
                    }
                }
            };

            return connection;
        }
        /*
        static async Task Run()
        {
            MqttClient client = new MqttClient();

            await client.StartAsync(e =>
            {
                Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();
            });

            PubSubConnectionDataType connection;
            connection = Create();
            Encode("localhost-connection.json", connection);
            connection = Decode("localhost-connection.json");

            await client.Subscribe(connection, "MinimalGroup", "boiler01.minimal");
            await client.Subscribe(connection, "FullGroup", "boiler01.full");

            int count = 1;

            while (true)
            {
                await Task.Delay(1000);

                var values = new List<DataValue>();
                values.Add(new DataValue(((double)(count++ % 10)), StatusCodes.Good, DateTime.UtcNow));
                values.Add(new DataValue((double)10, StatusCodes.Good, DateTime.UtcNow));

                await client.Publish(connection, "MinimalGroup", "boiler01", values);
                await client.Publish(connection, "FullGroup", "boiler01", values);
            }
        }
        */
    }
}
