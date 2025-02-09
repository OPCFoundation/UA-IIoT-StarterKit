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

namespace Boiler
{
    #region DataType Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class DataTypes
    {
        /// <remarks />
        public const uint EnergyConsumptionType = 27;

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
        public const uint EnergyConsumptionType_Encoding_DefaultBinary = 241;

        /// <remarks />
        public const uint ChangeSetPointsRequest_Encoding_DefaultBinary = 222;

        /// <remarks />
        public const uint ChangeSetPointsResponse_Encoding_DefaultBinary = 223;

        /// <remarks />
        public const uint EnergyConsumptionType_Encoding_DefaultXml = 245;

        /// <remarks />
        public const uint ChangeSetPointsRequest_Encoding_DefaultXml = 230;

        /// <remarks />
        public const uint ChangeSetPointsResponse_Encoding_DefaultXml = 231;

        /// <remarks />
        public const uint EnergyConsumptionType_Encoding_DefaultJson = 249;

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
        public const uint GenericSensorType_Output_EngineeringUnits = 13;

        /// <remarks />
        public const uint GenericActuatorType_Input = 15;

        /// <remarks />
        public const uint GenericActuatorType_Input_EURange = 18;

        /// <remarks />
        public const uint GenericActuatorType_EnergyConsumption = 28;

        /// <remarks />
        public const uint GenericActuatorType_EnergyConsumption_EngineeringUnits = 34;

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
        public const uint BoilerInputPipeType_FlowTransmitter1_Output_EngineeringUnits = 80;

        /// <remarks />
        public const uint BoilerInputPipeType_Valve_Input = 82;

        /// <remarks />
        public const uint BoilerInputPipeType_Valve_Input_EURange = 85;

        /// <remarks />
        public const uint BoilerInputPipeType_Valve_EnergyConsumption = 35;

        /// <remarks />
        public const uint BoilerInputPipeType_Valve_EnergyConsumption_EngineeringUnits = 42;

        /// <remarks />
        public const uint BoilerDrumType_LevelIndicator_Output = 90;

        /// <remarks />
        public const uint BoilerDrumType_LevelIndicator_Output_EURange = 93;

        /// <remarks />
        public const uint BoilerDrumType_LevelIndicator_Output_EngineeringUnits = 95;

        /// <remarks />
        public const uint BoilerOutputPipeType_FlowTransmitter2_Output = 98;

        /// <remarks />
        public const uint BoilerOutputPipeType_FlowTransmitter2_Output_EURange = 101;

        /// <remarks />
        public const uint BoilerOutputPipeType_FlowTransmitter2_Output_EngineeringUnits = 103;

        /// <remarks />
        public const uint SetPointMask_OptionSetValues = 219;

        /// <remarks />
        public const uint BoilerType_InputPipe_FlowTransmitter1_Output = 105;

        /// <remarks />
        public const uint BoilerType_InputPipe_FlowTransmitter1_Output_EURange = 108;

        /// <remarks />
        public const uint BoilerType_InputPipe_FlowTransmitter1_Output_EngineeringUnits = 110;

        /// <remarks />
        public const uint BoilerType_InputPipe_Valve_Input = 112;

        /// <remarks />
        public const uint BoilerType_InputPipe_Valve_Input_EURange = 115;

        /// <remarks />
        public const uint BoilerType_InputPipe_Valve_EnergyConsumption = 43;

        /// <remarks />
        public const uint BoilerType_InputPipe_Valve_EnergyConsumption_EngineeringUnits = 49;

        /// <remarks />
        public const uint BoilerType_Drum_LevelIndicator_Output = 59;

        /// <remarks />
        public const uint BoilerType_Drum_LevelIndicator_Output_EURange = 62;

        /// <remarks />
        public const uint BoilerType_Drum_LevelIndicator_Output_EngineeringUnits = 64;

        /// <remarks />
        public const uint BoilerType_OutputPipe_FlowTransmitter2_Output = 119;

        /// <remarks />
        public const uint BoilerType_OutputPipe_FlowTransmitter2_Output_EURange = 122;

        /// <remarks />
        public const uint BoilerType_OutputPipe_FlowTransmitter2_Output_EngineeringUnits = 124;

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
        public const uint Boiler1_InputPipe_FlowTransmitter1_Output_EngineeringUnits = 146;

        /// <remarks />
        public const uint Boiler1_InputPipe_Valve_Input = 148;

        /// <remarks />
        public const uint Boiler1_InputPipe_Valve_Input_EURange = 151;

        /// <remarks />
        public const uint Boiler1_InputPipe_Valve_EnergyConsumption = 50;

        /// <remarks />
        public const uint Boiler1_InputPipe_Valve_EnergyConsumption_EngineeringUnits = 240;

        /// <remarks />
        public const uint Boiler1_Drum_LevelIndicator_Output = 156;

        /// <remarks />
        public const uint Boiler1_Drum_LevelIndicator_Output_EURange = 159;

        /// <remarks />
        public const uint Boiler1_Drum_LevelIndicator_Output_EngineeringUnits = 161;

        /// <remarks />
        public const uint Boiler1_OutputPipe_FlowTransmitter2_Output = 164;

        /// <remarks />
        public const uint Boiler1_OutputPipe_FlowTransmitter2_Output_EURange = 167;

        /// <remarks />
        public const uint Boiler1_OutputPipe_FlowTransmitter2_Output_EngineeringUnits = 169;

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
        public const uint Boiler_BinarySchema_EnergyConsumptionType = 242;

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
        public const uint Boiler_XmlSchema_EnergyConsumptionType = 246;

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
        public static readonly ExpandedNodeId EnergyConsumptionType = new ExpandedNodeId(Boiler.DataTypes.EnergyConsumptionType, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId SetPointMask = new ExpandedNodeId(Boiler.DataTypes.SetPointMask, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ChangeSetPointsRequest = new ExpandedNodeId(Boiler.DataTypes.ChangeSetPointsRequest, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ChangeSetPointsResponse = new ExpandedNodeId(Boiler.DataTypes.ChangeSetPointsResponse, Boiler.Namespaces.Boiler);
    }
    #endregion

    #region Method Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class MethodIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_ChangeSetPoints = new ExpandedNodeId(Boiler.Methods.BoilerType_ChangeSetPoints, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_EmergencyShutdown = new ExpandedNodeId(Boiler.Methods.BoilerType_EmergencyShutdown, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_Restart = new ExpandedNodeId(Boiler.Methods.BoilerType_Restart, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_ChangeSetPoints = new ExpandedNodeId(Boiler.Methods.Boiler1_ChangeSetPoints, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_EmergencyShutdown = new ExpandedNodeId(Boiler.Methods.Boiler1_EmergencyShutdown, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_Restart = new ExpandedNodeId(Boiler.Methods.Boiler1_Restart, Boiler.Namespaces.Boiler);
    }
    #endregion

    #region Object Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ObjectIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId BoilerInputPipeType_FlowTransmitter1 = new ExpandedNodeId(Boiler.Objects.BoilerInputPipeType_FlowTransmitter1, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerInputPipeType_Valve = new ExpandedNodeId(Boiler.Objects.BoilerInputPipeType_Valve, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerDrumType_LevelIndicator = new ExpandedNodeId(Boiler.Objects.BoilerDrumType_LevelIndicator, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerOutputPipeType_FlowTransmitter2 = new ExpandedNodeId(Boiler.Objects.BoilerOutputPipeType_FlowTransmitter2, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_InputPipe = new ExpandedNodeId(Boiler.Objects.BoilerType_InputPipe, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_InputPipe_FlowTransmitter1 = new ExpandedNodeId(Boiler.Objects.BoilerType_InputPipe_FlowTransmitter1, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_InputPipe_Valve = new ExpandedNodeId(Boiler.Objects.BoilerType_InputPipe_Valve, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_Drum = new ExpandedNodeId(Boiler.Objects.BoilerType_Drum, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_Drum_LevelIndicator = new ExpandedNodeId(Boiler.Objects.BoilerType_Drum_LevelIndicator, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_OutputPipe = new ExpandedNodeId(Boiler.Objects.BoilerType_OutputPipe, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_OutputPipe_FlowTransmitter2 = new ExpandedNodeId(Boiler.Objects.BoilerType_OutputPipe_FlowTransmitter2, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_FlowController = new ExpandedNodeId(Boiler.Objects.BoilerType_FlowController, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_LevelController = new ExpandedNodeId(Boiler.Objects.BoilerType_LevelController, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_CustomController = new ExpandedNodeId(Boiler.Objects.BoilerType_CustomController, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1 = new ExpandedNodeId(Boiler.Objects.Boiler1, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_InputPipe = new ExpandedNodeId(Boiler.Objects.Boiler1_InputPipe, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_InputPipe_FlowTransmitter1 = new ExpandedNodeId(Boiler.Objects.Boiler1_InputPipe_FlowTransmitter1, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_InputPipe_Valve = new ExpandedNodeId(Boiler.Objects.Boiler1_InputPipe_Valve, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_Drum = new ExpandedNodeId(Boiler.Objects.Boiler1_Drum, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_Drum_LevelIndicator = new ExpandedNodeId(Boiler.Objects.Boiler1_Drum_LevelIndicator, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_OutputPipe = new ExpandedNodeId(Boiler.Objects.Boiler1_OutputPipe, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_OutputPipe_FlowTransmitter2 = new ExpandedNodeId(Boiler.Objects.Boiler1_OutputPipe_FlowTransmitter2, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_FlowController = new ExpandedNodeId(Boiler.Objects.Boiler1_FlowController, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_LevelController = new ExpandedNodeId(Boiler.Objects.Boiler1_LevelController, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_CustomController = new ExpandedNodeId(Boiler.Objects.Boiler1_CustomController, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId EnergyConsumptionType_Encoding_DefaultBinary = new ExpandedNodeId(Boiler.Objects.EnergyConsumptionType_Encoding_DefaultBinary, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ChangeSetPointsRequest_Encoding_DefaultBinary = new ExpandedNodeId(Boiler.Objects.ChangeSetPointsRequest_Encoding_DefaultBinary, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ChangeSetPointsResponse_Encoding_DefaultBinary = new ExpandedNodeId(Boiler.Objects.ChangeSetPointsResponse_Encoding_DefaultBinary, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId EnergyConsumptionType_Encoding_DefaultXml = new ExpandedNodeId(Boiler.Objects.EnergyConsumptionType_Encoding_DefaultXml, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ChangeSetPointsRequest_Encoding_DefaultXml = new ExpandedNodeId(Boiler.Objects.ChangeSetPointsRequest_Encoding_DefaultXml, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ChangeSetPointsResponse_Encoding_DefaultXml = new ExpandedNodeId(Boiler.Objects.ChangeSetPointsResponse_Encoding_DefaultXml, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId EnergyConsumptionType_Encoding_DefaultJson = new ExpandedNodeId(Boiler.Objects.EnergyConsumptionType_Encoding_DefaultJson, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ChangeSetPointsRequest_Encoding_DefaultJson = new ExpandedNodeId(Boiler.Objects.ChangeSetPointsRequest_Encoding_DefaultJson, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ChangeSetPointsResponse_Encoding_DefaultJson = new ExpandedNodeId(Boiler.Objects.ChangeSetPointsResponse_Encoding_DefaultJson, Boiler.Namespaces.Boiler);
    }
    #endregion

    #region ObjectType Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ObjectTypeIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId GenericControllerType = new ExpandedNodeId(Boiler.ObjectTypes.GenericControllerType, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId GenericSensorType = new ExpandedNodeId(Boiler.ObjectTypes.GenericSensorType, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId GenericActuatorType = new ExpandedNodeId(Boiler.ObjectTypes.GenericActuatorType, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId CustomControllerType = new ExpandedNodeId(Boiler.ObjectTypes.CustomControllerType, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId ValveType = new ExpandedNodeId(Boiler.ObjectTypes.ValveType, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId LevelControllerType = new ExpandedNodeId(Boiler.ObjectTypes.LevelControllerType, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId FlowControllerType = new ExpandedNodeId(Boiler.ObjectTypes.FlowControllerType, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId LevelIndicatorType = new ExpandedNodeId(Boiler.ObjectTypes.LevelIndicatorType, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId FlowTransmitterType = new ExpandedNodeId(Boiler.ObjectTypes.FlowTransmitterType, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerInputPipeType = new ExpandedNodeId(Boiler.ObjectTypes.BoilerInputPipeType, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerDrumType = new ExpandedNodeId(Boiler.ObjectTypes.BoilerDrumType, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerOutputPipeType = new ExpandedNodeId(Boiler.ObjectTypes.BoilerOutputPipeType, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType = new ExpandedNodeId(Boiler.ObjectTypes.BoilerType, Boiler.Namespaces.Boiler);
    }
    #endregion

    #region ReferenceType Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ReferenceTypeIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId FlowTo = new ExpandedNodeId(Boiler.ReferenceTypes.FlowTo, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId SignalTo = new ExpandedNodeId(Boiler.ReferenceTypes.SignalTo, Boiler.Namespaces.Boiler);
    }
    #endregion

    #region Variable Node Identifiers
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class VariableIds
    {
        /// <remarks />
        public static readonly ExpandedNodeId GenericControllerType_Measurement = new ExpandedNodeId(Boiler.Variables.GenericControllerType_Measurement, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId GenericControllerType_SetPoint = new ExpandedNodeId(Boiler.Variables.GenericControllerType_SetPoint, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId GenericControllerType_ControlOut = new ExpandedNodeId(Boiler.Variables.GenericControllerType_ControlOut, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId GenericSensorType_Output = new ExpandedNodeId(Boiler.Variables.GenericSensorType_Output, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId GenericSensorType_Output_EURange = new ExpandedNodeId(Boiler.Variables.GenericSensorType_Output_EURange, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId GenericSensorType_Output_EngineeringUnits = new ExpandedNodeId(Boiler.Variables.GenericSensorType_Output_EngineeringUnits, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId GenericActuatorType_Input = new ExpandedNodeId(Boiler.Variables.GenericActuatorType_Input, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId GenericActuatorType_Input_EURange = new ExpandedNodeId(Boiler.Variables.GenericActuatorType_Input_EURange, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId GenericActuatorType_EnergyConsumption = new ExpandedNodeId(Boiler.Variables.GenericActuatorType_EnergyConsumption, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId GenericActuatorType_EnergyConsumption_EngineeringUnits = new ExpandedNodeId(Boiler.Variables.GenericActuatorType_EnergyConsumption_EngineeringUnits, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId CustomControllerType_Input1 = new ExpandedNodeId(Boiler.Variables.CustomControllerType_Input1, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId CustomControllerType_Input2 = new ExpandedNodeId(Boiler.Variables.CustomControllerType_Input2, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId CustomControllerType_Input3 = new ExpandedNodeId(Boiler.Variables.CustomControllerType_Input3, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId CustomControllerType_ControlOut = new ExpandedNodeId(Boiler.Variables.CustomControllerType_ControlOut, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerInputPipeType_FlowTransmitter1_Output = new ExpandedNodeId(Boiler.Variables.BoilerInputPipeType_FlowTransmitter1_Output, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerInputPipeType_FlowTransmitter1_Output_EURange = new ExpandedNodeId(Boiler.Variables.BoilerInputPipeType_FlowTransmitter1_Output_EURange, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerInputPipeType_FlowTransmitter1_Output_EngineeringUnits = new ExpandedNodeId(Boiler.Variables.BoilerInputPipeType_FlowTransmitter1_Output_EngineeringUnits, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerInputPipeType_Valve_Input = new ExpandedNodeId(Boiler.Variables.BoilerInputPipeType_Valve_Input, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerInputPipeType_Valve_Input_EURange = new ExpandedNodeId(Boiler.Variables.BoilerInputPipeType_Valve_Input_EURange, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerInputPipeType_Valve_EnergyConsumption = new ExpandedNodeId(Boiler.Variables.BoilerInputPipeType_Valve_EnergyConsumption, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerInputPipeType_Valve_EnergyConsumption_EngineeringUnits = new ExpandedNodeId(Boiler.Variables.BoilerInputPipeType_Valve_EnergyConsumption_EngineeringUnits, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerDrumType_LevelIndicator_Output = new ExpandedNodeId(Boiler.Variables.BoilerDrumType_LevelIndicator_Output, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerDrumType_LevelIndicator_Output_EURange = new ExpandedNodeId(Boiler.Variables.BoilerDrumType_LevelIndicator_Output_EURange, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerDrumType_LevelIndicator_Output_EngineeringUnits = new ExpandedNodeId(Boiler.Variables.BoilerDrumType_LevelIndicator_Output_EngineeringUnits, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerOutputPipeType_FlowTransmitter2_Output = new ExpandedNodeId(Boiler.Variables.BoilerOutputPipeType_FlowTransmitter2_Output, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerOutputPipeType_FlowTransmitter2_Output_EURange = new ExpandedNodeId(Boiler.Variables.BoilerOutputPipeType_FlowTransmitter2_Output_EURange, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerOutputPipeType_FlowTransmitter2_Output_EngineeringUnits = new ExpandedNodeId(Boiler.Variables.BoilerOutputPipeType_FlowTransmitter2_Output_EngineeringUnits, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId SetPointMask_OptionSetValues = new ExpandedNodeId(Boiler.Variables.SetPointMask_OptionSetValues, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_InputPipe_FlowTransmitter1_Output = new ExpandedNodeId(Boiler.Variables.BoilerType_InputPipe_FlowTransmitter1_Output, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_InputPipe_FlowTransmitter1_Output_EURange = new ExpandedNodeId(Boiler.Variables.BoilerType_InputPipe_FlowTransmitter1_Output_EURange, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_InputPipe_FlowTransmitter1_Output_EngineeringUnits = new ExpandedNodeId(Boiler.Variables.BoilerType_InputPipe_FlowTransmitter1_Output_EngineeringUnits, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_InputPipe_Valve_Input = new ExpandedNodeId(Boiler.Variables.BoilerType_InputPipe_Valve_Input, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_InputPipe_Valve_Input_EURange = new ExpandedNodeId(Boiler.Variables.BoilerType_InputPipe_Valve_Input_EURange, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_InputPipe_Valve_EnergyConsumption = new ExpandedNodeId(Boiler.Variables.BoilerType_InputPipe_Valve_EnergyConsumption, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_InputPipe_Valve_EnergyConsumption_EngineeringUnits = new ExpandedNodeId(Boiler.Variables.BoilerType_InputPipe_Valve_EnergyConsumption_EngineeringUnits, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_Drum_LevelIndicator_Output = new ExpandedNodeId(Boiler.Variables.BoilerType_Drum_LevelIndicator_Output, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_Drum_LevelIndicator_Output_EURange = new ExpandedNodeId(Boiler.Variables.BoilerType_Drum_LevelIndicator_Output_EURange, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_Drum_LevelIndicator_Output_EngineeringUnits = new ExpandedNodeId(Boiler.Variables.BoilerType_Drum_LevelIndicator_Output_EngineeringUnits, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_OutputPipe_FlowTransmitter2_Output = new ExpandedNodeId(Boiler.Variables.BoilerType_OutputPipe_FlowTransmitter2_Output, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_OutputPipe_FlowTransmitter2_Output_EURange = new ExpandedNodeId(Boiler.Variables.BoilerType_OutputPipe_FlowTransmitter2_Output_EURange, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_OutputPipe_FlowTransmitter2_Output_EngineeringUnits = new ExpandedNodeId(Boiler.Variables.BoilerType_OutputPipe_FlowTransmitter2_Output_EngineeringUnits, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_FlowController_Measurement = new ExpandedNodeId(Boiler.Variables.BoilerType_FlowController_Measurement, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_FlowController_SetPoint = new ExpandedNodeId(Boiler.Variables.BoilerType_FlowController_SetPoint, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_FlowController_ControlOut = new ExpandedNodeId(Boiler.Variables.BoilerType_FlowController_ControlOut, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_LevelController_Measurement = new ExpandedNodeId(Boiler.Variables.BoilerType_LevelController_Measurement, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_LevelController_SetPoint = new ExpandedNodeId(Boiler.Variables.BoilerType_LevelController_SetPoint, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_LevelController_ControlOut = new ExpandedNodeId(Boiler.Variables.BoilerType_LevelController_ControlOut, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_CustomController_Input1 = new ExpandedNodeId(Boiler.Variables.BoilerType_CustomController_Input1, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_CustomController_Input2 = new ExpandedNodeId(Boiler.Variables.BoilerType_CustomController_Input2, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_CustomController_Input3 = new ExpandedNodeId(Boiler.Variables.BoilerType_CustomController_Input3, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_CustomController_ControlOut = new ExpandedNodeId(Boiler.Variables.BoilerType_CustomController_ControlOut, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_ChangeSetPoints_InputArguments = new ExpandedNodeId(Boiler.Variables.BoilerType_ChangeSetPoints_InputArguments, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_ChangeSetPoints_OutputArguments = new ExpandedNodeId(Boiler.Variables.BoilerType_ChangeSetPoints_OutputArguments, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_EmergencyShutdown_InputArguments = new ExpandedNodeId(Boiler.Variables.BoilerType_EmergencyShutdown_InputArguments, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_EmergencyShutdown_OutputArguments = new ExpandedNodeId(Boiler.Variables.BoilerType_EmergencyShutdown_OutputArguments, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_Restart_InputArguments = new ExpandedNodeId(Boiler.Variables.BoilerType_Restart_InputArguments, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId BoilerType_Restart_OutputArguments = new ExpandedNodeId(Boiler.Variables.BoilerType_Restart_OutputArguments, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_InputPipe_FlowTransmitter1_Output = new ExpandedNodeId(Boiler.Variables.Boiler1_InputPipe_FlowTransmitter1_Output, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_InputPipe_FlowTransmitter1_Output_EURange = new ExpandedNodeId(Boiler.Variables.Boiler1_InputPipe_FlowTransmitter1_Output_EURange, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_InputPipe_FlowTransmitter1_Output_EngineeringUnits = new ExpandedNodeId(Boiler.Variables.Boiler1_InputPipe_FlowTransmitter1_Output_EngineeringUnits, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_InputPipe_Valve_Input = new ExpandedNodeId(Boiler.Variables.Boiler1_InputPipe_Valve_Input, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_InputPipe_Valve_Input_EURange = new ExpandedNodeId(Boiler.Variables.Boiler1_InputPipe_Valve_Input_EURange, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_InputPipe_Valve_EnergyConsumption = new ExpandedNodeId(Boiler.Variables.Boiler1_InputPipe_Valve_EnergyConsumption, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_InputPipe_Valve_EnergyConsumption_EngineeringUnits = new ExpandedNodeId(Boiler.Variables.Boiler1_InputPipe_Valve_EnergyConsumption_EngineeringUnits, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_Drum_LevelIndicator_Output = new ExpandedNodeId(Boiler.Variables.Boiler1_Drum_LevelIndicator_Output, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_Drum_LevelIndicator_Output_EURange = new ExpandedNodeId(Boiler.Variables.Boiler1_Drum_LevelIndicator_Output_EURange, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_Drum_LevelIndicator_Output_EngineeringUnits = new ExpandedNodeId(Boiler.Variables.Boiler1_Drum_LevelIndicator_Output_EngineeringUnits, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_OutputPipe_FlowTransmitter2_Output = new ExpandedNodeId(Boiler.Variables.Boiler1_OutputPipe_FlowTransmitter2_Output, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_OutputPipe_FlowTransmitter2_Output_EURange = new ExpandedNodeId(Boiler.Variables.Boiler1_OutputPipe_FlowTransmitter2_Output_EURange, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_OutputPipe_FlowTransmitter2_Output_EngineeringUnits = new ExpandedNodeId(Boiler.Variables.Boiler1_OutputPipe_FlowTransmitter2_Output_EngineeringUnits, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_FlowController_Measurement = new ExpandedNodeId(Boiler.Variables.Boiler1_FlowController_Measurement, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_FlowController_SetPoint = new ExpandedNodeId(Boiler.Variables.Boiler1_FlowController_SetPoint, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_FlowController_ControlOut = new ExpandedNodeId(Boiler.Variables.Boiler1_FlowController_ControlOut, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_LevelController_Measurement = new ExpandedNodeId(Boiler.Variables.Boiler1_LevelController_Measurement, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_LevelController_SetPoint = new ExpandedNodeId(Boiler.Variables.Boiler1_LevelController_SetPoint, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_LevelController_ControlOut = new ExpandedNodeId(Boiler.Variables.Boiler1_LevelController_ControlOut, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_CustomController_Input1 = new ExpandedNodeId(Boiler.Variables.Boiler1_CustomController_Input1, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_CustomController_Input2 = new ExpandedNodeId(Boiler.Variables.Boiler1_CustomController_Input2, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_CustomController_Input3 = new ExpandedNodeId(Boiler.Variables.Boiler1_CustomController_Input3, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_CustomController_ControlOut = new ExpandedNodeId(Boiler.Variables.Boiler1_CustomController_ControlOut, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_ChangeSetPoints_InputArguments = new ExpandedNodeId(Boiler.Variables.Boiler1_ChangeSetPoints_InputArguments, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_ChangeSetPoints_OutputArguments = new ExpandedNodeId(Boiler.Variables.Boiler1_ChangeSetPoints_OutputArguments, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_EmergencyShutdown_InputArguments = new ExpandedNodeId(Boiler.Variables.Boiler1_EmergencyShutdown_InputArguments, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_EmergencyShutdown_OutputArguments = new ExpandedNodeId(Boiler.Variables.Boiler1_EmergencyShutdown_OutputArguments, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_Restart_InputArguments = new ExpandedNodeId(Boiler.Variables.Boiler1_Restart_InputArguments, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler1_Restart_OutputArguments = new ExpandedNodeId(Boiler.Variables.Boiler1_Restart_OutputArguments, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_BinarySchema = new ExpandedNodeId(Boiler.Variables.Boiler_BinarySchema, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_BinarySchema_NamespaceUri = new ExpandedNodeId(Boiler.Variables.Boiler_BinarySchema_NamespaceUri, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_BinarySchema_Deprecated = new ExpandedNodeId(Boiler.Variables.Boiler_BinarySchema_Deprecated, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_BinarySchema_EnergyConsumptionType = new ExpandedNodeId(Boiler.Variables.Boiler_BinarySchema_EnergyConsumptionType, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_BinarySchema_ChangeSetPointsRequest = new ExpandedNodeId(Boiler.Variables.Boiler_BinarySchema_ChangeSetPointsRequest, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_BinarySchema_ChangeSetPointsResponse = new ExpandedNodeId(Boiler.Variables.Boiler_BinarySchema_ChangeSetPointsResponse, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_XmlSchema = new ExpandedNodeId(Boiler.Variables.Boiler_XmlSchema, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_XmlSchema_NamespaceUri = new ExpandedNodeId(Boiler.Variables.Boiler_XmlSchema_NamespaceUri, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_XmlSchema_Deprecated = new ExpandedNodeId(Boiler.Variables.Boiler_XmlSchema_Deprecated, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_XmlSchema_EnergyConsumptionType = new ExpandedNodeId(Boiler.Variables.Boiler_XmlSchema_EnergyConsumptionType, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_XmlSchema_ChangeSetPointsRequest = new ExpandedNodeId(Boiler.Variables.Boiler_XmlSchema_ChangeSetPointsRequest, Boiler.Namespaces.Boiler);

        /// <remarks />
        public static readonly ExpandedNodeId Boiler_XmlSchema_ChangeSetPointsResponse = new ExpandedNodeId(Boiler.Variables.Boiler_XmlSchema_ChangeSetPointsResponse, Boiler.Namespaces.Boiler);
    }
    #endregion

    #region BrowseName Declarations
    /// <remarks />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class BrowseNames
    {
        /// <remarks />
        public const string Boiler_BinarySchema = "Boiler";

        /// <remarks />
        public const string Boiler_XmlSchema = "Boiler";

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
        public const string EnergyConsumption = "EnergyConsumption";

        /// <remarks />
        public const string EnergyConsumptionType = "EnergyConsumptionType";

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
        /// The URI for the Boiler namespace (.NET code namespace is 'Boiler').
        /// </summary>
        public const string Boiler = "urn:opcua.org:2025-01:iiot-starterkit:boiler";
    }
    #endregion
}