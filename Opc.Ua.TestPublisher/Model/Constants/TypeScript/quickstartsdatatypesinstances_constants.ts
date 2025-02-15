export const NS = 'http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances';

export class BrowseNames {
   static readonly BicycleType: string = 'BicycleType'
   static readonly DataTypeInstances_BinarySchema: string = 'Quickstarts.DataTypes.Instances'
   static readonly DataTypeInstances_XmlSchema: string = 'Quickstarts.DataTypes.Instances'
   static readonly DriverOfTheMonth: string = 'DriverOfTheMonth'
   static readonly LotType: string = 'LotType'
   static readonly ParkingLot: string = 'ParkingLot'
   static readonly ParkingLotType: string = 'ParkingLotType'
   static readonly ScooterType: string = 'ScooterType'
   static readonly TwoWheelerType: string = 'TwoWheelerType'
   static readonly VehiclesInLot: string = 'VehiclesInLot'
}

export class DataTypeIds {
    static readonly ParkingLotType: string = 'nsu=' + NS + ';i=378'
    static readonly TwoWheelerType: string = 'nsu=' + NS + ';i=15014'
    static readonly BicycleType: string = 'nsu=' + NS + ';i=15004'
    static readonly ScooterType: string = 'nsu=' + NS + ';i=15015'
}

export class ObjectIds {
    static readonly ParkingLot: string = 'nsu=' + NS + ';i=281'
    static readonly ParkingLot_DriverOfTheMonth: string = 'nsu=' + NS + ';i=375'
    static readonly TwoWheelerType_Encoding_DefaultBinary: string = 'nsu=' + NS + ';i=15016'
    static readonly BicycleType_Encoding_DefaultBinary: string = 'nsu=' + NS + ';i=15005'
    static readonly ScooterType_Encoding_DefaultBinary: string = 'nsu=' + NS + ';i=15017'
    static readonly TwoWheelerType_Encoding_DefaultXml: string = 'nsu=' + NS + ';i=15024'
    static readonly BicycleType_Encoding_DefaultXml: string = 'nsu=' + NS + ';i=15009'
    static readonly ScooterType_Encoding_DefaultXml: string = 'nsu=' + NS + ';i=15025'
    static readonly TwoWheelerType_Encoding_DefaultJson: string = 'nsu=' + NS + ';i=15032'
    static readonly BicycleType_Encoding_DefaultJson: string = 'nsu=' + NS + ';i=15013'
    static readonly ScooterType_Encoding_DefaultJson: string = 'nsu=' + NS + ';i=15033'
}

export class VariableIds {
    static readonly ParkingLotType_EnumValues: string = 'nsu=' + NS + ';i=15001'
    static readonly ParkingLot_LotType: string = 'nsu=' + NS + ';i=380'
    static readonly ParkingLot_DriverOfTheMonth_PrimaryVehicle: string = 'nsu=' + NS + ';i=376'
    static readonly ParkingLot_DriverOfTheMonth_OwnedVehicles: string = 'nsu=' + NS + ';i=377'
    static readonly ParkingLot_VehiclesInLot: string = 'nsu=' + NS + ';i=283'
    static readonly DataTypeInstances_BinarySchema: string = 'nsu=' + NS + ';i=353'
    static readonly DataTypeInstances_BinarySchema_NamespaceUri: string = 'nsu=' + NS + ';i=355'
    static readonly DataTypeInstances_BinarySchema_Deprecated: string = 'nsu=' + NS + ';i=15002'
    static readonly DataTypeInstances_BinarySchema_TwoWheelerType: string = 'nsu=' + NS + ';i=15018'
    static readonly DataTypeInstances_BinarySchema_BicycleType: string = 'nsu=' + NS + ';i=15006'
    static readonly DataTypeInstances_BinarySchema_ScooterType: string = 'nsu=' + NS + ';i=15021'
    static readonly DataTypeInstances_XmlSchema: string = 'nsu=' + NS + ';i=341'
    static readonly DataTypeInstances_XmlSchema_NamespaceUri: string = 'nsu=' + NS + ';i=343'
    static readonly DataTypeInstances_XmlSchema_Deprecated: string = 'nsu=' + NS + ';i=15003'
    static readonly DataTypeInstances_XmlSchema_TwoWheelerType: string = 'nsu=' + NS + ';i=15026'
    static readonly DataTypeInstances_XmlSchema_BicycleType: string = 'nsu=' + NS + ';i=15010'
    static readonly DataTypeInstances_XmlSchema_ScooterType: string = 'nsu=' + NS + ';i=15029'
}
