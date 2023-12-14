## OPC UA IIoT StarterKit – Quickstart 003
### Using Connection Metadata to discover Message Structure

The quickstarts so far relied on the Subscriber having hardcoded knowledge of the structure of the messages being sent. While this is a valid approach, it is not very flexible. In a real-world scenario, the Subscriber would need to be able to discover what is being published and adapt accordingly. This is where the PubSubConnection metadata comes in. The PubSubConnection metadata defines structure of the messages being sent and where they are being sent.

This quickstart demonstrates how to create and consume the PubSubConnection metadata.

1. [Build a Publisher](UaMqttPublisher/)
2. [Build a Subscriber](UaMqttSubscriber/)
