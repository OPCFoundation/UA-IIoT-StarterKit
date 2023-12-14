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
using System.Text;
using System.Text.Json.Nodes;
using MQTTnet;
using MQTTnet.Client;
using Opc.Ua;
using UaMqttCommon;

await new Subscriber().Connect();

internal class Subscriber
{
    const string BrokerUrl = "broker.hivemq.com";
    const string TopicPrefix = "opcua";

    private ServiceMessageContext m_context;
    private MqttFactory m_factory;
    private IMqttClient m_client;
    private readonly Dictionary<string, Writer> m_writers = new();
    private readonly Dictionary<string, Group> m_groups = new();

    private class Group
    {
        public string PublisherId { get; set; }
        public string GroupName { get; set; }
        public bool HasNetworkMessageHeader { get; set; }
        public bool HasDataSetMessageHeader { get; set; }
        public bool HasMultipleDataSetMessages { get; set; }
        public string DataTopic { get; set; }
        public List<Writer> Writers { get; set; } = new();
    }

    private class Writer
    {
        public string PublisherId { get; set; }
        public string WriterName { get; set; }
        public int? DataSetWriterId { get; set; }
        public WriterGroupDataType WriterGroup { get; set; }
        public DataSetWriterDataType DataSetWriter { get; set; }
        public DataSetMetaDataType DataSetMetaData { get; set; }
        public Dictionary<string, string> EngineeringUnits { get; set; } = new();
        public string MetaDataTopic { get; set; }
    }

    public async Task Connect()
    {
        m_context = new ServiceMessageContext();
        m_factory = new MqttFactory();

        using (m_client = m_factory.CreateMqttClient())
        {
            var options = new MqttClientOptionsBuilder().WithTcpServer(BrokerUrl)
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
                Console.WriteLine("Subscriber Connected!");
            }

            m_client.ApplicationMessageReceivedAsync += async delegate (MqttApplicationMessageReceivedEventArgs args)
            {
                string topic = args.ApplicationMessage.Topic;

                Console.WriteLine($"Received on Topic: {topic}");

                if (topic.StartsWith($"{TopicPrefix}/json/{MessageTypes.Status}"))
                {
                    await HandleStatus(args.ApplicationMessage);
                }
                else if (topic.StartsWith($"{TopicPrefix}/json/{MessageTypes.Connection}"))
                {
                    await HandleConnection(args.ApplicationMessage);
                }
                else if (topic.StartsWith($"{TopicPrefix}/json/{MessageTypes.DataSetMetaData}"))
                {
                    await HandleDataSetMetaData(args.ApplicationMessage);
                }
                else
                {
                    await HandleData(args.ApplicationMessage);
                }
            };

            await Subscribe(new Topic()
            {
                TopicPrefix = TopicPrefix,
                MessageType = MessageTypes.Status,
                PublisherId = "#"
            }.Build());

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();

            var disconnectOptions = m_factory.CreateClientDisconnectOptionsBuilder().Build();
            await m_client.DisconnectAsync(disconnectOptions, CancellationToken.None);
            Console.WriteLine("Subscriber Disconnected!");
        }
    }

    private async Task Subscribe(string topic)
    {
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

        var options = m_factory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(f => { f.WithTopic(topic); })
            .Build();

        var response = await m_client.SubscribeAsync(options, CancellationToken.None);

        if (!String.IsNullOrEmpty(response?.ReasonString))
        {
            Console.WriteLine($"Subscribe Failed:'{response?.ReasonString}'.");
        }
        else
        {
            Console.WriteLine($"Subscribed: '{topic}'.");
        }
    }
    private async Task Unsubscribe(string topic)
    {
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

        var options = m_factory.CreateUnsubscribeOptionsBuilder()
            .WithTopicFilter(topic)
            .Build();

        var response = await m_client.UnsubscribeAsync(options, CancellationToken.None);

        if (!String.IsNullOrEmpty(response?.ReasonString))
        {
            Console.WriteLine($"Unsubscribe Failed: '{response?.ReasonString}'.");
        }
        else
        {
            Console.WriteLine($"Unsubscribed: '{topic}'.");
        }
    }

    private void WriteFieldValue(string name, DataSetField field, Writer writer)
    {
        StringBuilder sb = new();

        if (field?.SourceTimestamp != null)
        {
            sb.Append($"[{field?.SourceTimestamp:HH:mm:ss}] ");
        }

        if (StatusCode.IsNotGood(field.StatusCode ?? 0))
        {
            sb.Append($"{name}=[{field?.StatusCode}] ");
        }

        if (field?.Value is JsonObject @object)
        {
            if (@object.ContainsKey("Body"))
            {
                sb.Append($"{name}={@object["Body"]}");
            }
            else
            {
                sb.Append($"{name}={@object}");
            }
        }
        else
        {
            sb.Append($"{name}={field?.Value}");
        }

        if (writer != null)
        {
            lock (m_writers)
            {
                if (writer.EngineeringUnits.TryGetValue(name, out var unit))
                {
                    sb.Append($" {unit}");
                }
            }
        }

        Console.WriteLine(sb.ToString());
    }

    private void HandleDataSetMessage(Topic topic, JsonDataSetMessage dm)
    {
        var publisherId = $"{dm?.PublisherId}";

        Console.WriteLine(new string('=', 60));

        if (!dm.ExcludeHeader)
        {
            Console.WriteLine($"PublisherId: {publisherId}");
            Console.WriteLine($"DataSetWriterId: {dm?.DataSetWriterId}");
            Console.WriteLine($"MessageType: {dm?.MessageType}");
            Console.WriteLine($"SequenceNumber: {dm?.SequenceNumber}");
            Console.WriteLine($"MinorVersion: {dm?.MinorVersion}");
            Console.WriteLine($"Timestamp: {dm?.Timestamp:HH:mm:ss.fff}");
            Console.WriteLine(new string('-', 60));
        }

        if (dm.Payload != null)
        {
            Writer writer = null;

            // find the writer using information in the header.
            if (!dm.ExcludeHeader)
            {
                var writerId = $"{publisherId}.{dm?.DataSetWriterId}";

                lock (m_writers)
                {
                    if (!m_writers.TryGetValue(writerId, out writer))
                    {
                        Console.WriteLine($"[Writer for Data message not found: {writerId}]");
                    }
                }
            }

            // find the writer using information in the topic.
            else
            {
                var groupId = $"{topic?.PublisherId}.{topic?.GroupName}";

                lock (m_writers)
                {
                    if (m_groups.TryGetValue(groupId, out var group))
                    {
                        writer = group.Writers.Where(x => x.WriterName == topic?.WriterName).FirstOrDefault();
                    }
                }

                if (writer == null)
                {
                    Console.WriteLine($"[Writer for Data message not found: {topic?.WriterName}]");
                }
            }

            foreach (var item in dm.Payload)
            {
                var field = DataSetField.Decode(m_context, item.Value.ToJsonString());
                WriteFieldValue(item.Key, field, writer);
            }
        }

        Console.WriteLine(new string('=', 60));
    }

    private Task HandleData(MqttApplicationMessage message)
    {
        byte[] payload = message.PayloadSegment.Array;

        if (payload != null)
        {
            var json = Encoding.UTF8.GetString(payload);

            try
            {
                var nm = JsonNetworkMessage.Decode(m_context, json);

                if (nm?.Messages != null)
                {
                    var topic = Topic.Parse(message.Topic);

                    foreach (var dm in nm.Messages)
                    {
                        HandleDataSetMessage(topic, dm);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Parsing Failed: '{e.Message}' [{json}]");
            }
        }

        return Task.CompletedTask;
    }

    private async Task HandleStatus(MqttApplicationMessage message)
    {
        byte[] payload = message.PayloadSegment.Array;

        if (payload != null)
        {
            var json = Encoding.UTF8.GetString(payload);

            try
            {
                var status = JsonStatusMessage.Decode(m_context, json);
                Console.WriteLine($"{status?.PublisherId}: Status={((status?.Status != null) ? status.Status.Value : "")}");

                var connectionTopic = new Topic()
                {
                    TopicPrefix = TopicPrefix,
                    MessageType = MessageTypes.Connection,
                    PublisherId = status?.PublisherId
                }.Build();

                if (status?.Status == PubSubState.Operational)
                {
                    await Subscribe(connectionTopic);
                }
                else
                {
                    await Unsubscribe(connectionTopic);

                    // unsubscribe from all data topics for this publisher.
                    List<string> topics = new();

                    lock (m_writers)
                    {
                        foreach (var group in m_groups.Values)
                        {
                            if (group.PublisherId == status?.PublisherId)
                            {
                                if (group.DataTopic != null)
                                {
                                    topics.Add(group.DataTopic);
                                }

                                foreach (var writer in group.Writers)
                                {
                                    if (writer.MetaDataTopic != null)
                                    {
                                        topics.Add(writer.MetaDataTopic);
                                    }
                                }
                            }
                        }
                    }

                    foreach (var topic in topics)
                    {
                        await Unsubscribe(topic);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Parsing Failed: '{e.Message}' [{json}]");
            }
        }
    }

    private Task HandleDataSetMetaData(MqttApplicationMessage message)
    {
        byte[] payload = message.PayloadSegment.Array;

        if (payload != null)
        {
            var json = Encoding.UTF8.GetString(payload);

            try
            {
                var metadata = JsonDataSetMetaDataMessage.Decode(m_context, json);
                var writerId = $"{metadata?.PublisherId}.{metadata?.DataSetWriterId}";
                var source = metadata?.MetaData?.Fields;

                Console.WriteLine($"DataSetMetaData Message: '{writerId}'");

                if (source != null)
                {
                    lock (m_writers)
                    {
                        if (!m_writers.TryGetValue(writerId, out var writer))
                        {
                            writer = new Writer()
                            {
                                PublisherId = metadata?.PublisherId,
                                DataSetMetaData = metadata?.MetaData
                            };

                            m_writers[writerId] = writer;
                        }

                        Dictionary<string, string> fields = writer.EngineeringUnits;

                        foreach (var field in source)
                        {
                            if (field.Name == null || field.Properties == null)
                            {
                                continue;
                            }

                            var value = field.Properties
                                .Where(x => x.Key?.Name == BrowseNames.EngineeringUnits)
                                .Select(x => x.Value)
                                .FirstOrDefault();

                            if (value != Variant.Null)
                            {
                                var eu = ExtensionObject.ToEncodeable((ExtensionObject)value.Value) as EUInformation;

                                if (eu != null)
                                {
                                    fields[field.Name] = eu.DisplayName.Text;
                                }
                            }
                        }

                        writer.EngineeringUnits = fields;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Parsing Failed: '{e.Message}' [{json}]");
            }
        }

        return Task.CompletedTask;
    }

    public async Task HandleConnection(MqttApplicationMessage message)
    {
        byte[] payload = message.PayloadSegment.Array;

        if (payload == null)
        {
            return;
        }

        JsonPubSubConnectionMessage connection;
        var json = Encoding.UTF8.GetString(payload);

        try
        {
            connection = JsonPubSubConnectionMessage.Decode(m_context, json);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Parsing Failed: '{e.Message}' [{json}]");
            return;
        }

        if (connection?.Connection?.WriterGroups != null)
        {
            foreach (var ii in connection.Connection.WriterGroups)
            {
                var groupId = $"{connection?.PublisherId}.{ii?.Name}";
                Group group = null;

                lock (m_writers)
                {
                    if (!m_groups.TryGetValue(groupId, out group))
                    {
                        group = new Group()
                        {
                            PublisherId = connection?.PublisherId,
                            GroupName = ii?.Name
                        };

                        m_groups[groupId] = group;
                    }
                }

                group.HasNetworkMessageHeader = false;
                group.HasDataSetMessageHeader = false;
                group.HasMultipleDataSetMessages = false;
                group.DataTopic = null;

                if (ii?.MessageSettings?.Body is JsonWriterGroupMessageDataType wgm)
                {
                    var mask = wgm.NetworkMessageContentMask;
                    group.HasNetworkMessageHeader = (mask & 0x01) != 0;
                    group.HasDataSetMessageHeader = (mask & 0x02) != 0;
                    group.HasMultipleDataSetMessages = (mask & 0x04) == 0;
                }

                if (ii?.TransportSettings?.Body is BrokerWriterGroupTransportDataType wgt)
                {
                    group.DataTopic = wgt.QueueName;
                }

                if (group.DataTopic == null)
                {
                    group.DataTopic = new Topic()
                    {
                        TopicPrefix = TopicPrefix,
                        MessageType = MessageTypes.Data,
                        PublisherId = connection?.PublisherId,
                        GroupName = ii?.Name,
                        WriterName = "#"
                    }.Build();
                }

                // this quickstart is only interested in data messages with multiple messages.
                if (!group.HasMultipleDataSetMessages && !group.HasNetworkMessageHeader)
                {
                    await Subscribe(group.DataTopic);
                }

                if (ii?.DataSetWriters != null)
                {
                    foreach (var jj in ii.DataSetWriters)
                    {
                        string metadataTopic = null;

                        if (jj?.TransportSettings?.Body is BrokerDataSetWriterTransportDataType dwt)
                        {
                            metadataTopic = dwt.MetaDataQueueName;
                        }

                        if (metadataTopic == null)
                        {
                            metadataTopic = new Topic()
                            {
                                TopicPrefix = TopicPrefix,
                                MessageType = MessageTypes.DataSetMetaData,
                                PublisherId = connection?.PublisherId,
                                GroupName = ii?.Name,
                                WriterName = jj?.Name
                            }.Build();
                        }

                        lock (m_writers)
                        {
                            var writerId = $"{connection?.PublisherId}.{jj?.DataSetWriterId}";

                            if (!m_writers.TryGetValue(writerId, out var writer))
                            {
                                writer = new Writer()
                                {
                                    PublisherId = connection?.PublisherId,
                                    DataSetWriterId = jj?.DataSetWriterId,
                                };

                                m_writers[writerId] = writer;
                                group.Writers.Add(writer);
                            }

                            writer.MetaDataTopic = metadataTopic;
                            writer.WriterGroup = ii;
                            writer.DataSetWriter = jj;
                        }

                        await Subscribe(metadataTopic);
                    }
                }
            }
        }
    }
}
