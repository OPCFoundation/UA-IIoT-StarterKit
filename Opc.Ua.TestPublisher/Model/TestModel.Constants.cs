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

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Runtime.Serialization;
using Opc.Ua;

namespace TestModel
{
    #region DataType Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class DataTypes
    {
        /// <remarks />
        public const uint AbstractStructure = 61;

        /// <remarks />
        public const uint ConcreteStructure = 62;

        /// <remarks />
        public const uint EnumerationWithGaps = 63;

        /// <remarks />
        public const uint ScalarStructure = 65;

        /// <remarks />
        public const uint ScalarStructureWithAllowSubtypes = 66;

        /// <remarks />
        public const uint ArrayStructure = 67;

        /// <remarks />
        public const uint ArrayStructureWithAllowSubtypes = 68;

        /// <remarks />
        public const uint NestedStructure = 69;

        /// <remarks />
        public const uint NestedStructureWithAllowSubtypes = 70;

        /// <remarks />
        public const uint BasicUnion = 143;

        /// <remarks />
        public const uint StructureWithOptionalFields = 144;
    }
    #endregion

    #region Object Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Objects
    {
        /// <remarks />
        public const uint AbstractStructure_Encoding_DefaultBinary = 71;

        /// <remarks />
        public const uint ConcreteStructure_Encoding_DefaultBinary = 72;

        /// <remarks />
        public const uint ScalarStructure_Encoding_DefaultBinary = 73;

        /// <remarks />
        public const uint ScalarStructureWithAllowSubtypes_Encoding_DefaultBinary = 74;

        /// <remarks />
        public const uint ArrayStructure_Encoding_DefaultBinary = 75;

        /// <remarks />
        public const uint ArrayStructureWithAllowSubtypes_Encoding_DefaultBinary = 76;

        /// <remarks />
        public const uint NestedStructure_Encoding_DefaultBinary = 77;

        /// <remarks />
        public const uint NestedStructureWithAllowSubtypes_Encoding_DefaultBinary = 78;

        /// <remarks />
        public const uint BasicUnion_Encoding_DefaultBinary = 145;

        /// <remarks />
        public const uint StructureWithOptionalFields_Encoding_DefaultBinary = 146;

        /// <remarks />
        public const uint AbstractStructure_Encoding_DefaultXml = 103;

        /// <remarks />
        public const uint ConcreteStructure_Encoding_DefaultXml = 104;

        /// <remarks />
        public const uint ScalarStructure_Encoding_DefaultXml = 105;

        /// <remarks />
        public const uint ScalarStructureWithAllowSubtypes_Encoding_DefaultXml = 106;

        /// <remarks />
        public const uint ArrayStructure_Encoding_DefaultXml = 107;

        /// <remarks />
        public const uint ArrayStructureWithAllowSubtypes_Encoding_DefaultXml = 108;

        /// <remarks />
        public const uint NestedStructure_Encoding_DefaultXml = 109;

        /// <remarks />
        public const uint NestedStructureWithAllowSubtypes_Encoding_DefaultXml = 110;

        /// <remarks />
        public const uint BasicUnion_Encoding_DefaultXml = 153;

        /// <remarks />
        public const uint StructureWithOptionalFields_Encoding_DefaultXml = 154;

        /// <remarks />
        public const uint AbstractStructure_Encoding_DefaultJson = 135;

        /// <remarks />
        public const uint ConcreteStructure_Encoding_DefaultJson = 136;

        /// <remarks />
        public const uint ScalarStructure_Encoding_DefaultJson = 137;

        /// <remarks />
        public const uint ScalarStructureWithAllowSubtypes_Encoding_DefaultJson = 138;

        /// <remarks />
        public const uint ArrayStructure_Encoding_DefaultJson = 139;

        /// <remarks />
        public const uint ArrayStructureWithAllowSubtypes_Encoding_DefaultJson = 140;

        /// <remarks />
        public const uint NestedStructure_Encoding_DefaultJson = 141;

        /// <remarks />
        public const uint NestedStructureWithAllowSubtypes_Encoding_DefaultJson = 142;

        /// <remarks />
        public const uint BasicUnion_Encoding_DefaultJson = 161;

        /// <remarks />
        public const uint StructureWithOptionalFields_Encoding_DefaultJson = 162;
    }
    #endregion

    #region Variable Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Variables
    {
        /// <remarks />
        public const uint EnumerationWithGaps_EnumValues = 64;

        /// <remarks />
        public const uint TestModel_BinarySchema = 13;

        /// <remarks />
        public const uint TestModel_BinarySchema_NamespaceUri = 15;

        /// <remarks />
        public const uint TestModel_BinarySchema_Deprecated = 16;

        /// <remarks />
        public const uint TestModel_BinarySchema_AbstractStructure = 79;

        /// <remarks />
        public const uint TestModel_BinarySchema_ConcreteStructure = 82;

        /// <remarks />
        public const uint TestModel_BinarySchema_ScalarStructure = 85;

        /// <remarks />
        public const uint TestModel_BinarySchema_ScalarStructureWithAllowSubtypes = 88;

        /// <remarks />
        public const uint TestModel_BinarySchema_ArrayStructure = 91;

        /// <remarks />
        public const uint TestModel_BinarySchema_ArrayStructureWithAllowSubtypes = 94;

        /// <remarks />
        public const uint TestModel_BinarySchema_NestedStructure = 97;

        /// <remarks />
        public const uint TestModel_BinarySchema_NestedStructureWithAllowSubtypes = 100;

        /// <remarks />
        public const uint TestModel_BinarySchema_BasicUnion = 147;

        /// <remarks />
        public const uint TestModel_BinarySchema_StructureWithOptionalFields = 150;

        /// <remarks />
        public const uint TestModel_XmlSchema = 37;

        /// <remarks />
        public const uint TestModel_XmlSchema_NamespaceUri = 39;

        /// <remarks />
        public const uint TestModel_XmlSchema_Deprecated = 40;

        /// <remarks />
        public const uint TestModel_XmlSchema_AbstractStructure = 111;

        /// <remarks />
        public const uint TestModel_XmlSchema_ConcreteStructure = 114;

        /// <remarks />
        public const uint TestModel_XmlSchema_ScalarStructure = 117;

        /// <remarks />
        public const uint TestModel_XmlSchema_ScalarStructureWithAllowSubtypes = 120;

        /// <remarks />
        public const uint TestModel_XmlSchema_ArrayStructure = 123;

        /// <remarks />
        public const uint TestModel_XmlSchema_ArrayStructureWithAllowSubtypes = 126;

        /// <remarks />
        public const uint TestModel_XmlSchema_NestedStructure = 129;

        /// <remarks />
        public const uint TestModel_XmlSchema_NestedStructureWithAllowSubtypes = 132;

        /// <remarks />
        public const uint TestModel_XmlSchema_BasicUnion = 155;

        /// <remarks />
        public const uint TestModel_XmlSchema_StructureWithOptionalFields = 158;
    }
    #endregion

    #region DataType Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class DataTypeIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId AbstractStructure = new ExpandedNodeId(TestModel.DataTypes.AbstractStructure, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ConcreteStructure = new ExpandedNodeId(TestModel.DataTypes.ConcreteStructure, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId EnumerationWithGaps = new ExpandedNodeId(TestModel.DataTypes.EnumerationWithGaps, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ScalarStructure = new ExpandedNodeId(TestModel.DataTypes.ScalarStructure, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ScalarStructureWithAllowSubtypes = new ExpandedNodeId(TestModel.DataTypes.ScalarStructureWithAllowSubtypes, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ArrayStructure = new ExpandedNodeId(TestModel.DataTypes.ArrayStructure, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ArrayStructureWithAllowSubtypes = new ExpandedNodeId(TestModel.DataTypes.ArrayStructureWithAllowSubtypes, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId NestedStructure = new ExpandedNodeId(TestModel.DataTypes.NestedStructure, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId NestedStructureWithAllowSubtypes = new ExpandedNodeId(TestModel.DataTypes.NestedStructureWithAllowSubtypes, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId BasicUnion = new ExpandedNodeId(TestModel.DataTypes.BasicUnion, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId StructureWithOptionalFields = new ExpandedNodeId(TestModel.DataTypes.StructureWithOptionalFields, TestModel.Namespaces.TestModel);
    }
    #endregion

    #region Object Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ObjectIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId AbstractStructure_Encoding_DefaultBinary = new ExpandedNodeId(TestModel.Objects.AbstractStructure_Encoding_DefaultBinary, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ConcreteStructure_Encoding_DefaultBinary = new ExpandedNodeId(TestModel.Objects.ConcreteStructure_Encoding_DefaultBinary, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ScalarStructure_Encoding_DefaultBinary = new ExpandedNodeId(TestModel.Objects.ScalarStructure_Encoding_DefaultBinary, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ScalarStructureWithAllowSubtypes_Encoding_DefaultBinary = new ExpandedNodeId(TestModel.Objects.ScalarStructureWithAllowSubtypes_Encoding_DefaultBinary, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ArrayStructure_Encoding_DefaultBinary = new ExpandedNodeId(TestModel.Objects.ArrayStructure_Encoding_DefaultBinary, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ArrayStructureWithAllowSubtypes_Encoding_DefaultBinary = new ExpandedNodeId(TestModel.Objects.ArrayStructureWithAllowSubtypes_Encoding_DefaultBinary, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId NestedStructure_Encoding_DefaultBinary = new ExpandedNodeId(TestModel.Objects.NestedStructure_Encoding_DefaultBinary, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId NestedStructureWithAllowSubtypes_Encoding_DefaultBinary = new ExpandedNodeId(TestModel.Objects.NestedStructureWithAllowSubtypes_Encoding_DefaultBinary, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId BasicUnion_Encoding_DefaultBinary = new ExpandedNodeId(TestModel.Objects.BasicUnion_Encoding_DefaultBinary, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId StructureWithOptionalFields_Encoding_DefaultBinary = new ExpandedNodeId(TestModel.Objects.StructureWithOptionalFields_Encoding_DefaultBinary, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId AbstractStructure_Encoding_DefaultXml = new ExpandedNodeId(TestModel.Objects.AbstractStructure_Encoding_DefaultXml, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ConcreteStructure_Encoding_DefaultXml = new ExpandedNodeId(TestModel.Objects.ConcreteStructure_Encoding_DefaultXml, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ScalarStructure_Encoding_DefaultXml = new ExpandedNodeId(TestModel.Objects.ScalarStructure_Encoding_DefaultXml, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ScalarStructureWithAllowSubtypes_Encoding_DefaultXml = new ExpandedNodeId(TestModel.Objects.ScalarStructureWithAllowSubtypes_Encoding_DefaultXml, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ArrayStructure_Encoding_DefaultXml = new ExpandedNodeId(TestModel.Objects.ArrayStructure_Encoding_DefaultXml, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ArrayStructureWithAllowSubtypes_Encoding_DefaultXml = new ExpandedNodeId(TestModel.Objects.ArrayStructureWithAllowSubtypes_Encoding_DefaultXml, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId NestedStructure_Encoding_DefaultXml = new ExpandedNodeId(TestModel.Objects.NestedStructure_Encoding_DefaultXml, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId NestedStructureWithAllowSubtypes_Encoding_DefaultXml = new ExpandedNodeId(TestModel.Objects.NestedStructureWithAllowSubtypes_Encoding_DefaultXml, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId BasicUnion_Encoding_DefaultXml = new ExpandedNodeId(TestModel.Objects.BasicUnion_Encoding_DefaultXml, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId StructureWithOptionalFields_Encoding_DefaultXml = new ExpandedNodeId(TestModel.Objects.StructureWithOptionalFields_Encoding_DefaultXml, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId AbstractStructure_Encoding_DefaultJson = new ExpandedNodeId(TestModel.Objects.AbstractStructure_Encoding_DefaultJson, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ConcreteStructure_Encoding_DefaultJson = new ExpandedNodeId(TestModel.Objects.ConcreteStructure_Encoding_DefaultJson, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ScalarStructure_Encoding_DefaultJson = new ExpandedNodeId(TestModel.Objects.ScalarStructure_Encoding_DefaultJson, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ScalarStructureWithAllowSubtypes_Encoding_DefaultJson = new ExpandedNodeId(TestModel.Objects.ScalarStructureWithAllowSubtypes_Encoding_DefaultJson, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ArrayStructure_Encoding_DefaultJson = new ExpandedNodeId(TestModel.Objects.ArrayStructure_Encoding_DefaultJson, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId ArrayStructureWithAllowSubtypes_Encoding_DefaultJson = new ExpandedNodeId(TestModel.Objects.ArrayStructureWithAllowSubtypes_Encoding_DefaultJson, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId NestedStructure_Encoding_DefaultJson = new ExpandedNodeId(TestModel.Objects.NestedStructure_Encoding_DefaultJson, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId NestedStructureWithAllowSubtypes_Encoding_DefaultJson = new ExpandedNodeId(TestModel.Objects.NestedStructureWithAllowSubtypes_Encoding_DefaultJson, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId BasicUnion_Encoding_DefaultJson = new ExpandedNodeId(TestModel.Objects.BasicUnion_Encoding_DefaultJson, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId StructureWithOptionalFields_Encoding_DefaultJson = new ExpandedNodeId(TestModel.Objects.StructureWithOptionalFields_Encoding_DefaultJson, TestModel.Namespaces.TestModel);
    }
    #endregion

    #region Variable Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class VariableIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId EnumerationWithGaps_EnumValues = new ExpandedNodeId(TestModel.Variables.EnumerationWithGaps_EnumValues, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_BinarySchema = new ExpandedNodeId(TestModel.Variables.TestModel_BinarySchema, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_BinarySchema_NamespaceUri = new ExpandedNodeId(TestModel.Variables.TestModel_BinarySchema_NamespaceUri, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_BinarySchema_Deprecated = new ExpandedNodeId(TestModel.Variables.TestModel_BinarySchema_Deprecated, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_BinarySchema_AbstractStructure = new ExpandedNodeId(TestModel.Variables.TestModel_BinarySchema_AbstractStructure, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_BinarySchema_ConcreteStructure = new ExpandedNodeId(TestModel.Variables.TestModel_BinarySchema_ConcreteStructure, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_BinarySchema_ScalarStructure = new ExpandedNodeId(TestModel.Variables.TestModel_BinarySchema_ScalarStructure, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_BinarySchema_ScalarStructureWithAllowSubtypes = new ExpandedNodeId(TestModel.Variables.TestModel_BinarySchema_ScalarStructureWithAllowSubtypes, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_BinarySchema_ArrayStructure = new ExpandedNodeId(TestModel.Variables.TestModel_BinarySchema_ArrayStructure, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_BinarySchema_ArrayStructureWithAllowSubtypes = new ExpandedNodeId(TestModel.Variables.TestModel_BinarySchema_ArrayStructureWithAllowSubtypes, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_BinarySchema_NestedStructure = new ExpandedNodeId(TestModel.Variables.TestModel_BinarySchema_NestedStructure, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_BinarySchema_NestedStructureWithAllowSubtypes = new ExpandedNodeId(TestModel.Variables.TestModel_BinarySchema_NestedStructureWithAllowSubtypes, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_BinarySchema_BasicUnion = new ExpandedNodeId(TestModel.Variables.TestModel_BinarySchema_BasicUnion, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_BinarySchema_StructureWithOptionalFields = new ExpandedNodeId(TestModel.Variables.TestModel_BinarySchema_StructureWithOptionalFields, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_XmlSchema = new ExpandedNodeId(TestModel.Variables.TestModel_XmlSchema, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_XmlSchema_NamespaceUri = new ExpandedNodeId(TestModel.Variables.TestModel_XmlSchema_NamespaceUri, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_XmlSchema_Deprecated = new ExpandedNodeId(TestModel.Variables.TestModel_XmlSchema_Deprecated, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_XmlSchema_AbstractStructure = new ExpandedNodeId(TestModel.Variables.TestModel_XmlSchema_AbstractStructure, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_XmlSchema_ConcreteStructure = new ExpandedNodeId(TestModel.Variables.TestModel_XmlSchema_ConcreteStructure, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_XmlSchema_ScalarStructure = new ExpandedNodeId(TestModel.Variables.TestModel_XmlSchema_ScalarStructure, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_XmlSchema_ScalarStructureWithAllowSubtypes = new ExpandedNodeId(TestModel.Variables.TestModel_XmlSchema_ScalarStructureWithAllowSubtypes, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_XmlSchema_ArrayStructure = new ExpandedNodeId(TestModel.Variables.TestModel_XmlSchema_ArrayStructure, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_XmlSchema_ArrayStructureWithAllowSubtypes = new ExpandedNodeId(TestModel.Variables.TestModel_XmlSchema_ArrayStructureWithAllowSubtypes, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_XmlSchema_NestedStructure = new ExpandedNodeId(TestModel.Variables.TestModel_XmlSchema_NestedStructure, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_XmlSchema_NestedStructureWithAllowSubtypes = new ExpandedNodeId(TestModel.Variables.TestModel_XmlSchema_NestedStructureWithAllowSubtypes, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_XmlSchema_BasicUnion = new ExpandedNodeId(TestModel.Variables.TestModel_XmlSchema_BasicUnion, TestModel.Namespaces.TestModel);

        /// <remarks />
        public static readonly ExpandedNodeId TestModel_XmlSchema_StructureWithOptionalFields = new ExpandedNodeId(TestModel.Variables.TestModel_XmlSchema_StructureWithOptionalFields, TestModel.Namespaces.TestModel);
    }
    #endregion

    #region BrowseName Declarations
    /// <remarks />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class BrowseNames
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
    #endregion

    #region Namespace Declarations
    /// <remarks />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Namespaces
    {
        /// <summary>
        /// The URI for the TestModel namespace (.NET code namespace is 'TestModel').
        /// </summary>
        public const string TestModel = "urn:opcfoundation.org:2024-01:TestModel";

        /// <summary>
        /// The URI for the TestModelXsd namespace (.NET code namespace is 'TestModel').
        /// </summary>
        public const string TestModelXsd = "urn:opcfoundation.org:2024-01:TestModelTypes.xsd";

        /// <summary>
        /// The URI for the OpcUa namespace (.NET code namespace is 'Opc.Ua').
        /// </summary>
        public const string OpcUa = "http://opcfoundation.org/UA/";

        /// <summary>
        /// The URI for the OpcUaXsd namespace (.NET code namespace is 'Opc.Ua').
        /// </summary>
        public const string OpcUaXsd = "http://opcfoundation.org/UA/2008/02/Types.xsd";
    }
    #endregion
}