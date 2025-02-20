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
using System.Text.Json;
using System.Text.Json.Serialization;
using MQTTnet;
using MQTTnet.Formatter;
using Opc.Ua.WebApi.Model;
using UaMqttCommon;
using UaMqttPublisher;

await new Publisher().Connect();

internal class Publisher
{
    readonly Configuration m_configuration = new Configuration()
    {
        BrokerHost = "broker.hivemq.com",
        BrokerPort = 1883,
        TopicPrefix = "opcua-quickstarts",
        PublisherId = "Quickstart001"
    };

    string BrokerUrl => m_configuration.BrokerHost;
    string TopicPrefix => m_configuration.TopicPrefix;
    string PublisherId => m_configuration.PublisherId;
    
    const string GroupName = "Conveyor";
    const string WriterName = "Motor";

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

            var json = JsonSerializer.Serialize(willPayload, new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault });

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

            await PublishStatus(PubSubState.Disabled);
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

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault });

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

    private async Task Publish()
    {
        if (m_client == null || m_factory == null) throw new InvalidOperationException();

        await PublishStatus(PubSubState.Operational);

        var topic = new Topic()
        {
            TopicPrefix = TopicPrefix,
            MessageType = TopicTypes.Data,
            PublisherId = PublisherId,
            GroupName = GroupName,
            WriterName = WriterName
        }.Build();

        var random = new Random();

        Console.WriteLine("Press enter to exit.");

        while (true)
        {
            var data = new EnergyMetrics()
            {
                CalculationPeriod = 3600,
                Consumption = 42 + random.NextDouble(),
                DutyCycle = random.NextDouble()
            };

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault });

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
                Console.WriteLine($"Consumption={data?.Consumption}; DutyCycle={data?.DutyCycle}");
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
        }
    }
}
