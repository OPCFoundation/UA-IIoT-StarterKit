using Newtonsoft.Json;
using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using Opc.Ua.PubSub;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MqttAgent
{
    public class IOSimulator : IIOManager
    {
        private object m_lock = new object();
        private DataSetMetaDataType m_metadata;
        private Dictionary<string, DataValue> m_values;
        private DateTime m_timestamp;
        private Random m_random;
        private Timer m_timer;

        public IOSimulator(ushort datasetId, DataSetMetaDataType metadata)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            m_metadata = metadata;
            m_values = new Dictionary<string, DataValue>();
            m_random = new Random();

            DataSetId = datasetId;

            DataSet = new PublishedDataSetDataType()
            {
                Name = metadata.Name,
                DataSetMetaData = metadata
            };

            PublishedDataItemsDataType source = new PublishedDataItemsDataType();
            source.PublishedData = new PublishedVariableDataTypeCollection();

            foreach (var ii in metadata.Fields)
            {
                var typeInfo = new TypeInfo((BuiltInType)ii.BuiltInType, ii.ValueRank);

                m_values[ii.Name] = new DataValue()
                {
                    WrappedValue = new Variant(TypeInfo.GetDefaultValue(ii.BuiltInType, ii.ValueRank), typeInfo),
                    SourceTimestamp = DateTime.UtcNow
                };

                source.PublishedData.Add(
                    new PublishedVariableDataType()
                    {
                        PublishedVariable = new NodeId(ii.Name, datasetId),
                        AttributeId = Attributes.Value,
                    });
            }

            DataSet.DataSetSource = new ExtensionObject(source);
        }

        public ushort DataSetId { get; }

        public PublishedDataSetDataType DataSet { get; }

        public void Start()
        {
            lock (m_lock)
            {
                if (m_timer != null)
                {
                    m_timer.Dispose();
                }

                m_timer = new Timer(
                    (e) => { DoUpdate(); },
                    null,
                    1000,
                    1000);
            }
        }

        private StatusCode DoUpdate()
        {
            lock (m_lock)
            {
                m_timestamp = DateTime.UtcNow;

                foreach (var ii in m_metadata.Fields)
                {
                    DataValue value;
                    Variant newValue;

                    if (m_values.TryGetValue(ii.Name, out value))
                    {
                        switch ((BuiltInType)ii.BuiltInType)
                        {
                            case BuiltInType.Boolean:
                                {
                                    newValue = (m_random.Next() % 2) == 0;
                                    break;
                                }

                            case BuiltInType.Double:
                                {
                                    newValue = m_random.NextDouble();
                                    break;
                                }
                        }
                    }

                    if (newValue != value.WrappedValue)
                    {
                        m_values[ii.Name] = new DataValue() { Value = newValue, SourceTimestamp = m_timestamp };
                    }
                }
            }

            return StatusCodes.Good;
        }

        public DataValue ReadPublishedDataItem(NodeId nodeId, uint attributeId = 13, bool deltaFrame = false)
        {
            var name = nodeId?.Identifier as string;

            if (name == null)
            {
                return null;
            }

            lock (m_lock)
            {
                DataValue value;

                if (!m_values.TryGetValue(name, out value))
                {
                    return null;
                }

                return value;
            }
        }

        public void WritePublishedDataItem(NodeId nodeId, uint attributeId = 13, DataValue dataValue = null)
        {
            throw new NotImplementedException();
        }
    }
}
