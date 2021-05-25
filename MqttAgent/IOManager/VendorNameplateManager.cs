using Newtonsoft.Json;
using Opc.Ua;
using System;
using System.IO;

namespace MqttAgent
{
    public class VendorNameplateManager : IIOManager
    {
        private string m_filePath;
        private string m_applicationId;
        private VendorNameplate m_nameplate;

        public VendorNameplateManager(
            ushort datasetId, 
            DataSetMetaDataType metadata, 
            string filePath,
            string applicationId)
        {
            m_filePath = filePath;
            m_applicationId = applicationId;

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
        }

        public DataValue ReadPublishedDataItem(NodeId nodeId, uint attributeId = 13, bool deltaFrame = false)
        {
            var name = nodeId?.Identifier as string;

            if (name == null)
            {
                return null;
            }

            if ( m_nameplate == null)
            {
                var text = File.ReadAllText(m_filePath ?? "config/nameplate.json");
                text = text.Replace("[ApplicationId]", m_applicationId ?? System.Net.Dns.GetHostName());
                m_nameplate = JsonConvert.DeserializeObject<VendorNameplate>(text);
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
}
