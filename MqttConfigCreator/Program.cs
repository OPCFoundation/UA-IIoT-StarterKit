using Opc.Ua;
using System;
using System.IO;

namespace MqttConfigCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateGateMonitorDataSet(@".\config\gate.json");
        }


        static void CreateGateMonitorDataSet(string path)
        {
            PublishedDataItemsDataType items = new PublishedDataItemsDataType();

            items.PublishedData.Add(new PublishedVariableDataType()
            {
                PublishedVariable = new NodeId("s=Gate1.State", 2),
                AttributeId = Attributes.Value,
                SamplingIntervalHint = 1000,
                SubstituteValue = new Variant(false),
                MetaDataProperties = new QualifiedName[] { BrowseNames.TrueState, BrowseNames.FalseState }
            });
            items.PublishedData.Add(new PublishedVariableDataType()
            { 
                PublishedVariable= new NodeId("s=Gate1.CycleCount", 2),
                AttributeId = Attributes.Value,
                SamplingIntervalHint = 1000,
                SubstituteValue = new Variant((uint)0)
            });
            items.PublishedData.Add(new PublishedVariableDataType()
            {
                PublishedVariable = new NodeId("s=Gate1.CycleTime", 2),
                AttributeId = Attributes.Value,
                SamplingIntervalHint = 1000,
                SubstituteValue = new Variant((uint)0),
                MetaDataProperties = new QualifiedName[] { BrowseNames.EUInformation, BrowseNames.EURange }
            });


            using (StreamWriter writer = new StreamWriter(path))
            {
                using (JsonEncoder encoder = new JsonEncoder(ServiceMessageContext.GlobalContext, true, writer, false))
                {
                    items.Encode(encoder);
                    encoder.Close();
                }
            }

        }
    }
}
