namespace TestModel.WebApi
{
    /// <summary>
    /// The namespaces used in the model.
    /// </summary>
    public static class Namespaces
    {
        /// <remarks />
        public const string Uri = "urn:opcfoundation.org:2024-01:TestModel";
    }

    /// <summary>
    /// The browse names defined in the model.
    /// </summary>
    public static class BrowseNames
    {
        /// <remarks />
        public const string AbstractStructure = "AbstractStructure";
        /// <remarks />
        public const string ArrayStructure = "ArrayStructure";
        /// <remarks />
        public const string ArrayStructureWithAllowSubtypes = "ArrayStructureWithAllowSubtypes";
        /// <remarks />
        public const string BasicUnion = "BasicUnion";
        /// <remarks />
        public const string ConcreteStructure = "ConcreteStructure";
        /// <remarks />
        public const string EnumerationWithGaps = "EnumerationWithGaps";
        /// <remarks />
        public const string NestedStructure = "NestedStructure";
        /// <remarks />
        public const string NestedStructureWithAllowSubtypes = "NestedStructureWithAllowSubtypes";
        /// <remarks />
        public const string ScalarStructure = "ScalarStructure";
        /// <remarks />
        public const string ScalarStructureWithAllowSubtypes = "ScalarStructureWithAllowSubtypes";
        /// <remarks />
        public const string StructureWithOptionalFields = "StructureWithOptionalFields";
        /// <remarks />
        public const string TestModel_BinarySchema = "TestModel";
        /// <remarks />
        public const string TestModel_XmlSchema = "TestModel";
    }

    /// <summary>
    /// The well known identifiers for DataType nodes.
    /// </summary>
    public static class DataTypeIds {
        /// <remarks />
        public const string AbstractStructure = "nsu=" + Namespaces.Uri + ";i=61";
        /// <remarks />
        public const string ConcreteStructure = "nsu=" + Namespaces.Uri + ";i=62";
        /// <remarks />
        public const string EnumerationWithGaps = "nsu=" + Namespaces.Uri + ";i=63";
        /// <remarks />
        public const string ScalarStructure = "nsu=" + Namespaces.Uri + ";i=65";
        /// <remarks />
        public const string ScalarStructureWithAllowSubtypes = "nsu=" + Namespaces.Uri + ";i=66";
        /// <remarks />
        public const string ArrayStructure = "nsu=" + Namespaces.Uri + ";i=67";
        /// <remarks />
        public const string ArrayStructureWithAllowSubtypes = "nsu=" + Namespaces.Uri + ";i=68";
        /// <remarks />
        public const string NestedStructure = "nsu=" + Namespaces.Uri + ";i=69";
        /// <remarks />
        public const string NestedStructureWithAllowSubtypes = "nsu=" + Namespaces.Uri + ";i=70";
        /// <remarks />
        public const string BasicUnion = "nsu=" + Namespaces.Uri + ";i=143";
        /// <remarks />
        public const string StructureWithOptionalFields = "nsu=" + Namespaces.Uri + ";i=144";

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
        public const string AbstractStructure_Encoding_DefaultBinary = "nsu=" + Namespaces.Uri + ";i=71";
        /// <remarks />
        public const string ConcreteStructure_Encoding_DefaultBinary = "nsu=" + Namespaces.Uri + ";i=72";
        /// <remarks />
        public const string ScalarStructure_Encoding_DefaultBinary = "nsu=" + Namespaces.Uri + ";i=73";
        /// <remarks />
        public const string ScalarStructureWithAllowSubtypes_Encoding_DefaultBinary = "nsu=" + Namespaces.Uri + ";i=74";
        /// <remarks />
        public const string ArrayStructure_Encoding_DefaultBinary = "nsu=" + Namespaces.Uri + ";i=75";
        /// <remarks />
        public const string ArrayStructureWithAllowSubtypes_Encoding_DefaultBinary = "nsu=" + Namespaces.Uri + ";i=76";
        /// <remarks />
        public const string NestedStructure_Encoding_DefaultBinary = "nsu=" + Namespaces.Uri + ";i=77";
        /// <remarks />
        public const string NestedStructureWithAllowSubtypes_Encoding_DefaultBinary = "nsu=" + Namespaces.Uri + ";i=78";
        /// <remarks />
        public const string BasicUnion_Encoding_DefaultBinary = "nsu=" + Namespaces.Uri + ";i=145";
        /// <remarks />
        public const string StructureWithOptionalFields_Encoding_DefaultBinary = "nsu=" + Namespaces.Uri + ";i=146";
        /// <remarks />
        public const string AbstractStructure_Encoding_DefaultXml = "nsu=" + Namespaces.Uri + ";i=103";
        /// <remarks />
        public const string ConcreteStructure_Encoding_DefaultXml = "nsu=" + Namespaces.Uri + ";i=104";
        /// <remarks />
        public const string ScalarStructure_Encoding_DefaultXml = "nsu=" + Namespaces.Uri + ";i=105";
        /// <remarks />
        public const string ScalarStructureWithAllowSubtypes_Encoding_DefaultXml = "nsu=" + Namespaces.Uri + ";i=106";
        /// <remarks />
        public const string ArrayStructure_Encoding_DefaultXml = "nsu=" + Namespaces.Uri + ";i=107";
        /// <remarks />
        public const string ArrayStructureWithAllowSubtypes_Encoding_DefaultXml = "nsu=" + Namespaces.Uri + ";i=108";
        /// <remarks />
        public const string NestedStructure_Encoding_DefaultXml = "nsu=" + Namespaces.Uri + ";i=109";
        /// <remarks />
        public const string NestedStructureWithAllowSubtypes_Encoding_DefaultXml = "nsu=" + Namespaces.Uri + ";i=110";
        /// <remarks />
        public const string BasicUnion_Encoding_DefaultXml = "nsu=" + Namespaces.Uri + ";i=153";
        /// <remarks />
        public const string StructureWithOptionalFields_Encoding_DefaultXml = "nsu=" + Namespaces.Uri + ";i=154";
        /// <remarks />
        public const string AbstractStructure_Encoding_DefaultJson = "nsu=" + Namespaces.Uri + ";i=135";
        /// <remarks />
        public const string ConcreteStructure_Encoding_DefaultJson = "nsu=" + Namespaces.Uri + ";i=136";
        /// <remarks />
        public const string ScalarStructure_Encoding_DefaultJson = "nsu=" + Namespaces.Uri + ";i=137";
        /// <remarks />
        public const string ScalarStructureWithAllowSubtypes_Encoding_DefaultJson = "nsu=" + Namespaces.Uri + ";i=138";
        /// <remarks />
        public const string ArrayStructure_Encoding_DefaultJson = "nsu=" + Namespaces.Uri + ";i=139";
        /// <remarks />
        public const string ArrayStructureWithAllowSubtypes_Encoding_DefaultJson = "nsu=" + Namespaces.Uri + ";i=140";
        /// <remarks />
        public const string NestedStructure_Encoding_DefaultJson = "nsu=" + Namespaces.Uri + ";i=141";
        /// <remarks />
        public const string NestedStructureWithAllowSubtypes_Encoding_DefaultJson = "nsu=" + Namespaces.Uri + ";i=142";
        /// <remarks />
        public const string BasicUnion_Encoding_DefaultJson = "nsu=" + Namespaces.Uri + ";i=161";
        /// <remarks />
        public const string StructureWithOptionalFields_Encoding_DefaultJson = "nsu=" + Namespaces.Uri + ";i=162";

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
        public const string EnumerationWithGaps_EnumValues = "nsu=" + Namespaces.Uri + ";i=64";
        /// <remarks />
        public const string TestModel_BinarySchema = "nsu=" + Namespaces.Uri + ";i=13";
        /// <remarks />
        public const string TestModel_BinarySchema_NamespaceUri = "nsu=" + Namespaces.Uri + ";i=15";
        /// <remarks />
        public const string TestModel_BinarySchema_Deprecated = "nsu=" + Namespaces.Uri + ";i=16";
        /// <remarks />
        public const string TestModel_BinarySchema_AbstractStructure = "nsu=" + Namespaces.Uri + ";i=79";
        /// <remarks />
        public const string TestModel_BinarySchema_ConcreteStructure = "nsu=" + Namespaces.Uri + ";i=82";
        /// <remarks />
        public const string TestModel_BinarySchema_ScalarStructure = "nsu=" + Namespaces.Uri + ";i=85";
        /// <remarks />
        public const string TestModel_BinarySchema_ScalarStructureWithAllowSubtypes = "nsu=" + Namespaces.Uri + ";i=88";
        /// <remarks />
        public const string TestModel_BinarySchema_ArrayStructure = "nsu=" + Namespaces.Uri + ";i=91";
        /// <remarks />
        public const string TestModel_BinarySchema_ArrayStructureWithAllowSubtypes = "nsu=" + Namespaces.Uri + ";i=94";
        /// <remarks />
        public const string TestModel_BinarySchema_NestedStructure = "nsu=" + Namespaces.Uri + ";i=97";
        /// <remarks />
        public const string TestModel_BinarySchema_NestedStructureWithAllowSubtypes = "nsu=" + Namespaces.Uri + ";i=100";
        /// <remarks />
        public const string TestModel_BinarySchema_BasicUnion = "nsu=" + Namespaces.Uri + ";i=147";
        /// <remarks />
        public const string TestModel_BinarySchema_StructureWithOptionalFields = "nsu=" + Namespaces.Uri + ";i=150";
        /// <remarks />
        public const string TestModel_XmlSchema = "nsu=" + Namespaces.Uri + ";i=37";
        /// <remarks />
        public const string TestModel_XmlSchema_NamespaceUri = "nsu=" + Namespaces.Uri + ";i=39";
        /// <remarks />
        public const string TestModel_XmlSchema_Deprecated = "nsu=" + Namespaces.Uri + ";i=40";
        /// <remarks />
        public const string TestModel_XmlSchema_AbstractStructure = "nsu=" + Namespaces.Uri + ";i=111";
        /// <remarks />
        public const string TestModel_XmlSchema_ConcreteStructure = "nsu=" + Namespaces.Uri + ";i=114";
        /// <remarks />
        public const string TestModel_XmlSchema_ScalarStructure = "nsu=" + Namespaces.Uri + ";i=117";
        /// <remarks />
        public const string TestModel_XmlSchema_ScalarStructureWithAllowSubtypes = "nsu=" + Namespaces.Uri + ";i=120";
        /// <remarks />
        public const string TestModel_XmlSchema_ArrayStructure = "nsu=" + Namespaces.Uri + ";i=123";
        /// <remarks />
        public const string TestModel_XmlSchema_ArrayStructureWithAllowSubtypes = "nsu=" + Namespaces.Uri + ";i=126";
        /// <remarks />
        public const string TestModel_XmlSchema_NestedStructure = "nsu=" + Namespaces.Uri + ";i=129";
        /// <remarks />
        public const string TestModel_XmlSchema_NestedStructureWithAllowSubtypes = "nsu=" + Namespaces.Uri + ";i=132";
        /// <remarks />
        public const string TestModel_XmlSchema_BasicUnion = "nsu=" + Namespaces.Uri + ";i=155";
        /// <remarks />
        public const string TestModel_XmlSchema_StructureWithOptionalFields = "nsu=" + Namespaces.Uri + ";i=158";

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