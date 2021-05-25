## OPC UA IoT Starter Kit – MqttAgent
### Overview
The main application is called MqttAgent. 

It can act as a publisher or subscriber depending on the command line arguments. 

The available commands are:
```
Usage: MqttAgent [options] [command]

Options:
  -?|-h|--help  Show help information

Commands:
  discover   Discovers OPC UA publishers.
  publish    Publishes I/O data to an MQTT broker.
  subscribe  Subscribes for data from OPC UA publishers.

Use "MqttAgent [command] --help" for more information about a command.
```
### Confguration Files
The MqttAgent depends on the following configuration files stored in the ‘./config’ directory:

| File | Description |
| --- | ----------- |
| \*-connection.json | A PubSub connection configuration. It specifies the number and the location of topics which the application publishes to/subscribes to.<br/>The contents of the file are described [here](https://reference.opcfoundation.org/v104/Core/docs/Part14/6.2.6/#6.2.6.5.1). |
| \*-nameplate.json | A JSON file that stores metadata about the host.<br/>It is simple self-describing set of name-value pairs. |
| datasets/\*.json | A folder containing JSON files that describe datasets that may be published to the MQTT broker.<br/>The contents of a dataset file are described [here](https://reference.opcfoundation.org/v104/Core/docs/Part14/6.2.2/#6.2.2.1.2). | 

The file formats are defined by the OPC UA specification and may be exchanged between applications developed by different vendors.

### Getting the Code
Clone the GitHub repository into the workspace directory: 
```
git clone https://github.com/OPCF-Members/UA-IoT-StarterKit.git
cd UA-IoT-StarterKit
git submodule update –init
```
Building the code Windows: 
```
Open MqttAgent.sln with Visual Studio
Build solution
```

Building the code for the RaspberryPi: 
```
dotnet publish -f netcoreapp3.1 -r linux-arm -o ./build/MqttAgent ./MqttAgent/MqttAgent.csproj
```
Copy code to RaspberryPi (based on setup instructions): 
```
scp -i ../.ssh/id_rsa -r ./build/MqttAgent pi@raspberrypi:~/
```
