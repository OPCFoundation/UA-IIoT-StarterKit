## OPC UA IIoT StarterKit – Subscribing To Data
### Overview

1. [Command Line Arguments](#1)
2. [Configuring the Subscriber](#2)
3. [Running the Subscriber](#3)
4. [Understanding the Results](#4)

### <a name='1'>Command Line Arguments

The command line parameters are:
```
Usage: metadata [options]

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

See [subscribing](../subscribing/#2).

### <a name='3'>Running the Subscriber

Before running the subcriber, a publisher needs to be running. It can be started with the following command:
```
dotnet MqttAgent.dll publish -b=mqtt://[broker ip]:1883 -a=mydevice:one
```
Where 

[broker ip] is the the IP address or DNS name of the broker machine; 

The subscriber can be started with the following command:
```
dotnet MqttAgent.dll metadata -b==mqtt://[broker ip]:1883 -p=mydevice:one -g=full
```
Where 

[broker ip] is the the IP address or DNS name of the broker machine; 

"full" is the name of the ReaderGroup in subscriber-connection.json file. 

### <a name='4'>Understanding the Results
The metadata is published to the topic:
```
opcua/<publisher>/<group>/<dataset reader>/$metdata
```
The retain flag is set to true so whenever a subscriber starts a new subscription it get a current metadata. 

The metadata has a field called ["ConfigurationVersion"](https://reference.opcfoundation.org/v104/Core/docs/Part14/6.2.2/#6.2.2.1.5) 
```
"ConfigurationVersion": {
   "MajorVersion": 1,
   "MinorVersion": 0
}
```
This field may be included in data messages which allows a subscriber to know which version of the metadata to use when interpreting a message. 

The metadata is read from files in the config/dataset directory. 

It is possible to simulate changes to the dataset by editing '[gate.json](https://github.com/OPCF-Members/UA-IIoT-StarterKit/blob/master/MqttAgent/config/datasets/gate.json)' on the publisher machine and restarting the publisher. 
