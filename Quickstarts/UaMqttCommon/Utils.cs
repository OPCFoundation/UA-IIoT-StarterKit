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
using MQTTnet;
using MQTTnet.Formatter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Buffers;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace UaMqttCommon
{
    public static class Utils
    {
        public static string GetPrivateHostName()
        {
            // hash the local host name to avoid leaking private information.
            var encoding = new UTF8Encoding(false);
            var bytes = encoding.GetBytes(Dns.GetHostName());

            using (var sha = SHA256.Create())
            {
                using (var strm = new MemoryStream(bytes))
                {
                    var hash = sha.ComputeHash(strm);
                    return Convert.ToHexString(hash).Substring(0, 10).ToLowerInvariant();
                }
            }
        }

        public static async Task DeleteAllTopics(Configuration configuration, int waitPeriod, CancellationToken ct)
        {
            var factory = new MqttClientFactory();

            using (var client = factory.CreateMqttClient())
            {
                var options = new MqttClientOptionsBuilder()
                    .WithProtocolVersion(MqttProtocolVersion.V500)
                    .WithTcpServer(configuration.BrokerHost)
                    .WithWillRetain(true)
                    .WithWillDelayInterval(60)
                    .WithClientId($"{GetPrivateHostName()}.{configuration.TopicPrefix}.{configuration.PublisherId}")
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

                var response = await client.ConnectAsync(options, ct);

                if (response.ResultCode != MqttClientConnectResultCode.Success)
                {
                    Console.WriteLine($"Connect Failed: {response.ResultCode} {response.ResultCode} {response.ReasonString}");
                }
                else
                {
                    await DeleteAllTopics(factory, client, configuration, waitPeriod, ct);
                }

                var disconnectOptions = factory.CreateClientDisconnectOptionsBuilder().Build();
                await client.DisconnectAsync(disconnectOptions, ct);
            }
        }

        private static async Task DeleteAllTopics(
            MqttClientFactory factory, 
            IMqttClient client, 
            Configuration configuration,
            int waitPeriod,
            CancellationToken ct)
        {
            var callback = async (MqttApplicationMessageReceivedEventArgs args) =>
            {
                string topic = args.ApplicationMessage.Topic;

                if (args.ApplicationMessage.Retain && configuration?.PublisherId != null && topic.Contains("/" + configuration.PublisherId))
                {
                    var applicationMessage = new MqttApplicationMessageBuilder()
                        .WithTopic(topic)
                        .WithPayload("")
                        .WithRetainFlag(true)
                        .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                        .Build();

                    await client.PublishAsync(applicationMessage);
                    Console.WriteLine($"Deleted Topic: {topic}");
                }
            };

            try
            {
                client.ApplicationMessageReceivedAsync += callback;

                var filter = $"{configuration.TopicPrefix}/json/#";

                await client.SubscribeAsync(
                    factory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(f => { f.WithTopic(filter); })
                    .Build(), ct);

                await Task.Delay(10000, ct);

                await client.UnsubscribeAsync(
                    factory.CreateUnsubscribeOptionsBuilder()
                    .WithTopicFilter(filter)
                    .Build(), ct);
            }
            finally
            {
                client.ApplicationMessageReceivedAsync -= callback;
            }
        }

        public static int GetVersionTime()
        {
            return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        public static JObject ToObject(string typeId, object body)
        {
            var parent = (body != null) ? JObject.FromObject(body) : new JObject();
            
            if (typeId != null)
            {
                parent.AddFirst(new JProperty("UaTypeId", typeId));
            }

            return parent;
        }

        public static T? FromJson<T>(ReadOnlySequence<byte> sequence) where T : class
        {
            using var stream = new MemoryStream();

            foreach (var segment in sequence)
            {
                stream.Write(segment.Span);
            }
            
            stream.Position = 0;

            using var reader = new StreamReader(stream);
            return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
        }

        public static T? FromJson<T>(JObject @object) where T : class
        {
            return JsonConvert.DeserializeObject<T>(@object.ToString());
        }

        public static string ToJson(object body)
        {
            return JObject.FromObject(
                body,
                JsonSerializer.Create(
                    new JsonSerializerSettings()
                    {
                        Formatting = Formatting.None,
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    }
                )
            ).ToString();
        }
    }
}
