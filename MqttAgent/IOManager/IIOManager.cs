using Opc.Ua;
using Opc.Ua.PubSub;
using System;
using System.Collections.Generic;

namespace MqttAgent
{
    public interface IIOManager : IUaPubSubDataStore
    {
        ushort DataSetId { get; }

        PublishedDataSetDataType DataSet { get; }
        
        void Start();
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

        public DataValue ReadPublishedDataItem(NodeId nodeId, uint attributeId = 13, bool isDeltaFrame = false)
        {
            IIOManager ioManager;

            if (!m_ioManagers.TryGetValue(nodeId.NamespaceIndex, out ioManager))
            {
                return null;
            }

            return ioManager.ReadPublishedDataItem(nodeId, attributeId, isDeltaFrame);
        }

        public void WritePublishedDataItem(NodeId nodeId, uint attributeId = 13, DataValue dataValue = null)
        {
            throw new NotImplementedException();
        }

        public void UpdateMetaData(PublishedDataSetDataType publishedDataSet)
        {
        }
    }
}
