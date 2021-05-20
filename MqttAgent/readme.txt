The OPC UA IoT Starter Kit is an implementation of OPC UA PubSub over MQTT using the JSON encoding. It is designed to illustrate how to a fully compliant OPC UA PubSub implementation can be developed quickly using freely available MQTT and JSON libraries.

This package includes JSON files which accompany the code (to be published soon).

1) localhost-connection.json - the OPC UA PubSub connection configuaration file for Subscribers and Publishers.

This file defines a simple dataset ("boiler01") and 2 dataset writers/readers ("boiler01.full" and "boiler01.minimal"). The "boiler01.full" publishes the dataset with all PubSub metadata. "boiler01.minimal" publishes the dataset with the absolute minimum amount of PubSub metadata.

2) Examples of messages can be found in message-full.json and message-minimal.json.



 