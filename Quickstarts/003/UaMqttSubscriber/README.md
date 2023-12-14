## OPC UA IIoT StarterKit – Quickstart 002 - Subscriber
### Using Connection Metadata to discover Message Structure
This quickstart demonstrates how to modify the simple Subscriber from [Quickstart_002](../../002/) to consume PubSubConnection metadata.

1. [Subscribe for the PubSubConnection Metadata](#1)
2. [Subscribe to Data Messages based on PubSubConnection Metadata](#2)
3. [Build and Run the Quickstarts](#3)

### <a name='1'></a>Subscribe for the PubSubConnection Metadata
The PubSubConnection metadata is sent to the topic:
```
opc.ua/json/connection/<PublisherId>
```
The quickstart subscribes to connection messages in the Operational state. 

The code to establish the subscription is:
```
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
```
If the status is not Operational the Subscriber unsubscribes from the connection topic and any data topics associated with the Publisher.

### <a name='2'></a>Subscribe to Data Messages based on PubSubConnection Metadata
When a PubSubConnection metadata arrives the Subscriber parses checks the structure of the published messages. If the structure follows the [JSON-NetworkMessage Header Layout Profile](https://reference.opcfoundation.org/Core/Part14/v105/docs/A.3.4) the it subscribes to the Data and Metadata topics.

If the Publisher Status is no longer operational Data and Metadata topics are unsubscribed.

The PubSubConnection also allows the Publisher to specify system specific Data and Metadata topics. These are useful when a system has an existing topic structure that a new Publisher need to conform to.

The code to select and subscribe to the Data topics is:
```  
// this quickstart is only interested in data messages with multiple messages.
if (group.HasMultipleDataSetMessages && group.HasNetworkMessageHeader)
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
```
In this example, Data messages may have data from multiple writers so the topic is specified at the group level. 

DataSetMetaData messages are specific to the DataSetWriter so the DataSetMetaData topic is specified at the DataSetWriter level. This is true even if the number and type of fields are the same because the value of additional Properties may be different.

### <a name='3'></a>Build and Run the Quickstarts
The solution [Quickstart_003.sln]() can be used to load the Publisher and Subscriber for this quickstart.

Build and run the Subscriber and then build and run the Publisher.

The output of the Subscriber should be like:
```
Broker Certificate: 'CN=mqttdashboard.com' None
Subscriber Connected!
Subscribed: 'opcua/json/status/#'.
Press enter to exit.
Received on Topic: opcua/json/status/(Quickstart003)
(Quickstart003): Status=Operational
Subscribed: 'opcua/json/connection/(Quickstart003)'.
Received on Topic: opcua/json/connection/(Quickstart003)
Subscribed: 'opcua/json/data/(Quickstart003)/Conveyor/#'.
Subscribed: 'opcua/json/metadata/(Quickstart003)/Conveyor/#'.
Received on Topic: opcua/json/metadata/(Quickstart003)/Conveyor/Motor1
DataSetMetaData Message: '(Quickstart003).101'
Received on Topic: opcua/json/metadata/(Quickstart003)/Conveyor/Motor2
DataSetMetaData Message: '(Quickstart003).201'
Received on Topic: opcua/json/data/(Quickstart003)/Conveyor
============================================================
PublisherId: (Quickstart003)
DataSetWriterId: 101
SequenceNumber: 2
MinorVersion: 1698634786
Timestamp: 02:59:57.508
------------------------------------------------------------
Consumption=42.750221182834586 kW·h
DutyCycle=0.9367880361923502 %
CalculationPeriod=3600 ms
============================================================
============================================================
PublisherId: (Quickstart003)
DataSetWriterId: 201
SequenceNumber: 3
MinorVersion: 1698634786
Timestamp: 02:59:57.508
------------------------------------------------------------
Consumption=684.3517009274635 electric hp
DutyCycle=0.6347297202574974 %
CalculationPeriod=3600 ms
============================================================
```
