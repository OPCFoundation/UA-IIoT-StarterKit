## OPC UA IoT Toolkit – Publishing
### Overview
This document describes how to publish data with the agent.

The command line parameters are:
```
Usage: MqttAgent publish [options]

Options:
  -?|-h|--help                          Show help information
  -b|--broker <BrokerUrl>               The MQTT broker URL. Overrides the setting in the connection configuration.
  -a|--appid <ApplicationId>            A unique name for the application instance. Overrides the setting in the connection configuration.
  -c|--connection <ConnectionFilePath>  The file containing the the OPC UA PubSub connection configuration.
  -d|--datasets <DataSetDirectoryPath>  The directory containing the the OPC UA PubSub dataset configurations.
  -n|--nameplate <NameplaceFilePath>    The file containing the the nameplate information for the device where the agent is running.
```
### Publishing Data
Verify MQTT broker is running. 

Start mqtt-spy and subscribe to all topics. 

On the Raspberry Pi:
```
cd ~/MqttAgent
dotnet MqttAgent.dll publish -b=mqtt://<broker>:1883
```
Replace <broker> with the IP address of the machine running the MQTT broker. 

Use mqtt-spy to monitor the messages now being published from the Raspberry Pi. 
