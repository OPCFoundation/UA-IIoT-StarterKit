## OPC UA IIoT StarterKit – UaMqttPublisher
### Overview

This application is an OPC UA publisher that collects data from any OPC UA server and sends it to an MQTT broker.  

The UaMqttPublisher is configured with a JSON [configuration file](config/uapublisher-config.json). The configuation file has 2 sections: 'Brokers' and 'Connections'. 

The 'Brokers' section specifies the address and credentials needed to access an MQTT Broker. A few public MQTT Brokers are included in the default configuration file. When using public brokers there will be messages generated from many different users. Changing the TopicPrefix in the Broker settings for the Publisher and Subscriber creates a private sandbox for testing.  

The 'Connections' section specifies the mapping from variables in the OPC UA Server address space to DataSet messages. The structure roughly follows the [PubSubConnection](https://reference.opcfoundation.org/Core/Part14/v105/docs/6.2.7.5) information model but adds additional parameters which are specific to the implementation.

The UaMqttPublisher has an internal server which starts automatically when publishing starts. This server is not used if an external ServerUrl is specified in the Connection configuration. An XML [configuration file](config/uaserver-configuration.xml) is used to initialize the internal server and the OPC UA Client used to connect to an external OPC UA Server.

The following URL allows and OPC UA Client to connection to the internal server:
```
opc.tcp://localhost:48040
```
Any OPC UA Client can be used to write data to the mapped variables and verify that the values are published to the MQTT broker.

The mapping to variables is done by setting the Source field for each of the fields defined for a Writer. The syntax requires the entire NamespaceUri instead of the NamespaceIndex. The following is an example of a field definition:
```
{
  "Name": "CycleTime",
  "Description": "The fraction of the calculation period when the date.",
  "Source": "nsu=tag:opcua.org,2023-11:iot-starterkit:gpio;s=Gate1_CycleTime",
  "Properties": [
    {
      "Name": "EngineeringUnits",
      "Source": "nsu=tag:opcua.org,2023-11:iot-starterkit:gpio;s=Gate1_CycleTime_EngineeringUnits"
    }
  ]
}
```
The 'tag' URI scheme is the recommended URI scheme when there is no webpage associated with the namespace. Any other URI scheme is valid.

The current value of Properties specified in the field will appear with the DataSetMetaData. If the values of these Properties change a new DataSetMetaData message is sent. Only rarely changing configuration information should be included in the Properties list.

The layout of the messages published is set by the HeaderProfile field of the Group. The HeaderProfile may be one of the URIs specified in [Annex A](https://reference.opcfoundation.org/Core/Part14/v105/docs/A.3). The following HeaderProfileUris are supported:

|URI|Description|
|---|---|
|http://opcfoundation.org/UA/PubSub-Layouts/JSON-Minimal|Publishes a simple message without any headers.|
|http://opcfoundation.org/UA/PubSub-Layouts/JSON-DataSetMessage|Publishes a message containing a single [DataSetMessage](https://reference.opcfoundation.org/Core/Part14/v105/docs/7.2.5.4).|
|http://opcfoundation.org/UA/PubSub-Layouts/JSON-NetworkMessage|Publishes a [NetworkMessage](https://reference.opcfoundation.org/Core/Part14/v105/docs/7.2.5.3) containing a multiple [DataSetMessage](https://reference.opcfoundation.org/Core/Part14/v105/docs/7.2.5.4).|

UA PubSub allows Publshers to send messages that only have changes since the last message. These messages are called 'ua-deltaframe' messages. However, Subscribers can come an go at any time so they need a complete set of fields. 'ua-keyframe' messages with all fields are sent periodically for this reason. The KeyFrameCount field specifies the interval between ua-keyframe messages.

The FieldMasks is the numeric value for the [DataSetFieldContentMask](https://reference.opcfoundation.org/Core/Part14/v105/docs/6.2.4.2). This controls how the value of each field is represented. The following are useful values:

|Value|Description|
|---|---|
|0|Only the Value is published with a [Built-in Type](https://reference.opcfoundation.org/Core/Part6/v105/docs/5.1.2).|
|5|The Value (with it's Built-in Type), StatusCode and SourceTimestamp are published.|
|32|The raw Value using a simplied JSON encoding is published.|

The TopicForData field in the Group or Writer specifies a non-standard topic name to use. This is an import feature when OPC UA PubSub applications are deployed in environments that have existing MQTT applications and a specific topic structure.

### Command Line Parameters
The UaMqttPublisher supports the following command line parameters:
```
Produces OPC UA PubSub messages sends them to an MQTT broker.

Usage: UaMqttPublisher publish [options]

Options:
  -?|-h|--help  Show help information.
  --log         The path to the log file.
  --config      The path to the configuration file.
  --broker      The name of the broker configuration to use.
  --connection  The name of the connection configuration to use.
  --group       The name of the dataset group configuration to use.
```
If no --config is provides the default file in the same directory as the executable is used.

If the --broker, --connection or --group parameters are omitted the first entry in the configuration file is choosen.