## OPC UA IoT StarterKit – Discovering Publishers
### Overview

1. [Command Line Arguments](#1)
2. [Configuring the Subscriber](#2)
3. [Running the Subscriber](#3)
4. [Understanding the Results](#4)

### <a name='1'>Command Line Arguments

The command line parameters are:
```
Usage: MqttAgent discover [options]

Options:
  -?|-h|--help     Show help information
  -b|--broker      The MQTT broker URL. Overrides the setting in the connection configuration.
  -c|--connection  The file containing the the OPC UA PubSub connection configuration.
  -d|--datasets    The directory containing the the OPC UA PubSub dataset configurations.
```
The StarterKit applications use this topic to publish their identity:
```
opcua/<application>/identity
```
Where 

&lt;application&gt; is the publisher application name.

The [identity](https://github.com/OPCF-Members/UA-IoT-StarterKit/blob/master/MqttAgent/config/datasets/nameplate.json) is a special DataSet which provides metadata about the publisher. The contents of the DataSet are fully described by the [IVendorNameplate](https://reference.opcfoundation.org/v104/DI/v102/docs/5.5.2/) interface.

### <a name='2'>Configuring the Subscriber

The discovery capability makes used of a DataSetReader with a MQTT wildcard specified as the QueueName. 

The subscriber connetion file is [here](https://github.com/OPCF-Members/UA-IoT-StarterKit/blob/master/MqttAgent/config/subscriber-connection.json). 

The QueueName for identity ReaderGroup is:
```
opcua/+/identity
```

### <a name='3'>Running the Subscriber
The subscriber should be started before the publisher with the following command
```
dotnet MqttAgent.dll discover -b==mqtt://[broker ip]:1883 
```
Where 

[broker ip] is the the IP address or DNS name of the broker machine. 

The publisher can be started with the command:
```
dotnet MqttAgent.dll publish -b=mqtt://[broker ip]:1883 -a=MyPublisher
```
The subscriber should print out the following:
```
Detected Publisher (opcua/MyPublisher/identity).
  Manufacturer: Arbutus Widgets Inc
  ManufacturerUri: https://arbutus-widgets.com/
  ProductInstanceUri: urn:MyPublisher:arbutus-widgets.com:iot-starterkit:123456789
  Model: Roadrunner Detector
  SerialNumber: 123456789
  HardwareRevision: Model B Rev 1.2
  SoftwareRevision: 1.00
```
Most of the information comes from the [nameplate.json](https://github.com/OPCF-Members/UA-IoT-StarterKit/blob/master/MqttAgent/config/nameplate.json) file which used by the publisher. Some parts are overriden by command line parameters.

Restart the publisher with a different application name:

```
dotnet MqttAgent.dll publish -b=mqtt://[broker ip]:1883 -a=AnotherPublisher
```
Verify that the new publisher was detected by the subscriber.

### <a name='4'>Understanding the Results
The MQTT topic wildcard is a convenient way to subscribe to topics from multiple publishers. 

The topic tree used by the StarterKit is designed to allow the use of MQTT wildcards with the syntax:
```
opcua/<PublisherId>/<WriterGroup.Name>/<DataSetWriter.Name>
```
The metadata for the DataSet published by the DataSetWriter has the following topic:
```
opcua/<PublisherId>/<WriterGroup.Name>/<DataSetWriter.Name>/$metadata
```
While the topic in the StarterKey are examples, the 1.05 version of the OPC UA specification will introduce a standard topic tree.
