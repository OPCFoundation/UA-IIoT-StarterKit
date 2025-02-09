using Opc.Ua;

namespace UaPublisher
{
    internal class ActionWriter
    {
        private readonly ActionSource m_source;
        protected Dictionary<string, Variant> m_cache = new();

        public ActionWriter(ushort id, string name, ActionSource source)
        {
            Id = id;
            Name = name;
            m_source = source;
        }

        public ushort Id { get; set; }

        public string Name { get; private set; }

        public string DataSetName { get; private set; }

        public bool Enabled { get; set; }

        public DataSetFieldContentMask FieldMask { get; set; }

        public uint KeyFrameCount { get; set; }

        public string TopicForMetaData { get; set; }

        public BrokerTransportQualityOfService Qos { get; set; } = BrokerTransportQualityOfService.NotSpecified;

        public ActionSource Source => m_source;
        
    }
}