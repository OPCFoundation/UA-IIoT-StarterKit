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

    private MqttClientFactory? m_factory;
    private IMqttClient? m_client;
    private readonly Dictionary<string, Writer> m_writers = new();

    private class Writer
    {
        public string? PublisherId { get; set; }
        public DataSetMetaDataType? DataSetMetaData { get; set; }
        public Dictionary<string, string> EngineeringUnits { get; set; } = new();
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
                else if (topic.StartsWith($"{TopicPrefix}/json/{TopicTypes.DataSetMetaData}"))
                {
                    await HandleDataSetMetaData(args.ApplicationMessage);
                }
                else if (topic.StartsWith($"{TopicPrefix}/json/{TopicTypes.Data}"))
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

    private Task HandleData(MqttApplicationMessage message)
    {
        try
        {
            var dm = Utils.FromJson<JsonDataSetMessage>(message.Payload);

            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"PublisherId: {dm?.PublisherId}");
            Console.WriteLine($"DataSetWriterId: {dm?.DataSetWriterId}");
            Console.WriteLine($"SequenceNumber: {dm?.SequenceNumber}");
            Console.WriteLine($"MinorVersion: {dm?.MinorVersion}");
            Console.WriteLine($"Timestamp: {dm?.Timestamp:HH:mm:ss.fff}");
            Console.WriteLine(new string('-', 60));

            JObject? data = (JObject?)dm?.Payload;

            if (data != null)
            {
                var writerId = $"{dm?.PublisherId}.{dm?.DataSetWriterId}";

                lock (m_writers)
                {
                    if (!m_writers.TryGetValue(writerId, out var writer))
                    {
                        Console.WriteLine($"Writer for Data message not found: {writerId}");
                    }

                    foreach (var item in data.Children<JProperty>())
                    {
                        if (item != null && writer != null && writer.EngineeringUnits.TryGetValue(item.Name, out var unit))
                        {
                            Console.WriteLine($"{item.Name}={item.Value} {unit}");
                        }
                        else
                        {
                            Console.WriteLine($"{item?.Name}={item?.Value}");
                        }
                    }
                }
            }

            Console.WriteLine(new string('=', 60));
        }
        catch (Exception e)
        {
            Console.WriteLine($"Parsing Failed: '{e.Message}'");
        }

        return Task.CompletedTask;
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

                    if (metadata?.MetaData != null)
                    {
                        writer.DataSetMetaData = metadata?.MetaData;
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

                        if (value != null)
                        {
                            if (value.UaType == (int)BuiltInType.ExtensionObject && value.Value != null)
                            {
                                var eu = ((JObject)value.Value).ToObject<EUInformation>();

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
            Console.WriteLine($"Parsing Failed: '{e.Message}'");
        }

        return Task.CompletedTask;
    }

    private async Task HandleStatus(MqttApplicationMessage message)
    {
        try
        {
            var status = Utils.FromJson<JsonStatusMessage>(message.Payload);
            Console.WriteLine($"{status?.PublisherId}: Status={((status?.Status != null) ? (PubSubState)status.Status : "")}");

            var dataTopic = new Topic()
            {
                TopicPrefix = TopicPrefix,
                MessageType = TopicTypes.Data,
                PublisherId = status?.PublisherId,
                GroupName = "#"
            }.Build();

            var metaDataTopic = new Topic()
            {
                TopicPrefix = TopicPrefix,
                MessageType = TopicTypes.DataSetMetaData,
                PublisherId = status?.PublisherId,
                GroupName = "#"
            }.Build();

            if (status?.Status == (int)PubSubState.Operational)
            {
                await Subscribe(dataTopic);
                await Subscribe(metaDataTopic);
            }
            else
            {
                await Unsubscribe(dataTopic);
                await Unsubscribe(metaDataTopic);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Parsing Failed: '{e.Message}'");
        }
    }
}
