namespace Quickstarts.DataTypes.Instances.WebApi
{
    /// <summary>
    /// The namespaces used in the model.
    /// </summary>
    public static class Namespaces
    {
        /// <remarks />
        public const string Uri = "http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances";
    }

    /// <summary>
    /// The browse names defined in the model.
    /// </summary>
    public static class BrowseNames
    {
        /// <remarks />
        public const string BicycleType = "BicycleType";
        /// <remarks />
        public const string DataTypeInstances_BinarySchema = "Quickstarts.DataTypes.Instances";
        /// <remarks />
        public const string DataTypeInstances_XmlSchema = "Quickstarts.DataTypes.Instances";
        /// <remarks />
        public const string DriverOfTheMonth = "DriverOfTheMonth";
        /// <remarks />
        public const string LotType = "LotType";
        /// <remarks />
        public const string ParkingLot = "ParkingLot";
        /// <remarks />
        public const string ParkingLotType = "ParkingLotType";
        /// <remarks />
        public const string ScooterType = "ScooterType";
        /// <remarks />
        public const string TwoWheelerType = "TwoWheelerType";
        /// <remarks />
        public const string VehiclesInLot = "VehiclesInLot";
    }

    /// <summary>
    /// The well known identifiers for DataType nodes.
    /// </summary>
    public static class DataTypeIds {
        /// <remarks />
        public const string ParkingLotType = "nsu=" + Namespaces.Uri + ";i=378";
        /// <remarks />
        public const string TwoWheelerType = "nsu=" + Namespaces.Uri + ";i=15014";
        /// <remarks />
        public const string BicycleType = "nsu=" + Namespaces.Uri + ";i=15004";
        /// <remarks />
        public const string ScooterType = "nsu=" + Namespaces.Uri + ";i=15015";

        /// <summary>
        /// Converts a value to a name for display.
        /// </summary>
        public static string ToName(string value)
        {
            foreach (var field in typeof(DataTypeIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.GetValue(null).Equals(value))
                {
                    return field.Name;
                }
            }

            return value.ToString();
        }
    }

    /// <summary>
    /// The well known identifiers for Object nodes.
    /// </summary>
    public static class ObjectIds {
        /// <remarks />
        public const string ParkingLot = "nsu=" + Namespaces.Uri + ";i=281";
        /// <remarks />
        public const string ParkingLot_DriverOfTheMonth = "nsu=" + Namespaces.Uri + ";i=375";
        /// <remarks />
        public const string TwoWheelerType_Encoding_DefaultBinary = "nsu=" + Namespaces.Uri + ";i=15016";
        /// <remarks />
        public const string BicycleType_Encoding_DefaultBinary = "nsu=" + Namespaces.Uri + ";i=15005";
        /// <remarks />
        public const string ScooterType_Encoding_DefaultBinary = "nsu=" + Namespaces.Uri + ";i=15017";
        /// <remarks />
        public const string TwoWheelerType_Encoding_DefaultXml = "nsu=" + Namespaces.Uri + ";i=15024";
        /// <remarks />
        public const string BicycleType_Encoding_DefaultXml = "nsu=" + Namespaces.Uri + ";i=15009";
        /// <remarks />
        public const string ScooterType_Encoding_DefaultXml = "nsu=" + Namespaces.Uri + ";i=15025";
        /// <remarks />
        public const string TwoWheelerType_Encoding_DefaultJson = "nsu=" + Namespaces.Uri + ";i=15032";
        /// <remarks />
        public const string BicycleType_Encoding_DefaultJson = "nsu=" + Namespaces.Uri + ";i=15013";
        /// <remarks />
        public const string ScooterType_Encoding_DefaultJson = "nsu=" + Namespaces.Uri + ";i=15033";

        /// <summary>
        /// Converts a value to a name for display.
        /// </summary>
        public static string ToName(string value)
        {
            foreach (var field in typeof(ObjectIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.GetValue(null).Equals(value))
                {
                    return field.Name;
                }
            }

            return value.ToString();
        }
    }

    /// <summary>
    /// The well known identifiers for Variable nodes.
    /// </summary>
    public static class VariableIds {
        /// <remarks />
        public const string ParkingLotType_EnumValues = "nsu=" + Namespaces.Uri + ";i=15001";
        /// <remarks />
        public const string ParkingLot_LotType = "nsu=" + Namespaces.Uri + ";i=380";
        /// <remarks />
        public const string ParkingLot_DriverOfTheMonth_PrimaryVehicle = "nsu=" + Namespaces.Uri + ";i=376";
        /// <remarks />
        public const string ParkingLot_DriverOfTheMonth_OwnedVehicles = "nsu=" + Namespaces.Uri + ";i=377";
        /// <remarks />
        public const string ParkingLot_VehiclesInLot = "nsu=" + Namespaces.Uri + ";i=283";
        /// <remarks />
        public const string DataTypeInstances_BinarySchema = "nsu=" + Namespaces.Uri + ";i=353";
        /// <remarks />
        public const string DataTypeInstances_BinarySchema_NamespaceUri = "nsu=" + Namespaces.Uri + ";i=355";
        /// <remarks />
        public const string DataTypeInstances_BinarySchema_Deprecated = "nsu=" + Namespaces.Uri + ";i=15002";
        /// <remarks />
        public const string DataTypeInstances_BinarySchema_TwoWheelerType = "nsu=" + Namespaces.Uri + ";i=15018";
        /// <remarks />
        public const string DataTypeInstances_BinarySchema_BicycleType = "nsu=" + Namespaces.Uri + ";i=15006";
        /// <remarks />
        public const string DataTypeInstances_BinarySchema_ScooterType = "nsu=" + Namespaces.Uri + ";i=15021";
        /// <remarks />
        public const string DataTypeInstances_XmlSchema = "nsu=" + Namespaces.Uri + ";i=341";
        /// <remarks />
        public const string DataTypeInstances_XmlSchema_NamespaceUri = "nsu=" + Namespaces.Uri + ";i=343";
        /// <remarks />
        public const string DataTypeInstances_XmlSchema_Deprecated = "nsu=" + Namespaces.Uri + ";i=15003";
        /// <remarks />
        public const string DataTypeInstances_XmlSchema_TwoWheelerType = "nsu=" + Namespaces.Uri + ";i=15026";
        /// <remarks />
        public const string DataTypeInstances_XmlSchema_BicycleType = "nsu=" + Namespaces.Uri + ";i=15010";
        /// <remarks />
        public const string DataTypeInstances_XmlSchema_ScooterType = "nsu=" + Namespaces.Uri + ";i=15029";

        /// <summary>
        /// Converts a value to a name for display.
        /// </summary>
        public static string ToName(string value)
        {
            foreach (var field in typeof(VariableIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.GetValue(null).Equals(value))
                {
                    return field.Name;
                }
            }

            return value.ToString();
        }
    }
    
}