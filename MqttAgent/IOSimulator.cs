using Newtonsoft.Json;
using Opc.Ua;
using Opc.Ua.PubSub;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace MqttAgent
{
    public interface IIOManager : IUaPubSubDataStore
    {
        ushort DataSetId { get; }

        PublishedDataSetDataType DataSet { get; }
        
        void Start();
    }

    public class VendorNameplateManager : IIOManager
    {
        private string m_filePath;
        private VendorNameplate m_nameplate;

        public VendorNameplateManager(ushort datasetId, DataSetMetaDataType metadata, string filePath)
        {
            m_filePath = filePath;

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
            var text = File.ReadAllText(m_filePath ?? "config/nameplate.json");
            m_nameplate = JsonConvert.DeserializeObject<VendorNameplate>(text);
        }

        public DataValue ReadPublishedDataItem(NodeId nodeId, uint attributeId = 13)
        {
            var name = nodeId?.Identifier as string;

            if (name == null)
            {
                return null;
            }

            DataValue value;

            switch (name)
            {
                default: { return null; }

                case "Manufacturer": { value = new DataValue() { WrappedValue = m_nameplate.Manufacturer }; break; }
                case "ManufacturerUri": { value = new DataValue() { WrappedValue = m_nameplate.ManufacturerUri }; break; }
                case "ProductInstanceUri": { value = new DataValue() { WrappedValue = m_nameplate.ProductInstanceUri }; break; }
                case "Model": { value = new DataValue() { WrappedValue = m_nameplate.Model }; break; }
                case "SerialNumber": { value = new DataValue() { WrappedValue = m_nameplate.SerialNumber }; break; }
                case "HardwareRevision": { value = new DataValue() { WrappedValue = m_nameplate.HardwareRevision }; break; }
                case "SoftwareRevision": { value = new DataValue() { WrappedValue = m_nameplate.SoftwareRevision }; break; }
            }

            value.SourceTimestamp = DateTime.UtcNow;
            return value;
        }

        public void WritePublishedDataItem(NodeId nodeId, uint attributeId = 13, DataValue dataValue = null)
        {
            throw new NotImplementedException();
        }
    }

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

        public DataValue ReadPublishedDataItem(NodeId nodeId, uint attributeId = 13)
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

    public class DataStore : IUaPubSubDataStore
    {
        private Dictionary<ushort, IIOManager> m_ioManagers;

        public DataStore(IEnumerable<IIOManager> ioManagers)
        {
            m_ioManagers = new Dictionary<ushort, IIOManager>();

            foreach (var ii in ioManagers)
            {
                m_ioManagers.Add(ii.DataSetId, ii);
            }
        }
        public void Start()
        {
            foreach (var ii in m_ioManagers.Values)
            {
                ii.Start();
            }
        }

        public DataValue ReadPublishedDataItem(NodeId nodeId, uint attributeId = 13)
        {
            IIOManager ioManager;

            if (!m_ioManagers.TryGetValue(nodeId.NamespaceIndex, out ioManager))
            {
                return null;
            }

            return ioManager.ReadPublishedDataItem(nodeId);
        }

        public void WritePublishedDataItem(NodeId nodeId, uint attributeId = 13, DataValue dataValue = null)
        {
            throw new NotImplementedException();
        }
    }
}
