## OPC UA IIoT StarterKit – Quickstart 003 - Publisher
### Using Connection Metadata to discover Message Structure
This quickstart demonstrates how to modify the simple Publisher from [Quickstart_002](../../002/) to publish its Connection metadata.

1. [Create Classes for Connection Metadata](#1)
2. [Publish the Connection Metadata Message](#2)
3. [Build and Run the Quickstarts](#3)

### <a name='1'></a>Create Classes for Connection Metadata
The Connection metadata is a large structure because it addresses many different use cases. Most developers of OPC UA solutions will rely on tools that generate code from the formal definitions in the [NodeSet](https://reference.opcfoundation.org/nodesets/?u=http://opcfoundation.org/UA/). This quickstart uses [handcrafted classes](../../UaMqttCommon/PubSubConnection.cs) that support the JSON encoding to demonstrate that a basic OPC UA PubSub implemention can be easily built without additional libraries.

The Connection metadata provides information about all of the datasets published to a broker by a Publisher. These datasets are combined into groups. A group may allow all datasets within the group to be published in a single message. In these cases, the messages are published to a topic for the group instead of the dataset. If each dataset is published in its own message the topic is specific to the dataset. 

The following code initializes the Connection metadata for the Publisher:
```
  var connection = new PubSubConnectionDataType()
{
    Name = PublisherId,
    PublisherId = new Variant() { Type = (int)BuiltInType.String, Body = PublisherId }, // Used in the metadata topic names
    Enabled = true,
    WriterGroups = new List<WriterGroupDataType>()
    {
        new WriterGroupDataType()
        {
            Name = GroupName, // Used in the metadata topic names
            HeaderLayoutUri = "http://opcfoundation.org/UA/PubSub-Layouts/JSON-NetworkMessage",
            Enabled = true,
            MessageSettings = new ExtensionObject<JsonWriterGroupMessageDataType>()
            {
                TypeId = JsonWriterGroupMessageDataType.TypeId,
                Body = new JsonWriterGroupMessageDataType()
                {
                        // NetworkMessageHeader | DataSetMessageHeader | PublisherId 
                    NetworkMessageContentMask = 0x01 | 0x02 | 0x08
                }
            },
            TransportSettings = new ExtensionObject<BrokerWriterGroupTransportDataType>()
            {
                TypeId = BrokerWriterGroupTransportDataType.TypeId,
                Body = new BrokerWriterGroupTransportDataType()
                {
                    // have to publish the Data topic name even if the standard topic is used
                    // since the Subscriber is expected to use this field to find the data.
                    // This value may be overridden at the DataSetWriter level.
                    QueueName = new Topic()
                    {
                        TopicPrefix = TopicPrefix,
                        MessageType = MessageTypes.Data,
                        PublisherId = PublisherId,
                        GroupName = GroupName
                    }.Build()
                }   
            },
            DataSetWriters = new List<DataSetWriterDataType>()
            {
                new DataSetWriterDataType()
                {
                    Name = Writer1Name, // Used in the metadata topic names.
                    DataSetFieldContentMask = 0x20, // RawData (i.e. no timestamps or status and simplified encoding)
                    KeyFrameCount = 1, // Each message has all fields.
                    Enabled = true,
                    DataSetName = DataSetName,
                    DataSetWriterId = 101, // Unique across all Writers which are part of the Connection.
                    MessageSettings = new ExtensionObject<JsonDataSetWriterMessageDataType>()
                    {
                        TypeId = JsonDataSetWriterMessageDataType.TypeId,
                        Body = new JsonDataSetWriterMessageDataType()
                        {
                                // DataSetWriterId | SequenceNumber | Timestamp | Status | MinorVersion
                                DataSetMessageContentMask = 0x01 | 0x04 | 0x08 | 0x10 | 0x400
                        }
                    },
                    TransportSettings = new ExtensionObject<BrokerDataSetWriterTransportDataType>()
                    {
                        TypeId = BrokerDataSetWriterTransportDataType.TypeId,
                        Body = new BrokerDataSetWriterTransportDataType()
                        {
                            // have to publish the MetaData topic name even if the standard topic is used
                            // since the Subscriber is expected to use this field to find the metadata. 
                            MetaDataQueueName = new Topic()
                            {
                                TopicPrefix = TopicPrefix,
                                MessageType = MessageTypes.DataSetMetaData,
                                PublisherId = PublisherId,
                                GroupName = GroupName,
                                WriterName = Writer1Name
                            }.Build()
                        }
                    }
                },
                new DataSetWriterDataType()
                {
                    Name = Writer2Name, // Used in the metadata topic names.
                    DataSetFieldContentMask = 0x20, // RawData (i.e. no timestamps or status and simplified encoding)
                    KeyFrameCount = 1, // Each message has all fields.
                    Enabled = true,
                    DataSetName = DataSetName,
                    DataSetWriterId = 201, // Unique across all Writers which are part of the Connection.
                    MessageSettings = new ExtensionObject<JsonDataSetWriterMessageDataType>()
                    {
                        TypeId = JsonDataSetWriterMessageDataType.TypeId,
                        Body = new JsonDataSetWriterMessageDataType()
                        {
                                // DataSetWriterId | SequenceNumber | Timestamp | Status | MinorVersion
                                DataSetMessageContentMask = 0x01 | 0x04 | 0x08 | 0x10 | 0x400
                        }
                    },
                    TransportSettings = new ExtensionObject<BrokerDataSetWriterTransportDataType>()
                    {
                        TypeId = BrokerDataSetWriterTransportDataType.TypeId,
                        Body = new BrokerDataSetWriterTransportDataType()
                        {
                            // have to publish the MetaData topic name even if the standard topic is used
                            // since the Subscriber is expected to use this field to find the metadata. 
                            MetaDataQueueName = new Topic()
                            {
                                TopicPrefix = TopicPrefix,
                                MessageType = MessageTypes.DataSetMetaData,
                                PublisherId = PublisherId,
                                GroupName = GroupName,
                                WriterName = Writer2Name
                            }.Build()
                        }
                    }
                }
            }
        }
    }
};
```
The Connection metadata includes the topic names that are used to publish the Data and DataSetMeta messages. These fields need to be filled in even if the standand topic names are used since the Subscriber is expected to use these fields to find Data messages.

The Connection metadata also has a few masks (i.e. [DataSetFieldContentMask](https://reference.opcfoundation.org/Core/Part14/v105/docs/6.2.4.2)) which specify the layout of the messages. In this quickstart, the layout conforms to the [multiple dataset Header Profile](https://reference.opcfoundation.org/Core/Part14/v105/docs/A.3.4) so the values are mostly fixed. 

The TypeIds are needed when a field allows for different structures depending on the type of Connection. In this example, TypeIds are fixed because only JSON messages sent to an MQTT broker are used. The numeric values are found in the [Core NodeSet](https://reference.opcfoundation.org/nodesets/?u=http://opcfoundation.org/UA/) but they can allow be looked up via a [website](https://reference.opcfoundation.org/Search?n=BrokerDataSetWriterTransportDataType).

### <a name='2'></a>Publish the Connection Metadata Message
The Connection metadata is sent to the topic:
```
opcua/json/connection/(Quickstart003)
```

The code to build the message is here:
```
var topic = new Topic()
{
    TopicPrefix = TopicPrefix,
    MessageType = MessageTypes.Connection,
    PublisherId = PublisherId
}.Build();

PubSubConnectionMessage payload = new PubSubConnectionMessage()
{
    MessageId = Guid.NewGuid().ToString(),
    PublisherId = PublisherId,
    Timestamp = DateTime.UtcNow,
    Connection = connection
};

var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault });

var applicationMessage = new MqttApplicationMessageBuilder()
    .WithTopic(topic)
    .WithPayload(json)
    .WithMessageExpiryInterval(7200)
    .WithRetainFlag(true)
    .Build();

var result = await m_client.PublishAsync(applicationMessage, CancellationToken.None);
```
The data is metadata so the retain flag is set to TRUE. 

The MessageExpiryInterval is used since this quickstart making use of a public broker. Note this feature is only available in MQTT 5.0.

### <a name='3'></a>Build and Run the Quickstarts
The solution [Quickstart_003.sln]() can be used to load the Publisher and Subscriber for this quickstart.

Build and run the Subscriber and then build and run the Publisher.

The output of the Publisher should be like:
```
Broker Certificate: 'CN=mqttdashboard.com' None
Publisher Connected!
Status Message Sent!
Connection Message Sent!
Press enter to exit.
DataSetMetaData Message Sent.
DataSetMetaData Message Sent.
Network (2) Messages Sent!
Network (1) Messages Sent!
Network (2) Messages Sent!
```
