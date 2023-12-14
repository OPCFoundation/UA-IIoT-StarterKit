## OPC UA IIoT StarterKit – Quickstart 004 - Subscriber
### Acquiring Data from a OPC UA Server
This quickstart demonstrates how to modify the Publisher from [Quickstart_003](../../003/) to acquire live data from an OPC UA Server. The Subscriber is largely the same except the Data messages now include a timestamp associated with each value and conform to the JSON-DataSetMessage HeaderLayout profile.

1. [Update to Data and Connection Message Handlers](#1)
2. [Build and Run the Quickstarts](#2)

### <a name='1'></a>Update to Data and Connection Message Handlers 
The Connection metadata processing code to subscribe Data messages that follow the JSON-DataSetMessage HeaderLayout profile:

```
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
```

Update the Data message processing code to select only messages that support the header profile:
```
// ignore data messages that don't follow the expected profile.
if (!group.HasNetworkMessageHeader && !group.HasMultipleDataSetMessages)
{
    var dm = DataSetMessage.Parse(json);
    HandleDataSetMessage(dm);
}
```

### <a name='2'></a>Build and Run the Quickstarts
The solution [Quickstart_004.sln]() can be used to load the Publisher and Subscriber for this quickstart.

Build and run the Subscriber and then build and run the Publisher.

The output of the Subscriber should be like:
```
Broker Certificate: 'CN=mqttdashboard.com' None
Subscriber Connected!
Subscribed: 'opcua/json/status/#'.
Press enter to exit.
Received on Topic: opcua/json/connection/(Quickstart004)
Subscribed: 'opcua/json/data/(Quickstart004)/Conveyor'.
Subscribed: 'opcua/json/metadata/(Quickstart004)/Conveyor/Motor'.
Received on Topic: opcua/json/metadata/(Quickstart004)/Conveyor/Motor
DataSetMetaData Message: '(Quickstart004).101'
Received on Topic: opcua/json/data/(Quickstart004)/Conveyor
============================================================
PublisherId: (Quickstart004)
MessageType: ua-deltaframe
DataSetWriterId: 101
SequenceNumber: 3
MinorVersion: 1700814536
Timestamp: 08:29:01.753
------------------------------------------------------------
[08:29:01] Consumption=-0.0187 mV
[08:29:01] DutyCycle=0.007410000000000001
============================================================
```
