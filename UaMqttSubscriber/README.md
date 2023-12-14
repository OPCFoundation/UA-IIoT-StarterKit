## OPC UA IIoT StarterKit – UaMqttSubscriber
### Overview

This application is an OPC UA subscriber that collects data from sent to an MQTT broker.  

The UaMqttSubscriber is configured with a JSON [configuration file](config/uasubscriber-config.json). The configuation file has 1 section: 'Brokers'. 

The 'Brokers' section specifies the address and credentials needed to access an MQTT Broker. A few public MQTT Brokers are included in the default configuration file. When using public brokers there will be messages generated from many different users. Changing the TopicPrefix in the Broker settings in the Publisher and Subscriber creates a private sandbox for testing.  

The UaMqttSubscriber subscribes to different different topics to:

1. Discover the available publishers;
2. Discover the content of the data messages;
3. Discover the where the publisher is sending data and metadata messages;
4. Subscribe to the data messages.

To discover the available publishers the UaMqttSubscriber subscribes to the 'status' topic using a wildcard. If the UaMqttSubscriber connects to a public MQTT broker then their will likely be a lot of inactive publishers created by other users. For this reason, the UaMqttSubscriber ignores inactive publishers and only subscribes to the 'connection' topic for currently active publishers.

The connection topic publishes information about the structure and destination for the published data messages. The destinations could be the default topic names constructed from the names of the groups and writers or they could be custom topics. When a connection message arrives the UaMqttSubscriber subscribes to the data and metadata topics specified in the connection message.

The metadata message defines the contents of the messages and provides the value of rarely changing properties associated with the field values such as EngineeringUnits. The UaMqttSubscriber caches this information and uses it when processing data messages.

The data message contains the live data published to the broker. The UaMqttSubscriber parses the message and displays the values with selected properties from the metadata messages. The UaMqttSubscriber can handle any of the standard message structures defined by the HeaderProfile, however, it is written in way that it detects how a message is structured so it does not need to know, in advance, what HeaderProfile the message is using.

### Command Line Parameters
The UaMqttSubscriber supports the following command line parameters:
```
Subscribes for data sent to an MQTT broker using UA PubSub.

Usage: UaMqttSubscriber subscribe [options]

Options:
  -?|-h|--help  Show help information.
  --log         The path to the log file.
  --config      The path to the configuration file.
  --broker      The name of the broker configuration to use.
```
If no --config is provides the default file in the same directory as the executable is used.

