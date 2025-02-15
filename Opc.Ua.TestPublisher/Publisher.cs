using System.Security.Authentication;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;
using UaMqttPublisher;
using Opc.Ua;
using MQTTnet.Protocol;
using System.Reflection;
using System.IO.Compression;
using Opc.Ua.Test;
using Export = Opc.Ua.Export;

internal partial class Publisher
{
    private Configuration m_configuration;
    private MqttFactory m_factory;
    private IMqttClient m_client;
    private ServiceMessageContext m_messageContext;
    private TypeTable m_typeTable;
    private bool m_deleteAllTopicsOnStart = false;
    private Dictionary<NodeId, DataTypeState> m_dataTypes = new();
    private Dictionary<string, uint> m_sequenceNumbers = new();

    internal string BrokerHost => m_configuration?.BrokerHost;
    internal int BrokerPort => m_configuration?.BrokerPort ?? 1883;
    internal string UserName => m_configuration?.UserName;
    internal string Password => m_configuration?.Password;
    internal string TopicPrefix => m_configuration?.TopicPrefix;
    internal string PublisherId => m_configuration?.PublisherId;
    internal bool UseNewEncodings => m_configuration?.UseNewEncodings ?? true;

    private PubSubConnectionDataType DefaultConnection { get; set; }

    private DataGenerator DataGenerator { get; set; } = new(new RandomSource())
    {
        BoundaryValueFrequency = 10,
        MaxArrayLength = 3,
        MaxStringLength = 12,
        MaxXmlElementCount = 3,
        MaxXmlAttributeCount = 3,
        NamespaceUris = NamespaceUris,
        ServerUris = ServerUris
    };

    public Publisher(Configuration configuration)
    {
        m_configuration = configuration;

        m_messageContext = new ServiceMessageContext();
        m_messageContext.NamespaceUris = new NamespaceTable(DataGenerator.NamespaceUris.ToArray());
        m_messageContext.ServerUris = new StringTable(DataGenerator.ServerUris.ToArray());
        m_messageContext.Factory.AddEncodeableTypes(Assembly.GetExecutingAssembly());

        m_typeTable = new TypeTable(m_messageContext.NamespaceUris);
        LoadCoreNodeSet(m_messageContext);
        LoadNodeSets(m_messageContext);

        foreach (var ii in DataSets.Values)
        {
            UpdateDataSetMeta(ii);
        }

        DefaultConnection = CreatePubSubConnection();
    }

    private byte[] Encode(IEncodeable message, bool compress = false)
    {
        using (var istrm = new MemoryStream())
        {
            using (var encoder = new JsonEncoder(
                m_messageContext,
                UseNewEncodings ? JsonEncodingType.Compact : JsonEncodingType.Reversible,
                stream: istrm,
                leaveOpen: true))
            {
                message.Encode(encoder);
            }

            istrm.Close();
            return istrm.ToArray();
        }
    }

    public async Task Connect(CancellationToken token)
    {
        m_factory = new MqttFactory();

        while (!token.IsCancellationRequested)
        {
            try
            {
                await Run(token);
                break;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Publishing Error: [{e.GetType().Name}] {e.Message}");
            }

            Console.WriteLine($"Reconnecting in 10s...");

            try
            {
                await Task.Delay(10000, token);
            }
            catch (TaskCanceledException)
            {
                break;
            }
        }
    }

    private async Task Run(CancellationToken token)
    {
        using (m_client = m_factory.CreateMqttClient())
        {
            var willTopic = new Topic()
            {
                TopicPrefix = TopicPrefix,
                MessageType = MessageTypes.Status,
                PublisherId = PublisherId
            }.Build();

            JsonStatusMessage willMessage = new()
            {
                MessageType = "ua-status",
                MessageId = Guid.NewGuid().ToString(),
                PublisherId = PublisherId,
                Status = PubSubState.Error,
                IsCyclic = false
            };

            var willPayload = Encode(willMessage);

            var options = new MqttClientOptionsBuilder()
                .WithProtocolVersion(MqttProtocolVersion.V500)
                .WithTcpServer(BrokerHost, BrokerPort)
                .WithCredentials(UserName, Password)
                .WithWillTopic(willTopic)
                .WithWillRetain(true)
                .WithWillDelayInterval(60)
                .WithWillPayload(willPayload)
                .WithClientId($"{TopicPrefix}.{PublisherId}")
                .WithTlsOptions(
                    o =>
                    {
                        o.UseTls(true);

                        o.WithCertificateValidationHandler(e =>
                        {
                            Console.WriteLine($"Broker Certificate: '{e.Certificate.Subject}' {e.SslPolicyErrors}");
                            return true;
                        });

                        // The default value is determined by the OS. Set manually to force version.
                        o.WithSslProtocols(SslProtocols.Tls12);
                    })
                .Build();

            var response = await m_client.ConnectAsync(options, token);

            if (response.ResultCode != MqttClientConnectResultCode.Success)
            {
                Console.WriteLine($"Connect Failed: {response.ResultCode} {response.ResultCode} {response.ReasonString}");
            }
            else
            {
                Console.WriteLine("Publisher Connected!");

                if (m_deleteAllTopicsOnStart)
                {
                    await DeleteAllTopics(token);
                }

                try
                {
                    await Publish(token);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Publishing Error: [{e.GetType().Name}] {e.Message}");
                }
            }

            await PublishStatus(PubSubState.Disabled, token);
            var disconnectOptions = m_factory.CreateClientDisconnectOptionsBuilder().Build();
            await m_client.DisconnectAsync(disconnectOptions, token);
            Console.WriteLine("Publisher Disconnected!");
        }
    }

    private async Task DeleteAllTopics(CancellationToken token)
    {
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

        m_client.ApplicationMessageReceivedAsync += async delegate (MqttApplicationMessageReceivedEventArgs args)
        {
            string topic = args.ApplicationMessage.Topic;

            if (m_deleteAllTopicsOnStart && args.ApplicationMessage.Retain)
            {
                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload("")
                    .WithRetainFlag(true)
                    .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                    .Build();

                await m_client.PublishAsync(applicationMessage, token);
            }
        };

        await Subscribe($"{TopicPrefix}/#", token);
        await Task.Delay(10000, token);
        await Unsubscribe($"{TopicPrefix}/#", token);
    }

    private async Task Subscribe(string topic, CancellationToken token)
    {
        if (topic == null) throw new ArgumentNullException(nameof(topic));
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

        var options = m_factory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(f => { f.WithTopic(topic); })
            .Build();

        var response = await m_client.SubscribeAsync(options, token);

        if (!String.IsNullOrEmpty(response?.ReasonString))
        {
            Console.WriteLine($"Subscribe Failed:'{response?.ReasonString}'.");
        }
        else
        {
            Console.WriteLine($"Subscribed: '{topic}'.");
        }
    }

    private async Task Unsubscribe(string topic, CancellationToken token)
    {
        if (topic == null) throw new ArgumentNullException(nameof(topic));
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

        var options = m_factory.CreateUnsubscribeOptionsBuilder()
            .WithTopicFilter(topic)
            .Build();

        var response = await m_client.UnsubscribeAsync(options, token);

        if (!String.IsNullOrEmpty(response?.ReasonString))
        {
            Console.WriteLine($"Unsubscribe Failed: '{response?.ReasonString}'.");
        }
        else
        {
            Console.WriteLine($"Unsubscribed: '{topic}'.");
        }
    }

    private async Task PublishStatus(PubSubState state, CancellationToken token)
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
            MessageType = "ua-status",
            MessageId = Guid.NewGuid().ToString(),
            PublisherId = PublisherId,
            Status = state,
            IsCyclic = false
        };

        var message = Encode(payload);

        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(message)
            .WithRetainFlag(true)
            .WithContentType("application/json")
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
            .Build();

        var result = await m_client.PublishAsync(applicationMessage, token);

        if (!result.IsSuccess)
        {
            Console.WriteLine($"Error: {result.ReasonCode} {result.ReasonString}");
        }
        else
        {
            Console.WriteLine("Status Message Sent.");
        }
    }

    private async Task PublishConnection(PubSubConnectionDataType connection, CancellationToken token)
    {
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

        var topic = new Topic()
        {
            TopicPrefix = TopicPrefix,
            MessageType = MessageTypes.Connection,
            PublisherId = PublisherId
        }.Build();

        JsonPubSubConnectionMessage payload = new()
        {
            MessageType = "ua-connection",
            MessageId = Guid.NewGuid().ToString(),
            PublisherId = PublisherId,
            Timestamp = DateTime.UtcNow,
            Connection = connection
        };

        var message = Encode(payload);

        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(message)
            .WithRetainFlag(true)
            .WithContentType("application/json")
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
            .Build();

        var result = await m_client.PublishAsync(applicationMessage, token);

        if (!result.IsSuccess)
        {
            Console.WriteLine($"Error: {result.ReasonCode} {result.ReasonString}");
        }
        else
        {
            Console.WriteLine("Connection Message Sent.");
        }
    }

    private async Task PublishMetaData(string topic, JsonDataSetMetaDataMessage metadata, CancellationToken token)
    {
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

        var message = Encode(metadata);

        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(message)
            .WithRetainFlag(true)
            .WithContentType("application/json")
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
            .Build();

        var result = await m_client.PublishAsync(applicationMessage, token);

        if (!result.IsSuccess)
        {
            Console.WriteLine($"Error: {result.ReasonCode} {result.ReasonString}");
        }
        else
        {
            Console.WriteLine("MetaData Message Sent.");
        }
    }

    private async Task PublishData(string topic, string message, MqttQualityOfServiceLevel qos, CancellationToken token)
    {
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(message)
            .WithRetainFlag(true)
            .WithContentType("application/json")
            .WithQualityOfServiceLevel(qos)
            .Build();

        var result = await m_client.PublishAsync(applicationMessage, token);

        if (!result.IsSuccess)
        {
            Console.WriteLine($"Error: {result.ReasonCode} {result.ReasonString}");
        }
        else
        {
            Console.WriteLine("Data Message Sent.");
        }
    }

    private void WriteNetworkMessageHeader(
        JsonEncoder encoder,
        JsonNetworkMessageContentMask nmcm,
        WriterGroupDataType group)
    {
        if ((nmcm & JsonNetworkMessageContentMask.NetworkMessageHeader) != 0)
        {
            encoder.WriteString(nameof(JsonNetworkMessage.MessageId), Guid.NewGuid().ToString());
            encoder.WriteString(nameof(JsonNetworkMessage.MessageType), "ua-data");

            if ((nmcm & JsonNetworkMessageContentMask.PublisherId) != 0)
            {
                encoder.WriteString(nameof(JsonNetworkMessage.PublisherId), PublisherId);
            }

            if ((nmcm & JsonNetworkMessageContentMask.WriterGroupName) != 0)
            {
                encoder.WriteString(nameof(JsonNetworkMessage.WriterGroupName), group.Name);
            }
        }
    }

    private async Task Publish(CancellationToken token)
    {
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

        await PublishStatus(PubSubState.Operational, token);
        await PublishConnection(DefaultConnection, token);
        await PublishMetaData(DefaultConnection, token);

        while (!token.IsCancellationRequested)
        {
            await PublishData(DefaultConnection, token);

            try
            {
                await Task.Delay(10000, token);
            }
            catch (TaskCanceledException)
            {
                break;
            }
        }

        Console.WriteLine("Exiting publish loop gracefully.");
    }

    private MqttQualityOfServiceLevel GetQos(BrokerTransportQualityOfService groupQos, BrokerTransportQualityOfService writerQos)
    {
        var input = groupQos;

        if (writerQos != BrokerTransportQualityOfService.NotSpecified)
        {
            input = writerQos;
        }

        switch (input)
        {
            case BrokerTransportQualityOfService.ExactlyOnce:
            {
                return MqttQualityOfServiceLevel.ExactlyOnce;
            }

            case BrokerTransportQualityOfService.AtLeastOnce:
            {
                return MqttQualityOfServiceLevel.AtLeastOnce;
            }

            default:
            case BrokerTransportQualityOfService.NotSpecified:
            case BrokerTransportQualityOfService.AtMostOnce:
            case BrokerTransportQualityOfService.BestEffort:
            {
                return MqttQualityOfServiceLevel.AtMostOnce;
            }
        }
    }

    private async Task PublishMetaData(PubSubConnectionDataType connection, CancellationToken token)
    {
        if (connection == null) throw new ArgumentNullException(nameof(connection));

        foreach (var group in connection.WriterGroups)
        {
            foreach (var writer in group.DataSetWriters)
            {
                if (writer.DataSetName == null || !DataSets.TryGetValue(writer.DataSetName, out var metadata))
                {
                    continue;
                }

                string metadataTopic = null;

                if (writer.TransportSettings?.Body is BrokerDataSetWriterTransportDataType dsts)
                {
                    metadataTopic = dsts.MetaDataQueueName;
                }

                await PublishMetaData(
                    metadataTopic,
                    new JsonDataSetMetaDataMessage()
                    {
                        MessageId = Guid.NewGuid().ToString(),
                        MessageType = "ua-metadata",
                        DataSetWriterId = writer.DataSetWriterId,
                        DataSetWriterName = writer.Name,
                        PublisherId = PublisherId,
                        Timestamp = DateTime.UtcNow,
                        MetaData = metadata
                    },
                    token
                );
            }
        }
    }

    private JsonEncodingType FindEncoding(WriterGroupDataType group)
    {
        var nmcm = JsonNetworkMessageContentMask.None;
        var dscm = JsonDataSetMessageContentMask.None;

        if (group.MessageSettings?.Body is JsonWriterGroupMessageDataType wgms)
        {
            nmcm = (JsonNetworkMessageContentMask)wgms.NetworkMessageContentMask;
        }

        foreach (var writer in group.DataSetWriters)
        {
            if (!writer.Enabled)
            {
                continue;
            }

            if (writer.MessageSettings?.Body is JsonDataSetWriterMessageDataType dwms)
            {
                dscm = (JsonDataSetMessageContentMask)dwms.DataSetMessageContentMask;
                break;
            }
        }

        if ((uint)(dscm & JsonDataSetMessageContentMask.FieldEncoding1) != 0)
        {
            return ((uint)(dscm & JsonDataSetMessageContentMask.FieldEncoding2) != 0) ? JsonEncodingType.Compact : JsonEncodingType.Reversible;
        }
        else
        {
            return ((uint)(dscm & JsonDataSetMessageContentMask.FieldEncoding2) != 0) ? JsonEncodingType.Verbose : JsonEncodingType.NonReversible;
        }
    }

    private async Task PublishData(PubSubConnectionDataType connection, CancellationToken token)
    {
        foreach (var group in connection.WriterGroups)
        {
            if (!group.Enabled)
            {
                continue;
            }

            var nmcm = JsonNetworkMessageContentMask.None;
            var dscm = JsonDataSetMessageContentMask.None;

            if (group.MessageSettings?.Body is JsonWriterGroupMessageDataType wgms)
            {
                nmcm = (JsonNetworkMessageContentMask)wgms.NetworkMessageContentMask;
            }

            bool hasArrayOfMessages = (nmcm & JsonNetworkMessageContentMask.SingleDataSetMessage) == 0;

            if ((nmcm & JsonNetworkMessageContentMask.NetworkMessageHeader) != 0)
            {
                hasArrayOfMessages = false;
            }

            var encoder = new JsonEncoder(
                m_messageContext,
                FindEncoding(group),
                topLevelIsArray: hasArrayOfMessages);

            WriteNetworkMessageHeader(encoder, nmcm, group);

            string topic = null;
            string metadataTopic = null;
            var groupQos = BrokerTransportQualityOfService.NotSpecified;

            if (group.TransportSettings?.Body is BrokerWriterGroupTransportDataType wgts)
            {
                topic = wgts.QueueName;
                groupQos = wgts.RequestedDeliveryGuarantee;
            }

            bool first = true;
            bool popMessagesField = false;

            foreach (var writer in group.DataSetWriters)
            {
                if (!writer.Enabled)
                {
                    continue;
                }

                var metadata = DataSets[writer.DataSetName];

                double metadataUpdateTime = 0;
                var writerQos = BrokerTransportQualityOfService.NotSpecified;

                if (writer.TransportSettings?.Body is BrokerDataSetWriterTransportDataType dsts)
                {
                    topic = (nmcm & JsonNetworkMessageContentMask.SingleDataSetMessage) != 0 ? dsts.QueueName : topic;
                    metadataTopic = dsts.MetaDataQueueName;
                    metadataUpdateTime = dsts.MetaDataUpdateTime;
                    writerQos = dsts.RequestedDeliveryGuarantee;
                }

                if (writer.MessageSettings?.Body is JsonDataSetWriterMessageDataType dwms)
                {
                    dscm = (JsonDataSetMessageContentMask)dwms.DataSetMessageContentMask;
                }

                if ((nmcm & JsonNetworkMessageContentMask.SingleDataSetMessage) != 0)
                {
                    if ((nmcm & JsonNetworkMessageContentMask.NetworkMessageHeader) != 0)
                    {
                        if ((nmcm & JsonNetworkMessageContentMask.DataSetClassId) != 0)
                        {
                            if (metadata.DataSetClassId != Guid.Empty)
                            {
                                encoder.WriteGuid(nameof(metadata.DataSetClassId), metadata.DataSetClassId);
                            }
                        }

                        if (first)
                        {
                            encoder.PushStructure(nameof(JsonNetworkMessage.Messages));
                            popMessagesField = true;
                        }
                    }
                }
                else
                {
                    if ((nmcm & JsonNetworkMessageContentMask.NetworkMessageHeader) != 0)
                    {
                        if (first)
                        {
                            encoder.PushArray(nameof(JsonNetworkMessage.Messages));
                            popMessagesField = true;
                            first = false;
                        }
                    }

                    encoder.PushStructure(null);
                }

                uint sequenceNumber = 0;

                if (!m_sequenceNumbers.TryGetValue($"{PublisherId}{writer.DataSetWriterId}", out sequenceNumber))
                {
                    m_sequenceNumbers[$"{PublisherId}{writer.DataSetWriterId}"] = sequenceNumber = 0;
                }

                m_sequenceNumbers[$"{PublisherId}{writer.DataSetWriterId}"] = ++sequenceNumber;

                if (metadataUpdateTime > 0 && metadataTopic != null)
                {
                    var ratio = Math.Round(metadataUpdateTime / group.PublishingInterval);

                    if (sequenceNumber % ratio == 0)
                    {
                        await PublishMetaData(
                            metadataTopic,
                            new JsonDataSetMetaDataMessage()
                            {
                                MessageId = Guid.NewGuid().ToString(),
                                MessageType = "ua-metadata",
                                DataSetWriterId = writer.DataSetWriterId,
                                DataSetWriterName = writer.Name,
                                PublisherId = PublisherId,
                                Timestamp = DateTime.UtcNow,
                                MetaData = metadata
                            },
                            token);
                    }
                }

                if ((nmcm & JsonNetworkMessageContentMask.DataSetMessageHeader) != 0)
                {
                    if ((dscm & JsonDataSetMessageContentMask.MessageType) != 0)
                    {
                        encoder.WriteString(nameof(JsonDataSetMessage.MessageType), "ua-keyframe");
                    }

                    if ((dscm & JsonDataSetMessageContentMask.PublisherId) != 0)
                    {
                        encoder.WriteString(nameof(JsonDataSetMessage.PublisherId), PublisherId);
                    }

                    if ((dscm & JsonDataSetMessageContentMask.DataSetWriterId) != 0)
                    {
                        encoder.WriteUInt16(nameof(JsonDataSetMessage.DataSetWriterId), writer.DataSetWriterId);
                    }

                    if ((dscm & JsonDataSetMessageContentMask.WriterGroupName) != 0)
                    {
                        encoder.WriteString(nameof(JsonDataSetMessage.WriterGroupName), group.Name);
                    }

                    if ((dscm & JsonDataSetMessageContentMask.DataSetWriterName) != 0)
                    {
                        encoder.WriteString(nameof(JsonDataSetMessage.DataSetWriterName), writer.Name);
                    }

                    if ((dscm & JsonDataSetMessageContentMask.MetaDataVersion) != 0)
                    {
                        encoder.WriteEncodeable(nameof(JsonDataSetMessage.MetaDataVersion), metadata.ConfigurationVersion, null);
                    }

                    if ((dscm & JsonDataSetMessageContentMask.MinorVersion) != 0)
                    {
                        encoder.WriteUInt32(nameof(JsonDataSetMessage.MinorVersion), metadata.ConfigurationVersion.MinorVersion);
                    }

                    if ((dscm & JsonDataSetMessageContentMask.SequenceNumber) != 0)
                    {
                        encoder.WriteUInt32(nameof(JsonDataSetMessage.SequenceNumber), sequenceNumber);
                    }

                    if ((dscm & JsonDataSetMessageContentMask.Timestamp) != 0)
                    {
                        encoder.WriteDateTime(nameof(JsonDataSetMessage.Timestamp), DateTime.UtcNow);
                    }

                    if ((dscm & JsonDataSetMessageContentMask.Status) != 0)
                    {
                        encoder.WriteStatusCode(nameof(JsonDataSetMessage.Status), DataGenerator.GetRandomStatusCode());
                    }

                    encoder.PushStructure(nameof(JsonDataSetMessage.Payload));
                }

                var values = GetFieldValues(metadata);

                for (int ii = 0; ii < values.Count; ii++)
                {
                    encoder.WriteRawValue(metadata.Fields[ii], values[ii], (DataSetFieldContentMask)writer.DataSetFieldContentMask);
                }

                if ((nmcm & JsonNetworkMessageContentMask.DataSetMessageHeader) != 0)
                {
                    encoder.PopStructure();
                }

                if ((nmcm & JsonNetworkMessageContentMask.SingleDataSetMessage) != 0)
                {
                    if (popMessagesField)
                    {
                        encoder.PopStructure();
                    }

                    await PublishData(
                        topic,
                        encoder.CloseAndReturnText(),
                        GetQos(groupQos, writerQos),
                        token);

                    encoder = new JsonEncoder(m_messageContext, FindEncoding(group));
                    WriteNetworkMessageHeader(encoder, nmcm, group);
                    first = true;
                }
                else
                {
                    encoder.PopStructure();
                }
            }

            if ((nmcm & JsonNetworkMessageContentMask.SingleDataSetMessage) == 0)
            {
                if (popMessagesField)
                {
                    encoder.PopArray();
                }

                await PublishData(
                    topic,
                    encoder.CloseAndReturnText(),
                    GetQos(groupQos, BrokerTransportQualityOfService.NotSpecified),
                    token);
            }
        }
    }
    private void ReadAndIndexDataTypes(ServiceMessageContext context, Stream istrm)
    {
        var nodeset = Export.UANodeSet.Read(istrm);

        var systemContext = new SystemContext()
        {
            NamespaceUris = context.NamespaceUris,
            ServerUris = context.ServerUris
        };

        NodeStateCollection collection = new();
        nodeset.Import(systemContext, collection);

        foreach (var node in collection)
        {
            if (node.NodeClass == NodeClass.DataType)
            {
                if (!m_dataTypes.ContainsKey(node.NodeId))
                {
                    DataTypeState dt = (DataTypeState)node;
                    m_dataTypes[node.NodeId] = dt;
                }
            }
        }

        foreach (var dt in m_dataTypes.Values)
        {
            if (m_typeTable.IsKnown(dt.NodeId))
            {
                continue;
            }

            var st = dt.SuperTypeId;
            var stack = new Stack<DataTypeState>();
            stack.Push(dt);

            while (st != null)
            {
                if (!m_dataTypes.TryGetValue(st, out var type))
                {
                    break;
                }

                stack.Push(type);
                st = type.SuperTypeId;

                if (m_typeTable.IsKnown(st))
                {
                    break;
                }
            }

            while (stack.Count > 0)
            {
                var type = stack.Pop();
                m_typeTable.AddSubtype(type.NodeId, type.SuperTypeId);
            }
        }
    }

    private void LoadCoreNodeSet(ServiceMessageContext context)
    {
        var collection = new NodeStateCollection();

        foreach (var name in typeof(Opc.Ua.Namespaces).Assembly.GetManifestResourceNames())
        {
            if (name.EndsWith(".NodeSet2.xml.zip"))
            {
                using (Stream istrm = typeof(Opc.Ua.Namespaces).Assembly.GetManifestResourceStream(name))
                {
                    using (ZipArchive archive = new ZipArchive(istrm, ZipArchiveMode.Read))
                    {
                        using (Stream zstrm = archive.Entries.First().Open())
                        {
                            ReadAndIndexDataTypes(context, zstrm);
                        }
                    }
                }
            }
        }
    }

    private NodeStateCollection LoadNodeSets(ServiceMessageContext context)
    {
        var collection = new NodeStateCollection();

        foreach (var name in Assembly.GetExecutingAssembly().GetManifestResourceNames())
        {
            if (name.EndsWith(".NodeSet2.xml"))
            {
                using (Stream istrm = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
                {
                    ReadAndIndexDataTypes(context, istrm);
                }
            }
        }

        return collection;
    }

    private void AddDataTypeToMetaData(DataSetMetaDataType metadata, NodeId dataTypeId)
    {
        if (dataTypeId.NamespaceIndex <= 0)
        {
            return;
        }

        var uri = metadata.Namespaces[dataTypeId.NamespaceIndex - 1];
        var ns = m_messageContext.NamespaceUris.GetIndexOrAppend(uri);
        var nodeId = new NodeId(dataTypeId.Identifier, ns);

        if (!m_dataTypes.TryGetValue(nodeId, out var dataType))
        {
            throw new ServiceResultException(StatusCodes.BadDataTypeIdUnknown);
        }

        if (dataType.DataTypeDefinition?.Body is StructureDefinition st)
        {
            if (!metadata.StructureDataTypes.Where(x => x.DataTypeId == dataTypeId).Any())
            {
                var clone = (StructureDefinition)st.Clone();

                var superTypeId = dataType.SuperTypeId;

                while (superTypeId != null && m_dataTypes.TryGetValue(superTypeId, out var superType))
                {
                    if (superType.DataTypeDefinition?.Body is StructureDefinition superTypeDefinition)
                    {
                        clone.Fields.InsertRange(0, superTypeDefinition.Fields);
                    }

                    superTypeId = superType.SuperTypeId;
                }

                metadata.StructureDataTypes.Add(new StructureDescription()
                {
                    Name = dataType.BrowseName.Name,
                    DataTypeId = dataTypeId,
                    StructureDefinition = clone,
                });

                foreach (var field in clone.Fields)
                {
                    if (field.DataType.NamespaceIndex > 0)
                    {
                        uri = m_messageContext.NamespaceUris.GetString(field.DataType.NamespaceIndex);
                        ns = (ushort)(metadata.Namespaces.FindIndex(ns => ns == uri) + 1);
                        field.DataType = new NodeId(field.DataType.Identifier, ns);
                        AddDataTypeToMetaData(metadata, field.DataType);
                    }
                }
            }
        }
        else if (dataType.DataTypeDefinition?.Body is EnumDefinition et)
        {
            if (!metadata.EnumDataTypes.Where(x => x.DataTypeId == dataTypeId).Any())
            {
                metadata.EnumDataTypes.Add(new EnumDescription()
                {
                    Name = dataType.BrowseName.Name,
                    DataTypeId = dataTypeId,
                    EnumDefinition = et,
                    BuiltInType = (byte)Opc.Ua.TypeInfo.GetBuiltInType(nodeId, m_typeTable)
                });
            }
        }
        else
        {
            if (!metadata.SimpleDataTypes.Where(x => x.DataTypeId == dataTypeId).Any())
            {
                var baseTypeId = dataType.SuperTypeId;
                uri = m_messageContext.NamespaceUris.GetString(baseTypeId.NamespaceIndex);
                ns = (ushort)(metadata.Namespaces.FindIndex(ns => ns == uri) + 1);

                metadata.SimpleDataTypes.Add(new SimpleTypeDescription()
                {
                    Name = dataType.BrowseName.Name,
                    DataTypeId = dataTypeId,
                    BuiltInType = (byte)Opc.Ua.TypeInfo.GetBuiltInType(nodeId, m_typeTable),
                    BaseDataType = new NodeId(baseTypeId.Identifier, ns)
                });
            }
        }
    }

    private void UpdateDataSetMeta(DataSetMetaDataType metadata)
    {
        foreach (var field in metadata.Fields)
        {
            AddDataTypeToMetaData(metadata, field.DataType);
        }
    }

    private List<DataValue> GetFieldValues(DataSetMetaDataType metadata)
    {
        NamespaceTable metadataUris = new NamespaceTable();
        metadata.Namespaces.ForEach(ns => metadataUris.GetIndexOrAppend(ns));

        List<DataValue> values = new();

        foreach (var field in metadata.Fields)
        {
            object value = null;

            if (field.ValueRank <= ValueRanks.Scalar)
            {
                value = DataGenerator.GetStaticValue((BuiltInType)field.BuiltInType, field.DataType, metadataUris);
            }
            else if (field.ValueRank >= 0)
            {
                var dimensions = new int[(field.ValueRank > 0) ? field.ValueRank : 1];
                for (int ii = 0; ii < dimensions.Length; ii++) dimensions[ii] = 3;
                value = DataGenerator.GetStaticArray((BuiltInType)field.BuiltInType, field.DataType, metadataUris, dimensions);

                if (field.ValueRank > 1)
                {
                    if (value is Array array)
                    {
                        array = Utils.FlattenArray(array);

                        if ((BuiltInType)field.BuiltInType == BuiltInType.ExtensionObject && array is not IList<ExtensionObject>)
                        {
                            var copy = new ExtensionObject[array.Length];

                            for (int ii = 0; ii < array.Length; ii++)
                            {
                                copy[ii] = new ExtensionObject(array.GetValue(ii));
                            }

                            array = copy;
                        }

                        value = new Matrix(array, (BuiltInType)field.BuiltInType, dimensions);
                    }
                }
            }

            values.Add(new DataValue()
            {
                Value = value,
                StatusCode = StatusCodes.GoodLocalOverride,
                ServerTimestamp = DateTime.UtcNow,
                SourceTimestamp = DateTime.UtcNow
            });
        }

        return values;
    }

    private static uint GetVersionTime()
    {
        return (uint)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
    }
}
