## OPC UA IIoT StarterKit – UaPublisher
### Overview

This application is an OPC UA publisher that simulates processes that produce data and publishes changes using OPC UA over MQTT.

The UaPublisher creates instances of the [PublisherSource](..\UaPubSubCommon\PublisherSource.cs) class to similate the data. The [Boiler](..\UaPubSubCommon\Boiler.cs) subclass simulates the Boiler feed forward loop that has been used in other OPC UA sample applications. The PublisherSource class generates the DataSetMetaData that is published to the Broker.

The Publisher instantiates the PublisherSources and creates the PublisherGroups that specify how the information is published to the MQTT Broker. The sample creates one PublisherGroup for each of the standard [Header Layout Profiles](https://reference.opcfoundation.org/Core/Part14/v105/docs/A.3). The HeaderProfile may be one of the URIs specified in [Annex A](https://reference.opcfoundation.org/Core/Part14/v105/docs/A.3). The following HeaderProfileUris are supported:

|URI|Description|
|---|---|
|http://opcfoundation.org/UA/PubSub-Layouts/JSON-Minimal|Publishes a simple message without any headers.|
|http://opcfoundation.org/UA/PubSub-Layouts/JSON-DataSetMessage|Publishes a message containing a single [DataSetMessage](https://reference.opcfoundation.org/Core/Part14/v105/docs/7.2.5.4).|
|http://opcfoundation.org/UA/PubSub-Layouts/JSON-NetworkMessage|Publishes a [NetworkMessage](https://reference.opcfoundation.org/Core/Part14/v105/docs/7.2.5.3) containing a multiple [DataSetMessage](https://reference.opcfoundation.org/Core/Part14/v105/docs/7.2.5.4).|

The PublisherGroup class specifies the topics that are use to send the data and metadata messages. The example uses one set of topics based on ISA-95 enterprise naming convensions. These types of topics are useful to publishers that are being integrated into a UNS (Unified Namespace) topic structure. 

The Publisher is also responsible for triggering changes to the PublisherSources. The sample has a single loop that triggers chnages then publishes. A real application would likely have a background thread dealing with an underlying system. 

The PublisherConnection class is used to generate the PubSubConnection message which allows Subscribers to discover where the Publishers are sending the data and what message layout options are enabled.

The PubisherSource includes current value of Properties specified for a field in the DataSetMetaData. If the values of these Properties change a new DataSetMetaData message is sent. Only rarely changing configuration information should be included in the Properties list.

The PublishSource classes adopt a naming convention for field names that place the field within an OPC UA AddressSpace. The sample does not have an embedded OPC UA server to keep the code as simple as possible, however, this convension allows a subscriber to reconstruction the heirarchy that appears in the OPC UA AddressSpace.

UA PubSub over MQTT allows Publshers to send messages that only have changes since the last message. These messages are called 'ua-deltaframe' messages. However, Subscribers can come an go at any time so they need a complete set of fields. 'ua-keyframe' messages with all fields are sent periodically for this reason. The KeyFrameCount field specifies the interval between ua-keyframe messages.

The Configuration class specifies the location of the Broker, the PublisherId and the TopicPrefix. The PublisherId needs to be unique within the context of a system. The TopicPrefix allows all publishers within a system to use a common topic root.