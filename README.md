## OPC UA IIoT StarterKit

**This repository is based on DRAFT enhancements to the specification.**

**The public version of the [UA-IIoT-StarterKit](https://github.com/OPCFoundation/UA-IIoT-StarterKit) has code that only uses the published specification** 

### Overview
OPC UA PubSub is an extension to OPC UA that enables communication between OPC UA applications using a Publish-Subscribe message pattern instead of the Request-Response message pattern. The Publish-Subscribe message pattern decouples senders of messages from their receivers which allows for the development if systems based on middleware such as an MQTT broker.  The difference between the two patterns is illustrated in the following figure:

![PubSub Overview](docs/images/image001.png "OPC UA IIoT StarterKit Context")

The Publish-Subscribe message pattern is a powerful tool for factory owners that want to collect data from large numbers of publishers. In particular, it is a technology that is well suited for publishing factory data to the cloud. Publish-Subscribe is not well suited for use cases, such as device configuration, where a Client is blocked until it receives information back from the Server. OPC UA supports both message patterns because it is the best way to meet the complex needs of modern factories. 

The purpose of this starter is to provide a bare bones implementation of OPC UA PubSub which illustrates the following:

1) Implementing OPC UA PubSub is easy to do using standard Open-source libraries; 
2) OPC UA PubSub allows Subscribers discover Publishers and metadata about what they are publishing;
3) How to integrating OPC UA PubSub with OPC Client-Server. 

### Changes

|Date|Changes|
|--|--|
|2021-12-01|Added command line arguments to set MQTT broker username/password.|
|2022-07-30|Major update to support the draft topics defined by the specification and support for actions and binary data.|
|2023-11-06|Major update to support the 1.05.03 version of the specification. Features that have not been released have been removed for now.|
|2025-02-02|Major update to support the 1.05.04 version of the specification.|

### Licence and Usage Model
The code in this repository is covered under the [MIT license](https://opcfoundation.org/license/mit.html), however, some of the applications link to the [UA-.NETStandard](https://github.com/OPCFoundation/UA-.NETStandard) project which is has a [dual license model](https://opcfoundation.github.io/UA-.NETStandard/). Note that Quickstarts [001](./Quickstarts/001/) through [003](./Quickstarts/003/) do not use the UA-.NETStandard stack and show how to implement a basic PubSub application using only MIT licenced code.

This code is developed as a learning tool and is missing some of the features that a commercial product would require, such as robust error handling.

### Requirements
This StarterKit contains software designed to run on a [Raspberry Pi](https://www.raspberrypi.org/products/raspberry-pi-4-model-b/) 

This version of StarterKit uses [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/). 

The recommended development tool to build for the Raspberry Pi is [Visual Studio Code](https://code.visualstudio.com/) 

The recommended development tool to build for Windows platforms is [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/).

An MQTT broker is required. [Eclipse Mosquitto](https://mosquitto.org/) is used in the StarterKit documentation.  

A MQTT monitoring application, such as [MQTT Explorer](https://mqtt-explorer.com/), is also helpful. 

All of the code and documentation can be found on [GitHub](https://github.com/OPCFoundation/UA-IIoT-StarterKit). 

### Next Steps

1. Setting up the Build Environment

    1.1 [Linux](docs/setup/linux)  
    1.2 [Raspberry Pi with a Windows Development Environment](docs/setup/raspberrypi) 

2. Quickstarts

    2.1 [Building a Simple Publisher and Subscriber](Quickstarts/001/)  
    2.2 [Describing the Content of Data Messages](Quickstarts/002/)  
    2.3 [Using Connection Metadata to discover Message Structure](Quickstarts/003/)  
    2.4 [Acquiring Data from an OPC UA Server](Quickstarts/004/)  
    2.5 [Handling Multiple Header Profiles](Quickstarts/005/)  

3. Complete Applications

    3.1 [Publisher](UaPublisher)  
    3.2 [Subscriber](UaSubscriber)
