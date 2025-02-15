/* ========================================================================
 * Copyright (c) 2005-2024 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 * 
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/
using System.IO.Compression;
using System.Reflection;
using Opc.Ua;
using Opc.Ua.Export;
using Opc.Ua.Test;
using UaMqttPublisher;
using LocalizedText = Opc.Ua.LocalizedText;

internal partial class Publisher
{
    static readonly NamespaceTable NamespaceUris = new NamespaceTable([
        Opc.Ua.Namespaces.OpcUa,
        TestModel.Namespaces.TestModel,
        "http://cheese.com/cheddar",
        "http://acme.com/wilyecoyote",
        "urn:polarexpress.org:2024-12:santas:workshop",
        "urn:tatooine.gov:1977-12:these:are:not:the:droids"
    ]);

    static readonly StringTable ServerUris = new StringTable([
        "http://springfield.duff.com/line1/malting-vat2/inputcontroller",
        "http://springfield.duff.com/line2/malting-vat3/levelsensor",
        "urn:mordor.org:2000-01:human:witchking:angmar",
        "urn:rivendell.gov:1988-04:first:fellowship"
    ]);

    const uint Minimal_NetworkMessageContentMask = (uint)(
        JsonNetworkMessageContentMask.SingleDataSetMessage
    );

    const uint Minimal_DataSetMessageContentMask_OldEncoding = (uint)(
        JsonDataSetMessageContentMask.None
    );

    const uint Minimal_DataSetMessageContentMask_NewEncoding = (uint)(
        JsonDataSetMessageContentMask.FieldEncoding2
    );

    const uint Minimal_DataSetFieldContentMask = (uint)(
        DataSetFieldContentMask.RawData
    );

    const uint Single_NetworkMessageContentMask = (uint)(
        JsonNetworkMessageContentMask.DataSetMessageHeader |
        JsonNetworkMessageContentMask.SingleDataSetMessage
    );

    const uint Single_DataSetMessageContentMask_OldEncoding = (uint)(
        JsonDataSetMessageContentMask.DataSetWriterId |
        JsonDataSetMessageContentMask.SequenceNumber |
        JsonDataSetMessageContentMask.Timestamp |
        JsonDataSetMessageContentMask.Status |
        JsonDataSetMessageContentMask.MessageType |
        JsonDataSetMessageContentMask.PublisherId |
        JsonDataSetMessageContentMask.MinorVersion
    );

    const uint Single_DataSetMessageContentMask_NewEncoding = (uint)(
        Single_DataSetMessageContentMask_OldEncoding |
        (uint)JsonDataSetMessageContentMask.FieldEncoding2
    );

    const uint Single_DataSetFieldContentMask = (uint)(
        DataSetFieldContentMask.None
    );

    const uint Multiple_NetworkMessageContentMask = (uint)(
        JsonNetworkMessageContentMask.NetworkMessageHeader |
        JsonNetworkMessageContentMask.DataSetMessageHeader |
        JsonNetworkMessageContentMask.PublisherId
    );

    const uint Multiple_DataSetMessageContentMask_OldEncoding = (uint)(
        JsonDataSetMessageContentMask.DataSetWriterId |
        JsonDataSetMessageContentMask.SequenceNumber |
        JsonDataSetMessageContentMask.Timestamp |
        JsonDataSetMessageContentMask.Status |
        JsonDataSetMessageContentMask.MessageType |
        JsonDataSetMessageContentMask.MinorVersion
    );

    const uint Multiple_DataSetMessageContentMask_NewEncoding = (uint)(
        Multiple_DataSetMessageContentMask_OldEncoding |
        (uint)JsonDataSetMessageContentMask.FieldEncoding2
    );

    const uint Multiple_DataSetFieldContentMask = (uint)(
        DataSetFieldContentMask.StatusCode |
        DataSetFieldContentMask.ServerTimestamp
    );

    private PubSubConnectionDataType CreatePubSubConnection()
    {
        return new PubSubConnectionDataType()
        {
            PublisherId = PublisherId,
            Enabled = true,
            Name = "Default",
            TransportProfileUri = null,
            WriterGroups = new WriterGroupDataTypeCollection()
            {
                CreateGroup(
                    true,
                    "Minimal-New",
                    100,
                    "http://opcfoundation.org/UA/PubSub-Layouts/JSON-Minimal",
                    Minimal_NetworkMessageContentMask,
                    Minimal_DataSetFieldContentMask,
                    Minimal_DataSetMessageContentMask_NewEncoding
                ),
                CreateGroup(
                    true,
                    "Single-New",
                    200,
                    "http://opcfoundation.org/UA/PubSub-Layouts/JSON-DataSetMessage",
                    Single_NetworkMessageContentMask,
                    Single_DataSetFieldContentMask,
                    Single_DataSetMessageContentMask_NewEncoding
                ),
                CreateGroup(
                    true,
                    "Multiple-New",
                    300,
                    "http://opcfoundation.org/UA/PubSub-Layouts/JSON-NetworkMessage",
                    Multiple_NetworkMessageContentMask,
                    Multiple_DataSetFieldContentMask,
                    Multiple_DataSetMessageContentMask_NewEncoding
                ),
                //CreateGroup(
                //    true,
                //    "Minimal-Old",
                //    400,
                //    "http://opcfoundation.org/UA/PubSub-Layouts/JSON-Minimal",
                //    Minimal_NetworkMessageContentMask,
                //    Minimal_DataSetFieldContentMask,
                //    Minimal_DataSetMessageContentMask_OldEncoding
                //),
                //CreateGroup(
                //    true,
                //    "Single-Old",
                //    500,
                //    "http://opcfoundation.org/UA/PubSub-Layouts/JSON-DataSetMessage",
                //    Single_NetworkMessageContentMask,
                //    Single_DataSetFieldContentMask,
                //    Single_DataSetMessageContentMask_OldEncoding
                //),
                //CreateGroup(
                //    true,
                //    "Multiple-Old",
                //    600,
                //    "http://opcfoundation.org/UA/PubSub-Layouts/JSON-NetworkMessage",
                //    Multiple_NetworkMessageContentMask,
                //    Multiple_DataSetFieldContentMask,
                //    Multiple_DataSetMessageContentMask_OldEncoding
                //)
            }
        };
    }

    private WriterGroupDataType CreateGroup(
        bool enabled,
        string groupName,
        ushort groupId,
        string headerLayoutUri,
        uint networkMessageContentMask,
        uint fieldContentMask,
        uint dataSetMessageContentMask)
    {
        return new WriterGroupDataType()
        {
            Name = groupName,
            WriterGroupId = groupId,
            Enabled = enabled,
            HeaderLayoutUri = headerLayoutUri,
            MaxNetworkMessageSize = 1024 * 1024,
            PublishingInterval = 10000,
            KeepAliveTime = 60000,
            MessageSettings = new ExtensionObject(DataTypeIds.JsonWriterGroupMessageDataType, new JsonWriterGroupMessageDataType()
            {
                NetworkMessageContentMask = networkMessageContentMask
            }),
            TransportSettings = new ExtensionObject(DataTypeIds.BrokerWriterGroupTransportDataType, new BrokerWriterGroupTransportDataType()
            {
                QueueName = ((networkMessageContentMask & (uint)JsonNetworkMessageContentMask.SingleDataSetMessage) == 0) ? $"{TopicPrefix}/json/{MessageTypes.Data}/{PublisherId}/{groupName}" : null
            }),
            DataSetWriters = new DataSetWriterDataTypeCollection()
            {
                CreateWriter(groupName, "Scalars", (ushort)(groupId+1), ((networkMessageContentMask & (uint)JsonNetworkMessageContentMask.SingleDataSetMessage) == 0), fieldContentMask, dataSetMessageContentMask),
                CreateWriter(groupName, "Arrays", (ushort)(groupId+2), ((networkMessageContentMask & (uint)JsonNetworkMessageContentMask.SingleDataSetMessage) == 0), fieldContentMask,  dataSetMessageContentMask),
                CreateWriter(groupName, "Structures", (ushort)(groupId+3), ((networkMessageContentMask & (uint)JsonNetworkMessageContentMask.SingleDataSetMessage) == 0), fieldContentMask, dataSetMessageContentMask),
                // CreateWriter(groupName, "StructuresWithAllowSubtypes", (ushort)(groupId+4), ((networkMessageContentMask & (uint)JsonNetworkMessageContentMask.SingleDataSetMessage) == 0), fieldContentMask, dataSetMessageContentMask),
                CreateWriter(groupName, "Unions",(ushort)(groupId+5), ((networkMessageContentMask & (uint)JsonNetworkMessageContentMask.SingleDataSetMessage) == 0), fieldContentMask, dataSetMessageContentMask),
                CreateWriter(groupName, "OptionalFields", (ushort)(groupId+6), ((networkMessageContentMask & (uint)JsonNetworkMessageContentMask.SingleDataSetMessage) == 0), fieldContentMask, dataSetMessageContentMask),
                CreateWriter(groupName, "Matrix", (ushort)(groupId+7), ((networkMessageContentMask & (uint)JsonNetworkMessageContentMask.SingleDataSetMessage) == 0), fieldContentMask, dataSetMessageContentMask)
            }
        };
    }

    private DataSetWriterDataType CreateWriter(
        string groupName,
        string writerName,
        ushort dataSetWriterId,
        bool useGroupTopic,
        uint fieldContentMask,
        uint dataSetMessageContentMask
    )
    {
        return new DataSetWriterDataType()
        {
            Name = writerName,
            DataSetWriterId = dataSetWriterId,
            Enabled = true,
            KeyFrameCount = 0,
            DataSetName = writerName,
            DataSetFieldContentMask = fieldContentMask,
            MessageSettings = new ExtensionObject(DataTypeIds.JsonDataSetWriterMessageDataType, new JsonDataSetWriterMessageDataType()
            {
                DataSetMessageContentMask = dataSetMessageContentMask
            }),
            TransportSettings = new ExtensionObject(DataTypeIds.BrokerDataSetWriterTransportDataType, new BrokerDataSetWriterTransportDataType()
            {
                QueueName = (useGroupTopic) ? null : $"{TopicPrefix}/json/{MessageTypes.Data}/{PublisherId}/{groupName}/{writerName}",
                MetaDataQueueName = $"{TopicPrefix}/json/{MessageTypes.DataSetMetaData}/{PublisherId}/{groupName}/{writerName}",
                RequestedDeliveryGuarantee = BrokerTransportQualityOfService.AtLeastOnce,
                MetaDataUpdateTime = 60000
            }),
        };
    }

    static readonly Dictionary<string, DataSetMetaDataType> DataSets = new()
    {
        ["Scalars"] = new DataSetMetaDataType()
        {
            Name = "Scalars",
            Description = new LocalizedText("A set of all simple built in types."),
            ConfigurationVersion = { MajorVersion = GetVersionTime(), MinorVersion = GetVersionTime() },
            Fields = new FieldMetaDataCollection()
            {
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.Boolean,
                    DataType = DataTypeIds.Boolean,
                    Name = "Boolean",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.SByte,
                    DataType = DataTypeIds.SByte,
                    Name = "SByte",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.Byte,
                    DataType = DataTypeIds.Byte,
                    Name = "Byte",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.Int16,
                    DataType = DataTypeIds.Int16,
                    Name = "Int16",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.UInt16,
                    DataType = DataTypeIds.UInt16,
                    Name = "UInt16",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.Int32,
                    DataType = DataTypeIds.Int32,
                    Name = "Int32",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.UInt32,
                    DataType = DataTypeIds.UInt32,
                    Name = "UInt32",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.Int64,
                    DataType = DataTypeIds.Int64,
                    Name = "Int64",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.UInt64,
                    DataType = DataTypeIds.UInt64,
                    Name = "UInt64",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.Float,
                    DataType = DataTypeIds.Float,
                    Name = "Float",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.Double,
                    DataType = DataTypeIds.Double,
                    Name = "Double",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.String,
                    DataType = DataTypeIds.String,
                    Name = "String",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.ByteString,
                    DataType = DataTypeIds.ByteString,
                    Name = "ByteString",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.DateTime,
                    DataType = DataTypeIds.DateTime,
                    Name = "DateTime",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.Guid,
                    DataType = DataTypeIds.Guid,
                    Name = "Guid",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.NodeId,
                    DataType = DataTypeIds.NodeId,
                    Name = "NodeId",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.ExpandedNodeId,
                    DataType = DataTypeIds.ExpandedNodeId,
                    Name = "ExpandedNodeId",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.QualifiedName,
                    DataType = DataTypeIds.QualifiedName,
                    Name = "QualifiedName",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.LocalizedText,
                    DataType = DataTypeIds.LocalizedText,
                    Name = "LocalizedText",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.StatusCode,
                    DataType = DataTypeIds.StatusCode,
                    Name = "StatusCode",
                    ValueRank = ValueRanks.Scalar
                }
            }
        },
        ["Arrays"] = new DataSetMetaDataType()
        {
            Name = "Arrays",
            Description = new LocalizedText("A set of arrays of all simple built in types."),
            ConfigurationVersion = { MajorVersion = GetVersionTime(), MinorVersion = GetVersionTime() },
            Fields = new FieldMetaDataCollection()
            {
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.Boolean,
                    DataType = DataTypeIds.Boolean,
                    Name = "Boolean",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.SByte,
                    DataType = DataTypeIds.SByte,
                    Name = "SByte",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.Byte,
                    DataType = DataTypeIds.Byte,
                    Name = "Byte",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.Int16,
                    DataType = DataTypeIds.Int16,
                    Name = "Int16",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.UInt16,
                    DataType = DataTypeIds.UInt16,
                    Name = "UInt16",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.Int32,
                    DataType = DataTypeIds.Int32,
                    Name = "Int32",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.UInt32,
                    DataType = DataTypeIds.UInt32,
                    Name = "UInt32",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.Int64,
                    DataType = DataTypeIds.Int64,
                    Name = "Int64",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.UInt64,
                    DataType = DataTypeIds.UInt64,
                    Name = "UInt64",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.Float,
                    DataType = DataTypeIds.Float,
                    Name = "Float",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.Double,
                    DataType = DataTypeIds.Double,
                    Name = "Double",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.String,
                    DataType = DataTypeIds.String,
                    Name = "String",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.ByteString,
                    DataType = DataTypeIds.ByteString,
                    Name = "ByteString",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.DateTime,
                    DataType = DataTypeIds.DateTime,
                    Name = "DateTime",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.Guid,
                    DataType = DataTypeIds.Guid,
                    Name = "Guid",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.NodeId,
                    DataType = DataTypeIds.NodeId,
                    Name = "NodeId",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.ExpandedNodeId,
                    DataType = DataTypeIds.ExpandedNodeId,
                    Name = "ExpandedNodeId",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.QualifiedName,
                    DataType = DataTypeIds.QualifiedName,
                    Name = "QualifiedName",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.LocalizedText,
                    DataType = DataTypeIds.LocalizedText,
                    Name = "LocalizedText",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.StatusCode,
                    DataType = DataTypeIds.StatusCode,
                    Name = "StatusCode",
                    ValueRank = ValueRanks.OneDimension
                }
            }
        },
        ["Structures"] = new DataSetMetaDataType()
        {
            Namespaces = new StringCollection([
                TestModel.Namespaces.TestModel
            ]),
            Name = "Structures",
            Description = new LocalizedText("A set of structure types."),
            ConfigurationVersion = { MajorVersion = GetVersionTime(), MinorVersion = GetVersionTime() },
            Fields = new FieldMetaDataCollection()
            {
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.ExtensionObject,
                    DataType = new NodeId(TestModel.DataTypes.ConcreteStructure, 1),
                    Name = "TestConcreteStructure",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.ExtensionObject,
                    DataType = new NodeId(TestModel.DataTypes.ScalarStructure, 1),
                    Name = "TestScalarStructure",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.ExtensionObject,
                    DataType = new NodeId(TestModel.DataTypes.ArrayStructure, 1),
                    Name = "TestArrayStructure",
                    ValueRank = ValueRanks.Scalar
                }
            }
        },
        ["StructuresWithAllowSubtypes"] = new DataSetMetaDataType()
        {
            Namespaces = new StringCollection([
                TestModel.Namespaces.TestModel
            ]),
            Name = "Structures",
            Description = new LocalizedText("A set of structure types."),
            ConfigurationVersion = { MajorVersion = GetVersionTime(), MinorVersion = GetVersionTime() },
            Fields = new FieldMetaDataCollection()
            {
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.ExtensionObject,
                    DataType = new NodeId(TestModel.DataTypes.ConcreteStructure, 1),
                    Name = "ConcreteStructure",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.ExtensionObject,
                    DataType = new NodeId(TestModel.DataTypes.ScalarStructureWithAllowSubtypes, 1),
                    Name = "ScalarStructure",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.ExtensionObject,
                    DataType = new NodeId(TestModel.DataTypes.ArrayStructureWithAllowSubtypes, 1),
                    Name = "ArrayStructure",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.ExtensionObject,
                    DataType = new NodeId(TestModel.DataTypes.ScalarStructureWithAllowSubtypes, 1),
                    Name = "ScalarStructureArray",
                    ValueRank = ValueRanks.OneDimension
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.ExtensionObject,
                    DataType = new NodeId(TestModel.DataTypes.ArrayStructureWithAllowSubtypes, 1),
                    Name = "ArrayStructureArray",
                    ValueRank = ValueRanks.OneDimension
                }
            }
        },
        ["Unions"] = new DataSetMetaDataType()
        {
            Namespaces = new StringCollection([
                TestModel.Namespaces.TestModel
            ]),
            Name = "Unions",
            Description = new LocalizedText("A set of union types."),
            ConfigurationVersion = { MajorVersion = GetVersionTime(), MinorVersion = GetVersionTime() },
            Fields = new FieldMetaDataCollection()
            {
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.ExtensionObject,
                    DataType = new NodeId(TestModel.DataTypes.BasicUnion, 1),
                    Name = "Scalar",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.ExtensionObject,
                    DataType = new NodeId(TestModel.DataTypes.BasicUnion, 1),
                    Name = "Array",
                    ValueRank = ValueRanks.OneDimension
                }
            }
        },
        ["OptionalFields"] = new DataSetMetaDataType()
        {
            Namespaces = new StringCollection([
                TestModel.Namespaces.TestModel
            ]),
            Name = "OptionalFields",
            Description = new LocalizedText("A set of structures with optional fields."),
            ConfigurationVersion = { MajorVersion = GetVersionTime(), MinorVersion = GetVersionTime() },
            Fields = new FieldMetaDataCollection()
            {
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.ExtensionObject,
                    DataType = new NodeId(TestModel.DataTypes.StructureWithOptionalFields, 1),
                    Name = "Scalar",
                    ValueRank = ValueRanks.Scalar
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.ExtensionObject,
                    DataType = new NodeId(TestModel.DataTypes.StructureWithOptionalFields, 1),
                    Name = "Array",
                    ValueRank = ValueRanks.OneDimension
                }
            }
        },
        ["Matrix"] = new DataSetMetaDataType()
        {
            Namespaces = new StringCollection([
                TestModel.Namespaces.TestModel
            ]),
            Name = "Matrix",
            Description = new LocalizedText("A set of matrix values."),
            ConfigurationVersion = { MajorVersion = GetVersionTime(), MinorVersion = GetVersionTime() },
            Fields = new FieldMetaDataCollection()
            {
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.Double,
                    DataType = Opc.Ua.DataTypeIds.Double,
                    Name = "2D",
                    ValueRank = 2
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.String,
                    DataType = Opc.Ua.DataTypeIds.String,
                    Name = "3D",
                    ValueRank = 3
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.ExtensionObject,
                    DataType = new NodeId(TestModel.DataTypes.ConcreteStructure, 1),
                    Name = "2D-Structures",
                    ValueRank = 2
                },
                new FieldMetaData()
                {
                    BuiltInType = (byte)BuiltInType.Variant,
                    DataType = Opc.Ua.DataTypeIds.BaseDataType,
                    Name = "2D-Variants",
                    ValueRank = 2
                }
            }
        }
    };
}
