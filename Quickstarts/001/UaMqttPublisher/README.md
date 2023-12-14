## OPC UA IIoT StarterKit – Quickstart 001 – Publisher
### Building a Simple Publisher and Subscriber
This quickstart demonstrates how to create a compliant OPC UA PubSub Publisher using MQTT and the JSON encoding.

The quickstart makes it clear that OPC UA PubSub is a suitable platform for developing quick prototypes without requiring OPC UA specific libraries. The quickstart publishes energy consumption metrics from a motor. 

To keep things simple it does not publish any metadata, however, it does send status messages that allow Subscribers to automatically discover new Publishers and know if they go offline.

1. [Create Classes for Message Serialization](#1)
2. [Define the MQTT Topics](#2)
3. [Publish the Status Message](#3)
4. [Publish the Data Message](#4)
5. [Build and Run the Quickstarts](#5)

### <a name='1'></a>Create Classes for Message Serialization
Publisher discovery with OPC UA PubSub relies on [Status Messages](https://reference.opcfoundation.org/Core/Part14/v105/docs/7.2.5.5.5).  
The following is a C# class that can be used to create the Status Messages:
```
internal class StatusMessage
{
    public string? MessageId { get; set; }
    public string? MessageType { get; set; }
    public string? PublisherId { get; set; }
    public DateTime? Timestamp { get; set; }
    public bool? IsCyclic { get; set; }
    public int? Status { get; set; }
    public DateTime? NextReportTime { get; set; }
}
```

Data is published with [DataSetMessages]([NetworkMessages](https://reference.opcfoundation.org/Core/Part14/v105/docs/7.2.5.4)) contained in [NetworkMessages](https://reference.opcfoundation.org/Core/Part14/v105/docs/7.2.5.3). This quickstart uses the minimal [Header Profile](https://reference.opcfoundation.org/Core/Part14/v105/docs/A.3.2) with one DataSetMessage per NetworkMessage and no header information. This means the following C# class can be used the serialize the NetworkMessages:

```
internal class EnergyMetrics
{
    public double? Consumption { get; set; }
    public double? DutyCycle { get; set; }
    public double? CalculationPeriod { get; set; }
}
```

### <a name='2'></a>Define the MQTT Topics
OPC UA PubSub over MQTT defines a standard topic tree to facilitate auto-discovery. The quickstart defines constants that are used to create the topics using the [patterns]((https://reference.opcfoundation.org/Core/Part14/v105/docs/6.3.2)) defined in the specification.

These constants are:
```
const string BrokerUrl = "broker.hivemq.com";
const string TopicPrefix = "opcua";
const string PublisherId = "(change-this-value)";
const string GroupName = "WestConveyor";
const string WriterName = "MotorEnergyMetrics";
```

The TopicPrefix should be changed before running the quickstarts to avoid conflicts with other users of the quickstarts. The PublisherId, GroupName and DataSetName describe the source of the data.

### <a name='3'></a>Publish the Status Message

The first step is to connect to the Broker and publish the Status Message:
```
private async Task PublishStatus()
{
    if (m_client == null || m_factory == null) throw new InvalidOperationException();

    var topic = new Topic()
    {
        TopicPrefix = TopicPrefix,
        MessageType = MessageTypes.Status,
        PublisherId = PublisherId
    }.Build();

    StatusMessage payload = new StatusMessage()
    {
        MessageId = Guid.NewGuid().ToString(),
        PublisherId = PublisherId,
        Status = (int)PubSubState.Operational
    };

    var json = JsonSerializer.Serialize(payload);

    var applicationMessage = new MqttApplicationMessageBuilder()
        .WithTopic(topic)
        .WithPayload(json)
        .WithMessageExpiryInterval(7200)
        .WithRetainFlag(true)
        .Build();

    var result = await m_client.PublishAsync(applicationMessage, CancellationToken.None);

    if (!result.IsSuccess)
    {
        Console.WriteLine($"Error: {result.ReasonCode} {result.ReasonString}");
    }
}
```
This version of the Status Message is non-cyclic which means the Publisher only sends it once. However, this also means the Publisher must provide a WILL message when connecting to the Broker. The WILL message is sent when the Broker detects that Publisher has disappeared. 

The following code initializes the connection with the WILL message:

```
var willTopic = new Topic()
{
    TopicPrefix = TopicPrefix,
    MessageType = MessageTypes.Status,
    PublisherId = PublisherId
}.Build();

StatusMessage willPayload = new StatusMessage()
{
    MessageId = Guid.NewGuid().ToString(),
    PublisherId = PublisherId,
    Status = (int)PubSubState.Error
};

var json = JsonSerializer.Serialize(willPayload);

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
```

### <a name='4'></a>Publish the Data Messages
The next step is to publish data messages. The quickstart uses random data to create data messages once every 5s.

The following code publishes the data messages:
```
var topic = new Topic()
{
    TopicPrefix = TopicPrefix,
    MessageType = MessageTypes.Data,
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

    var json = JsonSerializer.Serialize(data);

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
```
This quickstart uses the default data topic, however, OPC UA PubSub allows Publishers to use whatever topic structure they need for data. The only caveat is if a non-default data topic message is used then Subscribers will have to handle metadata messages if they wish to discover Publishers. This capability was left out to make this quickstart as simple as possible.

### <a name='5'></a>Build and Run the Quickstarts
The solution [Quickstart_001.sln](../Quickstart_001.sln) can be use to load the Publisher and Subscriber for this quickstart.

Build and run the Subscriber and then build and run the Publisher.

The output of the Publisher should be like:
```
Broker Certificate: 'CN=mqttdashboard.com' None
Publisher Connected!
Status Message Sent.
Press enter to exit.
Consumption=42.2750502349597; DutyCycle=0.4096448125101504
Consumption=42.5483906302368; DutyCycle=0.5207218102201033
```

