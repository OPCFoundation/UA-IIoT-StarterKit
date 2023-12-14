## OPC UA IIoT StarterKit – Quickstart 004 - Publisher
### Acquiring Data from a OPC UA Server
This quickstart demonstrates how to modify the Publisher from [Quickstart_003](../../003/) to acquire live data from an OPC UA Server. 

1. [Create an OPC UA Client using the NETStandard Stack](#1)
2. [Update MetaData and Data Publishing](#2)
3. [Build and Run the Quickstarts](#3)

### <a name='1'></a>Create an OPC UA Client using the NETStandard Stack
The [NETStandard Stack](https://github.com/OPCFoundation/UA-.NETStandard) is a cross-platform implementation of the OPC UA specification. It is available as a [NuGet package](https://www.nuget.org/packages/OPCFoundation.NetStandard.Opc.Ua/). This quickstart creates a simple OPC UA Client that connects to a server subscribes a few nodes which are mapping onto DataSets which are published to the broker.

The Quickstart assumes the [Reference Server](https://github.com/OPCFoundation/UA-.NETStandard/tree/master/Applications/ReferenceServer) is running on the same machine. The URL can be changed with a constant at the top of the [Program.cs](Program.cs) file. The Quickstart uses no security and maps random variables onto the DataSet fields to demonstrate how to do it. A real application would use a DataSet that reflects the information model of the server (e.g. the names of Variables are the names of the fields in the DataSet).

Note that the handcrafted classes that were used previous quickstarts for have been replaced with the classes that are part of the NETStandard Stack. The Subscriber still uses the handcrafted classes since it is not using the NETStandard Stack.

The code to map the variables onto the DataSet fields is here:
```
 m_cache = new()
{
    [nameof(EnergyMetrics.Consumption)] = new(m_lock)
    {
        Name = nameof(EnergyMetrics.Consumption),
        Description = "The energy consumed by the device during the calculation period.",
        NodeId = "nsu=http://opcfoundation.org/UA/Boiler/;i=1272",
        StatusCode = Opc.Ua.StatusCodes.BadWaitingForInitialData,
        Properties = new()
        {
            new CachedValue(m_lock) {
                Name = Opc.Ua.BrowseNames.EngineeringUnits,
                NodeId = "nsu=http://opcfoundation.org/Quickstarts/ReferenceServer;s=DataAccess_AnalogType_DataAccess_AnalogType_Double_EngineeringUnits",
                StatusCode = Opc.Ua.StatusCodes.BadWaitingForInitialData
            }
        }
    },
    [nameof(EnergyMetrics.DutyCycle)] = new(m_lock)
    {
        Name = nameof(EnergyMetrics.DutyCycle),
        Description = "The fraction of the calulation period where the device is consuming power.",
        NodeId = "nsu=http://opcfoundation.org/UA/Boiler/;i=1242",
        StatusCode = Opc.Ua.StatusCodes.BadWaitingForInitialData
    },
    [nameof(EnergyMetrics.CalculationPeriod)] = new(m_lock)
    {
        Name = nameof(EnergyMetrics.CalculationPeriod),
        Description = "The period, in ms, over which power calculations are computed.",
        NodeId = "nsu=http://opcfoundation.org/Quickstarts/ReferenceServer;s=DataAccess_AnalogType_Double",
        StatusCode = Opc.Ua.StatusCodes.BadWaitingForInitialData
    }
};

m_metadataVersion = GetVersionTime();

m_datasource = await UAClient.Run(
    new UAClientSettings()
    {
        ServerUrl = ServerUrl,
        NoSecurity = NoSecurity,
        UserName = UserName,
        Password = Password,
        AutoAccept = true
    },
    m_cache.Values
);
```
The CacheValue is the interface between the Publisher and the UA Client. When UA Client receives an update it updates the CacheValue. The Publisher will periodically check the CacheValue for changes and construct a message if there are changes to send.

The Publisher also subscribes to properties of the variables which are published in the DataSetMetaData message. There is no fixed rule on when a property is best published as a field in the DataSet vs. a property in the DataSetMetaData. Properties that change rarely should be in the DataSetMetaData. Properties that change frequently should be in the DataSet. 

There are few constants which control the behavior of the Publisher:
```
const int PublishingInterval = 5000;
const int MetaDataPublishingCount = 10;
const int KeepAliveCount = 3;
const int KeyFrameCount = 12;
```

The PublishingInterval is the rate at which the Publisher will send messages to the broker. It is used as a base to calculate other intervals.

The KeyFrameCount is standard parameter that indicates how frequently the Publisher should send all of the fields. The default is 1 which means every message has all of the fields. The Quickstart sets it to 12 which means every 12th message has all of the fields (a.k.a keyframe). The other messages only have the fields that have changed since the last key frame message (a.k.a. deltaframe). The keyframe messages are needed because Subscribers could start listening at any time and would not know the current values of slow changing fields if they were not resent periodically.

The KeepAliveCount indicates when an empty keep alive message should be sent. May be sent instead of a deltaframe message. The value is 3 which every 3rd delta frame with no changes will be replaced with a keep alive message.

The MetaDataPublishingCount indicates how frequently the Publisher should send the DataSetMetaData message. 
### <a name='2'></a>Update MetaData and Data Publishing
The Data and DataSetMetaData messages are now generated from the cache used to initialize the UA Client. The code to build the DataSetMetaData message is here:
```
lock (m_lock)
{
    foreach (var value in m_cache.Values)
    {
        var field = new FieldMetaData()
        {
            Name = value.Name,
            BuiltInType = (int)BuiltInType.Double,
            DataType = new NodeId((int)BuiltInType.Double),
            Description = new LocalizedText(value.Description),
            ValueRank = -1,
        };

        if (value.Properties?.Count > 0)
        {
            field.Properties = new();

            foreach (var property in value.Properties)
            {
                field.Properties.Add(new Opc.Ua.KeyValuePair()
                {
                    Key = new QualifiedName(property.Name),
                    Value = property.Value
                });

                if (property.IsDirty)
                {
                    updateVersion = true;
                    property.IsDirty = false;
                }
            }
        }

        fields.Add(field);
    }
}

if (count % MetaDataPublishingCount != 0 && !updateVersion)
{
    return;
}

var metadata = new DataSetMetaDataType()
{
    Name = DataSetName, // same name appears in the DataSetWriter.
    Description = new LocalizedText("A set of energy consumption metrics for a device."),
    Fields = fields
};

if (updateVersion)
{
    m_metadataVersion = GetVersionTime();
}

metadata.ConfigurationVersion = new ConfigurationVersionDataType()
{
    MajorVersion = m_metadataVersion,
    MinorVersion = m_metadataVersion
};
```

Since the Publisher is now using the NETStandard Stack, the messages can be serialized using the JsonEncoder class. The code to serialize the Data message is here:
```
JsonEncoder encoder = new(MessageContext, true);

encoder.WriteUInt16("DataSetWriterId", DataSetWriterId);
encoder.WriteString("PublisherId", PublisherId);
encoder.WriteInt32("SequenceNumber", count + 1);
encoder.WriteDateTime("Timestamp", DateTime.UtcNow);
encoder.WriteUInt32("MinorVersion", m_metadataVersion);

lock (m_lock)
{
    var values = m_cache.Values.ToArray();

    if (count % KeyFrameCount == 0 || values.Where(x => x.IsDirty).Any())
    {
        encoder.WriteString("MessageType", (count % KeyFrameCount == 0) ? "ua-keyframe" : "ua-deltaframe");
        encoder.PushStructure("Payload");

        foreach (var value in values)
        {
            if (count % KeyFrameCount == 0 || value.IsDirty)
            {
                encoder.WriteDataValue(value.Name, new Opc.Ua.DataValue()
                {
                    WrappedValue = value.Value,
                    SourceTimestamp = value.Timestamp,
                    ServerTimestamp = DateTime.MinValue,
                    StatusCode = value.StatusCode
                });

                value.IsDirty = false;
            }
        }

        encoder.PopStructure();
        lastDataCount = count;
    }
    else
    {
        encoder.WriteString("MessageType", "ua-keepalive");

        if ((count - lastDataCount) % KeepAliveCount != 0)
        {
            continue;
        }
    }
}
```

### <a name='3'></a>Build and Run the Quickstarts
The solution [Quickstart_004.sln]() can be used to load the Publisher and Subscriber for this quickstart.

[MqttSpy](../../../docs/setup/linux) is the best way to view the messages before starting the Subscriber. 

The Publisher needs the ReferenceServer to be running, however, it is useful to not start the ReferenceServer right away. The Publisher will try to connect to the ReferenceServer and will fail. The values published will have an StatusCode and no Value. Since the errors do not change keepalive message will be sent every 15s followed by a keyframe every 60s.

Now start the ReferenceServer and the restart the Publisher. The Publisher will connect to the ReferenceServer and start publishing values. The Subscriber can be started at any time.

The output of the Publisher should be like:
```
Broker Certificate: 'CN=mqttdashboard.com' None
Publisher Connected!
Connecting to... opc.tcp://localhost:62541/Quickstarts/ReferenceServer
BadCertificateUntrusted 'Certificate is not trusted.'
Untrusted Certificate accepted. Subject = CN=Quickstart Reference Server, C=US, S=Arizona, O=OPC Foundation, DC=greycat
New Subscription created with SubscriptionId = 1.
New Session Created with SessionName = PubSub Quickstart
Connected!
Status Message Sent!
Connection Message Sent!
Press enter to exit.
DataSetMetaData Message Sent.
DataSetMessage Sent!
DataSetMetaData Message Sent.
DataSetMessage Sent!
DataSetMessage Sent!
```
