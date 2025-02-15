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
using System.Runtime.CompilerServices;
using System.Xml;
using Opc.Ua.Test;
using Opc.Ua;
using TestModel;

namespace UaMqttPublisher
{
    /// <summary>
    /// A class to hold the energy metrics data publlished to the broker.
    /// </summary>
    internal static class Extensions
    {
        public static object GetStaticValue(
            this DataGenerator generator, 
            BuiltInType builtInType,
            NodeId dataTypeId, 
            NamespaceTable namespaceUris)
        {
            object value = null;

            if (builtInType == BuiltInType.ExtensionObject)
            {
                if (dataTypeId == ExpandedNodeId.ToNodeId(TestModel.DataTypeIds.ScalarStructure, namespaceUris))
                {
                    value = generator.GetStaticScalarStructure();
                }
                else if(dataTypeId == ExpandedNodeId.ToNodeId(TestModel.DataTypeIds.ScalarStructureWithAllowSubtypes, namespaceUris))
                {
                    value = generator.GetStaticScalarStructureWithAllowSubtypes();
                }
                else if (dataTypeId == ExpandedNodeId.ToNodeId(TestModel.DataTypeIds.ArrayStructure, namespaceUris))
                {
                    value = generator.GetStaticArrayStructure();
                }
                else if(dataTypeId == ExpandedNodeId.ToNodeId(TestModel.DataTypeIds.ArrayStructureWithAllowSubtypes, namespaceUris))
                {
                    value = generator.GetStaticArrayStructureWithAllowSubtypes();
                }
                else if (dataTypeId == ExpandedNodeId.ToNodeId(TestModel.DataTypeIds.EnumerationWithGaps, namespaceUris))
                {
                    value = TestModel.EnumerationWithGaps.Green;
                }
                else if (dataTypeId == ExpandedNodeId.ToNodeId(TestModel.DataTypeIds.ConcreteStructure, namespaceUris))
                {
                    value = generator.GetStaticConcreteStructure();
                }
                else if (dataTypeId == ExpandedNodeId.ToNodeId(TestModel.DataTypeIds.NestedStructure, namespaceUris))
                {
                    value = generator.GetStaticNestedStructure();
                }
                else if (dataTypeId == ExpandedNodeId.ToNodeId(TestModel.DataTypeIds.BasicUnion, namespaceUris))
                {
                    value = generator.GetStaticBasicUnion();
                }
                else if (dataTypeId == ExpandedNodeId.ToNodeId(TestModel.DataTypeIds.StructureWithOptionalFields, namespaceUris))
                {
                    value = generator.GetStaticStructureWithOptionalFields();
                }
                else
                {
                    value = new ThreeDFrame()
                    {
                        CartesianCoordinates = new ThreeDCartesianCoordinates()
                        {
                             X = 1000,
                             Y = 2000,
                             Z = 8000
                        },
                        Orientation = new ThreeDOrientation()
                        {
                            A = 25,
                            B = 10,
                            C = 90
                        }
                    };
                }

                return value;
            }
            
            if (builtInType == BuiltInType.Variant)
            {
                if (generator.GetRandomBoolean())
                {
                    return new Variant(new ThreeDCartesianCoordinates()
                    {
                        X = 10,
                        Y = 20,
                        Z = 40
                    });
                }
                else
                {
                    return new Variant(1001);
                }
            }

            return generator.GetStaticScalarValue(builtInType);
        }

        public static object GetStaticArray(
            this DataGenerator generator,
            BuiltInType builtInType, 
            NodeId dataTypeId, 
            NamespaceTable namespaceUris,
            params int[] dimensions)
        {
            if (dimensions == null || dimensions.Length == 0)
            {
                return null;
            }


            var length = 1;
            for (int ii = 0; ii < dimensions.Length; ii++) length *= dimensions[ii];

            var type = TypeInfo.GetSystemType(builtInType, ValueRanks.Scalar);

            if (builtInType == BuiltInType.Variant)
            {
                type = typeof(Variant);
            }
            else if (builtInType == BuiltInType.ExtensionObject)
            {
                var value = GetStaticValue(generator, builtInType, dataTypeId, namespaceUris);
                type = value.GetType();
            }

            var array = Array.CreateInstance(type, dimensions);

            for (int ii = 0; ii < length; ii++)
            {
                var point = new int[dimensions.Length];

                for (int jj = dimensions.Length - 1; jj >= 0; jj--)
                {
                    int devisor = 1;

                    for (int kk = jj+1; kk < dimensions.Length; kk++)
                    {
                        devisor *= dimensions[kk];
                    }

                    point[jj] = ii/devisor % dimensions[jj];
                }

                array.SetValue(GetStaticValue(generator, builtInType, dataTypeId, namespaceUris), point);
            }

            return array;
        }

        public static object GetStaticScalarValue(this DataGenerator generator, BuiltInType builtInType)
        {
            switch (builtInType)
            {
                case BuiltInType.Boolean: return true;
                case BuiltInType.SByte: return (sbyte)-100;
                case BuiltInType.Byte: return (byte)200;
                case BuiltInType.Int16: return (short)-11211;
                case BuiltInType.UInt16: return (ushort)50000;
                case BuiltInType.Int32: return (int)-100000;
                case BuiltInType.UInt32: return (uint)3000000000;
                case BuiltInType.Int64: return (long)-3000000000;
                case BuiltInType.UInt64: return (ulong)10000000000000000000;
                case BuiltInType.Float: return (float)1.192093e-07f;
                case BuiltInType.Double: return (double)4.94065645841247E-320;
                case BuiltInType.Guid: return new Uuid("f4bebe0e-d244-4afa-9791-e648b592e5c6");
                case BuiltInType.DateTime: return new DateTime(2024, 07, 15, 11, 45, 01);
                case BuiltInType.String: return "Apple";
                case BuiltInType.ByteString: return new byte[] { 0x79, 0x80, 0xc7, 0x2a, 0xf6, 0x47, 0x41, 0xd4, 0xa3, 0x6c, 0x88, 0xfb, 0x8e, 0x33, 0x2a, 0xa7 };
                case BuiltInType.NodeId: return new NodeId("Grape", 3);
                case BuiltInType.ExpandedNodeId: return new ExpandedNodeId(123456, "http://acme.com/wilyecoyote", 3);
                case BuiltInType.QualifiedName: return new QualifiedName("Cherry", 2);
                case BuiltInType.LocalizedText: return new LocalizedText("en-CA", "Orange");
                case BuiltInType.StatusCode: return new StatusCode(StatusCodes.BadConditionNotShelved);
                case BuiltInType.XmlElement: return new XmlDocument() { InnerXml = "<Red>Bicycle</Red>" }.DocumentElement;
                case BuiltInType.ExtensionObject: return new ExtensionObject(TestModel.DataTypeIds.ConcreteStructure, generator.GetStaticConcreteStructure());
                case BuiltInType.Enumeration: return EnumerationWithGaps.Blue;
                case BuiltInType.Variant: return new Variant("Banana");
            }

            return null;
        }

        public static ScalarStructure GetRandomScalarStructure(this DataGenerator generator)
        {
            var instance = new ScalarStructure();
            instance.A = generator.GetRandomBoolean();
            instance.B = generator.GetRandomSByte();
            instance.C = generator.GetRandomByte();
            instance.D = generator.GetRandomInt16();
            instance.E = generator.GetRandomUInt16();
            instance.F = generator.GetRandomInt32();
            instance.G = generator.GetRandomUInt32();
            instance.H = generator.GetRandomInt64();
            instance.I = generator.GetRandomUInt64();
            instance.J = generator.GetRandomFloat();
            instance.K = generator.GetRandomDouble();
            instance.L = generator.GetRandomUuid();
            instance.M = generator.GetRandomDateTime();
            instance.N = generator.GetRandomString();
            instance.O = generator.GetRandomByteString();
            instance.P = generator.GetRandomNodeId();
            instance.Q = generator.GetRandomExpandedNodeId();
            instance.R = generator.GetRandomQualifiedName();
            instance.S = generator.GetRandomLocalizedText();
            instance.T = generator.GetRandomStatusCode();
            instance.U = generator.GetRandomXmlElement();
            instance.V = generator.GetRandomConcreteStructure();
            instance.W = generator.GetRandomEnumerationWithGaps();
            return instance;
        }

        public static ScalarStructure GetStaticScalarStructure(this DataGenerator generator)
        {
            var instance = new ScalarStructure();
            instance.A = (bool)generator.GetStaticScalarValue(BuiltInType.Boolean);
            instance.B = -100;
            instance.C = 200;
            instance.D = -11211;
            instance.E = 50000;
            instance.F = -100000;
            instance.G = 3000000000;
            instance.H = -3000000000;
            instance.I = 10000000000000000000;
            instance.J = 1.192093e-07f;
            instance.K = 4.94065645841247E-324;
            instance.L = new Uuid("f4bebe0e-d244-4afa-9791-e648b592e5c6");
            instance.M = new DateTime(2024, 07, 15, 11, 45, 01);
            instance.N = "Apple";
            instance.O = [0x79, 0x80, 0xc7, 0x2a, 0xf6, 0x47, 0x41, 0xd4, 0xa3, 0x6c, 0x88, 0xfb, 0x8e, 0x33, 0x2a, 0xa7];
            instance.P = new NodeId("Grape", 3);
            instance.Q = new ExpandedNodeId(123456, "http://acme.com/wilyecoyote", 3);
            instance.R = new QualifiedName("Cherry", 2);
            instance.S = new LocalizedText("en-CA", "Banana");
            instance.T = StatusCodes.BadConditionNotShelved;
            instance.U = new XmlDocument() { InnerXml = "<Red>Bicycle</Red>" }.DocumentElement;
            instance.V = generator.GetStaticConcreteStructure();
            instance.W = TestModel.EnumerationWithGaps.Green;
            return instance;
        }

        public static ScalarStructureWithAllowSubtypes GetRandomScalarStructureWithAllowSubtypes(this DataGenerator generator)
        {
            var instance = new ScalarStructureWithAllowSubtypes();
            instance.A = generator.GetRandomBoolean();
            instance.B = generator.GetRandomSByte();
            instance.C = generator.GetRandomByte();
            instance.D = generator.GetRandomInt16();
            instance.E = generator.GetRandomUInt16();
            instance.F = generator.GetRandomInt32();
            instance.G = generator.GetRandomUInt32();
            instance.H = generator.GetRandomInt64();
            instance.I = generator.GetRandomUInt64();
            instance.J = generator.GetRandomFloat();
            instance.K = generator.GetRandomDouble();
            instance.L = generator.GetRandomUuid();
            instance.M = generator.GetRandomDateTime();
            instance.N = generator.GetRandomString();
            instance.O = generator.GetRandomByteString();
            instance.P = generator.GetRandomNodeId();
            instance.Q = generator.GetRandomExpandedNodeId();
            instance.R = generator.GetRandomQualifiedName();
            instance.S = generator.GetRandomLocalizedText();
            instance.T = generator.GetRandomStatusCode();
            instance.U = generator.GetRandomXmlElement();
            instance.V = generator.GetRandomConcreteStructure();
            instance.W = generator.GetRandomEnumerationWithGaps();
            instance.X = generator.GetRandomDataValue();
            instance.Y = generator.GetRandomVariant();
            instance.Z = generator.GetRandomExtensionObject();
            return instance;
        }

        public static ScalarStructureWithAllowSubtypes GetStaticScalarStructureWithAllowSubtypes(this DataGenerator generator)
        {
            var instance = new ScalarStructureWithAllowSubtypes();
            instance.A = true;
            instance.B = -100;
            instance.C = 200;
            instance.D = -11211;
            instance.E = 50000;
            instance.F = -100000;
            instance.G = 3000000000;
            instance.H = -3000000000;
            instance.I = 10000000000000000000;
            instance.J = 1.192093e-07f;
            instance.K = 4.94065645841247E-324;
            instance.L = new Uuid("f4bebe0e-d244-4afa-9791-e648b592e5c6");
            instance.M = new DateTime(2024, 07, 15, 11, 45, 01);
            instance.N = "Apple";
            instance.O = [0x79, 0x80, 0xc7, 0x2a, 0xf6, 0x47, 0x41, 0xd4, 0xa3, 0x6c, 0x88, 0xfb, 0x8e, 0x33, 0x2a, 0xa7];
            instance.P = new NodeId("Grape", 3);
            instance.Q = new ExpandedNodeId(123456, "http://acme.com/wilyecoyote", 3);
            instance.R = new QualifiedName("Cherry", 2);
            instance.S = new LocalizedText("en-CA", "Orange");
            instance.T = StatusCodes.BadConditionNotShelved;
            instance.U = new XmlDocument() { InnerXml = "<Red>Bicycle</Red>" }.DocumentElement;
            instance.V = generator.GetStaticConcreteStructure();
            instance.W = TestModel.EnumerationWithGaps.Green;
            instance.X = new DataValue() { WrappedValue = new Variant(4804837202), StatusCode = StatusCodes.UncertainLastUsableValue, SourceTimestamp = new DateTime(2024, 03, 24, 16, 05, 23), SourcePicoseconds = 102 };
            instance.Y = new Variant("Watermelon");
            instance.Z = new ExtensionObject(Opc.Ua.DataTypeIds.ThreeDVector, new ThreeDVector() { X = 1, Y = 2, Z = 3 });
            return instance;
        }

        public static ArrayStructure GetRandomArrayStructure(this DataGenerator generator)
        {
            var instance = new ArrayStructure();
            instance.A = generator.GetRandomArray<bool>(true, 3, true);
            instance.B = generator.GetRandomArray<sbyte>(true, 3, true);
            instance.C = generator.GetRandomArray<byte>(true, 3, true);
            instance.D = generator.GetRandomArray<short>(true, 3, true);
            instance.E = generator.GetRandomArray<ushort>(true, 3, true);
            instance.F = generator.GetRandomArray<int>(true, 3, true);
            instance.G = generator.GetRandomArray<uint>(true, 3, true);
            instance.H = generator.GetRandomArray<long>(true, 3, true);
            instance.I = generator.GetRandomArray<ulong>(true, 3, true);
            instance.J = generator.GetRandomArray<float>(true, 3, true);
            instance.K = generator.GetRandomArray<double>(true, 3, true);
            instance.L = generator.GetRandomArray<Uuid>(true, 3, true);
            instance.M = generator.GetRandomArray<DateTime>(true, 3, true);
            instance.N = generator.GetRandomArray<string>(true, 3, true);
            instance.O = generator.GetRandomArray<byte[]>(true, 3, true);
            instance.P = generator.GetRandomArray<NodeId>(true, 3, true);
            instance.Q = generator.GetRandomArray<ExpandedNodeId>(true, 3, true);
            instance.R = generator.GetRandomArray<QualifiedName>(true, 3, true);
            instance.S = generator.GetRandomArray<LocalizedText>(true, 3, true);
            instance.T = generator.GetRandomArray<StatusCode>(true, 3, true);
            instance.U = generator.GetRandomArray<XmlElement>(true, 3, true);
            instance.V = generator.GetRandomStructureArray(generator.GetRandomConcreteStructure);
            instance.W = generator.GetRandomStructureArray(generator.GetRandomEnumerationWithGaps);
            return instance;
        }

        public static ArrayStructure GetStaticArrayStructure(this DataGenerator generator)
        {
            var instance = new ArrayStructure();
            instance.A = [true, false, true];
            instance.B = [50, -100, 45];
            instance.C = [12, 200, 68];
            instance.D = [80, -11211, 3456];
            instance.E = [7892, 50000, 61273];
            instance.F = [19374, -100000, 137891];
            instance.G = [5371943, 3000000000, 370190];
            instance.H = [864602, -3000000000, -9378912];
            instance.I = [3168909124, 10000000000000000000, 73812794];
            instance.J = [3.1415f, 1.192093e-07f, 8.739437f];
            instance.K = [3.1415926535897931, 4.94065645841247E-324, 1.7976931348623157E+308];
            instance.L = [new Uuid("2749B384-BD3B-4E86-A744-5FFE5AE38B1D"), new Uuid("f4bebe0e-d244-4afa-9791-e648b592e5c6"), new Uuid("823BE286-9660-4BB4-B3DE-BD5126797B6E")];
            instance.M = [new DateTime(2000, 10, 31, 13, 59, 23), new DateTime(2024, 07, 15, 11, 45, 01), new DateTime(1989, 06, 06, 03, 21, 15)];
            instance.N = ["Apple", "Orange", "Banana"];
            instance.O = [[0x41, 0xd4, 0xa3, 0x6c, 0x88, 0xfb, 0x8e, 0x33, 0x2a, 0xa7], [0x79, 0x80, 0xc7, 0x2a, 0xf6, 0x47, 0x41, 0xd4, 0xa3, 0x6c, 0x88, 0xfb, 0x8e, 0x33, 0x2a, 0xa7], [0x2a, 0xa7, 0x79, 0x80, 0xc7, 0x2a, 0xf6, 0x47, 0x41, 0x2a, 0xa7]];
            instance.P = [new NodeId(2256), new NodeId("Grape", 3), new NodeId("Cherry", 1)];
            instance.Q = [new ExpandedNodeId(new Guid("44122EF0-96F9-44AD-BFC5-075EF5DCF968"), 1, null, 3), new ExpandedNodeId(123456, "http://acme.com/wilyecoyote", 3), new ExpandedNodeId("BeepBeep", 0, "http://acme.com/roadrunner", 2)];
            instance.R = [new QualifiedName(Opc.Ua.BrowseNames.Vector), new QualifiedName("Lemon", 2), new QualifiedName("Lime", 1)];
            instance.S = [new LocalizedText("ja-JP", "アップル"), new LocalizedText("en-CA", "Apple"), new LocalizedText("de-DE", "Apfel")];
            instance.T = [StatusCodes.Good, StatusCodes.UncertainLastUsableValue, StatusCodes.BadConditionNotShelved];
            instance.U = [new XmlDocument() { InnerXml = "<Blue Yellow='true'>Car</Blue>" }.DocumentElement, new XmlDocument() { InnerXml = "<Green Order='Banana' />" }.DocumentElement, new XmlDocument() { InnerXml = "<Red>Bicycle</Red>" }.DocumentElement];
            instance.V = [generator.GetStaticConcreteStructure(), generator.GetStaticConcreteStructure(), generator.GetStaticConcreteStructure()];
            instance.W = [TestModel.EnumerationWithGaps.Green, TestModel.EnumerationWithGaps.Red, TestModel.EnumerationWithGaps.Invalid];
            return instance;
        }

        public static ArrayStructure GetRandomArrayStructureWithAllowSubtypes(this DataGenerator generator)
        {
            var instance = new ArrayStructureWithAllowSubtypes();
            instance.A = generator.GetRandomArray<bool>(true, 3, true);
            instance.B = generator.GetRandomArray<sbyte>(true, 3, true);
            instance.C = generator.GetRandomArray<byte>(true, 3, true);
            instance.D = generator.GetRandomArray<short>(true, 3, true);
            instance.E = generator.GetRandomArray<ushort>(true, 3, true);
            instance.F = generator.GetRandomArray<int>(true, 3, true);
            instance.G = generator.GetRandomArray<uint>(true, 3, true);
            instance.H = generator.GetRandomArray<long>(true, 3, true);
            instance.I = generator.GetRandomArray<ulong>(true, 3, true);
            instance.J = generator.GetRandomArray<float>(true, 3, true);
            instance.K = generator.GetRandomArray<double>(true, 3, true);
            instance.L = generator.GetRandomArray<Uuid>(true, 3, true);
            instance.M = generator.GetRandomArray<DateTime>(true, 3, true);
            instance.N = generator.GetRandomArray<string>(true, 3, true);
            instance.O = generator.GetRandomArray<byte[]>(true, 3, true);
            instance.P = generator.GetRandomArray<NodeId>(true, 3, true);
            instance.Q = generator.GetRandomArray<ExpandedNodeId>(true, 3, true);
            instance.R = generator.GetRandomArray<QualifiedName>(true, 3, true);
            instance.S = generator.GetRandomArray<LocalizedText>(true, 3, true);
            instance.T = generator.GetRandomArray<StatusCode>(true, 3, true);
            instance.U = generator.GetRandomArray<XmlElement>(true, 3, true);
            instance.V = generator.GetRandomStructureArray(generator.GetRandomConcreteStructure);
            instance.W = generator.GetRandomStructureArray(generator.GetRandomEnumerationWithGaps);
            instance.X = generator.GetRandomArray<DataValue>(true, 3, true);
            instance.Y = generator.GetRandomArray<Variant>(true, 3, true);
            instance.Z = generator.GetRandomArray<ExtensionObject>(true, 3, true);
            return instance;
        }

        public static ArrayStructureWithAllowSubtypes GetStaticArrayStructureWithAllowSubtypes(this DataGenerator generator)
        {
            var instance = new ArrayStructureWithAllowSubtypes();
            instance.A = [true, false, true];
            instance.B = [50, -100, 45];
            instance.C = [12, 200, 68];
            instance.D = [80, -11211, 3456];
            instance.E = [7892, 50000, 61273];
            instance.F = [19374, -100000, 137891];
            instance.G = [5371943, 3000000000, 370190];
            instance.H = [864602, -3000000000, -9378912];
            instance.I = [3168909124, 10000000000000000000, 73812794];
            instance.J = [3.1415f, 1.192093e-07f, 8.739437f];
            instance.K = [3.1415926535897931, 4.94065645841247E-324, 1.7976931348623157E+308];
            instance.L = [new Uuid("2749B384-BD3B-4E86-A744-5FFE5AE38B1D"), new Uuid("f4bebe0e-d244-4afa-9791-e648b592e5c6"), new Uuid("823BE286-9660-4BB4-B3DE-BD5126797B6E")];
            instance.M = [new DateTime(2000, 10, 31, 13, 59, 23), new DateTime(2024, 07, 15, 11, 45, 01), new DateTime(1989, 06, 06, 03, 21, 15)];
            instance.N = ["Apple", "Orange", "Banana"];
            instance.O = [[0x41, 0xd4, 0xa3, 0x6c, 0x88, 0xfb, 0x8e, 0x33, 0x2a, 0xa7], [0x79, 0x80, 0xc7, 0x2a, 0xf6, 0x47, 0x41, 0xd4, 0xa3, 0x6c, 0x88, 0xfb, 0x8e, 0x33, 0x2a, 0xa7], [0x2a, 0xa7, 0x79, 0x80, 0xc7, 0x2a, 0xf6, 0x47, 0x41, 0x2a, 0xa7]];
            instance.P = [new NodeId(2256), new NodeId("Grape", 3), new NodeId("Cherry", 1)];
            instance.Q = [new ExpandedNodeId(new Guid("44122EF0-96F9-44AD-BFC5-075EF5DCF968"), 1, null, 3), new ExpandedNodeId(123456, "http://acme.com/wilyecoyote", 3), new ExpandedNodeId("BeepBeep", 0, "http://acme.com/roadrunner", 2)];
            instance.R = [new QualifiedName(Opc.Ua.BrowseNames.Vector), new QualifiedName("Lemon", 2), new QualifiedName("Lime", 1)];
            instance.S = [new LocalizedText("ja-JP", "アップル"), new LocalizedText("en-CA", "Apple"), new LocalizedText("de-DE", "Apfel")];
            instance.T = [StatusCodes.Good, StatusCodes.UncertainLastUsableValue, StatusCodes.BadConditionNotShelved];
            instance.U = [new XmlDocument() { InnerXml = "<Blue Yellow='true'>Car</Blue>" }.DocumentElement, new XmlDocument() { InnerXml = "<Green Order='Banana' />" }.DocumentElement, new XmlDocument() { InnerXml = "<Red>Bicycle</Red>" }.DocumentElement];
            instance.V = [generator.GetStaticConcreteStructure(), generator.GetStaticConcreteStructure(), generator.GetStaticConcreteStructure()];
            instance.W = [TestModel.EnumerationWithGaps.Green, TestModel.EnumerationWithGaps.Red, TestModel.EnumerationWithGaps.Invalid];
            instance.X = [new DataValue() { WrappedValue = new Variant(100U), StatusCode = StatusCodes.GoodClamped, ServerTimestamp = new DateTime(2000, 04, 30, 18, 15, 33) }, new DataValue() { WrappedValue = new Variant("I see blue"), StatusCode = StatusCodes.UncertainLastUsableValue, SourceTimestamp = new DateTime(1969, 07, 29, 04, 15, 55) }, new DataValue() { WrappedValue = new Variant(4804837202), StatusCode = StatusCodes.UncertainLastUsableValue, SourceTimestamp = new DateTime(2024, 03, 24, 16, 05, 23), SourcePicoseconds = 102 }];
            instance.Y = [new Variant(-80000), new Variant("Watermelon"), new Variant(generator.GetStaticConcreteStructure())];
            instance.Z = [new ExtensionObject(TestModel.DataTypeIds.ConcreteStructure, generator.GetStaticConcreteStructure()), new ExtensionObject(Opc.Ua.DataTypeIds.Range, new Opc.Ua.Range() { High = 100, Low = 10 }), new ExtensionObject(Opc.Ua.DataTypeIds.ThreeDVector, new ThreeDVector() { X = 1, Y = 2, Z = 3 })];
            return instance;
        }

        public static ConcreteStructure GetRandomConcreteStructure(this DataGenerator generator)
        {
            var instance = new ConcreteStructure();
            instance.A = generator.GetRandomInt16();
            instance.B = generator.GetRandomDouble();
            instance.C = generator.GetRandomString();
            instance.D = generator.GetRandomInt16();
            instance.E = generator.GetRandomDouble();
            instance.F = generator.GetRandomString();
            return instance;
        }

        public static ConcreteStructure GetStaticConcreteStructure(this DataGenerator generator)
        {
            var instance = new ConcreteStructure();
            instance.A = 20000;
            instance.B = 100.001;
            instance.C = "Lemon";
            instance.D = 1000;
            instance.E = 420.098;
            instance.F = "Lime";
            return instance;
        }

        public static NestedStructure GetRandomNestedStructure(this DataGenerator generator)
        {
            var instance = new NestedStructure();
            instance.A = generator.GetRandomScalarStructure();
            instance.B = generator.GetRandomArrayStructure();
            instance.C = generator.GetRandomStructureArray(generator.GetRandomScalarStructure);
            instance.D = generator.GetRandomStructureArray(generator.GetRandomArrayStructure);
            return instance;
        }

        public static NestedStructure GetStaticNestedStructure(this DataGenerator generator)
        {
            var instance = new NestedStructure();
            instance.A = generator.GetStaticScalarStructure();
            instance.B = generator.GetStaticArrayStructure();
            instance.C = [generator.GetStaticScalarStructure(), generator.GetStaticScalarStructure(), generator.GetStaticScalarStructure()];
            instance.D = [generator.GetStaticArrayStructure(), generator.GetStaticArrayStructure(), generator.GetStaticArrayStructure()];
            return instance;
        }

        public static BasicUnion GetRandomBasicUnion(this DataGenerator generator)
        {
            var instance = new BasicUnion();
            instance.SwitchField = (BasicUnionFields)(generator.GetRandomInt32() % 6);

            switch (instance.SwitchField)
            {
                case BasicUnionFields.A: { instance.A = generator.GetRandomUInt32(); break; }
                case BasicUnionFields.B: { instance.B = generator.GetRandomArray<string>(true, 3, true); break; }
                case BasicUnionFields.C: { instance.C = generator.GetRandomByteString(); break; }
                case BasicUnionFields.D: { instance.D = generator.GetRandomConcreteStructure(); break; }
                case BasicUnionFields.E: { instance.E = generator.GetRandomEnumerationWithGaps(); break; }
            }

            return instance;
        }

        public static BasicUnion GetStaticBasicUnion(this DataGenerator generator)
        {
            var instance = new BasicUnion();
            instance.SwitchField = (BasicUnionFields)(generator.GetRandomInt32() % 5 + 1);

            switch (instance.SwitchField)
            {
                case BasicUnionFields.A: { instance.A = 11211U; break; }
                case BasicUnionFields.B: { instance.B = ["Gold", "Silver", "Bronze"]; break; }
                case BasicUnionFields.C: { instance.C = Convert.FromHexString("1c989d2c7fe3bab06952d2fc"); break; }
                case BasicUnionFields.D: { instance.D = generator.GetStaticConcreteStructure(); break; }
                case BasicUnionFields.E: { instance.E = EnumerationWithGaps.Blue; break; }
            }

            return instance;
        }

        public static StructureWithOptionalFields GetRandomStructureWithOptionalFields(this DataGenerator generator)
        {
            var instance = new StructureWithOptionalFields();
            instance.EncodingMask = (uint)StructureWithOptionalFieldsFields.None;

            if (generator.GetRandomBoolean())
            {
                instance.EncodingMask |= (uint)StructureWithOptionalFieldsFields.A;
                instance.A = generator.GetRandomUInt32();
            }

            if (generator.GetRandomBoolean())
            {
                instance.EncodingMask |= (uint)StructureWithOptionalFieldsFields.B;
                instance.B = generator.GetRandomArray<string>(true, 3, true);
            }

            if (generator.GetRandomBoolean())
            {
                instance.EncodingMask |= (uint)StructureWithOptionalFieldsFields.C;
                instance.C = generator.GetRandomByteString();
            }

            if (generator.GetRandomBoolean())
            {
                instance.EncodingMask |= (uint)StructureWithOptionalFieldsFields.D;
                instance.D = generator.GetRandomConcreteStructure();
            }

            if (generator.GetRandomBoolean())
            {
                instance.EncodingMask |= (uint)StructureWithOptionalFieldsFields.E;
                instance.E = generator.GetRandomEnumerationWithGaps();
            }

            return instance;
        }

        public static StructureWithOptionalFields GetStaticStructureWithOptionalFields(this DataGenerator generator)
        {
            var instance = new StructureWithOptionalFields();
            instance.EncodingMask = (uint)StructureWithOptionalFieldsFields.None;

            if (generator.GetRandomBoolean())
            {
                instance.EncodingMask |= (uint)StructureWithOptionalFieldsFields.A;
                instance.A = 0;
            }

            if (generator.GetRandomBoolean())
            {
                instance.EncodingMask |= (uint)StructureWithOptionalFieldsFields.B;
                instance.B = ["Apple", "Banana", "Cantaloupe"];
            }

            if (generator.GetRandomBoolean())
            {
                instance.EncodingMask |= (uint)StructureWithOptionalFieldsFields.C;
                instance.C = Convert.FromHexString("0b6abd0bb92c2036872876bf");
            }

            if (generator.GetRandomBoolean())
            {
                instance.EncodingMask |= (uint)StructureWithOptionalFieldsFields.D;
                instance.D = generator.GetStaticConcreteStructure();
            }

            if (generator.GetRandomBoolean())
            {
                instance.EncodingMask |= (uint)StructureWithOptionalFieldsFields.E;
                instance.E = EnumerationWithGaps.Red;
            }

            return instance;
        }

        public static EnumerationWithGaps GetRandomEnumerationWithGaps(this DataGenerator generator)
        {
            var values = Enum.GetValues(typeof(EnumerationWithGaps));
            var index = generator.GetRandomInt32() % values.Length;
            return (EnumerationWithGaps)values.GetValue(index);
        }

        public static T[] GetRandomStructureArray<T>(this DataGenerator generator, Func<T> newElement)
        {
            var length = generator.GetRandomInt16() % 5;

            var array = new List<T>();

            for (int ii = 0; ii < length; ii++)
            {
                array.Add(newElement());
            }

            return array.ToArray();
        }
    }
}
