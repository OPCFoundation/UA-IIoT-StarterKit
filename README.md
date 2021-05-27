## OPC UA IoT Starter Kit
### Overview
OPC UA PubSub is an extension to OPC UA that enables communication between OPC UA applications using a Publish-Subscribe message patten instead of the Request-Response message pattern. The Publish-Subscribe message patten decouples senders of messages from their receivers which allows for the development if systems based on middleware such as an MQTT broker.  The difference between the two patterns is illustrated in the following figure:

![PubSub Overview](docs/images/image001.png "OPC UA IoT Starter Kit Context")

The Publish-Subscribe message pattern is a powerful tool for factory owners that want to collect data from large numbers of publishers. In particular, it is a technology that is well suited for publishing factory data to the cloud. Publish-Subscribe is not well suited for use cases, such as device configuration, where a Client is blocked until it receives information back from the Server. OPC UA supports both message patterns because it is the best way to meet the complex needs of modern factories. 

The purpose of this starter is to provide a bare bones implementation of OPC UA PubSub which illustrates the following:
1) Implementing OPC UA PubSub is easy to do using standard Open-source libraries;
2) OPC UA Pub Sub allows factory owners to take advantage of the MQTT infrastructure to visualize their factory;
3) Using OPC UA Information Models to configure applications and define the contents of messages. 

### Requirements
This starter kit contains software designed to run on a [Raspberry Pi](https://www.raspberrypi.org/products/raspberry-pi-4-model-b/) or other Linux device with GPIO ports.  

It will also run on a Linux or Windows machine without GPIO ports in simulation mode. 

This version of starter kit uses [.NET Core 3.1 LTS](https://dotnet.microsoft.com/download/dotnet/3.1), however, future versions will also provide NodeJS, Python and ANSI C samples. 

The recommended development tool to build for the Raspberry Pi is [Visual Studio Code](https://code.visualstudio.com/) 

The recommended development tool to build for Windows platforms is [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/).

An MQTT broker is required. [Eclipse Mosquitto](https://mosquitto.org/) is used in the starter kit documentation.  

A MQTT monitoring application, such as [MQTT Spy](https://www.eclipse.org/paho/index.php?page=components/mqtt-spy/index.php), is also helpful. 

All of the code and documentation can be found on [GitHub](https://github.com/OPCF-Members/UA-IoT-StarterKit). 

### Next Steps

1. Setting up the Build Environment

    1.1 [Linux](docs/setup/linux) 

    1.2 [Windows and Raspberry Pi](docs/setup/raspberrypi) 

2. [Understanding the MQTT Agent](docs/agent)
3. [Publishing Data](docs/publishing)
4. [Discovering Publishers](docs/discovery)
5. [Subscribing to Data](docs/subscribing)
6. [Information Models](docs/models)

