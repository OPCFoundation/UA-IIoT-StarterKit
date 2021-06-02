## OPC UA IoT StarterKit – Subscribing To Data
### Overview

1. [Command Line Arguments](#1)
2. [Configuring the Subscriber](#2)
3. [Running the Subscriber](#3)
4. [Understanding the Results](#4)

### <a name='1'>Command Line Arguments

The command line parameters are:
```
Usage: MqttAgent subscribe [options]

Options:
  -?|-h|--help    Show help information
  -b|--broker     The MQTT broker URL. Overrides the setting in the connection configuration.
  -c|--connection The file containing the the OPC UA PubSub connection configuration.
  -d|--datasets   The directory containing the the OPC UA PubSub dataset configurations.
  -p|--publisher  The identifier for the publisher to monitor.
  -g|--group      The name of the reader group to monitor.
```
The MQTT topic tree used by the StarterKit applications have the pattern:
```
opcua/<publisher>/<group>/<dataset reader>
```
Where 

&lt;publisher&gt; is the value passed with the --publisher option. 

&lt;group&gt; is the value passed with the --group option. 

&lt;dataset&gt; name of the dataset reader. 

### <a name='2'>Configuring the Subscriber

There are 2 ways to process UA PubSub messages: 
 
1) Process the raw JSON; 

2) Configure UA PubSub subscriber; 

mqtt-spy can be used too monitor the raw JSON. 

The subscriber configuration [file](https://github.com/OPCF-Members/UA-IoT-StarterKit/blob/master/MqttAgent/config/subscriber-connection.json) is used to configure the UA PubSub subscriber. The structure of the file: 

    ReaderGroups []
        DataSetReaders []
            DataSetMetaData
                Name: <The DataSet Name>
                ConfigurationVersion: <The DataSet Version>
            TransportSettings
                QueueName: <MQTT Topic Name>

In the StarterKit examples, the "Name" of the "DataSetMetaData" in the DataSetReader is used to locate the current DataSet metadata [file](https://github.com/OPCF-Members/UA-IoT-StarterKit/blob/master/MqttAgent/config/datasets/gate.json). The ConfigurationVersion something that can be sent in a UA PubSub message and allows the subscriber to know if the DataSet metadata has changed. The complete definition of the file structure is [here](https://reference.opcfoundation.org/v104/Core/docs/Part14/6.2.6/#6.2.6.5.1).

An example [dataset](https://github.com/OPCF-Members/UA-IoT-StarterKit/blob/master/MqttAgent/config/datasets/gate.json) represents a conveyor belt gate with two sensors. The structure of the file: 

    Fields []
        Name : <The Field Name>
        Properties []
            <Additional Metadata>

The "Fields" array specifies the name and order of elements that appear in the UA PubSub message. Each field may have additional properties, such as EURange, which may be used to interpret the published data. The complete definition of the file structure is [here](https://reference.opcfoundation.org/v104/Core/docs/Part14/6.2.2/#6.2.2.1.2).

### <a name='3'>Running the Subscriber

Before running the subcriber, a publisher needs to be running. It can be started with the following command:
```
dotnet MqttAgent.dll publish -b=mqtt://[broker ip]:1883 -a=MyPublisher
```
Where 

[broker ip] is the the IP address or DNS name of the broker machine; 

The subscriber can be started with the following command:
```
dotnet MqttAgent.dll subscribe -b==mqtt://[broker ip]:1883 -p=MyPublisher -g=minimal
```

Where 

[broker ip] is the the IP address or DNS name of the broker machine; 

"minimal" is the name of the ReaderGroup in subscriber-connection.json file.

### <a name='4'>Understanding the Results

The "minimal" ReaderGroup is configured to produce a simple JSON message that contains the names of fields and their current value. 

An sample of the message produced is:

```
{
    "CycleCount": 100,
    "State": false
}
```
The JSON message has 2 fields which are defined in the [metadata](https://github.com/OPCF-Members/UA-IoT-StarterKit/blob/master/MqttAgent/config/datasets/gate.json). 

The metadata also specifies that the "State" field is "The current state of the gate." and has a label "Closed" for TRUE and "Open" when FALSE. 

If the "full" ReaderGroup is used the message looks like this: 

```
{
          "DataSetClassId": "95254774-8145-4df7-92b5-789e5dca9a0d",
          "MessageId": "a8da2c2a-b01b-422c-9089-5d2b3f44f11d",
          "MessageType": "ua-data",
          "Messages": {
                    "DataSetWriterId": 3,
                    "MetaDataVersion": {
                              "MajorVersion": 1,
                              "MinorVersion": 0
                    },
                    "Payload": {
                              "CycleCount": {
                                        "Body": 16,
                                        "Type": 7
                              },
                              "State": {
                                        "Body": false,
                                        "Type": 1
                              }
                    },
                    "SequenceNumber": 37,
                    "Timestamp": "2021-05-26T20:01:09.9105361Z"
          },
          "PublisherId": "BlackIce"
}
```
The actual data is the same, however, additional metadata is included. 

In these messages the field values are JSON objects with "Body" and "Type" fields. The "Type" field allows publisher to provide DataType information that is lost in the conversion to JSON. The complete set of values for the Type field is defined [here](https://reference.opcfoundation.org/Core/docs/Part6/5.1.2/).

The "DataSetClassId" is a unique identifier for the DataSet and allows subscribers to know when a DataSet with the same fields is published by different publishers.  