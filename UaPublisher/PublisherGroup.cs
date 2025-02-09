using Opc.Ua;
using UaPubSubCommon;

namespace UaPublisher
{
    internal class PublisherGroup
    {
        private readonly List<PublisherWriter> m_writers;

        public PublisherGroup(ushort id, string name, List<PublisherWriter> writers)
        {
            Id = id;
            Name = name;
            m_writers = writers;

            // re-number writers. 
            for (int ii = 0; ii < m_writers.Count; ii++)
            {
                if (m_writers[ii].Id == 0)
                {
                    m_writers[ii].Id = (ushort)(Id + ii + 1);
                }
            }
        }

        public ushort Id { get; private set; }

        public string Name { get; private set; }

        public bool Enabled { get; set; }

        public int PublishingInterval { get; set; }

        public int KeepAliveCount { get; set; }

        public int MetaDataPublishingCount { get; set; }

        public string HeaderLayoutUri { get; set; }

        public string TopicForData { get; set; }

        public BrokerTransportQualityOfService Qos { get; set; } = BrokerTransportQualityOfService.NotSpecified;

        public List<PublisherWriter> Writers => m_writers;

        public WriterGroupDataType BuildWriterGroup(string publisherId, string topicPrefix = null)
        {
            var mask = GetNetworkMessageContentMask(HeaderLayoutUri);
            string groupTopicForData = null;

            // topic set at the group level if multiple datasets in a network message.
            if ((mask & JsonNetworkMessageContentMask.SingleDataSetMessage) == 0)
            {
                groupTopicForData = TopicForData;

                if (String.IsNullOrEmpty(groupTopicForData))
                {
                    var topic = new Topic()
                    {
                        TopicPrefix = topicPrefix,
                        MessageType = TopicTypes.DataSetMessage,
                        PublisherId = publisherId,
                        GroupName = Name
                    };

                    groupTopicForData = topic.Build();
                }
            }

            var wg = new WriterGroupDataType()
            {
                Name = Name,
                WriterGroupId = Id,
                Enabled = Enabled,
                PublishingInterval = PublishingInterval,
                KeepAliveTime = KeepAliveCount * PublishingInterval,
                HeaderLayoutUri = HeaderLayoutUri,
                MessageSettings = new ExtensionObject(DataTypeIds.JsonWriterGroupMessageDataType, new JsonWriterGroupMessageDataType()
                {
                    NetworkMessageContentMask = (uint)mask
                }),
                TransportSettings = new ExtensionObject(new BrokerWriterGroupTransportDataType()
                {
                    QueueName = groupTopicForData ?? null,
                    RequestedDeliveryGuarantee = Qos
                }),
                DataSetWriters = new()
            };

            foreach (var writer in m_writers)
            {
                // data topic set at the writer level if not set at group level.
                string writerTopicForData = writer.TopicForData;

                if (String.IsNullOrEmpty(groupTopicForData) && String.IsNullOrEmpty(writerTopicForData))
                {
                    var topic = new Topic()
                    {
                        TopicPrefix = topicPrefix,
                        MessageType = TopicTypes.DataSetMessage,
                        PublisherId = publisherId,
                        GroupName = Name,
                        WriterName = writer.Name
                    };

                    writerTopicForData = topic.Build();
                }

                // metadata topic always set at the writer level.
                string topicForMetaData = writer.TopicForMetaData;

                if (String.IsNullOrEmpty(topicForMetaData))
                {
                    var topic = new Topic()
                    {
                        TopicPrefix = topicPrefix,
                        MessageType = TopicTypes.DataSetMetaData,
                        PublisherId = publisherId,
                        GroupName = Name,
                        WriterName = writer.Name
                    };

                    topicForMetaData = topic.Build();
                }

                var dsw = new DataSetWriterDataType()
                {
                    Name = writer.Name,
                    DataSetWriterId = writer.Id,
                    DataSetFieldContentMask = (uint)writer.FieldMask,
                    KeyFrameCount = writer.KeyFrameCount,
                    Enabled = writer.Enabled,
                    DataSetName = writer.DataSetName ?? writer.Name,
                    MessageSettings = new ExtensionObject(DataTypeIds.JsonDataSetWriterMessageDataType, new JsonDataSetWriterMessageDataType()
                    {
                        DataSetMessageContentMask = (uint)GetDataSetMessageContentMask(HeaderLayoutUri, writer.KeyFrameCount)
                    }),
                    TransportSettings = new ExtensionObject(new BrokerDataSetWriterTransportDataType()
                    {
                        QueueName = writerTopicForData,
                        MetaDataQueueName = topicForMetaData,
                        MetaDataUpdateTime = (double)(MetaDataPublishingCount * PublishingInterval),
                        RequestedDeliveryGuarantee = writer.Qos
                    })
                };

                wg.DataSetWriters.Add(dsw);
            }

            return wg;
        }

        private static JsonNetworkMessageContentMask GetNetworkMessageContentMask(string profileUri)
        {
            switch (profileUri)
            {
                default:
                case HeaderProfiles.JsonMinimal:
                    return JsonNetworkMessageContentMask.SingleDataSetMessage;
                case HeaderProfiles.JsonDataSetMessage:
                    return (
                        JsonNetworkMessageContentMask.DataSetMessageHeader |
                        JsonNetworkMessageContentMask.SingleDataSetMessage
                    );
                case HeaderProfiles.JsonNetworkMessage:
                    return (
                        JsonNetworkMessageContentMask.NetworkMessageHeader |
                        JsonNetworkMessageContentMask.DataSetMessageHeader |
                        JsonNetworkMessageContentMask.PublisherId
                    );
            }
        }

        private static JsonDataSetMessageContentMask GetDataSetMessageContentMask(string profileUri, uint? keyFrameCount = null)
        {
            JsonDataSetMessageContentMask contentMask = 0;

            switch (profileUri)
            {
                default:
                case HeaderProfiles.JsonMinimal:
                    contentMask = (
                        JsonDataSetMessageContentMask.FieldEncoding2
                    );
                    break;
                case HeaderProfiles.JsonDataSetMessage:
                    contentMask = (
                        JsonDataSetMessageContentMask.DataSetWriterId |
                        JsonDataSetMessageContentMask.SequenceNumber |
                        JsonDataSetMessageContentMask.Timestamp |
                        JsonDataSetMessageContentMask.Status |
                        JsonDataSetMessageContentMask.PublisherId |
                        JsonDataSetMessageContentMask.MinorVersion |
                        JsonDataSetMessageContentMask.FieldEncoding2
                    );
                    break;
                case HeaderProfiles.JsonNetworkMessage:
                    contentMask = (
                        JsonDataSetMessageContentMask.DataSetWriterId |
                        JsonDataSetMessageContentMask.SequenceNumber |
                        JsonDataSetMessageContentMask.Timestamp |
                        JsonDataSetMessageContentMask.Status |
                        JsonDataSetMessageContentMask.MinorVersion |
                        JsonDataSetMessageContentMask.FieldEncoding2
                    );
                    break;
                case HeaderProfiles.JsonActionMessage:
                    contentMask = (
                        JsonDataSetMessageContentMask.MessageType |
                        JsonDataSetMessageContentMask.DataSetWriterId |
                        JsonDataSetMessageContentMask.Timestamp |
                        JsonDataSetMessageContentMask.MinorVersion |
                        JsonDataSetMessageContentMask.FieldEncoding2
                    );
                    break;
            }

            if ((keyFrameCount ?? 1) != 1)
            {
                contentMask |= JsonDataSetMessageContentMask.MessageType;
            }

            return contentMask;
        }
    }
}