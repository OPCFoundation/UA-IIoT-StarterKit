## OPC UA IIoT StarterKit – Quickstart 001 – Subscriber
### Building a Simple Publisher and Subscriber
This quickstart demonstrates how to create a compliant OPC UA PubSub Subscriber using MQTT and the JSON encoding.

The quickstart makes it clear that OPC UA PubSub is a suitable platform for developing quick prototypes without requiring OPC UA specific libraries. The quickstart publishes energy consumption metrics from a motor. 

To keep things simple it does not subscribe for any metadata and relies on pre-knowledge of the structure of the information being published, however, it does handle the status messages that allow Subscribers to automatically discover new Publishers and know if they go offline.

1. [Define the MQTT Topics](#2)
2. [Subscribe for Status Messages](#3)
3. [Subscribe for Data Messages](#4)
4. [Build and Run the Quickstarts](#5)

### <a name='1'></a>Define the MQTT Topics

OPC UA PubSub over MQTT defines a standard topic tree to facilitate auto-discovery. The quickstart defines constants that are used to create the topics using the [patterns]((https://reference.opcfoundation.org/Core/Part14/v105/docs/6.3.2)) defined in the specification.

These constants are:
```
const string TopicPrefix = "opcua";
```

The TopicPrefix should be changed before running the quickstarts to avoid conflicts with other users of the quickstarts. The [Publisher](..\UaMqttPublisher) must use the same TopicPrefix.

### <a name='2'></a>Subscribe for Status Messages
The first step is to connect to the Broker and subscribe for the Status messages.
The quickstart uses a wildcard ('#') in the topic which means it will be notified whenever any Publisher appears in the system.

```
await Subscribe(new Topic()
{
    TopicPrefix = TopicPrefix,
    MessageType = MessageTypes.Status,
    PublisherId = "#"
}.Build());
```

There are 2 types of Status Messages: cyclic and non-cyclic. For non-cyclic messages the broker reports an error if the Publisher dissappears (i.e. the broker reports the MQTT WILL message). For cyclic the message contains timestamps and the Subscriber needs to have logic to detect when a Publisher has timed out. This quickstart only handles non-cyclic status messages.

The code to process the Status message is:
```
private Task HandleStatus(MqttApplicationMessage message)
{
    byte[]? payload = message.PayloadSegment.Array;

    if (payload != null)
    {
        var json = Encoding.UTF8.GetString(payload);

        try
        {
            var status = (StatusMessage?)JsonSerializer.Deserialize(json, typeof(StatusMessage));
            Console.WriteLine($"{status?.PublisherId}: Status={((status?.Status != null) ? (PubSubState)status.Status.Value : "")}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Parsing Failed: '{e.Message}' [{json}]");
        }
    }

    return Task.CompletedTask;
}
```

### <a name='3'></a>Subscribe for Data Messages
The next step is to subscribe for data messages. The quickstart uses a wildcard ('#') in the topic which means it will be notified whenever any Publisher sends data to the default data topic. If a Publisher uses system specific data topics then the Subscriber will need to process the metadata messages.

The following code subscribes to the data messages.
```
await Subscribe(new Topic()
{
    TopicPrefix = TopicPrefix,
    MessageType = MessageTypes.Data,
    PublisherId = "#"
}.Build());
```

The following code processes the data messages:
```
private Task HandleData(MqttApplicationMessage message)
{
    byte[]? payload = message.PayloadSegment.Array;

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
```
The Subscriber does not use the class used by the Publisher and instead processes the JSON document as a set of name-value pairs. If a Subscriber needs more information about the data it would also subscribe for the DataSetMetadata but that is not required in the minimum implementation.

### <a name='5'></a>Build and Run the Quickstarts
The solution [Quickstart_001.sln]() can be use to load the Publisher and Subscriber for this quickstart.

Build and run the Subscriber and then build and run the Publisher.

The output of the Subscriber should be like:
```
Broker Certificate: 'CN=mqttdashboard.com' None
Subscriber Connected!
Subscribed: 'opcua/json/status/#'.
Received on Topic: opcua/json/status/(change-this-value)
(change-this-value): Status=Error
Subscribed: 'opcua/json/data/#'.
Press enter to exit.
Received on Topic: opcua/json/status/(change-this-value)
(change-this-value): Status=Operational
Received on Topic: opcua/json/data/(change-this-value)/WestConveyor/MotorEnergyMetrics
============================================================
  Consumption=42.40593797091618
  DutyCycle=0.20876222923765464
  CalculationPeriod=3600
============================================================
Received on Topic: opcua/json/data/(change-this-value)/WestConveyor/MotorEnergyMetrics
============================================================
  Consumption=42.62529734587679
  DutyCycle=0.8349482616987242
  CalculationPeriod=3600
============================================================
```
