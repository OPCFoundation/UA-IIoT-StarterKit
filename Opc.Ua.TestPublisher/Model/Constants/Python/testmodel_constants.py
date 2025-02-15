from enum import Enum

class Namespaces(Enum):
     Uri = "urn:opcfoundation.org:2024-01:TestModel"

class BrowseNames(Enum):
    AbstractStructure = "AbstractStructure"
    ArrayStructure = "ArrayStructure"
    ArrayStructureWithAllowSubtypes = "ArrayStructureWithAllowSubtypes"
    BasicUnion = "BasicUnion"
    ConcreteStructure = "ConcreteStructure"
    EnumerationWithGaps = "EnumerationWithGaps"
    NestedStructure = "NestedStructure"
    NestedStructureWithAllowSubtypes = "NestedStructureWithAllowSubtypes"
    ScalarStructure = "ScalarStructure"
    ScalarStructureWithAllowSubtypes = "ScalarStructureWithAllowSubtypes"
    StructureWithOptionalFields = "StructureWithOptionalFields"
    TestModel_BinarySchema = "TestModel"
    TestModel_XmlSchema = "TestModel"

class DataTypeIds(Enum):
    AbstractStructure = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=61"
    ConcreteStructure = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=62"
    EnumerationWithGaps = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=63"
    ScalarStructure = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=65"
    ScalarStructureWithAllowSubtypes = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=66"
    ArrayStructure = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=67"
    ArrayStructureWithAllowSubtypes = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=68"
    NestedStructure = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=69"
    NestedStructureWithAllowSubtypes = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=70"
    BasicUnion = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=143"
    StructureWithOptionalFields = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=144"

def get_DataTypeIds_name(value: str) -> str:
    try:
        return DataTypeIds(value).name
    except ValueError:
        return None


class ObjectIds(Enum):
    AbstractStructure_Encoding_DefaultBinary = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=71"
    ConcreteStructure_Encoding_DefaultBinary = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=72"
    ScalarStructure_Encoding_DefaultBinary = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=73"
    ScalarStructureWithAllowSubtypes_Encoding_DefaultBinary = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=74"
    ArrayStructure_Encoding_DefaultBinary = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=75"
    ArrayStructureWithAllowSubtypes_Encoding_DefaultBinary = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=76"
    NestedStructure_Encoding_DefaultBinary = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=77"
    NestedStructureWithAllowSubtypes_Encoding_DefaultBinary = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=78"
    BasicUnion_Encoding_DefaultBinary = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=145"
    StructureWithOptionalFields_Encoding_DefaultBinary = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=146"
    AbstractStructure_Encoding_DefaultXml = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=103"
    ConcreteStructure_Encoding_DefaultXml = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=104"
    ScalarStructure_Encoding_DefaultXml = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=105"
    ScalarStructureWithAllowSubtypes_Encoding_DefaultXml = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=106"
    ArrayStructure_Encoding_DefaultXml = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=107"
    ArrayStructureWithAllowSubtypes_Encoding_DefaultXml = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=108"
    NestedStructure_Encoding_DefaultXml = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=109"
    NestedStructureWithAllowSubtypes_Encoding_DefaultXml = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=110"
    BasicUnion_Encoding_DefaultXml = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=153"
    StructureWithOptionalFields_Encoding_DefaultXml = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=154"
    AbstractStructure_Encoding_DefaultJson = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=135"
    ConcreteStructure_Encoding_DefaultJson = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=136"
    ScalarStructure_Encoding_DefaultJson = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=137"
    ScalarStructureWithAllowSubtypes_Encoding_DefaultJson = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=138"
    ArrayStructure_Encoding_DefaultJson = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=139"
    ArrayStructureWithAllowSubtypes_Encoding_DefaultJson = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=140"
    NestedStructure_Encoding_DefaultJson = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=141"
    NestedStructureWithAllowSubtypes_Encoding_DefaultJson = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=142"
    BasicUnion_Encoding_DefaultJson = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=161"
    StructureWithOptionalFields_Encoding_DefaultJson = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=162"

def get_ObjectIds_name(value: str) -> str:
    try:
        return ObjectIds(value).name
    except ValueError:
        return None


class VariableIds(Enum):
    EnumerationWithGaps_EnumValues = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=64"
    TestModel_BinarySchema = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=13"
    TestModel_BinarySchema_NamespaceUri = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=15"
    TestModel_BinarySchema_Deprecated = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=16"
    TestModel_BinarySchema_AbstractStructure = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=79"
    TestModel_BinarySchema_ConcreteStructure = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=82"
    TestModel_BinarySchema_ScalarStructure = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=85"
    TestModel_BinarySchema_ScalarStructureWithAllowSubtypes = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=88"
    TestModel_BinarySchema_ArrayStructure = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=91"
    TestModel_BinarySchema_ArrayStructureWithAllowSubtypes = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=94"
    TestModel_BinarySchema_NestedStructure = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=97"
    TestModel_BinarySchema_NestedStructureWithAllowSubtypes = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=100"
    TestModel_BinarySchema_BasicUnion = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=147"
    TestModel_BinarySchema_StructureWithOptionalFields = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=150"
    TestModel_XmlSchema = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=37"
    TestModel_XmlSchema_NamespaceUri = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=39"
    TestModel_XmlSchema_Deprecated = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=40"
    TestModel_XmlSchema_AbstractStructure = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=111"
    TestModel_XmlSchema_ConcreteStructure = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=114"
    TestModel_XmlSchema_ScalarStructure = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=117"
    TestModel_XmlSchema_ScalarStructureWithAllowSubtypes = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=120"
    TestModel_XmlSchema_ArrayStructure = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=123"
    TestModel_XmlSchema_ArrayStructureWithAllowSubtypes = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=126"
    TestModel_XmlSchema_NestedStructure = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=129"
    TestModel_XmlSchema_NestedStructureWithAllowSubtypes = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=132"
    TestModel_XmlSchema_BasicUnion = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=155"
    TestModel_XmlSchema_StructureWithOptionalFields = "nsu=urn:opcfoundation.org:2024-01:TestModel;i=158"

def get_VariableIds_name(value: str) -> str:
    try:
        return VariableIds(value).name
    except ValueError:
        return None

