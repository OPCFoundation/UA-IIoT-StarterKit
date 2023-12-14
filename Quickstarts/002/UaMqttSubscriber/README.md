## OPC UA IIoT StarterKit – Quickstart 002 – Subscriber
### Describing the Content of Data Messages
This quickstart demonstrates how to update [Quickstart_001](../../001/) to to handle DataSetMetaData messages that describe the content of the Data messages and to use the DataSetMessage header to correlate the Data message with the DataSetMetaData.

1. [Subscribe for the DataSet Metadata](#1)
2. [Cache Metadata for use when Data Messages Arrive](#2)
3. [Display the EngineeringUnits when displaying Data Message](#3)
4. [Build and Run the Quickstarts](#4)

### <a name='1'></a>Subscribe for the DataSet Metadata
The DataSet metadata is sent to the topic:
```
opcua/json/metadata/(change-this-value)/WestConveyor/MotorEnergyMetrics
```
The quickstart subscribes to the DataSet metadata for Publishers that are in the Operational state. This is done in the method that handles the Status message:
```
var dataTopic = new Topic()
{
    TopicPrefix = TopicPrefix,
    MessageType = MessageTypes.Data,
    PublisherId = status?.PublisherId,
    GroupName = "#"
}.Build();

var metaDataTopic = new Topic()
{
    TopicPrefix = TopicPrefix,
    MessageType = MessageTypes.DataSetMetaData,
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
```
A Subscriber could subscribe for all DataSet metadata messages, however, DataSet metadata messages are retained which could result in a lot of messages that are not currently active. OTOH, subscribing/unsubscribing to Data messages could result in missed messages during short term interruptions. A sophicated subscriber could use the contents of the DataSetMetadata to determine which Data messages to subscribe to and have a timeout before unsubscribing if the Status goes into a non-Operational state.

### <a name='2'></a>Cache Metadata for use when Data Messages Arrive
When a DataSetMetadata messsage arrives the Subscriber parses it caches the EngineeringUnit information. When a Data message arrives the Subscriber displays the EngineeringUnits with the value.

The Writer class is used to cache information from different metadata messages:
```
private class Writer
{
    public string? PublisherId { get; set; 
    public int? DataSetWriterId { get; set; }
    public DataSetMetaDataType? DataSetMetaData { get; set; }
    public Dictionary<string, string> EngineeringUnits { get; set; } = new();
}
```
The DataSetMetaData and EngineeringUnits are updated when a DataSetMetaData message arrives. The messages arrive on different threads so all access to the cache is protected by a lock.

The unique key for the Writer is the PublisherId and the DataSetWriterId. This information is also in the Data message header which allows the associated Writer to be found when a Data message arrives.

The code to cache the EngineeringUnits is:
```
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
```

### <a name='3'></a>Display the EngineeringUnits when displaying Data Message
To show that the DataSetMetaData is being used the Subscriber displays the EngineeringUnits with the value. 

Every minute the Publisher sends a new DataSet metadata message with different EngineeringUnits.

The code to display the EngineeringUnits is:
```
var writerId = $"{dm?.PublisherId}.{dm?.DataSetWriterId}";

lock (m_writers)
{
    if (!m_writers.TryGetValue(writerId, out var writer))
    {
        Console.WriteLine($"Writer for Data message not found: {writerId}");
    }

    foreach (var item in data.AsObject())
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
```

### <a name='4'></a>Build and Run the Quickstarts
The solution [Quickstart_002.sln]() can be used to load the Publisher and Subscriber for this quickstart.

Build and run the Subscriber and then build and run the Publisher.

The output of the Subscriber should be like:
```
Broker Certificate: 'CN=mqttdashboard.com' None
Subscriber Connected!
Subscribed: 'opcua/json/status/#'.
Press enter to exit.
Received on Topic: opcua/json/status/(Quickstart002)
(Quickstart002): Status=Operational
Subscribed: 'opcua/json/data/(Quickstart002)/#'.
Subscribed: 'opcua/json/metadata/(Quickstart002)/#'.
Received on Topic: opcua/json/metadata/(Quickstart002)/Conveyor/Motor
DataSetMetaData Message: '(Quickstart002).101'
Received on Topic: opcua/json/data/(Quickstart002)/Conveyor/Motor
============================================================
PublisherId: (Quickstart002)
DataSetWriterId: 101
SequenceNumber: 2
MinorVersion: 1700784364
Timestamp: 00:06:09.781
------------------------------------------------------------
Consumption=42.0457521594518 kW·h
DutyCycle=0.5563172036471359 %
CalculationPeriod=3600 ms
============================================================
```
