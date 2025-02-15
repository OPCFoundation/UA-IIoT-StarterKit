export const NS = 'http://opcfoundation.org/UA/Quickstarts/DataTypes/Instances';

export const BrowseNames = Object.freeze({
   BicycleType: 'BicycleType',
   DataTypeInstances_BinarySchema: 'Quickstarts.DataTypes.Instances',
   DataTypeInstances_XmlSchema: 'Quickstarts.DataTypes.Instances',
   DriverOfTheMonth: 'DriverOfTheMonth',
   LotType: 'LotType',
   ParkingLot: 'ParkingLot',
   ParkingLotType: 'ParkingLotType',
   ScooterType: 'ScooterType',
   TwoWheelerType: 'TwoWheelerType',
   VehiclesInLot: 'VehiclesInLot',
});

export const DataTypeIds = Object.freeze({
   ParkingLotType: 'nsu=' + NS + ';i=378',
   TwoWheelerType: 'nsu=' + NS + ';i=15014',
   BicycleType: 'nsu=' + NS + ';i=15004',
   ScooterType: 'nsu=' + NS + ';i=15015',
});

export const ObjectIds = Object.freeze({
   ParkingLot: 'nsu=' + NS + ';i=281',
   ParkingLot_DriverOfTheMonth: 'nsu=' + NS + ';i=375',
   TwoWheelerType_Encoding_DefaultBinary: 'nsu=' + NS + ';i=15016',
   BicycleType_Encoding_DefaultBinary: 'nsu=' + NS + ';i=15005',
   ScooterType_Encoding_DefaultBinary: 'nsu=' + NS + ';i=15017',
   TwoWheelerType_Encoding_DefaultXml: 'nsu=' + NS + ';i=15024',
   BicycleType_Encoding_DefaultXml: 'nsu=' + NS + ';i=15009',
   ScooterType_Encoding_DefaultXml: 'nsu=' + NS + ';i=15025',
   TwoWheelerType_Encoding_DefaultJson: 'nsu=' + NS + ';i=15032',
   BicycleType_Encoding_DefaultJson: 'nsu=' + NS + ';i=15013',
   ScooterType_Encoding_DefaultJson: 'nsu=' + NS + ';i=15033',
});

export const VariableIds = Object.freeze({
   ParkingLotType_EnumValues: 'nsu=' + NS + ';i=15001',
   ParkingLot_LotType: 'nsu=' + NS + ';i=380',
   ParkingLot_DriverOfTheMonth_PrimaryVehicle: 'nsu=' + NS + ';i=376',
   ParkingLot_DriverOfTheMonth_OwnedVehicles: 'nsu=' + NS + ';i=377',
   ParkingLot_VehiclesInLot: 'nsu=' + NS + ';i=283',
   DataTypeInstances_BinarySchema: 'nsu=' + NS + ';i=353',
   DataTypeInstances_BinarySchema_NamespaceUri: 'nsu=' + NS + ';i=355',
   DataTypeInstances_BinarySchema_Deprecated: 'nsu=' + NS + ';i=15002',
   DataTypeInstances_BinarySchema_TwoWheelerType: 'nsu=' + NS + ';i=15018',
   DataTypeInstances_BinarySchema_BicycleType: 'nsu=' + NS + ';i=15006',
   DataTypeInstances_BinarySchema_ScooterType: 'nsu=' + NS + ';i=15021',
   DataTypeInstances_XmlSchema: 'nsu=' + NS + ';i=341',
   DataTypeInstances_XmlSchema_NamespaceUri: 'nsu=' + NS + ';i=343',
   DataTypeInstances_XmlSchema_Deprecated: 'nsu=' + NS + ';i=15003',
   DataTypeInstances_XmlSchema_TwoWheelerType: 'nsu=' + NS + ';i=15026',
   DataTypeInstances_XmlSchema_BicycleType: 'nsu=' + NS + ';i=15010',
   DataTypeInstances_XmlSchema_ScooterType: 'nsu=' + NS + ';i=15029',
});
