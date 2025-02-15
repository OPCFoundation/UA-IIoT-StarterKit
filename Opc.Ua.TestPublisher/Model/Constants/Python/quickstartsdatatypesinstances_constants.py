from enum import Enum

class Namespaces(Enum):
     Uri = "http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances"

class BrowseNames(Enum):
    BicycleType = "BicycleType"
    DataTypeInstances_BinarySchema = "Quickstarts.DataTypes.Instances"
    DataTypeInstances_XmlSchema = "Quickstarts.DataTypes.Instances"
    DriverOfTheMonth = "DriverOfTheMonth"
    LotType = "LotType"
    ParkingLot = "ParkingLot"
    ParkingLotType = "ParkingLotType"
    ScooterType = "ScooterType"
    TwoWheelerType = "TwoWheelerType"
    VehiclesInLot = "VehiclesInLot"

class DataTypeIds(Enum):
    ParkingLotType = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=378"
    TwoWheelerType = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15014"
    BicycleType = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15004"
    ScooterType = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15015"

def get_DataTypeIds_name(value: str) -> str:
    try:
        return DataTypeIds(value).name
    except ValueError:
        return None


class ObjectIds(Enum):
    ParkingLot = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=281"
    ParkingLot_DriverOfTheMonth = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=375"
    TwoWheelerType_Encoding_DefaultBinary = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15016"
    BicycleType_Encoding_DefaultBinary = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15005"
    ScooterType_Encoding_DefaultBinary = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15017"
    TwoWheelerType_Encoding_DefaultXml = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15024"
    BicycleType_Encoding_DefaultXml = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15009"
    ScooterType_Encoding_DefaultXml = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15025"
    TwoWheelerType_Encoding_DefaultJson = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15032"
    BicycleType_Encoding_DefaultJson = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15013"
    ScooterType_Encoding_DefaultJson = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15033"

def get_ObjectIds_name(value: str) -> str:
    try:
        return ObjectIds(value).name
    except ValueError:
        return None


class VariableIds(Enum):
    ParkingLotType_EnumValues = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15001"
    ParkingLot_LotType = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=380"
    ParkingLot_DriverOfTheMonth_PrimaryVehicle = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=376"
    ParkingLot_DriverOfTheMonth_OwnedVehicles = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=377"
    ParkingLot_VehiclesInLot = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=283"
    DataTypeInstances_BinarySchema = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=353"
    DataTypeInstances_BinarySchema_NamespaceUri = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=355"
    DataTypeInstances_BinarySchema_Deprecated = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15002"
    DataTypeInstances_BinarySchema_TwoWheelerType = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15018"
    DataTypeInstances_BinarySchema_BicycleType = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15006"
    DataTypeInstances_BinarySchema_ScooterType = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15021"
    DataTypeInstances_XmlSchema = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=341"
    DataTypeInstances_XmlSchema_NamespaceUri = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=343"
    DataTypeInstances_XmlSchema_Deprecated = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15003"
    DataTypeInstances_XmlSchema_TwoWheelerType = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15026"
    DataTypeInstances_XmlSchema_BicycleType = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15010"
    DataTypeInstances_XmlSchema_ScooterType = "nsu=http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances;i=15029"

def get_VariableIds_name(value: str) -> str:
    try:
        return VariableIds(value).name
    except ValueError:
        return None

