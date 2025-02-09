namespace UaMqttCommon
{
    public class Configuration
    {
        public string BrokerHost { get; set; } = "iop-gateway-germany.opcfoundation.org";
        public int BrokerPort { get; set; } = 8883;
        public bool UseTls { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string TopicPrefix { get; set; } = "opcua";
        public string PublisherId { get; set; } = "iiot-starterkit-quickstart";
    }
}
