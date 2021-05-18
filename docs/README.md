## OPC UA IoT Toolkit
### Overview
OPC UA PubSub is an extension to OPC UA that enables communication between OPC UA applications using a Publish-Subscribe message patten instead of the Request-Response message pattern. The Publish-Subscribe message patten decouples senders of messages from their receivers which allows for the development if systems based on middleware such as an MQTT broker.  The difference between the two patterns is illustrated in the following figure:

![PubSub Overview](images/image001.png "OPC UA IoT Toolkit Context")

The Publish-Subscribe message pattern is a powerful tool for factory owners that want to collect data from large numbers of publishers. In particular, it is a technology that is well suited for publishing factory data to the cloud. Publish-Subscribe is not well suited for use cases, such as device configuration, where a Client is blocked until it receives information back from the Server. OPC UA supports both models because it is the best way to meet the complex needs of modern factories. 

The purpose of this toolkit is to provide a bare bones implementation of OPC UA PubSub which illustrates the following:
1) Implementing OPC UA is easy to do using standard Open-source libraries;
2) OPC UA Pub Sub allows factory owners to take advantage of the MQTT infrastructure to visualize their factory;
3) Using OPC UA Information Models to configure applications and define the contents of messages. 

### Requirements
This toolkit contains software designed to run on a Raspberry Pi or other Linux device with GPIO ports.  

It will also run on a Linux or Windows machine without GPIO ports in simulation mode. 

This version of toolkit uses .NET Core 3.1 LTS, however, future versions will also provide NodeJS, Python and ANSI C samples. 

The recommended development tool is Visual Studio Code and/or Visual Studio 2019.

An MQTT broker is required. Eclipse Mosquitto is used in the toolkit documentation.  

A MQTT monitoring application, such as MQTT Spy, is also helpful. 

All of the code and documentation can be found on [GitHub](https://github.com/OPCF-Members/UA-IoT-Toolkit). 
