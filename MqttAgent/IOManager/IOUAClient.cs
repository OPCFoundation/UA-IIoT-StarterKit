using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MqttAgent
{
    public class IOUAClient : IIOManager
    {
        private UAClient m_client;

        public ushort DataSetId { get; }

        public PublishedDataSetDataType DataSet { get; }

        public void Start()
        {
        }

        public DataValue ReadPublishedDataItem(NodeId nodeId, uint attributeId = 13)
        {
            return m_client.ReadPublishedDataItem(nodeId, attributeId);
        }

        public void WritePublishedDataItem(NodeId nodeId, uint attributeId = 13, DataValue dataValue = null)
        {
            throw new NotImplementedException();
        }

        public void UpdateMetaData(PublishedDataSetDataType publishedDataSet)
        {
        }

        public IOUAClient(ushort datasetId, DataSetMetaDataType metadata, UAClient client)
        {
            m_client = client;
            DataSetId = datasetId;

            DataSet = new PublishedDataSetDataType()
            {
                Name = metadata.Name,
                DataSetMetaData = (DataSetMetaDataType)Utils.Clone(metadata)
            };

            string json = File.ReadAllText($"config/sources/{metadata.Name}.json");

            var pdi = new PublishedDataItemsDataType();

            using (var decoder = new JsonDecoder(json, ServiceMessageContext.GlobalContext))
            {
                pdi.Decode(decoder);
            }

            PublishedDataItemsDataType source = new PublishedDataItemsDataType();
            source.PublishedData = new PublishedVariableDataTypeCollection();

            for (int ii = 0; ii < metadata.Fields.Count; ii++)
            {
                var field = metadata.Fields[ii];

                if (ii < pdi.PublishedData.Count)
                {
                    var item = pdi.PublishedData[ii];

                    var variable = new PublishedVariableDataType()
                    {
                        PublishedVariable = new NodeId(field.Name, datasetId),
                        AttributeId = item.AttributeId,
                        SamplingIntervalHint = item.SamplingIntervalHint,
                        SubstituteValue = item.SubstituteValue
                    };

                    source.PublishedData.Add(variable);

                    if (m_client != null)
                    {
                        m_client.Add(variable.PublishedVariable, item, null);
                    }
                }
                else
                {
                    source.PublishedData.Add(
                        new PublishedVariableDataType()
                        {
                            PublishedVariable = new NodeId(field.Name, datasetId),
                            AttributeId = Attributes.Value,
                        });
                }
            }

            DataSet.DataSetSource = new ExtensionObject(source);
        }
    }
}
