## OPC UA IoT Starter Kit – Discovery
### Overview
This page describes how to discover data with the agent.

The command line parameters are:
```
Usage: MqttAgent discover [options]

Options:
  -?|-h|--help                          Show help information
  -b|--broker <BrokerUrl>               The MQTT broker URL. Overrides the setting in the connection configuration.
  -a|--appid <ApplicationId>            A unique name for the application instance. Overrides the setting in the connection configuration.
  -c|--connection <ConnectionFilePath>  The file containing the the OPC UA PubSub connection configuration.
  -d|--datasets <DataSetDirectoryPath>  The directory containing the the OPC UA PubSub dataset configurations.
  -n|--nameplate <NameplaceFilePath>    The file containing the the nameplate information for the device where the agent is running.
```

### Discovering Publishers
Verify MQTT broker is running.
Start mqtt-spy and subscribe to all topics.

Start the discovert MqttAgent on any machine with these arguments:
```
MqttAgent discover -b=mqtt://<broker>:1883
```

On the Raspberry Pi 
```
cd ~/MqttAgent
dotnet MqttAgent.dll publish -b=mqtt://<broker>:1883
```

When the publisher starts the discovery agent will print something like:
```
Detected Publisher (opcua/RaspberryPi/identity).
  Manufacturer: OPC Foundation
  ManufacturerUri: https://opcfoundation.org/
  ProductInstanceUri: urn:raspberrypi:acme.org:iot-starterkit:123456789
  Model: OPC UA PubSub IoT StarterKit
  SerialNumber: 123456789
  HardwareRevision: Model B Rev 1.2
  SoftwareRevision: 1.00
```

Stop and restart the publisher:
```
dotnet MqttAgent.dll publish -b=mqtt://<broker>:1883 -a=MyDevice
```

A new device called ‘MyDevice’ should be automatically detected by the discovery agent.
