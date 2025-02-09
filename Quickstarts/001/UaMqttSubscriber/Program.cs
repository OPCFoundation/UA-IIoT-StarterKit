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
using System.Buffers;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using MQTTnet;
using Opc.Ua.WebApi.Model;
using UaMqttCommon;

await new Subscriber().Connect();

internal class Subscriber
{
    const string BrokerUrl = "broker.hivemq.com";
    const string TopicPrefix = "opcua-quickstarts";

    private MqttClientFactory? m_factory;
    private IMqttClient? m_client;

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

                if (topic.StartsWith($"{TopicPrefix}/json/{MessageTypes.Status}"))
                {
                    await HandleStatus(args.ApplicationMessage);
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

            await Subscribe(new Topic()
            {
                TopicPrefix = TopicPrefix,
                MessageType = TopicTypes.Data,
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

    private Task HandleData(MqttApplicationMessage message)
    {
        byte[]? payload = message.Payload.ToArray();

        if (payload != null)
        {
            var json = Encoding.UTF8.GetString(payload);

            try
            {
                var data = JsonObject.Parse(json);

                Console.WriteLine(new string('=', 60));

                if (data != null)
                {
                    foreach (var item in data.AsObject())
                    {
                        Console.WriteLine($"  {item.Key}={item.Value}");
                    }
                }

                Console.WriteLine(new string('=', 60));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Parsing Failed: '{e.Message}' [{json}]");
            }
        }

        return Task.CompletedTask;
    }

    private Task HandleStatus(MqttApplicationMessage message)
    {
        byte[]? payload = message.Payload.ToArray();

        if (payload != null)
        {
            var json = Encoding.UTF8.GetString(payload);

            try
            {
                var status = (JsonStatusMessage?)JsonSerializer.Deserialize(json, typeof(JsonStatusMessage));
                Console.WriteLine($"{status?.PublisherId}: Status={((status?.Status != null) ? (PubSubState)status.Status : "")}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Parsing Failed: '{e.Message}' [{json}]");
            }
        }

        return Task.CompletedTask;
    }
}
