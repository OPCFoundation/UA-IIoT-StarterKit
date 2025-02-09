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
using MQTTnet;
using MQTTnet.Formatter;
using Opc.Ua.WebApi;
using Opc.Ua.WebApi.Model;
using UaMqttCommon;
using UaMqttPublisher;
using KeyValuePair = Opc.Ua.WebApi.Model.KeyValuePair;

await new Publisher().Connect();

internal class Publisher
{
    readonly Configuration m_configuration = new Configuration()
    {
        BrokerHost = "broker.hivemq.com",
        BrokerPort = 1883,
        TopicPrefix = "opcua-quickstarts",
        PublisherId = "Quickstart002"
    };

    string BrokerUrl => m_configuration.BrokerHost;
    string TopicPrefix => m_configuration.TopicPrefix;
    string PublisherId => m_configuration.PublisherId;

    const string GroupName = "Conveyor";
    const string WriterName = "Motor";
    const string DataSetName = "EnergyMetrics";
    const int DataSetWriterId = 101;

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

    private MqttClientFactory? m_factory;
    private IMqttClient? m_client;

    public async Task Connect()
    {
        // cleans up topics. useful when developing/testing. not used for production.
        await Utils.DeleteAllTopics(m_configuration, 5000, CancellationToken.None);

        m_factory = new MqttClientFactory();

        using (m_client = m_factory.CreateMqttClient())
        {
            var willTopic = new Topic()
            {
                TopicPrefix = TopicPrefix,
                MessageType = TopicTypes.Status,
                PublisherId = PublisherId
            }.Build();

            JsonStatusMessage willPayload = new()
            {
                MessageId = Guid.NewGuid().ToString(),
                MessageType = MessageTypes.Status,
                PublisherId = PublisherId,
                Status = (int)PubSubState.Error,
                IsCyclic = false
            };

            var json = Utils.ToJson(willPayload);

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

    private async Task PublishStatus(PubSubState state)
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
            MessageId = Guid.NewGuid().ToString(),
            MessageType = MessageTypes.Status,
            PublisherId = PublisherId,
            Status = (int)state,
            IsCyclic = false
        };

        var json = Utils.ToJson(payload);

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
            Console.WriteLine("Status Message Sent.");
        }
    }

    private async Task CleanupRetainedMessages()
    {
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

        var topic = new Topic()
        {
            TopicPrefix = TopicPrefix,
            MessageType = TopicTypes.DataSetMetaData,
            PublisherId = PublisherId,
            GroupName = GroupName,
            WriterName = WriterName
        }.Build();

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
            Console.WriteLine("DataSetMetaData Message Removed.");
        }
    }

    private async Task PublishDataSetMetaData(EUInformation unitOfPower, ConfigurationVersionDataType version)
    {
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

        var metadata = new DataSetMetaDataType()
        {
            Name = DataSetName, // same name appears in the DataSetWriter.
            ConfigurationVersion = version,
            DataSetClassId = EnergyMetrics.DataSetClassId, // Used to indicate that many DataSetWriters report the same data.
            Description = new LocalizedText() { Text = "A set of energy consumption metrics for a device." },
            Fields = new List<FieldMetaData>()
            {
                new FieldMetaData()
                {
                    Name = "Consumption",
                    BuiltInType = (int)BuiltInType.Double,
                    DataType = DataTypeIds.Double,
                    Description = new LocalizedText() { Text = "The energy consumed by the device during the calculation period." },
                    ValueRank = -1,
                    Properties = new List<KeyValuePair>()
                    {
                        new KeyValuePair()
                        {
                            Key = BrowseNames.EngineeringUnits,
                            Value = new Variant()
                            {
                                UaType = (int)BuiltInType.ExtensionObject,
                                Value = Utils.ToObject(
                                    DataTypeIds.EUInformation,
                                    unitOfPower
                                )
                            }
                        }
                    }
                },
                new FieldMetaData()
                {
                    Name = "DutyCycle",
                    BuiltInType = (int)BuiltInType.Float,
                    DataType = DataTypeIds.Float,
                    Description = new LocalizedText() { Text = "The fraction of the calulation period where the device is consuming power." },
                    ValueRank = -1,
                    Properties = new List<KeyValuePair>()
                    {
                        new KeyValuePair()
                        {
                            Key = BrowseNames.EngineeringUnits,
                            Value = new Variant()
                            {
                                UaType = (int)BuiltInType.ExtensionObject,
                                Value = Utils.ToObject(
                                    DataTypeIds.EUInformation,
                                    new EUInformation(){ 
                                        NamespaceUri = "http://www.opcfoundation.org/UA/units/un/cefact",
                                        UnitId = 20529,
                                        DisplayName = new LocalizedText() { Text = "%" },
                                        Description = new LocalizedText() { Text = "percent" }
                                    }
                                )
                            }
                        }
                    }
                },
                new FieldMetaData()
                {
                    Name = "CalculationPeriod",
                    BuiltInType = (int)BuiltInType.Double,
                    DataType = DataTypeIds.Double,
                    Description = new LocalizedText() { Text = "The period, in ms, over which power calculations are computed." },
                    ValueRank = -1,
                    Properties = new List<KeyValuePair>()
                    {
                        new KeyValuePair()
                        {
                            Key = BrowseNames.EngineeringUnits,
                            Value = new Variant()
                            {
                                UaType = (int)BuiltInType.ExtensionObject,
                                Value = Utils.ToObject(
                                    DataTypeIds.EUInformation,
                                    new EUInformation(){
                                        NamespaceUri = "http://www.opcfoundation.org/UA/units/un/cefact",
                                        UnitId = 4403766,
                                        DisplayName = new LocalizedText() { Text = "ms" },
                                        Description = new LocalizedText() { Text = "millisecond" }
                                    }
                                )
                            }
                        }
                    }
                }
            }
        };

        var topic = new Topic()
        {
            TopicPrefix = TopicPrefix,
            MessageType = TopicTypes.DataSetMetaData,
            PublisherId = PublisherId,
            GroupName = GroupName,
            WriterName = WriterName
        }.Build();

        var payload = new JsonDataSetMetaDataMessage()
        {
            MessageId = Guid.NewGuid().ToString(),
            MessageType = MessageTypes.DataSetMetaData,
            PublisherId = PublisherId,
            DataSetWriterId = DataSetWriterId,
            MetaData = metadata
        };

        var json = Utils.ToJson(payload);

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
        }
    }

    private async Task Publish()
    {
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

        await PublishStatus(PubSubState.Operational);

        var random = new Random();
        int unitOfPower = 0;
        ConfigurationVersionDataType version = new();

        Console.WriteLine("Press enter to exit.");
        int count = 0;

        while (true)
        {
            // every 10th message, change the units of power.
            if (count % 10 == 0)
            {
                version.MajorVersion = version.MinorVersion = Utils.GetVersionTime();
                await PublishDataSetMetaData(UnitsOfPower[(unitOfPower++) % 2], version);
            }

            var data = new EnergyMetrics()
            {
                CalculationPeriod = 3600,
                Consumption = 42 + random.NextDouble(),
                DutyCycle = random.NextDouble()
            };

            JsonDataSetMessage message = new()
            {
                PublisherId = PublisherId,
                DataSetWriterId = DataSetWriterId,
                SequenceNumber = count + 1,
                MinorVersion = version.MinorVersion,
                Timestamp = DateTime.UtcNow,
                Payload = data
            };

            var json = Utils.ToJson(message);

            var topic = new Topic()
            {
                TopicPrefix = TopicPrefix,
                MessageType = TopicTypes.Data,
                PublisherId = PublisherId,
                GroupName = GroupName,
                WriterName = WriterName
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
                Console.WriteLine($"Data Message Sent: Consumption={data?.Consumption}; DutyCycle={data?.DutyCycle}");
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

            count++;
        }
    }
}
