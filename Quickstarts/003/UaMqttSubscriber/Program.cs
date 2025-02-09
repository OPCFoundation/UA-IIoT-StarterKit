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
using Newtonsoft.Json.Linq;
using Opc.Ua.WebApi;
using Opc.Ua.WebApi.Model;
using UaMqttCommon;

await new Subscriber().Connect();

internal class Subscriber
{
    const string BrokerUrl = "broker.hivemq.com";
    const string TopicPrefix = "opcua-quickstarts";

    private MqttClientFactory m_factory;
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
        m_factory = new MqttClientFactory();

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

                if (topic.StartsWith($"{TopicPrefix}/json/{TopicTypes.Status}"))
                {
                    await HandleStatus(args.ApplicationMessage);
                }
                else if (topic.StartsWith($"{TopicPrefix}/json/{TopicTypes.Connection}"))
                {
                    await HandleConnection(args.ApplicationMessage);
                }
                else if (topic.StartsWith($"{TopicPrefix}/json/{TopicTypes.DataSetMetaData}"))
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
                MessageType = TopicTypes.Status,
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

    private void HandleDataSetMessage(JsonNetworkMessage nm, JsonDataSetMessage dm)
    {
        var publisherId = $"{dm?.PublisherId ?? nm?.PublisherId}";

        Console.WriteLine(new string('=', 60));
        Console.WriteLine($"PublisherId: {publisherId}");
        Console.WriteLine($"DataSetWriterId: {dm?.DataSetWriterId}");
        Console.WriteLine($"SequenceNumber: {dm?.SequenceNumber}");
        Console.WriteLine($"MinorVersion: {dm?.MinorVersion}");
        Console.WriteLine($"Timestamp: {dm?.Timestamp:HH:mm:ss.fff}");
        Console.WriteLine(new string('-', 60));

        var data = dm?.Payload as JObject;

        if (data != null)
        {
            var writerId = $"{publisherId}.{dm?.DataSetWriterId}";

            lock (m_writers)
            {
                if (!m_writers.TryGetValue(writerId, out var writer))
                {
                    Console.WriteLine($"Writer for Data message not found: {writerId}");
                }

                foreach (var item in data)
                {
                    if (writer != null && writer.EngineeringUnits.TryGetValue(item.Key, out var unit))
                    {
                        Console.WriteLine($"{item.Key}={item.Value} {unit}");
                    }
                    else
                    {
                        Console.WriteLine($"{item.Key}={item.Value}");
                    }
                }
            }
        }

        Console.WriteLine(new string('=', 60));
    }

    private Task HandleData(MqttApplicationMessage message)
    {
        try
        {
            Group group = null;

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
            if (group.HasNetworkMessageHeader && group.HasMultipleDataSetMessages)
            {
                var nm = Utils.FromJson<JsonNetworkMessage>(message.Payload);
                 
                if (nm?.Messages != null)
                {
                    var list = nm.Messages as JArray;

                    if (list != null)
                    {
                        foreach (var item in list.OfType<JObject>())
                        {
                            var dm = Utils.FromJson<JsonDataSetMessage>(item);
                            HandleDataSetMessage(nm, dm);
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Parsing Failed: '{e.Message}'");
        }

        return Task.CompletedTask;
    }

    private async Task HandleStatus(MqttApplicationMessage message)
    {
        try
        {
            var status = Utils.FromJson<JsonStatusMessage>(message.Payload);
            Console.WriteLine($"{status?.PublisherId}: Status={(PubSubState)status.Status}");

            var connectionTopic = new Topic()
            {
                TopicPrefix = TopicPrefix,
                MessageType = TopicTypes.Connection,
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
            Console.WriteLine($"Parsing Failed: '{e.Message}'");
        }
    }

    private Task HandleDataSetMetaData(MqttApplicationMessage message)
    {
        try
        {
            var metadata = Utils.FromJson<JsonDataSetMetaDataMessage>(message.Payload);
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
                            .Where(x => x.Key == BrowseNames.EngineeringUnits)
                            .Select(x => x.Value)
                            .FirstOrDefault();

                        if (value?.UaType == (int)BuiltInType.ExtensionObject && value?.Value is JObject element)
                        {
                            var eu = ((JObject)value.Value).ToObject<EUInformation>();

                            if (eu != null)
                            {
                                fields[field.Name] = eu.DisplayName?.Text ?? String.Empty;
                            }
                        }
                    }

                    writer.EngineeringUnits = fields;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Parsing Failed: '{e.Message}'");
        }

        return Task.CompletedTask;
    }

    public async Task HandleConnection(MqttApplicationMessage message)
    {
        JsonPubSubConnectionMessage connection;

        try
        {
            connection = Utils.FromJson<JsonPubSubConnectionMessage>(message.Payload);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Parsing Failed: '{e.Message}'");
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

                if (ii?.MessageSettings != null)
                {
                    if (ii?.MessageSettings is JObject item)
                    {
                        var wgms = Utils.FromJson<JsonWriterGroupMessageDataType>(item);
                        var mask = wgms?.NetworkMessageContentMask;
                        group.HasNetworkMessageHeader = (mask & 0x01) != 0;
                        group.HasDataSetMessageHeader = (mask & 0x02) != 0;
                        group.HasMultipleDataSetMessages = (mask & 0x04) == 0;
                    }
                }

                // this quickstart is only interested in data messages with multiple messages.
                if (group.HasMultipleDataSetMessages && group.HasNetworkMessageHeader)
                {
                    if (ii?.TransportSettings is JObject item)
                    {
                        var wgts = Utils.FromJson<BrokerWriterGroupTransportDataType>(item);
                        group.DataTopic = wgts?.QueueName;
                    }

                    if (group.DataTopic == null)
                    {
                        group.DataTopic = new Topic()
                        {
                            TopicPrefix = TopicPrefix,
                            MessageType = TopicTypes.Data,
                            PublisherId = connection?.PublisherId,
                            GroupName = ii?.Name,
                            WriterName = "#"
                        }.Build();
                    }

                    await Subscribe(group.DataTopic);
                }

                if (ii?.DataSetWriters != null)
                {
                    foreach (var jj in ii.DataSetWriters)
                    {
                        string metadataTopic = null;

                        if (ii?.TransportSettings is JObject item)
                        {
                            var dswts = Utils.FromJson<BrokerDataSetWriterTransportDataType>(item);
                            metadataTopic = dswts?.MetaDataQueueName;
                        }

                        if (metadataTopic == null)
                        {
                            metadataTopic = new Topic()
                            {
                                TopicPrefix = TopicPrefix,
                                MessageType = TopicTypes.DataSetMetaData,
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
