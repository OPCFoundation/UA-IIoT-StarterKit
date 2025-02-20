export const NS = 'urn:opcfoundation.org:2024-01:TestModel';

export const BrowseNames = Object.freeze({
   AbstractStructure: 'AbstractStructure',
   ArrayStructure: 'ArrayStructure',
   ArrayStructureWithAllowSubtypes: 'ArrayStructureWithAllowSubtypes',
   BasicUnion: 'BasicUnion',
   ConcreteStructure: 'ConcreteStructure',
   EnumerationWithGaps: 'EnumerationWithGaps',
   NestedStructure: 'NestedStructure',
   NestedStructureWithAllowSubtypes: 'NestedStructureWithAllowSubtypes',
   ScalarStructure: 'ScalarStructure',
   ScalarStructureWithAllowSubtypes: 'ScalarStructureWithAllowSubtypes',
   StructureWithOptionalFields: 'StructureWithOptionalFields',
   TestModel_BinarySchema: 'TestModel',
   TestModel_XmlSchema: 'TestModel',
});

export const DataTypeIds = Object.freeze({
   AbstractStructure: 'nsu=' + NS + ';i=61',
   ConcreteStructure: 'nsu=' + NS + ';i=62',
   EnumerationWithGaps: 'nsu=' + NS + ';i=63',
   ScalarStructure: 'nsu=' + NS + ';i=65',
   ScalarStructureWithAllowSubtypes: 'nsu=' + NS + ';i=66',
   ArrayStructure: 'nsu=' + NS + ';i=67',
   ArrayStructureWithAllowSubtypes: 'nsu=' + NS + ';i=68',
   NestedStructure: 'nsu=' + NS + ';i=69',
   NestedStructureWithAllowSubtypes: 'nsu=' + NS + ';i=70',
   BasicUnion: 'nsu=' + NS + ';i=143',
   StructureWithOptionalFields: 'nsu=' + NS + ';i=144',
});

export const ObjectIds = Object.freeze({
   AbstractStructure_Encoding_DefaultBinary: 'nsu=' + NS + ';i=71',
   ConcreteStructure_Encoding_DefaultBinary: 'nsu=' + NS + ';i=72',
   ScalarStructure_Encoding_DefaultBinary: 'nsu=' + NS + ';i=73',
   ScalarStructureWithAllowSubtypes_Encoding_DefaultBinary: 'nsu=' + NS + ';i=74',
   ArrayStructure_Encoding_DefaultBinary: 'nsu=' + NS + ';i=75',
   ArrayStructureWithAllowSubtypes_Encoding_DefaultBinary: 'nsu=' + NS + ';i=76',
   NestedStructure_Encoding_DefaultBinary: 'nsu=' + NS + ';i=77',
   NestedStructureWithAllowSubtypes_Encoding_DefaultBinary: 'nsu=' + NS + ';i=78',
   BasicUnion_Encoding_DefaultBinary: 'nsu=' + NS + ';i=145',
   StructureWithOptionalFields_Encoding_DefaultBinary: 'nsu=' + NS + ';i=146',
   AbstractStructure_Encoding_DefaultXml: 'nsu=' + NS + ';i=103',
   ConcreteStructure_Encoding_DefaultXml: 'nsu=' + NS + ';i=104',
   ScalarStructure_Encoding_DefaultXml: 'nsu=' + NS + ';i=105',
   ScalarStructureWithAllowSubtypes_Encoding_DefaultXml: 'nsu=' + NS + ';i=106',
   ArrayStructure_Encoding_DefaultXml: 'nsu=' + NS + ';i=107',
   ArrayStructureWithAllowSubtypes_Encoding_DefaultXml: 'nsu=' + NS + ';i=108',
   NestedStructure_Encoding_DefaultXml: 'nsu=' + NS + ';i=109',
   NestedStructureWithAllowSubtypes_Encoding_DefaultXml: 'nsu=' + NS + ';i=110',
   BasicUnion_Encoding_DefaultXml: 'nsu=' + NS + ';i=153',
   StructureWithOptionalFields_Encoding_DefaultXml: 'nsu=' + NS + ';i=154',
   AbstractStructure_Encoding_DefaultJson: 'nsu=' + NS + ';i=135',
   ConcreteStructure_Encoding_DefaultJson: 'nsu=' + NS + ';i=136',
   ScalarStructure_Encoding_DefaultJson: 'nsu=' + NS + ';i=137',
   ScalarStructureWithAllowSubtypes_Encoding_DefaultJson: 'nsu=' + NS + ';i=138',
   ArrayStructure_Encoding_DefaultJson: 'nsu=' + NS + ';i=139',
   ArrayStructureWithAllowSubtypes_Encoding_DefaultJson: 'nsu=' + NS + ';i=140',
   NestedStructure_Encoding_DefaultJson: 'nsu=' + NS + ';i=141',
   NestedStructureWithAllowSubtypes_Encoding_DefaultJson: 'nsu=' + NS + ';i=142',
   BasicUnion_Encoding_DefaultJson: 'nsu=' + NS + ';i=161',
   StructureWithOptionalFields_Encoding_DefaultJson: 'nsu=' + NS + ';i=162',
});

export const VariableIds = Object.freeze({
   EnumerationWithGaps_EnumValues: 'nsu=' + NS + ';i=64',
   TestModel_BinarySchema: 'nsu=' + NS + ';i=13',
   TestModel_BinarySchema_NamespaceUri: 'nsu=' + NS + ';i=15',
   TestModel_BinarySchema_Deprecated: 'nsu=' + NS + ';i=16',
   TestModel_BinarySchema_AbstractStructure: 'nsu=' + NS + ';i=79',
   TestModel_BinarySchema_ConcreteStructure: 'nsu=' + NS + ';i=82',
   TestModel_BinarySchema_ScalarStructure: 'nsu=' + NS + ';i=85',
   TestModel_BinarySchema_ScalarStructureWithAllowSubtypes: 'nsu=' + NS + ';i=88',
   TestModel_BinarySchema_ArrayStructure: 'nsu=' + NS + ';i=91',
   TestModel_BinarySchema_ArrayStructureWithAllowSubtypes: 'nsu=' + NS + ';i=94',
   TestModel_BinarySchema_NestedStructure: 'nsu=' + NS + ';i=97',
   TestModel_BinarySchema_NestedStructureWithAllowSubtypes: 'nsu=' + NS + ';i=100',
   TestModel_BinarySchema_BasicUnion: 'nsu=' + NS + ';i=147',
   TestModel_BinarySchema_StructureWithOptionalFields: 'nsu=' + NS + ';i=150',
   TestModel_XmlSchema: 'nsu=' + NS + ';i=37',
   TestModel_XmlSchema_NamespaceUri: 'nsu=' + NS + ';i=39',
   TestModel_XmlSchema_Deprecated: 'nsu=' + NS + ';i=40',
   TestModel_XmlSchema_AbstractStructure: 'nsu=' + NS + ';i=111',
   TestModel_XmlSchema_ConcreteStructure: 'nsu=' + NS + ';i=114',
   TestModel_XmlSchema_ScalarStructure: 'nsu=' + NS + ';i=117',
   TestModel_XmlSchema_ScalarStructureWithAllowSubtypes: 'nsu=' + NS + ';i=120',
   TestModel_XmlSchema_ArrayStructure: 'nsu=' + NS + ';i=123',
   TestModel_XmlSchema_ArrayStructureWithAllowSubtypes: 'nsu=' + NS + ';i=126',
   TestModel_XmlSchema_NestedStructure: 'nsu=' + NS + ';i=129',
   TestModel_XmlSchema_NestedStructureWithAllowSubtypes: 'nsu=' + NS + ';i=132',
   TestModel_XmlSchema_BasicUnion: 'nsu=' + NS + ';i=155',
   TestModel_XmlSchema_StructureWithOptionalFields: 'nsu=' + NS + ';i=158',
});
