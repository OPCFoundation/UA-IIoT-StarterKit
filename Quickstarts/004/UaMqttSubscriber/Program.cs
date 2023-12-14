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
using System.Text.Json;
using System.Text.Json.Nodes;
using MQTTnet;
using MQTTnet.Client;
using UaMqttCommon;

await new Subscriber().Connect();

internal class Subscriber
{
    const string BrokerUrl = "broker.hivemq.com";
    const string TopicPrefix = "opcua";

    private MqttFactory? m_factory;
    private IMqttClient? m_client;
    private readonly Dictionary<string, Writer> m_writers = new();
    private readonly Dictionary<string, Group> m_groups = new();

    private class Group
    {
        public string? PublisherId { get; set; }
        public string? GroupName { get; set; }
        public bool HasNetworkMessageHeader { get; set; }
        public bool HasDataSetMessageHeader { get; set; }
        public bool HasMultipleDataSetMessages { get; set; }
        public string? DataTopic { get; set; }
        public List<Writer> Writers { get; set; } = new();
    }

    private class Writer
    {
        public string? PublisherId { get; set; }
        public string? WriterName { get; set; }
        public int? DataSetWriterId { get; set; }
        public WriterGroupDataType? WriterGroup { get; set; }
        public DataSetWriterDataType? DataSetWriter { get; set; }
        public DataSetMetaDataType? DataSetMetaData { get; set; }
        public Dictionary<string, string> EngineeringUnits { get; set; } = new();
        public string? MetaDataTopic { get; set; }
    }

    public async Task Connect()
    {
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

    private void HandleDataSetMessage(JsonDataSetMessage? dm)
    {
        var publisherId = $"{dm?.PublisherId}";

        Console.WriteLine(new string('=', 60));
        Console.WriteLine($"PublisherId: {publisherId}");
        Console.WriteLine($"MessageType: {dm?.MessageType}");
        Console.WriteLine($"DataSetWriterId: {dm?.DataSetWriterId}");
        Console.WriteLine($"SequenceNumber: {dm?.SequenceNumber}");
        Console.WriteLine($"MinorVersion: {dm?.MinorVersion}");
        Console.WriteLine($"Timestamp: {dm?.Timestamp:HH:mm:ss.fff}");
        Console.WriteLine(new string('-', 60));

        if (dm?.Payload != null)
        {
            var data = JsonObject.Parse(((JsonElement)dm?.Payload!).GetRawText());

            if (data != null)
            {
                var writerId = $"{publisherId}.{dm?.DataSetWriterId}";

                lock (m_writers)
                {
                    if (!m_writers.TryGetValue(writerId, out var writer))
                    {
                        Console.WriteLine($"Writer for Data message not found: {writerId}");
                    }

                    foreach (var item in data.AsObject())
                    {
                        var dv = (DataValue?)item.Value.Deserialize(typeof(DataValue));

                        if (dv?.StatusCode != null && dv?.StatusCode != 0)
                        {
                            Console.WriteLine($"[{dv?.SourceTimestamp:HH:mm:ss}] {item.Key}=0x{dv?.StatusCode:X8}");
                        }
                        else
                        {
                            if (writer != null && writer.EngineeringUnits.TryGetValue(item.Key, out var unit))
                            {
                                Console.WriteLine($"[{dv?.SourceTimestamp:HH:mm:ss}] {item.Key}={dv?.Value?.Body} {unit}");
                            }
                            else
                            {
                                Console.WriteLine($"[{dv?.SourceTimestamp:HH:mm:ss}] {item.Key}={dv?.Value?.Body}");
                            }
                        }
                    }
                }
            }
        }

        Console.WriteLine(new string('=', 60));
    }

    private Task HandleData(MqttApplicationMessage message)
    {
        byte[]? payload = message.PayloadSegment.Array;

        if (payload != null)
        {
            var json = Encoding.UTF8.GetString(payload);

            try
            {
                Group? group = null;

                lock (m_writers)
                {
                    // extract the publisher id and group name from the topic.
                    // if custom topics are used this information need to in NetworkMessage header.
                    var topic = Topic.Parse(message.Topic, TopicPrefix);
                    var groupId = $"{topic.PublisherId}.{topic.GroupName}";

                    if (!m_groups.TryGetValue(groupId, out group))
                    {
                        Console.WriteLine($"Writer for Data message not found: {groupId}");
                        return Task.CompletedTask;
                    }
                }

                // ignore data messages that don't follow the expected profile.
                if (!group.HasNetworkMessageHeader && !group.HasMultipleDataSetMessages)
                {
                    var dm = JsonDataSetMessage.Parse(json);
                    HandleDataSetMessage(dm);
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
        byte[]? payload = message.PayloadSegment.Array;

        if (payload != null)
        {
            var json = Encoding.UTF8.GetString(payload);

            try
            {
                var status = (JsonStatusMessage?)JsonSerializer.Deserialize(json, typeof(JsonStatusMessage));
                Console.WriteLine($"{status?.PublisherId}: Status={((status?.Status != null) ? (PubSubState)status.Status.Value : "")}");

                var connectionTopic = new Topic()
                {
                    TopicPrefix = TopicPrefix,
                    MessageType = MessageTypes.Connection,
                    PublisherId = status?.PublisherId
                }.Build();

                if (status?.Status == (int)PubSubState.Operational)
                {
                    await Subscribe(connectionTopic);
                }
                else
                {
                    await Unsubscribe(connectionTopic);

                    // unsubscribe from all data topics for this publisher.
                    List<string> topics = new();

                    lock (m_groups)
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
        byte[]? payload = message.PayloadSegment.Array;

        if (payload != null)
        {
            var json = Encoding.UTF8.GetString(payload);

            try
            {
                var metadata = (JsonDataSetMetaDataMessage?)JsonSerializer.Deserialize(json, typeof(JsonDataSetMetaDataMessage));
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
                                .Where(x => x.Key?.Name == "EngineeringUnits")
                                .Select(x => x.Value)
                                .FirstOrDefault();

                            if (value != null)
                            {
                                if (value.Type == (int)BuiltInType.ExtensionObject && value.Body != null)
                                {
                                    var eu = EUInformation.FromExtensionObject((JsonElement)value.Body);

                                    if (eu != null)
                                    {
                                        fields[field.Name] = eu.DisplayName?.Text ?? String.Empty;
                                    }
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
        byte[]? payload = message.PayloadSegment.Array;

        if (payload == null)
        {
            return;
        }

        JsonPubSubConnectionMessage? connection;
        var json = Encoding.UTF8.GetString(payload);

        try
        {
            connection = (JsonPubSubConnectionMessage?)JsonSerializer.Deserialize(json, typeof(JsonPubSubConnectionMessage));
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
                Group? group = null;

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

                if (ii?.MessageSettings?.Body != null)
                {
                    var mask = ii?.MessageSettings?.Body?.NetworkMessageContentMask;
                    group.HasNetworkMessageHeader = (mask & 0x01) != 0;
                    group.HasDataSetMessageHeader = (mask & 0x02) != 0;
                    group.HasMultipleDataSetMessages = (mask & 0x04) == 0;
                }

                // this quickstart is only interested in data messages with multiple messages.
                if (!group.HasMultipleDataSetMessages && !group.HasNetworkMessageHeader)
                {
                    group.DataTopic =
                        ii?.TransportSettings?.Body?.QueueName
                        ?? new Topic()
                        {
                            TopicPrefix = TopicPrefix,
                            MessageType = MessageTypes.Data,
                            PublisherId = connection?.PublisherId,
                            GroupName = ii?.Name,
                            WriterName = "#"
                        }.Build();

                    await Subscribe(group.DataTopic);
                }

                if (ii?.DataSetWriters != null)
                {
                    foreach (var jj in ii.DataSetWriters)
                    {
                        var metadataTopic =
                            jj?.TransportSettings?.Body?.MetaDataQueueName
                            ?? new Topic()
                            {
                                TopicPrefix = TopicPrefix,
                                MessageType = MessageTypes.Data,
                                PublisherId = connection?.PublisherId,
                                GroupName = ii?.Name,
                                WriterName = jj?.Name
                            }.Build();

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
