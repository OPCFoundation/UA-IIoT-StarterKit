using Opc.Ua;

namespace UaPublisher
{
    internal class PublisherWriter
    {
        private readonly PublisherSource m_source;
        protected Dictionary<string, Variant> m_cache = new();

        public PublisherWriter(ushort id, string name, PublisherSource source)
        {
            Id = id;
            Name = name;
            m_source = source;
        }

        public ushort Id { get; set; }

        public string Name { get; private set; }

        public bool Enabled { get; set; }

        public DataSetFieldContentMask FieldMask { get; set; }

        public uint KeyFrameCount { get; set; }

        public string DataSetName => m_source?.MetaData?.Name;

        public string TopicForData { get; set; }

        public string TopicForMetaData { get; set; }

        public BrokerTransportQualityOfService Qos { get; set; } = BrokerTransportQualityOfService.NotSpecified;

        public List<KeyValuePair<FieldMetaData, DataValue>> ReadChangedFields(bool isKeyFrame)
        {
            return m_source.ReadChangedFields(m_cache, isKeyFrame); 
        }
    }
}