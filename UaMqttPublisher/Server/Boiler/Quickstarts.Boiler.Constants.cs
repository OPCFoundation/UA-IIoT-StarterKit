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

using Opc.Ua;

namespace Quickstarts.Boiler
{
    #region DataType Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class DataTypes
    {
        /// <remarks />
        public const uint ControllerDataType = 183;

        /// <remarks />
        public const uint SetPointMask = 218;

        /// <remarks />
        public const uint ChangeSetPointsRequest = 220;

        /// <remarks />
        public const uint ChangeSetPointsResponse = 221;
    }
    #endregion

    #region Method Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Methods
    {
        /// <remarks />
        public const uint BoilerType_ChangeSetPoints = 200;

        /// <remarks />
        public const uint BoilerType_EmergencyShutdown = 203;

        /// <remarks />
        public const uint BoilerType_Restart = 206;

        /// <remarks />
        public const uint Boiler1_ChangeSetPoints = 209;

        /// <remarks />
        public const uint Boiler1_EmergencyShutdown = 212;

        /// <remarks />
        public const uint Boiler1_Restart = 215;
    }
    #endregion

    #region Object Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Objects
    {
        /// <remarks />
        public const uint BoilerInputPipeType_FlowTransmitter1 = 74;

        /// <remarks />
        public const uint BoilerInputPipeType_Valve = 81;

        /// <remarks />
        public const uint BoilerDrumType_LevelIndicator = 89;

        /// <remarks />
        public const uint BoilerOutputPipeType_FlowTransmitter2 = 97;

        /// <remarks />
        public const uint BoilerType_InputPipe = 56;

        /// <remarks />
        public const uint BoilerType_InputPipe_FlowTransmitter1 = 104;

        /// <remarks />
        public const uint BoilerType_InputPipe_Valve = 111;

        /// <remarks />
        public const uint BoilerType_Drum = 57;

        /// <remarks />
        public const uint BoilerType_Drum_LevelIndicator = 58;

        /// <remarks />
        public const uint BoilerType_OutputPipe = 65;

        /// <remarks />
        public const uint BoilerType_OutputPipe_FlowTransmitter2 = 118;

        /// <remarks />
        public const uint BoilerType_FlowController = 125;

        /// <remarks />
        public const uint BoilerType_LevelController = 129;

        /// <remarks />
        public const uint BoilerType_CustomController = 133;

        /// <remarks />
        public const uint Boiler1 = 138;

        /// <remarks />
        public const uint Boiler1_InputPipe = 139;

        /// <remarks />
        public const uint Boiler1_InputPipe_FlowTransmitter1 = 140;

        /// <remarks />
        public const uint Boiler1_InputPipe_Valve = 147;

        /// <remarks />
        public const uint Boiler1_Drum = 154;

        /// <remarks />
        public const uint Boiler1_Drum_LevelIndicator = 155;

        /// <remarks />
        public const uint Boiler1_OutputPipe = 162;

        /// <remarks />
        public const uint Boiler1_OutputPipe_FlowTransmitter2 = 163;

        /// <remarks />
        public const uint Boiler1_FlowController = 170;

        /// <remarks />
        public const uint Boiler1_LevelController = 174;

        /// <remarks />
        public const uint Boiler1_CustomController = 178;

        /// <remarks />
        public const uint ControllerDataType_Encoding_DefaultBinary = 191;

        /// <remarks />
        public const uint ChangeSetPointsRequest_Encoding_DefaultBinary = 222;

        /// <remarks />
        public const uint ChangeSetPointsResponse_Encoding_DefaultBinary = 223;

        /// <remarks />
        public const uint ControllerDataType_Encoding_DefaultXml = 184;

        /// <remarks />
        public const uint ChangeSetPointsRequest_Encoding_DefaultXml = 230;

        /// <remarks />
        public const uint ChangeSetPointsResponse_Encoding_DefaultXml = 231;

        /// <remarks />
        public const uint ControllerDataType_Encoding_DefaultJson = 15003;

        /// <remarks />
        public const uint ChangeSetPointsRequest_Encoding_DefaultJson = 238;

        /// <remarks />
        public const uint ChangeSetPointsResponse_Encoding_DefaultJson = 239;
    }
    #endregion

    #region ObjectType Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ObjectTypes
    {
        /// <remarks />
        public const uint GenericControllerType = 3;

        /// <remarks />
        public const uint GenericSensorType = 7;

        /// <remarks />
        public const uint GenericActuatorType = 14;

        /// <remarks />
        public const uint CustomControllerType = 21;

        /// <remarks />
        public const uint ValveType = 26;

        /// <remarks />
        public const uint LevelControllerType = 33;

        /// <remarks />
        public const uint FlowControllerType = 37;

        /// <remarks />
        public const uint LevelIndicatorType = 41;

        /// <remarks />
        public const uint FlowTransmitterType = 48;

        /// <remarks />
        public const uint BoilerInputPipeType = 73;

        /// <remarks />
        public const uint BoilerDrumType = 88;

        /// <remarks />
        public const uint BoilerOutputPipeType = 96;

        /// <remarks />
        public const uint BoilerType = 55;
    }
    #endregion

    #region ReferenceType Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ReferenceTypes
    {
        /// <remarks />
        public const uint FlowTo = 1;

        /// <remarks />
        public const uint SignalTo = 2;
    }
    #endregion

    #region Variable Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Variables
    {
        /// <remarks />
        public const uint GenericControllerType_Measurement = 4;

        /// <remarks />
        public const uint GenericControllerType_SetPoint = 5;

        /// <remarks />
        public const uint GenericControllerType_ControlOut = 6;

        /// <remarks />
        public const uint GenericSensorType_Output = 8;

        /// <remarks />
        public const uint GenericSensorType_Output_EURange = 11;

        /// <remarks />
        public const uint GenericActuatorType_Input = 15;

        /// <remarks />
        public const uint GenericActuatorType_Input_EURange = 18;

        /// <remarks />
        public const uint CustomControllerType_Input1 = 22;

        /// <remarks />
        public const uint CustomControllerType_Input2 = 23;

        /// <remarks />
        public const uint CustomControllerType_Input3 = 24;

        /// <remarks />
        public const uint CustomControllerType_ControlOut = 25;

        /// <remarks />
        public const uint BoilerInputPipeType_FlowTransmitter1_Output = 75;

        /// <remarks />
        public const uint BoilerInputPipeType_FlowTransmitter1_Output_EURange = 78;

        /// <remarks />
        public const uint BoilerInputPipeType_Valve_Input = 82;

        /// <remarks />
        public const uint BoilerInputPipeType_Valve_Input_EURange = 85;

        /// <remarks />
        public const uint BoilerDrumType_LevelIndicator_Output = 90;

        /// <remarks />
        public const uint BoilerDrumType_LevelIndicator_Output_EURange = 93;

        /// <remarks />
        public const uint BoilerOutputPipeType_FlowTransmitter2_Output = 98;

        /// <remarks />
        public const uint BoilerOutputPipeType_FlowTransmitter2_Output_EURange = 101;

        /// <remarks />
        public const uint SetPointMask_OptionSetValues = 219;

        /// <remarks />
        public const uint BoilerType_InputPipe_FlowTransmitter1_Output = 105;

        /// <remarks />
        public const uint BoilerType_InputPipe_FlowTransmitter1_Output_EURange = 108;

        /// <remarks />
        public const uint BoilerType_InputPipe_Valve_Input = 112;

        /// <remarks />
        public const uint BoilerType_InputPipe_Valve_Input_EURange = 115;

        /// <remarks />
        public const uint BoilerType_Drum_LevelIndicator_Output = 59;

        /// <remarks />
        public const uint BoilerType_Drum_LevelIndicator_Output_EURange = 62;

        /// <remarks />
        public const uint BoilerType_OutputPipe_FlowTransmitter2_Output = 119;

        /// <remarks />
        public const uint BoilerType_OutputPipe_FlowTransmitter2_Output_EURange = 122;

        /// <remarks />
        public const uint BoilerType_FlowController_Measurement = 126;

        /// <remarks />
        public const uint BoilerType_FlowController_SetPoint = 127;

        /// <remarks />
        public const uint BoilerType_FlowController_ControlOut = 128;

        /// <remarks />
        public const uint BoilerType_LevelController_Measurement = 130;

        /// <remarks />
        public const uint BoilerType_LevelController_SetPoint = 131;

        /// <remarks />
        public const uint BoilerType_LevelController_ControlOut = 132;

        /// <remarks />
        public const uint BoilerType_CustomController_Input1 = 134;

        /// <remarks />
        public const uint BoilerType_CustomController_Input2 = 135;

        /// <remarks />
        public const uint BoilerType_CustomController_Input3 = 136;

        /// <remarks />
        public const uint BoilerType_CustomController_ControlOut = 137;

        /// <remarks />
        public const uint BoilerType_ChangeSetPoints_InputArguments = 201;

        /// <remarks />
        public const uint BoilerType_ChangeSetPoints_OutputArguments = 202;

        /// <remarks />
        public const uint BoilerType_EmergencyShutdown_InputArguments = 204;

        /// <remarks />
        public const uint BoilerType_EmergencyShutdown_OutputArguments = 205;

        /// <remarks />
        public const uint BoilerType_Restart_InputArguments = 207;

        /// <remarks />
        public const uint BoilerType_Restart_OutputArguments = 208;

        /// <remarks />
        public const uint Boiler1_InputPipe_FlowTransmitter1_Output = 141;

        /// <remarks />
        public const uint Boiler1_InputPipe_FlowTransmitter1_Output_EURange = 144;

        /// <remarks />
        public const uint Boiler1_InputPipe_Valve_Input = 148;

        /// <remarks />
        public const uint Boiler1_InputPipe_Valve_Input_EURange = 151;

        /// <remarks />
        public const uint Boiler1_Drum_LevelIndicator_Output = 156;

        /// <remarks />
        public const uint Boiler1_Drum_LevelIndicator_Output_EURange = 159;

        /// <remarks />
        public const uint Boiler1_OutputPipe_FlowTransmitter2_Output = 164;

        /// <remarks />
        public const uint Boiler1_OutputPipe_FlowTransmitter2_Output_EURange = 167;

        /// <remarks />
        public const uint Boiler1_FlowController_Measurement = 171;

        /// <remarks />
        public const uint Boiler1_FlowController_SetPoint = 172;

        /// <remarks />
        public const uint Boiler1_FlowController_ControlOut = 173;

        /// <remarks />
        public const uint Boiler1_LevelController_Measurement = 175;

        /// <remarks />
        public const uint Boiler1_LevelController_SetPoint = 176;

        /// <remarks />
        public const uint Boiler1_LevelController_ControlOut = 177;

        /// <remarks />
        public const uint Boiler1_CustomController_Input1 = 179;

        /// <remarks />
        public const uint Boiler1_CustomController_Input2 = 180;

        /// <remarks />
        public const uint Boiler1_CustomController_Input3 = 181;

        /// <remarks />
        public const uint Boiler1_CustomController_ControlOut = 182;

        /// <remarks />
        public const uint Boiler1_ChangeSetPoints_InputArguments = 210;

        /// <remarks />
        public const uint Boiler1_ChangeSetPoints_OutputArguments = 211;

        /// <remarks />
        public const uint Boiler1_EmergencyShutdown_InputArguments = 213;

        /// <remarks />
        public const uint Boiler1_EmergencyShutdown_OutputArguments = 214;

        /// <remarks />
        public const uint Boiler1_Restart_InputArguments = 216;

        /// <remarks />
        public const uint Boiler1_Restart_OutputArguments = 217;

        /// <remarks />
        public const uint Boiler_BinarySchema = 192;

        /// <remarks />
        public const uint Boiler_BinarySchema_NamespaceUri = 194;

        /// <remarks />
        public const uint Boiler_BinarySchema_Deprecated = 15001;

        /// <remarks />
        public const uint Boiler_BinarySchema_ControllerDataType = 195;

        /// <remarks />
        public const uint Boiler_BinarySchema_ChangeSetPointsRequest = 224;

        /// <remarks />
        public const uint Boiler_BinarySchema_ChangeSetPointsResponse = 227;

        /// <remarks />
        public const uint Boiler_XmlSchema = 185;

        /// <remarks />
        public const uint Boiler_XmlSchema_NamespaceUri = 187;

        /// <remarks />
        public const uint Boiler_XmlSchema_Deprecated = 15002;

        /// <remarks />
        public const uint Boiler_XmlSchema_ControllerDataType = 188;

        /// <remarks />
        public const uint Boiler_XmlSchema_ChangeSetPointsRequest = 232;

        /// <remarks />
        public const uint Boiler_XmlSchema_ChangeSetPointsResponse = 235;
    }
    #endregion

    #region DataType Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class DataTypeIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId ControllerDataType = new ExpandedNodeId(Quickstarts.Boiler.DataTypes.ControllerDataType, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId SetPointMask = new ExpandedNodeId(Quickstarts.Boiler.DataTypes.SetPointMask, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ChangeSetPointsRequest = new ExpandedNodeId(Quickstarts.Boiler.DataTypes.ChangeSetPointsRequest, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ChangeSetPointsResponse = new ExpandedNodeId(Quickstarts.Boiler.DataTypes.ChangeSetPointsResponse, Quickstarts.Boiler.Namespaces.Boiler);
    }
    #endregion

    #region Method Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class MethodIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_ChangeSetPoints = new ExpandedNodeId(Quickstarts.Boiler.Methods.BoilerType_ChangeSetPoints, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_EmergencyShutdown = new ExpandedNodeId(Quickstarts.Boiler.Methods.BoilerType_EmergencyShutdown, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_Restart = new ExpandedNodeId(Quickstarts.Boiler.Methods.BoilerType_Restart, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_ChangeSetPoints = new ExpandedNodeId(Quickstarts.Boiler.Methods.Boiler1_ChangeSetPoints, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_EmergencyShutdown = new ExpandedNodeId(Quickstarts.Boiler.Methods.Boiler1_EmergencyShutdown, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_Restart = new ExpandedNodeId(Quickstarts.Boiler.Methods.Boiler1_Restart, Quickstarts.Boiler.Namespaces.Boiler);
    }
    #endregion

    #region Object Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ObjectIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId BoilerInputPipeType_FlowTransmitter1 = new ExpandedNodeId(Quickstarts.Boiler.Objects.BoilerInputPipeType_FlowTransmitter1, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerInputPipeType_Valve = new ExpandedNodeId(Quickstarts.Boiler.Objects.BoilerInputPipeType_Valve, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerDrumType_LevelIndicator = new ExpandedNodeId(Quickstarts.Boiler.Objects.BoilerDrumType_LevelIndicator, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerOutputPipeType_FlowTransmitter2 = new ExpandedNodeId(Quickstarts.Boiler.Objects.BoilerOutputPipeType_FlowTransmitter2, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_InputPipe = new ExpandedNodeId(Quickstarts.Boiler.Objects.BoilerType_InputPipe, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_InputPipe_FlowTransmitter1 = new ExpandedNodeId(Quickstarts.Boiler.Objects.BoilerType_InputPipe_FlowTransmitter1, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_InputPipe_Valve = new ExpandedNodeId(Quickstarts.Boiler.Objects.BoilerType_InputPipe_Valve, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_Drum = new ExpandedNodeId(Quickstarts.Boiler.Objects.BoilerType_Drum, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_Drum_LevelIndicator = new ExpandedNodeId(Quickstarts.Boiler.Objects.BoilerType_Drum_LevelIndicator, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_OutputPipe = new ExpandedNodeId(Quickstarts.Boiler.Objects.BoilerType_OutputPipe, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_OutputPipe_FlowTransmitter2 = new ExpandedNodeId(Quickstarts.Boiler.Objects.BoilerType_OutputPipe_FlowTransmitter2, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_FlowController = new ExpandedNodeId(Quickstarts.Boiler.Objects.BoilerType_FlowController, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_LevelController = new ExpandedNodeId(Quickstarts.Boiler.Objects.BoilerType_LevelController, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_CustomController = new ExpandedNodeId(Quickstarts.Boiler.Objects.BoilerType_CustomController, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1 = new ExpandedNodeId(Quickstarts.Boiler.Objects.Boiler1, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_InputPipe = new ExpandedNodeId(Quickstarts.Boiler.Objects.Boiler1_InputPipe, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_InputPipe_FlowTransmitter1 = new ExpandedNodeId(Quickstarts.Boiler.Objects.Boiler1_InputPipe_FlowTransmitter1, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_InputPipe_Valve = new ExpandedNodeId(Quickstarts.Boiler.Objects.Boiler1_InputPipe_Valve, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_Drum = new ExpandedNodeId(Quickstarts.Boiler.Objects.Boiler1_Drum, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_Drum_LevelIndicator = new ExpandedNodeId(Quickstarts.Boiler.Objects.Boiler1_Drum_LevelIndicator, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_OutputPipe = new ExpandedNodeId(Quickstarts.Boiler.Objects.Boiler1_OutputPipe, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_OutputPipe_FlowTransmitter2 = new ExpandedNodeId(Quickstarts.Boiler.Objects.Boiler1_OutputPipe_FlowTransmitter2, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_FlowController = new ExpandedNodeId(Quickstarts.Boiler.Objects.Boiler1_FlowController, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_LevelController = new ExpandedNodeId(Quickstarts.Boiler.Objects.Boiler1_LevelController, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_CustomController = new ExpandedNodeId(Quickstarts.Boiler.Objects.Boiler1_CustomController, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ControllerDataType_Encoding_DefaultBinary = new ExpandedNodeId(Quickstarts.Boiler.Objects.ControllerDataType_Encoding_DefaultBinary, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ChangeSetPointsRequest_Encoding_DefaultBinary = new ExpandedNodeId(Quickstarts.Boiler.Objects.ChangeSetPointsRequest_Encoding_DefaultBinary, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ChangeSetPointsResponse_Encoding_DefaultBinary = new ExpandedNodeId(Quickstarts.Boiler.Objects.ChangeSetPointsResponse_Encoding_DefaultBinary, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ControllerDataType_Encoding_DefaultXml = new ExpandedNodeId(Quickstarts.Boiler.Objects.ControllerDataType_Encoding_DefaultXml, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ChangeSetPointsRequest_Encoding_DefaultXml = new ExpandedNodeId(Quickstarts.Boiler.Objects.ChangeSetPointsRequest_Encoding_DefaultXml, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ChangeSetPointsResponse_Encoding_DefaultXml = new ExpandedNodeId(Quickstarts.Boiler.Objects.ChangeSetPointsResponse_Encoding_DefaultXml, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ControllerDataType_Encoding_DefaultJson = new ExpandedNodeId(Quickstarts.Boiler.Objects.ControllerDataType_Encoding_DefaultJson, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ChangeSetPointsRequest_Encoding_DefaultJson = new ExpandedNodeId(Quickstarts.Boiler.Objects.ChangeSetPointsRequest_Encoding_DefaultJson, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ChangeSetPointsResponse_Encoding_DefaultJson = new ExpandedNodeId(Quickstarts.Boiler.Objects.ChangeSetPointsResponse_Encoding_DefaultJson, Quickstarts.Boiler.Namespaces.Boiler);
    }
    #endregion

    #region ObjectType Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ObjectTypeIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId GenericControllerType = new ExpandedNodeId(Quickstarts.Boiler.ObjectTypes.GenericControllerType, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId GenericSensorType = new ExpandedNodeId(Quickstarts.Boiler.ObjectTypes.GenericSensorType, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId GenericActuatorType = new ExpandedNodeId(Quickstarts.Boiler.ObjectTypes.GenericActuatorType, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId CustomControllerType = new ExpandedNodeId(Quickstarts.Boiler.ObjectTypes.CustomControllerType, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ValveType = new ExpandedNodeId(Quickstarts.Boiler.ObjectTypes.ValveType, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId LevelControllerType = new ExpandedNodeId(Quickstarts.Boiler.ObjectTypes.LevelControllerType, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId FlowControllerType = new ExpandedNodeId(Quickstarts.Boiler.ObjectTypes.FlowControllerType, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId LevelIndicatorType = new ExpandedNodeId(Quickstarts.Boiler.ObjectTypes.LevelIndicatorType, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId FlowTransmitterType = new ExpandedNodeId(Quickstarts.Boiler.ObjectTypes.FlowTransmitterType, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerInputPipeType = new ExpandedNodeId(Quickstarts.Boiler.ObjectTypes.BoilerInputPipeType, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerDrumType = new ExpandedNodeId(Quickstarts.Boiler.ObjectTypes.BoilerDrumType, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerOutputPipeType = new ExpandedNodeId(Quickstarts.Boiler.ObjectTypes.BoilerOutputPipeType, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType = new ExpandedNodeId(Quickstarts.Boiler.ObjectTypes.BoilerType, Quickstarts.Boiler.Namespaces.Boiler);
    }
    #endregion

    #region ReferenceType Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ReferenceTypeIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId FlowTo = new ExpandedNodeId(Quickstarts.Boiler.ReferenceTypes.FlowTo, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId SignalTo = new ExpandedNodeId(Quickstarts.Boiler.ReferenceTypes.SignalTo, Quickstarts.Boiler.Namespaces.Boiler);
    }
    #endregion

    #region Variable Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class VariableIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId GenericControllerType_Measurement = new ExpandedNodeId(Quickstarts.Boiler.Variables.GenericControllerType_Measurement, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId GenericControllerType_SetPoint = new ExpandedNodeId(Quickstarts.Boiler.Variables.GenericControllerType_SetPoint, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId GenericControllerType_ControlOut = new ExpandedNodeId(Quickstarts.Boiler.Variables.GenericControllerType_ControlOut, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId GenericSensorType_Output = new ExpandedNodeId(Quickstarts.Boiler.Variables.GenericSensorType_Output, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId GenericSensorType_Output_EURange = new ExpandedNodeId(Quickstarts.Boiler.Variables.GenericSensorType_Output_EURange, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId GenericActuatorType_Input = new ExpandedNodeId(Quickstarts.Boiler.Variables.GenericActuatorType_Input, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId GenericActuatorType_Input_EURange = new ExpandedNodeId(Quickstarts.Boiler.Variables.GenericActuatorType_Input_EURange, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId CustomControllerType_Input1 = new ExpandedNodeId(Quickstarts.Boiler.Variables.CustomControllerType_Input1, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId CustomControllerType_Input2 = new ExpandedNodeId(Quickstarts.Boiler.Variables.CustomControllerType_Input2, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId CustomControllerType_Input3 = new ExpandedNodeId(Quickstarts.Boiler.Variables.CustomControllerType_Input3, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId CustomControllerType_ControlOut = new ExpandedNodeId(Quickstarts.Boiler.Variables.CustomControllerType_ControlOut, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerInputPipeType_FlowTransmitter1_Output = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerInputPipeType_FlowTransmitter1_Output, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerInputPipeType_FlowTransmitter1_Output_EURange = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerInputPipeType_FlowTransmitter1_Output_EURange, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerInputPipeType_Valve_Input = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerInputPipeType_Valve_Input, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerInputPipeType_Valve_Input_EURange = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerInputPipeType_Valve_Input_EURange, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerDrumType_LevelIndicator_Output = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerDrumType_LevelIndicator_Output, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerDrumType_LevelIndicator_Output_EURange = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerDrumType_LevelIndicator_Output_EURange, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerOutputPipeType_FlowTransmitter2_Output = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerOutputPipeType_FlowTransmitter2_Output, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerOutputPipeType_FlowTransmitter2_Output_EURange = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerOutputPipeType_FlowTransmitter2_Output_EURange, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId SetPointMask_OptionSetValues = new ExpandedNodeId(Quickstarts.Boiler.Variables.SetPointMask_OptionSetValues, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_InputPipe_FlowTransmitter1_Output = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_InputPipe_FlowTransmitter1_Output, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_InputPipe_FlowTransmitter1_Output_EURange = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_InputPipe_FlowTransmitter1_Output_EURange, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_InputPipe_Valve_Input = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_InputPipe_Valve_Input, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_InputPipe_Valve_Input_EURange = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_InputPipe_Valve_Input_EURange, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_Drum_LevelIndicator_Output = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_Drum_LevelIndicator_Output, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_Drum_LevelIndicator_Output_EURange = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_Drum_LevelIndicator_Output_EURange, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_OutputPipe_FlowTransmitter2_Output = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_OutputPipe_FlowTransmitter2_Output, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_OutputPipe_FlowTransmitter2_Output_EURange = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_OutputPipe_FlowTransmitter2_Output_EURange, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_FlowController_Measurement = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_FlowController_Measurement, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_FlowController_SetPoint = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_FlowController_SetPoint, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_FlowController_ControlOut = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_FlowController_ControlOut, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_LevelController_Measurement = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_LevelController_Measurement, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_LevelController_SetPoint = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_LevelController_SetPoint, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_LevelController_ControlOut = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_LevelController_ControlOut, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_CustomController_Input1 = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_CustomController_Input1, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_CustomController_Input2 = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_CustomController_Input2, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_CustomController_Input3 = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_CustomController_Input3, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_CustomController_ControlOut = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_CustomController_ControlOut, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_ChangeSetPoints_InputArguments = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_ChangeSetPoints_InputArguments, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_ChangeSetPoints_OutputArguments = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_ChangeSetPoints_OutputArguments, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_EmergencyShutdown_InputArguments = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_EmergencyShutdown_InputArguments, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_EmergencyShutdown_OutputArguments = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_EmergencyShutdown_OutputArguments, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_Restart_InputArguments = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_Restart_InputArguments, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_Restart_OutputArguments = new ExpandedNodeId(Quickstarts.Boiler.Variables.BoilerType_Restart_OutputArguments, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_InputPipe_FlowTransmitter1_Output = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_InputPipe_FlowTransmitter1_Output, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_InputPipe_FlowTransmitter1_Output_EURange = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_InputPipe_FlowTransmitter1_Output_EURange, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_InputPipe_Valve_Input = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_InputPipe_Valve_Input, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_InputPipe_Valve_Input_EURange = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_InputPipe_Valve_Input_EURange, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_Drum_LevelIndicator_Output = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_Drum_LevelIndicator_Output, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_Drum_LevelIndicator_Output_EURange = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_Drum_LevelIndicator_Output_EURange, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_OutputPipe_FlowTransmitter2_Output = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_OutputPipe_FlowTransmitter2_Output, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_OutputPipe_FlowTransmitter2_Output_EURange = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_OutputPipe_FlowTransmitter2_Output_EURange, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_FlowController_Measurement = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_FlowController_Measurement, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_FlowController_SetPoint = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_FlowController_SetPoint, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_FlowController_ControlOut = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_FlowController_ControlOut, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_LevelController_Measurement = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_LevelController_Measurement, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_LevelController_SetPoint = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_LevelController_SetPoint, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_LevelController_ControlOut = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_LevelController_ControlOut, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_CustomController_Input1 = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_CustomController_Input1, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_CustomController_Input2 = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_CustomController_Input2, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_CustomController_Input3 = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_CustomController_Input3, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_CustomController_ControlOut = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_CustomController_ControlOut, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_ChangeSetPoints_InputArguments = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_ChangeSetPoints_InputArguments, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_ChangeSetPoints_OutputArguments = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_ChangeSetPoints_OutputArguments, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_EmergencyShutdown_InputArguments = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_EmergencyShutdown_InputArguments, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_EmergencyShutdown_OutputArguments = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_EmergencyShutdown_OutputArguments, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_Restart_InputArguments = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_Restart_InputArguments, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_Restart_OutputArguments = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler1_Restart_OutputArguments, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_BinarySchema = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler_BinarySchema, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_BinarySchema_NamespaceUri = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler_BinarySchema_NamespaceUri, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_BinarySchema_Deprecated = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler_BinarySchema_Deprecated, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_BinarySchema_ControllerDataType = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler_BinarySchema_ControllerDataType, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_BinarySchema_ChangeSetPointsRequest = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler_BinarySchema_ChangeSetPointsRequest, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_BinarySchema_ChangeSetPointsResponse = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler_BinarySchema_ChangeSetPointsResponse, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_XmlSchema = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler_XmlSchema, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_XmlSchema_NamespaceUri = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler_XmlSchema_NamespaceUri, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_XmlSchema_Deprecated = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler_XmlSchema_Deprecated, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_XmlSchema_ControllerDataType = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler_XmlSchema_ControllerDataType, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_XmlSchema_ChangeSetPointsRequest = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler_XmlSchema_ChangeSetPointsRequest, Quickstarts.Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_XmlSchema_ChangeSetPointsResponse = new ExpandedNodeId(Quickstarts.Boiler.Variables.Boiler_XmlSchema_ChangeSetPointsResponse, Quickstarts.Boiler.Namespaces.Boiler);
    }
    #endregion

    #region BrowseName Declarations
    /// <remarks />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class BrowseNames
    {
        /// <remarks />
        public const string Boiler_BinarySchema = "Quickstarts.Boiler";

        /// <remarks />
        public const string Boiler_XmlSchema = "Quickstarts.Boiler";

        /// <remarks />
        public const string Boiler1 = "Boiler #1";

        /// <remarks />
        public const string BoilerDrumType = "BoilerDrumType";

        /// <remarks />
        public const string BoilerInputPipeType = "BoilerInputPipeType";

        /// <remarks />
        public const string BoilerOutputPipeType = "BoilerOutputPipeType";

        /// <remarks />
        public const string BoilerType = "BoilerType";

        /// <remarks />
        public const string ChangeSetPoints = "ChangeSetPoints";

        /// <remarks />
        public const string ChangeSetPointsRequest = "ChangeSetPointsRequest";

        /// <remarks />
        public const string ChangeSetPointsResponse = "ChangeSetPointsResponse";

        /// <remarks />
        public const string ControllerDataType = "ControllerDataType";

        /// <remarks />
        public const string ControlOut = "ControlOut";

        /// <remarks />
        public const string CustomController = "CCX001";

        /// <remarks />
        public const string CustomControllerType = "CustomControllerType";

        /// <remarks />
        public const string Drum = "DrumX001";

        /// <remarks />
        public const string EmergencyShutdown = "EmergencyShutdown";

        /// <remarks />
        public const string FlowController = "FCX001";

        /// <remarks />
        public const string FlowControllerType = "FlowControllerType";

        /// <remarks />
        public const string FlowTo = "FlowTo";

        /// <remarks />
        public const string FlowTransmitter1 = "FTX001";

        /// <remarks />
        public const string FlowTransmitter2 = "FTX002";

        /// <remarks />
        public const string FlowTransmitterType = "FlowTransmitterType";

        /// <remarks />
        public const string GenericActuatorType = "GenericActuatorType";

        /// <remarks />
        public const string GenericControllerType = "GenericControllerType";

        /// <remarks />
        public const string GenericSensorType = "GenericSensorType";

        /// <remarks />
        public const string Input = "Input";

        /// <remarks />
        public const string Input1 = "Input1";

        /// <remarks />
        public const string Input2 = "Input2";

        /// <remarks />
        public const string Input3 = "Input3";

        /// <remarks />
        public const string InputPipe = "PipeX001";

        /// <remarks />
        public const string LevelController = "LCX001";

        /// <remarks />
        public const string LevelControllerType = "LevelControllerType";

        /// <remarks />
        public const string LevelIndicator = "LIX001";

        /// <remarks />
        public const string LevelIndicatorType = "LevelIndicatorType";

        /// <remarks />
        public const string Measurement = "Measurement";

        /// <remarks />
        public const string Output = "Output";

        /// <remarks />
        public const string OutputPipe = "PipeX002";

        /// <remarks />
        public const string Restart = "Restart";

        /// <remarks />
        public const string SetPoint = "SetPoint";

        /// <remarks />
        public const string SetPointMask = "SetPointMask";

        /// <remarks />
        public const string SignalTo = "SignalTo";

        /// <remarks />
        public const string Valve = "ValveX001";

        /// <remarks />
        public const string ValveType = "ValveType";
    }
    #endregion

    #region Namespace Declarations
    /// <remarks />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Namespaces
    {
        /// <summary>
        /// The URI for the OpcUa namespace (.NET code namespace is 'Opc.Ua').
        /// </summary>
        public const string OpcUa = "http://opcfoundation.org/UA/";

        /// <summary>
        /// The URI for the OpcUaXsd namespace (.NET code namespace is 'Opc.Ua').
        /// </summary>
        public const string OpcUaXsd = "http://opcfoundation.org/UA/2008/02/Types.xsd";

        /// <summary>
        /// The URI for the Boiler namespace (.NET code namespace is 'Quickstarts.Boiler').
        /// </summary>
        public const string Boiler = "tag:opcua.org,2023-11:iot-starterkit:boiler";
    }
    #endregion
}
