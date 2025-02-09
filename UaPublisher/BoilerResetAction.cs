using Opc.Ua;
using UaPubSubCommon;

namespace UaPublisher
{
    internal class BoilerResetAction : ActionSource
    {
        public BoilerResetAction(List<ActionTarget> targets)
        {
            m_request = BuildRequestMetaData();
            m_response = BuildResponseMetaData();
            m_targets = targets;
        }

        public override DataSetMetaDataType BuildRequestMetaData()
        {
            DataSetMetaDataType metadata = new();

            metadata.Name = nameof(BoilerResetAction);
            metadata.Description = "Resets the boiler simulation.";
            metadata.ConfigurationVersion = new ConfigurationVersionDataType()
            {
                MajorVersion = PubSubUtils.GetVersionTime(),
                MinorVersion = PubSubUtils.GetVersionTime()
            };

            metadata.Fields = new FieldMetaDataCollection([
                new FieldMetaData()
                {
                    Name = "NewLevelSetPoint",
                    Description = "The drum level set point after reset.",
                    DataType = DataTypeIds.Double,
                    ValueRank = ValueRanks.Scalar,
                    BuiltInType = (byte)BuiltInType.Double
                }
            ]);

            return metadata;
        }

        public override DataSetMetaDataType BuildResponseMetaData()
        {
            DataSetMetaDataType metadata = new();

            metadata.Name = nameof(BoilerResetAction);
            metadata.ConfigurationVersion = m_request.ConfigurationVersion;

            metadata.Fields = new FieldMetaDataCollection([
                new FieldMetaData()
                {
                    Name = "OldLevelSetPoint",
                    Description = "The drum level set point before the reset.",
                    DataType = DataTypeIds.Double,
                    ValueRank = ValueRanks.Scalar,
                    BuiltInType = (byte)BuiltInType.Double
                }
            ]);

            return metadata;
        }
    }
}
