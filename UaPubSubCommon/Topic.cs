using System.Text;

namespace UaPubSubCommon
{
    public class Topic
    {
        public string TopicPrefix { get; set; }
        public string Encoding { get; set; }
        public string MessageType { get; set; }
        public string PublisherId { get; set; }
        public string GroupName { get; set; }
        public string WriterName { get; set; }

        public string Build()
        {
            StringBuilder topic = new();

            topic.Append(TopicPrefix);
            topic.Append(Encoding ?? "/json");
            topic.Append('/');
            topic.Append(MessageType);
            topic.Append('/');
            topic.Append(PublisherId);

            if (!String.IsNullOrEmpty(GroupName))
            {
                topic.Append('/');
                topic.Append(GroupName);

                if (!String.IsNullOrEmpty(WriterName))
                {
                    topic.Append('/');
                    topic.Append(WriterName);
                }
            }

            return topic.ToString();
        }

        public static Topic Parse(string topic, string prefix = "opcua")
        {
            Topic parsed = new();

            int index = topic.IndexOf(prefix);

            if (index != 0)
            {
                throw new ArgumentException($"Topic must start with {prefix}.");
            }

            topic = topic[(prefix.Length + 1)..^0];
            parsed.TopicPrefix = prefix;

            string[] parts = topic.Split('/');

            if (parts.Length > 0)
            {
                parsed.Encoding = parts[0];

                if (parts.Length > 1)
                {
                    parsed.MessageType = parts[1];

                    if (parts.Length > 2)
                    {
                        parsed.PublisherId = parts[2];

                        if (parts.Length > 3)
                        {
                            parsed.GroupName = parts[3];

                            if (parts.Length > 4)
                            {
                                parsed.WriterName = parts[4];
                            }
                        }
                    }
                }
            }

            return parsed;
        }
    }
}
