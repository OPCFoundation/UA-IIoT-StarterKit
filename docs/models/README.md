## OPC UA IIoT StarterKit – Information Models
### Overview
This page describes the information models exposed by the publisher and the OPC Server which is part of the publisher.

Information models are the most important feature of OPC UA PubSub because they provide a standard way describe information. This allows subscribers to easily integrate information from different publishers. It is particularily helpful when pushing data to cloud based analytics applications.

### VendorNameplate
The VendorNameplate provides informance about the device or application which is the source of data. 

The elements of the VendorNameplate are shown in the following figure: 

![VendorNameplate](../images/vendor-nameplate.png "VendorNameplate") 

A complete description of the field in the VendorNameplate can be found [here](https://reference.opcfoundation.org/DI/docs/4.5.2/).

In the samples, every publisher publishes its VendorNameplate. Subscribers can use wildcard topic subscriptions to detect new publishers.  

### Gate Monitor
The Gate Monitor model describes a conveyor belt gate that periodically opens and closes. The current state of the gate is publsihed.

The elements of the gate model are shown in the following figure: 

![gate](../images/single-led.png "gate") 

The gate builds on standard information model elements. 

The current state of the gate if represented by a TwoStateDiscreteType which described [here](https://reference.opcfoundation.org/v104/Core/docs/Part8/5.3.3/#5.3.3.2). 

The cycle time of the gate is represented by an AnalogUnitType which is described [here](https://reference.opcfoundation.org/v104/Core/docs/Part8/5.3.2/#5.3.2.4). 

The metadata that is published with the current include the value of the TrueState and FalseState.

