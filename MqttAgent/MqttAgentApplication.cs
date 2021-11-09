using Microsoft.Extensions.CommandLineUtils;
using MqttAgent.Server;
using Opc.Ua;
using Opc.Ua.PubSub;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Linq;

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
            app.Description = "An application that uses OPC UA PubSub to communicate with MQTT.";
            app.HelpOption("-?|-h|--help");

            Utils.SetTraceMask(Utils.TraceMasks.Error);
            Utils.SetTraceOutput(Utils.TraceOutput.DebugAndFile);

            app.Command("publish", (e) => Publish(e));
            app.Command("discover", (e) => Discover(e));
            app.Command("subscribe", (e) => Subscribe(e));
            app.Command("metadata", (e) => Metadata(e));
            // app.Command("create-subscriber", (e) => CreateSubscriber(e));

            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 0;
            });

            app.Execute(args);
        }

        private static void Publish(CommandLineApplication app)
        {
            app.Description = "Publishes I/O data to an MQTT broker.";
            app.HelpOption("-?|-h|--help");
            AddCommonOptions(app, false);

            app.OnExecute(() =>
            {
                var options = GetCommonOptions(app);

                UAClient client = new UAClient();
                Dictionary<string, IIOManager> ioManagers = LoadDataSets(options, client);

                var connection = LoadConnection(options, ioManagers.Values);

                PubSubConfigurationDataType configuration = new PubSubConfigurationDataType()
                {
                    Connections = new PubSubConnectionDataTypeCollection() { connection },
                    PublishedDataSets = new PublishedDataSetDataTypeCollection()
                };

                connection.ReaderGroups.Clear();

                foreach (var ii in ioManagers.Values)
                {
                    configuration.PublishedDataSets.Add(ii.DataSet);
                }

                var managers = new DataStore(ioManagers.Values);

                var useGPIO = app.Options.Find((ii) => { return ii.ShortName == "g" && ii.HasValue(); }) != null;
                var server = new GPIO();

                // Create the UA Publisher application using configuration file
                using (UaPubSubApplication application = UaPubSubApplication.Create(configuration, managers))
                {
                    Console.WriteLine($"Publishing to {GetConnectionUrl(connection)}.");
                    application.Start();

                    Console.WriteLine("Starting OPC UA server.");
                    server.Start(useGPIO).Wait();

                    client.StartAsync().Wait();

                    foreach (var ii in ioManagers.Values)
                    {
                        ii.Start();
                    }

                    Console.WriteLine("Press [X] to stop the program.");
                    HandleKeyPress();
                }

                Console.WriteLine("Stopping OPC UA server.");
                server.Stop().Wait();

                return 0;
            });
        }

        private static void Discover(CommandLineApplication app)
        {
            app.Description = "Discovers OPC UA publishers.";
            app.HelpOption("-?|-h|--help");
            AddCommonOptions(app, true);

            app.OnExecute(() =>
            {
                var options = GetCommonOptions(app, true);
                options.ConnectionFilePath = app.GetOption("c", "config/discovery-connection.json");

                Dictionary<string, IIOManager> ioManagers = LoadDataSets(options, null, "identity");

                var connection = LoadConnection(options, ioManagers.Values);

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

                foreach (var ii in ioManagers.Values)
                {
                    configuration.PublishedDataSets.Add(ii.DataSet);
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

        private static void Subscribe(CommandLineApplication app)
        {
            app.Description = "Subscribes for data from OPC UA publishers.";
            app.HelpOption("-?|-h|--help");
            AddCommonOptions(app, true);

            app.OnExecute(() =>
            {
                var options = GetCommonOptions(app, true);

                Dictionary<string, IIOManager> ioManagers = LoadDataSets(options);

                var connection = LoadConnection(options, ioManagers.Values);

                PubSubConfigurationDataType configuration = new PubSubConfigurationDataType()
                {
                    Connections = new PubSubConnectionDataTypeCollection() { connection },
                    PublishedDataSets = new PublishedDataSetDataTypeCollection()
                };

                // disable all but the selected group to avoid reporting too many updates.
                foreach (var ii in connection.ReaderGroups)
                {
                    foreach (var jj in ii.DataSetReaders)
                    {
                        if (jj.Name != options.DataSetReaderName)
                        {
                            jj.Enabled = false;
                        }
                    }
                }

                foreach (var ii in ioManagers.Values)
                {
                    configuration.PublishedDataSets.Add(ii.DataSet);
                }

                using (UaPubSubApplication application = UaPubSubApplication.Create(configuration))
                {
                    application.DataReceived += (sender, e) =>
                    {
                        if (e.NetworkMessage.DataSetMessages.Count <= 0)
                        {
                            return;
                        }

                        var dataset = e.NetworkMessage.DataSetMessages[0];
                        Console.WriteLine("");
                        Console.WriteLine($"Received SequenceNumber '{((dataset.SequenceNumber == 0)?"[Not In Message]":dataset.SequenceNumber.ToString())}' From ({e.Source}).");

                        foreach (var field in dataset.DataSet.Fields)
                        {
                            if (field.Value.WrappedValue != Variant.Null)
                            {
                                Console.WriteLine($"  {field.FieldMetaData.Name}: {field.Value.WrappedValue.TypeInfo} {field.Value.WrappedValue}");
                            }
                        }
                    };

                    Console.WriteLine($"Monitoring '{options.PublisherId}/{options.DataSetReaderName}' on {GetConnectionUrl(connection)}.");
                    application.Start();
                    Console.WriteLine("Press [X] to stop the program.");
                    HandleKeyPress();
                }

                return 0;
            });
        }

        private static void Metadata(CommandLineApplication app)
        {
            app.Description = "Subscribes for data from OPC UA publishers.";
            app.HelpOption("-?|-h|--help");
            AddCommonOptions(app, true);

            app.OnExecute(() =>
            {
                var options = GetCommonOptions(app, true);

                Dictionary<string, IIOManager> ioManagers = LoadDataSets(options);

                var connection = LoadConnection(options, ioManagers.Values);

                PubSubConfigurationDataType configuration = new PubSubConfigurationDataType()
                {
                    Connections = new PubSubConnectionDataTypeCollection() { connection },
                    PublishedDataSets = new PublishedDataSetDataTypeCollection()
                };

                foreach (var ii in connection.ReaderGroups)
                {
                    foreach (var jj in ii.DataSetReaders)
                    {
                        if (jj.Name != options.DataSetReaderName)
                        {
                            jj.Enabled = false;
                        }
                    }
                }

                foreach (var ii in ioManagers.Values)
                {
                    configuration.PublishedDataSets.Add(ii.DataSet);
                }

                using (UaPubSubApplication application = UaPubSubApplication.Create(configuration))
                {
                    application.MetaDataReceived += (sender, e) =>
                    {
                        var metadata = e.NetworkMessage.DataSetMetaData;

                        // note the MajorVersion/MinorVersions are reported as 'seconds since 2000-01-01'.
                        Console.WriteLine("");
                        Console.WriteLine($"Received MetaData Version ({metadata.ConfigurationVersion.MajorVersion}.{metadata.ConfigurationVersion.MinorVersion}) From ({e.Source}).");

                        foreach (var field in metadata.Fields)
                        {
                            Console.WriteLine($"  {field.Name}: {(BuiltInType)field.BuiltInType}{((field.ValueRank == ValueRanks.Scalar) ? "" : "[]")} {field.Description}");
                        }
                    };

                    Console.WriteLine($"Monitoring '{options.PublisherId}/{options.DataSetReaderName}' on {GetConnectionUrl(connection)}.");
                    application.Start();
                    Console.WriteLine("Press [X] to stop the program.");
                    HandleKeyPress();
                }

                return 0;
            });
        }

        private static void CreateSubscriber(CommandLineApplication app)
        {
            app.Description = "Create a subscriber connection file from a publisher connection.";
            app.HelpOption("-?|-h|--help");
            AddCommonOptions(app, true);

            app.OnExecute(() =>
            {
                var options = GetCommonOptions(app);

                Dictionary<string, IIOManager> ioManagers = LoadDataSets(options);

                var connection = LoadConnection(options, ioManagers.Values);

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
                        var dataset = ioManagers.Values.Where(x => x.DataSet.Name == jj.DataSetName).FirstOrDefault();

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
                                ConfigurationVersion = dataset?.DataSet?.DataSetMetaData?.ConfigurationVersion ?? new ConfigurationVersionDataType()
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

        private static string GetOption(this CommandLineApplication application, string name, string defaultValue = "")
        {
            var option = application.Options.Find((ii) => { return ii.ShortName == name && ii.HasValue(); });

            if (option == null)
            {
                return defaultValue;
            }

            return option.Value();
        }

        private static DataSetMetaDataType LoadDataSets(string filePath)
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

        private static Dictionary<string, IIOManager> LoadDataSets(CommonOptions options, UAClient client = null, string dataSetName = null)
        {
            Dictionary<string, IIOManager> ioManagers = new Dictionary<string, IIOManager>();

            ushort id = 1;

            foreach (var file in Directory.GetFiles(options.DatasetFilePath, "*.json"))
            {
                var dataset = LoadDataSets(file);

                if (dataSetName != null && dataSetName != dataset.Name)
                {
                    continue;
                }

                switch (dataset.Name)
                {
                    case "identity":
                    {
                        ioManagers[dataset.Name] = new VendorNameplateManager(id++, dataset, options.NameplateFilePath, options.ApplicationId);
                        break;
                    }

                    default:
                    {
                        if (client != null)
                        {
                            ioManagers[dataset.Name] = new IOUAClient(id++, dataset, client);
                        }
                        else
                        {
                            ioManagers[dataset.Name] = new IOSimulator(id++, dataset);
                        }

                        break;
                    }
                }
            }

            return ioManagers;
        }

        private static PubSubConnectionDataType LoadConnection(
            CommonOptions options,
            IEnumerable<IIOManager> datasets = null)
        {
            string json = File.ReadAllText(options.ConnectionFilePath);

            if (options.BrokerUrl != null)
            {
                Uri url;

                if (!Uri.TryCreate(options.BrokerUrl, UriKind.Absolute, out url))
                {
                    throw new ArgumentException("Broker URL is not a valid URL.", "brokerUrl");
                }

                json = json.Replace("mqtt://localhost:1883/", url.ToString());
            }

            json = json.Replace("[ApplicationName]", options.ApplicationId);
            json = json.Replace("[PublisherId]", (String.IsNullOrEmpty(options.PublisherId)) ? options.ApplicationId : options.PublisherId);

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
                            foreach (var dataset in datasets)
                            {
                                if (reader.DataSetMetaData.Name == dataset.DataSet.Name)
                                {
                                    reader.DataSetMetaData = dataset.DataSet.DataSetMetaData;
                                }
                            }
                        }
                    }
                }
            }

            return connection;
        }

        private static void AddCommonOptions(CommandLineApplication app, bool subscriber)
        {
            app.Option(
                "-b|--broker",
                "The MQTT broker URL. Overrides the setting in the connection configuration.",
                CommandOptionType.SingleValue);

            app.Option(
                "-c|--connection",
                "The file containing the the OPC UA PubSub connection configuration.",
                CommandOptionType.SingleValue);

            app.Option(
                "-d|--datasets",
                "The directory containing the the OPC UA PubSub dataset configurations.",
                CommandOptionType.SingleValue);

            if (subscriber)
            {
                app.Option(
                    "-p|--publisher",
                    "The identifier for the publisher to monitor.",
                    CommandOptionType.SingleValue);

                app.Option(
                    "-g|--dataset",
                    "The name of the dataset reader to monitor.",
                    CommandOptionType.SingleValue);
            }
            else
            {
                app.Option(
                    "-a|--application",
                    "A unique name for the application instance. Overrides the setting in the connection configuration.",
                    CommandOptionType.SingleValue);
             
                app.Option(
                    "-n|--nameplate <NameplaceFilePath>",
                    "The file containing the the nameplate information provided by the publisher.",
                    CommandOptionType.SingleValue);

                app.Option(
                    "-g|--gpio",
                    "Use GPIO instead of a software simulator.",
                    CommandOptionType.NoValue);
            }
        }

        class CommonOptions
        {
            public string DatasetFilePath { get; internal set; }
            public string NameplateFilePath { get; internal set; }
            public string ConnectionFilePath { get; internal set; }
            public string BrokerUrl { get; internal set; }
            public string ApplicationId { get; internal set; }
            public string PublisherId { get; internal set; }
            public string DataSetReaderName { get; internal set; }
        }

        private static CommonOptions GetCommonOptions(CommandLineApplication app, bool subscriber = false)
        {
            var options = new CommonOptions()
            {
                DatasetFilePath = app.GetOption("d", "config/datasets"),
                NameplateFilePath = app.GetOption("n", "config/nameplate.json"),
                ConnectionFilePath = app.GetOption("c", $"config/{((subscriber)?"subscriber":"publisher")}-connection.json"),
                BrokerUrl = app.GetOption("b", "mqtt://localhost:1883"),
                ApplicationId = app.GetOption("a", Dns.GetHostName()),
                PublisherId = app.GetOption("p", "raspberrypi:one"),
                DataSetReaderName = app.GetOption("g", "Gate1-Minimal")
            };

            return options;
        }

        private static void HandleKeyPress(Func<ConsoleKeyInfo,bool> handler = null)
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

        private static string GetConnectionUrl(PubSubConnectionDataType connection)
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
    }
}
