## OPC UA IIoT StarterKit – Quickstart 002 – Publisher
### Describing the Content of Data Messages
This quickstart demonstrates how to update [Quickstart_001](../../001/) to add a standard header to the Data messages and to publish DataSetMetaData that describes the content of the Data messages.

1. [Construct Data Message](#1)
3. [Construct the DataSetMetaData Message](#3)
4. [Publish the DataSetMetaData Message](#4)

### <a name='1'></a>Construct Data Message
The previous quickstarts sent an JSON message that only had information specific to the application. This quickstart will add a standard header to the message. The header is necessary when the Subscriber needs to use metadata messages to process the Data messages.

The following code adds the DataSetMessage header to the previously used JSON message:
```
var data = new EnergyMetrics()
{
    CalculationPeriod = 3600,
    Consumption = 42 + random.NextDouble(),
    DutyCycle = random.NextDouble()
};

DataSetMessage message = new()
{
    PublisherId = PublisherId,
    DataSetWriterId = DataSetWriterId,
    SequenceNumber = ii + 1,
    MinorVersion = version.MinorVersion,
    Timestamp = DateTime.UtcNow,
    Payload = data
};

var json = JsonSerializer.Serialize(message);

var dataMessage = new MqttApplicationMessageBuilder()
    .WithTopic(DataTopic)
    .WithPayload(json)
    .Build();

var result = await m_client.PublishAsync(dataMessage, CancellationToken.None);
```
The MinorVersion is part of the ConfigurationVersion. It allows Subscribers to check if it has the metadata that is needed to understand the message.

The SequenceNumber is used to detect missing messages. 

The Timestamp is when the Data message was created and is not related to when the data in the message was sampled.

### <a name='2'></a>Construct the DataSetMetaData Message
The DataSetMessage header allows Subscribers to correlate Data messages with metadata. The DataSetMetaData message is metadata that describes the content of the message. 

The following code constructs the DataSetMetaData message:
```
var metadata = new DataSetMetaDataType()
{
    Name = DataSetName, // same name appears in the DataSetWriter.
    ConfigurationVersion = version,
    DataSetClassId = EnergyMetrics.DataSetClassId.ToString(), // Used to indicate that many DataSetWriters report the same data.
    Description = new LocalizedText() { Text = "A set of energy consumption metrics for a device." },
    Fields = new List<FieldMetaData>()
    {
        new FieldMetaData()
        {
            Name = "Consumption",
            BuiltInType = (int)BuiltInType.Double,
            DataType = new NodeId((int)BuiltInType.Double),
            Description = new LocalizedText() { Text = "The energy consumed by the device during the calculation period." },
            ValueRank = -1,
            Properties = new List<UaMqttCommon.KeyValuePair>()
            {
                new UaMqttCommon.KeyValuePair()
                {
                    Key = new QualifiedName() { Name = "EngineeringUnits" },
                    Value = new Variant()
                    {
                        Type = (int)BuiltInType.ExtensionObject,
                        Body = new ExtensionObject<EUInformation>()
                        {
                            TypeId = EUInformation.TypeId,
                            Body = unitOfPower
                        }
                    }
                }
            }
        },
        new FieldMetaData()
        {
            Name = "DutyCycle",
            BuiltInType = (int)BuiltInType.Float,
            DataType = new NodeId((int)BuiltInType.Float),
            Description = new LocalizedText() { Text = "The fraction of the calulation period where the device is consuming power." },
            ValueRank = -1,
            Properties = new List<UaMqttCommon.KeyValuePair>()
            {
                new UaMqttCommon.KeyValuePair()
                {
                    Key = new QualifiedName() { Name = "EngineeringUnits" },
                    Value = new Variant()
                    {
                        Type = (int)BuiltInType.ExtensionObject,
                        Body = new ExtensionObject<EUInformation>()
                        {
                            TypeId = EUInformation.TypeId,
                            Body = new EUInformation()
                            {
                                NamespaceUri = "http://www.opcfoundation.org/UA/units/un/cefact",
                                UnitId = 20529,
                                DisplayName = new LocalizedText() { Text = "%" },
                                Description = new LocalizedText() { Text = "percent" }
                            }
                        }
                    }
                }
            }
        },
        new FieldMetaData()
        {
            Name = "CalculationPeriod",
            BuiltInType = (int)BuiltInType.Double,
            DataType = new NodeId((int)BuiltInType.Double),
            Description = new LocalizedText() { Text = "The period, in ms, over which power calculations are computed." },
            ValueRank = -1,
            Properties = new List<UaMqttCommon.KeyValuePair>()
            {
                new UaMqttCommon.KeyValuePair()
                {
                    Key = new QualifiedName() { Name = "EngineeringUnits" },
                    Value = new Variant()
                    {
                        Type = (int)BuiltInType.ExtensionObject,
                        Body = new ExtensionObject<EUInformation>()
                        {
                            TypeId = EUInformation.TypeId,
                            Body = new EUInformation()
                            {
                                NamespaceUri = "http://www.opcfoundation.org/UA/units/un/cefact",
                                UnitId = 4403766,
                                DisplayName = new LocalizedText() { Text = "ms" },
                                Description = new LocalizedText() { Text = "millisecond" }
                            }
                        }
                    }
                }
            }
        }
    }
};
```
Each instance of DataSet metadata has a [ConfigurationVersion](https://reference.opcfoundation.org/Core/Part14/v105/docs/6.2.3.2.6) which is used to detect changes to the metadata. In the quickstart, the DataSet metadata is changed each time it is sent so the version is updated because the EngineeringUnits for one of the fields changes.

Each field specifies the DataType defined in an OPC UA information model. It also provides additional properties. The quickstart adds EngineeringUnits property to each field. 

The EUInformation structure is the standard way to specify engineering units in OPC UA. The default units system is [UN/CEFACT](https://en.wikipedia.org/wiki/UN/CEFACT_Common_Codes). The UnitIds for UN/CEFACT units are published in a [CSV file](https://github.com/OPCFoundation/UA-Nodeset/blob/latest/Schema/UNECE_to_OPCUA.csv). 

### <a name='4'></a>Publish the DataSetMetaData Message
The code to publish the message is here:
```
var topic = new Topic()
{
    TopicPrefix = TopicPrefix,
    MessageType = MessageTypes.DataSetMetaData,
    PublisherId = PublisherId,
    GroupName = GroupName,
    WriterName = WriterName
}.Build();

var payload = new DataSetMetaDataMessage()
{
    MessageId = Guid.NewGuid().ToString(),
    PublisherId = PublisherId,
    DataSetWriterId = 1,
    MetaData = metadata
};

var json = JsonSerializer.Serialize(payload);

var applicationMessage = new MqttApplicationMessageBuilder()
    .WithTopic(topic)
    .WithPayload(json)
    .WithMessageExpiryInterval(7200)
    .WithRetainFlag(true)
    .Build();

var result = await m_client.PublishAsync(applicationMessage, CancellationToken.None);
```
The topic includes the GroupName and WriterName which allows Subscribers to create specific filters for the DataSets that they are interested in.

The message is metadata so the retain flag is set to TRUE. This means the MQTT broker will keep the message and send it to new Subscribers.

The MessageExpiryInterval is used since this quickstart making use of a public broker. Note this feature is only available in MQTT 5.0.

### <a name='5'></a>Build and Run the Quickstarts
The solution [Quickstart_002.sln](../Quickstart_002.sln) can be used to load the Publisher and Subscriber for this quickstart.

Build and run the Publisher.

The output of the Publisher should be like:
```
Broker Certificate: 'CN=mqttdashboard.com' None
Publisher Connected!
Status Message Sent.
Press enter to exit.
DataSetMetaData Message Sent.
Data Message Sent: Consumption=42.33396830992852; DutyCycle=0.5768242602759034
Data Message Sent: Consumption=42.40644487745771; DutyCycle=0.30308613896269
```
