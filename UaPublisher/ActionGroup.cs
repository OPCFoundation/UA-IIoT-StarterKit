using Opc.Ua;
using UaPubSubCommon;

namespace UaPublisher
{
    internal class ActionGroup
    {
        private readonly List<ActionWriter> m_writers;

        public ActionGroup(ushort id, string name, List<ActionWriter> writers)
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

        public double MetaDataPublishingInterval { get; set; }

        public string TopicForData { get; set; }

        public BrokerTransportQualityOfService Qos { get; set; } = BrokerTransportQualityOfService.NotSpecified;

        public List<ActionWriter> Writers => m_writers;

        public WriterGroupDataType BuildWriterGroup(string publisherId, string topicPrefix = null)
        {
            var mask = (
                JsonNetworkMessageContentMask.NetworkMessageHeader |
                JsonNetworkMessageContentMask.DataSetMessageHeader |
                JsonNetworkMessageContentMask.PublisherId |
                JsonNetworkMessageContentMask.ReplyTo
            );

            var groupTopicForData = TopicForData;

            if (String.IsNullOrEmpty(groupTopicForData))
            {
                var topic = new Topic()
                {
                    TopicPrefix = topicPrefix,
                    MessageType = TopicTypes.ActionRequest,
                    PublisherId = publisherId,
                    GroupName = Name
                };

                groupTopicForData = topic.Build();
            }

            var wg = new WriterGroupDataType()
            {
                Name = Name,
                WriterGroupId = Id,
                Enabled = Enabled,
                PublishingInterval = PublishingInterval,
                KeepAliveTime = PublishingInterval,
                HeaderLayoutUri = HeaderProfiles.JsonNetworkMessage,
                MessageSettings = new ExtensionObject(DataTypeIds.JsonWriterGroupMessageDataType, new JsonWriterGroupMessageDataType()
                {
                    NetworkMessageContentMask = (uint)mask
                }),
                TransportSettings = new ExtensionObject(new BrokerWriterGroupTransportDataType()
                {
                    QueueName = groupTopicForData ?? null,
                    RequestedDeliveryGuarantee = (Qos == BrokerTransportQualityOfService.NotSpecified) ? BrokerTransportQualityOfService.AtLeastOnce : Qos
                }),
                DataSetWriters = new()
            };

            foreach (var writer in m_writers)
            {
                // metadata topic always set at the writer level.
                string topicForMetaData = writer.TopicForMetaData;

                if (String.IsNullOrEmpty(topicForMetaData))
                {
                    var topic = new Topic()
                    {
                        TopicPrefix = topicPrefix,
                        MessageType = TopicTypes.ActionMetaData,
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
                        DataSetMessageContentMask = (uint)(
                            JsonDataSetMessageContentMask.DataSetWriterId |
                            JsonDataSetMessageContentMask.Timestamp |
                            JsonDataSetMessageContentMask.Status |
                            JsonDataSetMessageContentMask.MinorVersion |
                            JsonDataSetMessageContentMask.FieldEncoding2
                        )
                    }),
                    TransportSettings = new ExtensionObject(new BrokerDataSetWriterTransportDataType()
                    {
                        MetaDataQueueName = topicForMetaData,
                        MetaDataUpdateTime = MetaDataPublishingInterval,
                        RequestedDeliveryGuarantee = (Qos == BrokerTransportQualityOfService.NotSpecified) ? BrokerTransportQualityOfService.AtLeastOnce : Qos
                    })
                };

                wg.DataSetWriters.Add(dsw);
            }

            return wg;
        }
    }
}