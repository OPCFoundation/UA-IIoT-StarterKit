using Opc.Ua;

namespace UaPublisher
{
    internal class PublisherConnection
    {
        private readonly List<PublisherGroup> m_groups;
        private PubSubConnectionDataType m_connection;

        public PublisherConnection(string name, string topicPrefix, string publisherId, List<PublisherGroup> groups)
        {
            Name = name;
            TopicPrefix = topicPrefix;
            PublisherId = publisherId;
            m_groups = groups;
            m_connection = BuildConnection();
        }

        public string Name { get; private set; }
        public bool Enabled { get; set; }
        public string TopicPrefix { get; private set; }
        public string PublisherId { get; private set; }
        public PubSubConnectionDataType Connection => m_connection;
        public List<PublisherGroup> Groups => m_groups;

        private PubSubConnectionDataType BuildConnection()
        {
            PubSubConnectionDataType connection = new();

            connection.Name = Name;
            connection.PublisherId = PublisherId;
            connection.Enabled = Enabled;
            connection.TransportProfileUri = "http://opcfoundation.org/UA-Profile/Transport/pubsub-mqtt-json";
            connection.WriterGroups = new();

            foreach (var group in m_groups)
            {
                connection.WriterGroups.Add(group.BuildWriterGroup(PublisherId, TopicPrefix));
            }

            return connection;
        }
    }
}