using Microsoft.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using Opc.Ua;
using Opc.Ua.PubSub;
using Opc.Ua.PubSub.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MqttAgent
{
    static class MqttAgentApplication
    {
        public static string NameplateFilePath { get; private set; }

        static void Log(object sender, PublishingErrorEventArgs e)
        {
            Console.WriteLine($"[{e.ErrorCode}] {e.ErrorText}");
        }

        public static void Run(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "MqttAgent";
            app.Description = "An application that uses OPC UA PubSub to commicate with MQTT.";
            app.HelpOption("-?|-h|--help");

            Utils.SetTraceMask(Utils.TraceMasks.Error);
            Utils.SetTraceOutput(Utils.TraceOutput.DebugAndFile);

            app.Command("publish", (e) => Publish(e));
            app.Command("discover", (e) => Discover(e));
            app.Command("subscribe", (e) => Subscribe(e));
            // app.Command("create-subscriber", (e) => CreateSubscriber(e));

            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 0;
            });

            app.Execute(args);
        }

        public static string GetOption(this CommandLineApplication application, string name, string defaultValue = "")
        {
            var option = application.Options.Find((ii) => { return ii.ShortName == name && ii.HasValue(); });

            if (option == null)
            {
                return defaultValue;
            }

            return option.Value();
        }

        public static DataSetMetaDataType LoadDataSets(string filePath)
        {
            string json = File.ReadAllText(filePath);

            DataSetMetaDataType metadata = new DataSetMetaDataType();

            using (var decoder = new JsonDecoder(json, ServiceMessageContext.GlobalContext))
            {
                metadata.Decode(decoder);
            }

            foreach (var field in metadata.Fields)
            {
                if (NodeId.IsNull(field.DataType))
                {
                    field.DataType = new NodeId(field.BuiltInType);
                }

                if (field.ValueRank == 0)
                {
                    field.ValueRank = ValueRanks.Scalar;
                }
            }

            return metadata;
        }

        static PubSubConnectionDataType LoadConnection(
            CommonOptions options,
            List<DataSetMetaDataType> datasets = null)
        {
            string json = File.ReadAllText(options.ConnectionFilePath);

            if (options.BrokerUrl != null)
            {
                Uri url;

                if (!Uri.TryCreate(options.BrokerUrl, UriKind.Absolute, out url))
                {
                    throw new ArgumentException("Broker URL is not a valid URL.", "brokerUrl");
                }

                json = json.Replace("mqtt://localhost:1883", url.ToString());
            }

            if (options.ApplicationId != null)
            {
                json = json.Replace("[ApplicationName]", options.ApplicationId);
            }

            PubSubConnectionDataType connection = new PubSubConnectionDataType();

            using (var decoder = new JsonDecoder(json, ServiceMessageContext.GlobalContext))
            {
                connection.Decode(decoder);
            }

            // replace intial datasets with the last version cached.
            if (datasets != null)
            {
                foreach (var group in connection.ReaderGroups)
                {
                    foreach (var reader in group.DataSetReaders)
                    {
                        if (reader.DataSetMetaData != null)
                        {
                            var metadata = datasets.Find(x => x.Name == reader.DataSetMetaData.Name);

                            if (metadata != null)
                            {
                                reader.DataSetMetaData = metadata;
                            }
                        }
                    }
                }
            }

            return connection;
        }

        static void AddCommonOptions(CommandLineApplication command)
        {
            command.Option(
                "-b|--broker <BrokerUrl>",
                "The MQTT broker URL. Overrides the setting in the connection configuration.",
                CommandOptionType.SingleValue);

            command.Option(
                "-a|--appid <ApplicationId>",
                "A unique name for the application instance. Overrides the setting in the connection configuration.",
                CommandOptionType.SingleValue);

            command.Option(
                "-c|--connection <ConnectionFilePath>",
                "The file containing the the OPC UA PubSub connection configuration.",
                CommandOptionType.SingleValue);

            command.Option(
                "-d|--datasets <DataSetDirectoryPath>",
                "The directory containing the the OPC UA PubSub dataset configurations.",
                CommandOptionType.SingleValue);

            command.Option(
                "-n|--nameplate <NameplaceFilePath>",
                "The file containing the the nameplate information for the device where the agent is running.",
                CommandOptionType.SingleValue);
        }

        class CommonOptions
        {
            public string DatasetFilePath { get; internal set; }
            public string NameplateFilePath { get; internal set; }
            public string ConnectionFilePath { get; internal set; }
            public string BrokerUrl { get; internal set; }
            public string ApplicationId { get; internal set; }
        }

        static CommonOptions GetCommonOptions(CommandLineApplication app, bool subscriber = false)
        {
            var options = new CommonOptions()
            {
                DatasetFilePath = app.GetOption("d", "config/datasets"),
                NameplateFilePath = app.GetOption("n", "config/nameplate.json"),
                ConnectionFilePath = app.GetOption("c", $"config/{((subscriber)?"subscriber":"publisher")}-connection.json"),
                BrokerUrl = app.GetOption("b", "mqtt://localhost:1883"),
                ApplicationId = app.GetOption("a", Dns.GetHostName())
            };

            return options;
        }

        static void HandleKeyPress(Func<ConsoleKeyInfo,bool> handler = null)
        {
            do
            {
                while (!Console.KeyAvailable)
                {
                    Thread.Sleep(250);
                }
                
                var key = Console.ReadKey(true);

                if (key.KeyChar == 'x' || key.KeyChar == 'X')
                {
                    break;
                }

                if (handler != null)
                {
                    if (handler(key))
                    {
                        break;
                    }
                }
            }
            while (true);
        }

        static string GetConnectionUrl(PubSubConnectionDataType connection)
        {
            if (connection == null)
            {
                return String.Empty;
            }

            var address = ExtensionObject.ToEncodeable(connection.Address) as NetworkAddressUrlDataType;

            if (address == null)
            {
                return String.Empty;
            }

            return address.Url;
        }

        static void Publish(CommandLineApplication app)
        {
            app.Description = "Publishes I/O data to an MQTT broker.";
            app.HelpOption("-?|-h|--help");
            AddCommonOptions(app);

            app.OnExecute(() =>
            {
                var options = GetCommonOptions(app);

                // the I/O managers handle the interface with the GPIO system the device.
                Dictionary<string, IIOManager> m_ioManagers = new Dictionary<string, IIOManager>();

                // datasets many be updated when metadata changes are published.
                // they are saved as a seperate file and then used to update the connection configuration.
                List<DataSetMetaDataType> datasets = new List<DataSetMetaDataType>();

                ushort id = 1;

                foreach (var file in Directory.GetFiles(options.DatasetFilePath, "*.json"))
                {
                    var dataset = LoadDataSets(file);
                    datasets.Add(dataset);

                    switch (dataset.Name)
                    {
                        case "identity":
                        {
                            m_ioManagers[dataset.Name] = new VendorNameplateManager(id++, dataset, options.NameplateFilePath, options.ApplicationId);
                            break;
                        }

                        default:
                        {
                            m_ioManagers[dataset.Name] = new IOSimulator(id++, dataset);
                            break;
                        }
                    }
                }

                var connection = LoadConnection(options, datasets);

                PubSubConfigurationDataType configuration = new PubSubConfigurationDataType()
                {
                    Connections = new PubSubConnectionDataTypeCollection() { connection },
                    PublishedDataSets = new PublishedDataSetDataTypeCollection()
                };

                connection.ReaderGroups.Clear();

                foreach (var ii in m_ioManagers.Values)
                {
                    configuration.PublishedDataSets.Add(ii.DataSet);
                    ii.Start();
                }

                var managers = new DataStore(m_ioManagers.Values);

                // Create the UA Publisher application using configuration file
                using (UaPubSubApplication application = UaPubSubApplication.Create(configuration, managers))
                {
                    Console.WriteLine($"Publishing to {GetConnectionUrl(connection)}.");
                    application.Start();
                    Console.WriteLine("Press [X] to stop the program.");
                    HandleKeyPress();
                }

                return 0;
            });
        }

        static void Discover(CommandLineApplication app)
        {
            app.Description = "Discovers OPC UA publishers.";
            app.HelpOption("-?|-h|--help");
            AddCommonOptions(app);

            app.OnExecute(() =>
            {
                var options = GetCommonOptions(app, true);
                options.ConnectionFilePath = app.GetOption("c", "config/discovery-connection.json");

                List<DataSetMetaDataType> datasets = new List<DataSetMetaDataType>();

                foreach (var file in Directory.GetFiles(options.DatasetFilePath, "*.json"))
                {
                    var dataset = LoadDataSets(file);

                    if (dataset.Name == "identity")
                    {
                        datasets.Add(dataset);
                        break;
                    }
                }

                var connection = LoadConnection(options, datasets);

                PubSubConfigurationDataType configuration = new PubSubConfigurationDataType()
                {
                    Connections = new PubSubConnectionDataTypeCollection() { connection },
                    PublishedDataSets = new PublishedDataSetDataTypeCollection()
                };

                foreach (var ii in connection.ReaderGroups)
                {
                    if (ii.Name != "identity")
                    {
                        ii.Enabled = false;
                    }
                }

                foreach (var ii in datasets)
                {
                    configuration.PublishedDataSets.Add(new PublishedDataSetDataType()
                    {
                        Name = ii.Name,
                        DataSetMetaData = ii
                    });
                }

                // Create the UA Publisher application using configuration file
                using (UaPubSubApplication application = UaPubSubApplication.Create(configuration))
                {
                    application.DataReceived += (sender, e) =>
                    {
                        if (e.NetworkMessage.DataSetMessages.Count <= 0)
                        {
                            return;
                        }

                        Console.WriteLine("");
                        Console.WriteLine($"Detected Publisher ({e.Source}).");
                        var dataset = e.NetworkMessage.DataSetMessages[0];

                        foreach (var field in dataset.DataSet.Fields)
                        {
                            if (field.Value.WrappedValue != Variant.Null)
                            {
                                Console.WriteLine($"  {field.FieldMetaData.Name}: {field.Value.WrappedValue}");
                            }
                        }
                    };

                    Console.WriteLine($"Discovering Publishers on {GetConnectionUrl(connection)}.");
                    application.Start();
                    Console.WriteLine("Press [X] to stop the program.");
                    HandleKeyPress();
                }

                return 0;
            });
        }

        static void Subscribe(CommandLineApplication app)
        {
            app.Description = "Subscribes for data from OPC UA publishers.";
            app.HelpOption("-?|-h|--help");
            AddCommonOptions(app);

            app.OnExecute(() =>
            {
                var options = GetCommonOptions(app, true);

                List<DataSetMetaDataType> datasets = new List<DataSetMetaDataType>();

                foreach (var file in Directory.GetFiles(options.DatasetFilePath, "*.json"))
                {
                    var dataset = LoadDataSets(file);
                    datasets.Add(dataset);
                }

                var connection = LoadConnection(options, datasets);

                PubSubConfigurationDataType configuration = new PubSubConfigurationDataType()
                {
                    Connections = new PubSubConnectionDataTypeCollection() { connection },
                    PublishedDataSets = new PublishedDataSetDataTypeCollection()
                };

                foreach (var ii in connection.ReaderGroups)
                {
                    if (ii.Name == "identity")
                    {
                        ii.Enabled = false;
                    }
                }

                foreach (var ii in datasets)
                {
                    configuration.PublishedDataSets.Add(new PublishedDataSetDataType()
                    {
                        Name = ii.Name,
                        DataSetMetaData = ii
                    });
                }

                // Create the UA Publisher application using configuration file
                using (UaPubSubApplication application = UaPubSubApplication.Create(configuration))
                {
                    application.DataReceived += (sender, e) =>
                    {
                        if (e.NetworkMessage.DataSetMessages.Count <= 0)
                        {
                            return;
                        }

                        Console.WriteLine("");
                        Console.WriteLine($"Detected Publisher ({e.Source}).");
                        var dataset = e.NetworkMessage.DataSetMessages[0];

                        foreach (var field in dataset.DataSet.Fields)
                        {
                            if (field.Value.WrappedValue != Variant.Null)
                            {
                                Console.WriteLine($"  {field.FieldMetaData.Name}: {field.Value.WrappedValue}");
                            }
                        }
                    };

                    Console.WriteLine($"Discovering Publishers on {GetConnectionUrl(connection)}.");
                    application.Start();
                    Console.WriteLine("Press [X] to stop the program.");
                    HandleKeyPress();
                }

                return 0;
            });
        }

        static void CreateSubscriber(CommandLineApplication app)
        {
            app.Description = "Create a subscriber connection file from a publisher connection.";
            app.HelpOption("-?|-h|--help");
            AddCommonOptions(app);

            app.OnExecute(() =>
            {
                var options = GetCommonOptions(app);

                List<DataSetMetaDataType> datasets = new List<DataSetMetaDataType>();

                foreach (var file in Directory.GetFiles(options.DatasetFilePath, "*.json"))
                {
                    var dataset = LoadDataSets(file);
                    datasets.Add(dataset);
                }

                var connection = LoadConnection(options, datasets);

                foreach (var ii in connection.WriterGroups)
                {
                    ReaderGroupDataType readerGroup = new ReaderGroupDataType()
                    {
                        Name = ii.Name,
                        Enabled = ii.Enabled,
                        SecurityGroupId = ii.SecurityGroupId,
                        MaxNetworkMessageSize = ii.MaxNetworkMessageSize,
                        SecurityMode = ii.SecurityMode
                    };

                    var groupMessageSettings = ExtensionObject.ToEncodeable(ii.MessageSettings) as JsonWriterGroupMessageDataType;
                    var groupTransportSettings = ExtensionObject.ToEncodeable(ii.TransportSettings) as BrokerWriterGroupTransportDataType;

                    foreach (var jj in ii.DataSetWriters)
                    {
                        var datasetMessageSettings = ExtensionObject.ToEncodeable(jj.MessageSettings) as JsonDataSetWriterMessageDataType;
                        var datasetTransportSettings = ExtensionObject.ToEncodeable(jj.TransportSettings) as BrokerDataSetWriterTransportDataType;
                        var dataset = datasets.Find(x => x.Name == jj.DataSetName);

                        DataSetReaderDataType reader = new DataSetReaderDataType()
                        {
                            Name = jj.Name,
                            Enabled = ii.Enabled,
                            PublisherId = connection.PublisherId,
                            WriterGroupId = ii.WriterGroupId,
                            DataSetWriterId = jj.DataSetWriterId,
                            DataSetMetaData = new DataSetMetaDataType()
                            {
                                Name = jj.DataSetName,
                                ConfigurationVersion = dataset?.ConfigurationVersion ?? new ConfigurationVersionDataType() 
                            },
                            DataSetFieldContentMask = jj.DataSetFieldContentMask,
                            HeaderLayoutUri = ii.HeaderLayoutUri,
                            KeyFrameCount = jj.KeyFrameCount,
                            SecurityGroupId = ii.SecurityGroupId,
                            SecurityMode = ii.SecurityMode,
                            MessageReceiveTimeout = 0,
                        };

                        reader.MessageSettings = new ExtensionObject(
                            DataTypeIds.JsonDataSetReaderMessageDataType,
                            new JsonDataSetReaderMessageDataType()
                            {
                                NetworkMessageContentMask = groupMessageSettings?.NetworkMessageContentMask ?? (uint)JsonNetworkMessageContentMask.None,
                                DataSetMessageContentMask = datasetMessageSettings?.DataSetMessageContentMask ?? (uint)JsonDataSetMessageContentMask.None
                            });

                        reader.TransportSettings = new ExtensionObject(
                            DataTypeIds.BrokerDataSetReaderTransportDataType, 
                            new BrokerDataSetReaderTransportDataType()
                            {
                                QueueName = (datasetTransportSettings?.QueueName) ?? groupTransportSettings.QueueName,
                                MetaDataQueueName = datasetTransportSettings?.MetaDataQueueName ?? null,
                                RequestedDeliveryGuarantee = datasetTransportSettings?.RequestedDeliveryGuarantee ?? groupTransportSettings.RequestedDeliveryGuarantee,
                                AuthenticationProfileUri = groupTransportSettings?.AuthenticationProfileUri,
                                ResourceUri = groupTransportSettings?.ResourceUri
                            });

                        readerGroup.DataSetReaders.Add(reader);
                    }

                    connection.ReaderGroups.Add(readerGroup);
                }

                connection.WriterGroups.Clear();

                var output = Path.GetFileNameWithoutExtension(options.ConnectionFilePath).Replace("publisher-", "");
                var path = Path.GetDirectoryName(output);
                if (String.IsNullOrEmpty(path)) path = ".";
                output = $"{path}/subscriber-{output}.json";

                using (StreamWriter writer = new StreamWriter(output))
                {
                    using (JsonEncoder encoder = new JsonEncoder(ServiceMessageContext.GlobalContext, true, writer, false))
                    {
                        connection.Encode(encoder);
                        encoder.Close();
                    }
                }

                Console.WriteLine($"Wrote subscriber file to '{output}'.");
                return 0;
            });
        }
    }
}
