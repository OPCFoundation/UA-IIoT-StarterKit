## OPC UA IIoT StarterKit – Quickstart 005 - Subscriber
### Handling Multiple Header Profiles
This quickstart updates [Quickstart_004](../../004/) to use the JSON serializers which are part of the UA.NETStandard Stack. It also updates the Subscriber to handle different header profiles by inspecting the JSON messages. 

1. [Update to Data Message Handlers](#1)
2. [Build and Run the Quickstarts](#2)

### <a name='1'></a>Update 
The handcrafted message classes have all been replaced with message classes that use the types and serializers provided by the UA.NETStandard Stack. This makes it easier to write more generic code that can handle different header profiles (i.e. with and with NetworkMessage headers, single or multiple DataSetMessages in a message, with and without DataSetMessage headers, etc.).

The code to handle messages now looks like this:
```
var publisherId = $"{dm?.PublisherId}";

Console.WriteLine(new string('=', 60));

if (!dm.ExcludeHeader)
{
    Console.WriteLine($"PublisherId: {publisherId}");
    Console.WriteLine($"DataSetWriterId: {dm?.DataSetWriterId}");
    Console.WriteLine($"SequenceNumber: {dm?.SequenceNumber}");
    Console.WriteLine($"MinorVersion: {dm?.MinorVersion}");
    Console.WriteLine($"Timestamp: {dm?.Timestamp:HH:mm:ss.fff}");
    Console.WriteLine(new string('-', 60));
}

if (dm.Payload != null)
{
    Writer writer = null;

    // find the writer using information in the header.
    if (!dm.ExcludeHeader)
    {
        var writerId = $"{publisherId}.{dm?.DataSetWriterId}";

        lock (m_writers)
        {
            if (!m_writers.TryGetValue(writerId, out writer))
            {
                Console.WriteLine($"[Writer for Data message not found: {writerId}]");
            }
        }
    }

    // find the writer using information in the topic.
    else
    {
        var groupId = $"{topic?.PublisherId}.{topic?.GroupName}";

        lock (m_writers)
        {
            if (m_groups.TryGetValue(groupId, out var group))
            {
                writer = group.Writers.Where(x => x.WriterName == topic?.WriterName).FirstOrDefault();
            }
        }

        if (writer == null)
        {
            Console.WriteLine($"[Writer for Data message not found: {topic?.WriterName}]");
        }
    }

    foreach (var item in dm.Payload)
    {
        var field = DataSetField.Decode(m_context, item.Value.ToJsonString());
        WriteFieldValue(item.Key, field, writer);
    }
}

Console.WriteLine(new string('=', 60));
```
Data messages are linked to the DataSet metadata by using DataSetMessage header, if present or by parsing the topic name. 

### <a name='1'></a>Build and Run the Quickstarts
The solution [Quickstart_005.sln]() can be used to load the Publisher and Subscriber for this quickstart.

Build and run the Subscriber and then build and run the Publisher.

The output of the Subscriber should be like:
```
Broker Certificate: 'CN=mqttdashboard.com' None
Subscriber Connected!
Subscribed: 'opcua/json/status/#'.
Press enter to exit.
Received on Topic: opcua/json/status/(Quickstart005)
(Quickstart005): Status=Operational
Received on Topic: opcua/json/connection/(Quickstart005)
Subscribed: 'opcua/json/data/(Quickstart005)/Conveyor/#'.
Subscribed: 'opcua/json/metadata/(Quickstart005)/Conveyor/#'.
Received on Topic: opcua/json/metadata/(Quickstart005)/Conveyor/Motor
DataSetMetaData Message: '(Quickstart005).101'
Received on Topic: opcua/json/data/(Quickstart005)/Conveyor
============================================================
PublisherId: (Quickstart005)
DataSetWriterId: 101
SequenceNumber: 5
MinorVersion: 1699101469
Timestamp: 12:38:06.185
------------------------------------------------------------
[12:38:05] Consumption=0.47900000000000004 mV
[12:38:05] DutyCycle=0.454
============================================================
```
