namespace Boiler.WebApi
{
    /// <summary>
    /// The namespaces used in the model.
    /// </summary>
    public static class Namespaces
    {
        /// <remarks />
        public const string Uri = "urn:opcua.org:2025-01:iiot-starterkit:boiler";
    }

    /// <summary>
    /// The browse names defined in the model.
    /// </summary>
    public static class BrowseNames
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

    /// <summary>
    /// The well known identifiers for DataType nodes.
    /// </summary>
    public static class DataTypeIds {
        /// <remarks />
        public const string EnergyConsumptionType = "nsu=" + Namespaces.Uri + ";i=27";
        /// <remarks />
        public const string SetPointMask = "nsu=" + Namespaces.Uri + ";i=218";
        /// <remarks />
        public const string ChangeSetPointsRequest = "nsu=" + Namespaces.Uri + ";i=220";
        /// <remarks />
        public const string ChangeSetPointsResponse = "nsu=" + Namespaces.Uri + ";i=221";

        /// <summary>
        /// Converts a value to a name for display.
        /// </summary>
        public static string ToName(string value)
        {
            foreach (var field in typeof(DataTypeIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.GetValue(null).Equals(value))
                {
                    return field.Name;
                }
            }

            return value.ToString();
        }
    }

    /// <summary>
    /// The well known identifiers for Method nodes.
    /// </summary>
    public static class MethodIds {
        /// <remarks />
        public const string BoilerType_ChangeSetPoints = "nsu=" + Namespaces.Uri + ";i=200";
        /// <remarks />
        public const string BoilerType_EmergencyShutdown = "nsu=" + Namespaces.Uri + ";i=203";
        /// <remarks />
        public const string BoilerType_Restart = "nsu=" + Namespaces.Uri + ";i=206";
        /// <remarks />
        public const string Boiler1_ChangeSetPoints = "nsu=" + Namespaces.Uri + ";i=209";
        /// <remarks />
        public const string Boiler1_EmergencyShutdown = "nsu=" + Namespaces.Uri + ";i=212";
        /// <remarks />
        public const string Boiler1_Restart = "nsu=" + Namespaces.Uri + ";i=215";

        /// <summary>
        /// Converts a value to a name for display.
        /// </summary>
        public static string ToName(string value)
        {
            foreach (var field in typeof(MethodIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.GetValue(null).Equals(value))
                {
                    return field.Name;
                }
            }

            return value.ToString();
        }
    }

    /// <summary>
    /// The well known identifiers for Object nodes.
    /// </summary>
    public static class ObjectIds {
        /// <remarks />
        public const string BoilerInputPipeType_FlowTransmitter1 = "nsu=" + Namespaces.Uri + ";i=74";
        /// <remarks />
        public const string BoilerInputPipeType_Valve = "nsu=" + Namespaces.Uri + ";i=81";
        /// <remarks />
        public const string BoilerDrumType_LevelIndicator = "nsu=" + Namespaces.Uri + ";i=89";
        /// <remarks />
        public const string BoilerOutputPipeType_FlowTransmitter2 = "nsu=" + Namespaces.Uri + ";i=97";
        /// <remarks />
        public const string BoilerType_InputPipe = "nsu=" + Namespaces.Uri + ";i=56";
        /// <remarks />
        public const string BoilerType_InputPipe_FlowTransmitter1 = "nsu=" + Namespaces.Uri + ";i=104";
        /// <remarks />
        public const string BoilerType_InputPipe_Valve = "nsu=" + Namespaces.Uri + ";i=111";
        /// <remarks />
        public const string BoilerType_Drum = "nsu=" + Namespaces.Uri + ";i=57";
        /// <remarks />
        public const string BoilerType_Drum_LevelIndicator = "nsu=" + Namespaces.Uri + ";i=58";
        /// <remarks />
        public const string BoilerType_OutputPipe = "nsu=" + Namespaces.Uri + ";i=65";
        /// <remarks />
        public const string BoilerType_OutputPipe_FlowTransmitter2 = "nsu=" + Namespaces.Uri + ";i=118";
        /// <remarks />
        public const string BoilerType_FlowController = "nsu=" + Namespaces.Uri + ";i=125";
        /// <remarks />
        public const string BoilerType_LevelController = "nsu=" + Namespaces.Uri + ";i=129";
        /// <remarks />
        public const string BoilerType_CustomController = "nsu=" + Namespaces.Uri + ";i=133";
        /// <remarks />
        public const string Boiler1 = "nsu=" + Namespaces.Uri + ";i=138";
        /// <remarks />
        public const string Boiler1_InputPipe = "nsu=" + Namespaces.Uri + ";i=139";
        /// <remarks />
        public const string Boiler1_InputPipe_FlowTransmitter1 = "nsu=" + Namespaces.Uri + ";i=140";
        /// <remarks />
        public const string Boiler1_InputPipe_Valve = "nsu=" + Namespaces.Uri + ";i=147";
        /// <remarks />
        public const string Boiler1_Drum = "nsu=" + Namespaces.Uri + ";i=154";
        /// <remarks />
        public const string Boiler1_Drum_LevelIndicator = "nsu=" + Namespaces.Uri + ";i=155";
        /// <remarks />
        public const string Boiler1_OutputPipe = "nsu=" + Namespaces.Uri + ";i=162";
        /// <remarks />
        public const string Boiler1_OutputPipe_FlowTransmitter2 = "nsu=" + Namespaces.Uri + ";i=163";
        /// <remarks />
        public const string Boiler1_FlowController = "nsu=" + Namespaces.Uri + ";i=170";
        /// <remarks />
        public const string Boiler1_LevelController = "nsu=" + Namespaces.Uri + ";i=174";
        /// <remarks />
        public const string Boiler1_CustomController = "nsu=" + Namespaces.Uri + ";i=178";
        /// <remarks />
        public const string EnergyConsumptionType_Encoding_DefaultBinary = "nsu=" + Namespaces.Uri + ";i=241";
        /// <remarks />
        public const string ChangeSetPointsRequest_Encoding_DefaultBinary = "nsu=" + Namespaces.Uri + ";i=222";
        /// <remarks />
        public const string ChangeSetPointsResponse_Encoding_DefaultBinary = "nsu=" + Namespaces.Uri + ";i=223";
        /// <remarks />
        public const string EnergyConsumptionType_Encoding_DefaultXml = "nsu=" + Namespaces.Uri + ";i=245";
        /// <remarks />
        public const string ChangeSetPointsRequest_Encoding_DefaultXml = "nsu=" + Namespaces.Uri + ";i=230";
        /// <remarks />
        public const string ChangeSetPointsResponse_Encoding_DefaultXml = "nsu=" + Namespaces.Uri + ";i=231";
        /// <remarks />
        public const string EnergyConsumptionType_Encoding_DefaultJson = "nsu=" + Namespaces.Uri + ";i=249";
        /// <remarks />
        public const string ChangeSetPointsRequest_Encoding_DefaultJson = "nsu=" + Namespaces.Uri + ";i=238";
        /// <remarks />
        public const string ChangeSetPointsResponse_Encoding_DefaultJson = "nsu=" + Namespaces.Uri + ";i=239";

        /// <summary>
        /// Converts a value to a name for display.
        /// </summary>
        public static string ToName(string value)
        {
            foreach (var field in typeof(ObjectIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.GetValue(null).Equals(value))
                {
                    return field.Name;
                }
            }

            return value.ToString();
        }
    }

    /// <summary>
    /// The well known identifiers for ObjectType nodes.
    /// </summary>
    public static class ObjectTypeIds {
        /// <remarks />
        public const string GenericControllerType = "nsu=" + Namespaces.Uri + ";i=3";
        /// <remarks />
        public const string GenericSensorType = "nsu=" + Namespaces.Uri + ";i=7";
        /// <remarks />
        public const string GenericActuatorType = "nsu=" + Namespaces.Uri + ";i=14";
        /// <remarks />
        public const string CustomControllerType = "nsu=" + Namespaces.Uri + ";i=21";
        /// <remarks />
        public const string ValveType = "nsu=" + Namespaces.Uri + ";i=26";
        /// <remarks />
        public const string LevelControllerType = "nsu=" + Namespaces.Uri + ";i=33";
        /// <remarks />
        public const string FlowControllerType = "nsu=" + Namespaces.Uri + ";i=37";
        /// <remarks />
        public const string LevelIndicatorType = "nsu=" + Namespaces.Uri + ";i=41";
        /// <remarks />
        public const string FlowTransmitterType = "nsu=" + Namespaces.Uri + ";i=48";
        /// <remarks />
        public const string BoilerInputPipeType = "nsu=" + Namespaces.Uri + ";i=73";
        /// <remarks />
        public const string BoilerDrumType = "nsu=" + Namespaces.Uri + ";i=88";
        /// <remarks />
        public const string BoilerOutputPipeType = "nsu=" + Namespaces.Uri + ";i=96";
        /// <remarks />
        public const string BoilerType = "nsu=" + Namespaces.Uri + ";i=55";

        /// <summary>
        /// Converts a value to a name for display.
        /// </summary>
        public static string ToName(string value)
        {
            foreach (var field in typeof(ObjectTypeIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.GetValue(null).Equals(value))
                {
                    return field.Name;
                }
            }

            return value.ToString();
        }
    }

    /// <summary>
    /// The well known identifiers for ReferenceType nodes.
    /// </summary>
    public static class ReferenceTypeIds {
        /// <remarks />
        public const string FlowTo = "nsu=" + Namespaces.Uri + ";i=1";
        /// <remarks />
        public const string SignalTo = "nsu=" + Namespaces.Uri + ";i=2";

        /// <summary>
        /// Converts a value to a name for display.
        /// </summary>
        public static string ToName(string value)
        {
            foreach (var field in typeof(ReferenceTypeIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.GetValue(null).Equals(value))
                {
                    return field.Name;
                }
            }

            return value.ToString();
        }
    }

    /// <summary>
    /// The well known identifiers for Variable nodes.
    /// </summary>
    public static class VariableIds {
        /// <remarks />
        public const string GenericControllerType_Measurement = "nsu=" + Namespaces.Uri + ";i=4";
        /// <remarks />
        public const string GenericControllerType_SetPoint = "nsu=" + Namespaces.Uri + ";i=5";
        /// <remarks />
        public const string GenericControllerType_ControlOut = "nsu=" + Namespaces.Uri + ";i=6";
        /// <remarks />
        public const string GenericSensorType_Output = "nsu=" + Namespaces.Uri + ";i=8";
        /// <remarks />
        public const string GenericSensorType_Output_EURange = "nsu=" + Namespaces.Uri + ";i=11";
        /// <remarks />
        public const string GenericSensorType_Output_EngineeringUnits = "nsu=" + Namespaces.Uri + ";i=13";
        /// <remarks />
        public const string GenericActuatorType_Input = "nsu=" + Namespaces.Uri + ";i=15";
        /// <remarks />
        public const string GenericActuatorType_Input_EURange = "nsu=" + Namespaces.Uri + ";i=18";
        /// <remarks />
        public const string GenericActuatorType_EnergyConsumption = "nsu=" + Namespaces.Uri + ";i=28";
        /// <remarks />
        public const string GenericActuatorType_EnergyConsumption_EngineeringUnits = "nsu=" + Namespaces.Uri + ";i=34";
        /// <remarks />
        public const string CustomControllerType_Input1 = "nsu=" + Namespaces.Uri + ";i=22";
        /// <remarks />
        public const string CustomControllerType_Input2 = "nsu=" + Namespaces.Uri + ";i=23";
        /// <remarks />
        public const string CustomControllerType_Input3 = "nsu=" + Namespaces.Uri + ";i=24";
        /// <remarks />
        public const string CustomControllerType_ControlOut = "nsu=" + Namespaces.Uri + ";i=25";
        /// <remarks />
        public const string BoilerInputPipeType_FlowTransmitter1_Output = "nsu=" + Namespaces.Uri + ";i=75";
        /// <remarks />
        public const string BoilerInputPipeType_FlowTransmitter1_Output_EURange = "nsu=" + Namespaces.Uri + ";i=78";
        /// <remarks />
        public const string BoilerInputPipeType_FlowTransmitter1_Output_EngineeringUnits = "nsu=" + Namespaces.Uri + ";i=80";
        /// <remarks />
        public const string BoilerInputPipeType_Valve_Input = "nsu=" + Namespaces.Uri + ";i=82";
        /// <remarks />
        public const string BoilerInputPipeType_Valve_Input_EURange = "nsu=" + Namespaces.Uri + ";i=85";
        /// <remarks />
        public const string BoilerInputPipeType_Valve_EnergyConsumption = "nsu=" + Namespaces.Uri + ";i=35";
        /// <remarks />
        public const string BoilerInputPipeType_Valve_EnergyConsumption_EngineeringUnits = "nsu=" + Namespaces.Uri + ";i=42";
        /// <remarks />
        public const string BoilerDrumType_LevelIndicator_Output = "nsu=" + Namespaces.Uri + ";i=90";
        /// <remarks />
        public const string BoilerDrumType_LevelIndicator_Output_EURange = "nsu=" + Namespaces.Uri + ";i=93";
        /// <remarks />
        public const string BoilerDrumType_LevelIndicator_Output_EngineeringUnits = "nsu=" + Namespaces.Uri + ";i=95";
        /// <remarks />
        public const string BoilerOutputPipeType_FlowTransmitter2_Output = "nsu=" + Namespaces.Uri + ";i=98";
        /// <remarks />
        public const string BoilerOutputPipeType_FlowTransmitter2_Output_EURange = "nsu=" + Namespaces.Uri + ";i=101";
        /// <remarks />
        public const string BoilerOutputPipeType_FlowTransmitter2_Output_EngineeringUnits = "nsu=" + Namespaces.Uri + ";i=103";
        /// <remarks />
        public const string SetPointMask_OptionSetValues = "nsu=" + Namespaces.Uri + ";i=219";
        /// <remarks />
        public const string BoilerType_InputPipe_FlowTransmitter1_Output = "nsu=" + Namespaces.Uri + ";i=105";
        /// <remarks />
        public const string BoilerType_InputPipe_FlowTransmitter1_Output_EURange = "nsu=" + Namespaces.Uri + ";i=108";
        /// <remarks />
        public const string BoilerType_InputPipe_FlowTransmitter1_Output_EngineeringUnits = "nsu=" + Namespaces.Uri + ";i=110";
        /// <remarks />
        public const string BoilerType_InputPipe_Valve_Input = "nsu=" + Namespaces.Uri + ";i=112";
        /// <remarks />
        public const string BoilerType_InputPipe_Valve_Input_EURange = "nsu=" + Namespaces.Uri + ";i=115";
        /// <remarks />
        public const string BoilerType_InputPipe_Valve_EnergyConsumption = "nsu=" + Namespaces.Uri + ";i=43";
        /// <remarks />
        public const string BoilerType_InputPipe_Valve_EnergyConsumption_EngineeringUnits = "nsu=" + Namespaces.Uri + ";i=49";
        /// <remarks />
        public const string BoilerType_Drum_LevelIndicator_Output = "nsu=" + Namespaces.Uri + ";i=59";
        /// <remarks />
        public const string BoilerType_Drum_LevelIndicator_Output_EURange = "nsu=" + Namespaces.Uri + ";i=62";
        /// <remarks />
        public const string BoilerType_Drum_LevelIndicator_Output_EngineeringUnits = "nsu=" + Namespaces.Uri + ";i=64";
        /// <remarks />
        public const string BoilerType_OutputPipe_FlowTransmitter2_Output = "nsu=" + Namespaces.Uri + ";i=119";
        /// <remarks />
        public const string BoilerType_OutputPipe_FlowTransmitter2_Output_EURange = "nsu=" + Namespaces.Uri + ";i=122";
        /// <remarks />
        public const string BoilerType_OutputPipe_FlowTransmitter2_Output_EngineeringUnits = "nsu=" + Namespaces.Uri + ";i=124";
        /// <remarks />
        public const string BoilerType_FlowController_Measurement = "nsu=" + Namespaces.Uri + ";i=126";
        /// <remarks />
        public const string BoilerType_FlowController_SetPoint = "nsu=" + Namespaces.Uri + ";i=127";
        /// <remarks />
        public const string BoilerType_FlowController_ControlOut = "nsu=" + Namespaces.Uri + ";i=128";
        /// <remarks />
        public const string BoilerType_LevelController_Measurement = "nsu=" + Namespaces.Uri + ";i=130";
        /// <remarks />
        public const string BoilerType_LevelController_SetPoint = "nsu=" + Namespaces.Uri + ";i=131";
        /// <remarks />
        public const string BoilerType_LevelController_ControlOut = "nsu=" + Namespaces.Uri + ";i=132";
        /// <remarks />
        public const string BoilerType_CustomController_Input1 = "nsu=" + Namespaces.Uri + ";i=134";
        /// <remarks />
        public const string BoilerType_CustomController_Input2 = "nsu=" + Namespaces.Uri + ";i=135";
        /// <remarks />
        public const string BoilerType_CustomController_Input3 = "nsu=" + Namespaces.Uri + ";i=136";
        /// <remarks />
        public const string BoilerType_CustomController_ControlOut = "nsu=" + Namespaces.Uri + ";i=137";
        /// <remarks />
        public const string BoilerType_ChangeSetPoints_InputArguments = "nsu=" + Namespaces.Uri + ";i=201";
        /// <remarks />
        public const string BoilerType_ChangeSetPoints_OutputArguments = "nsu=" + Namespaces.Uri + ";i=202";
        /// <remarks />
        public const string BoilerType_EmergencyShutdown_InputArguments = "nsu=" + Namespaces.Uri + ";i=204";
        /// <remarks />
        public const string BoilerType_EmergencyShutdown_OutputArguments = "nsu=" + Namespaces.Uri + ";i=205";
        /// <remarks />
        public const string BoilerType_Restart_InputArguments = "nsu=" + Namespaces.Uri + ";i=207";
        /// <remarks />
        public const string BoilerType_Restart_OutputArguments = "nsu=" + Namespaces.Uri + ";i=208";
        /// <remarks />
        public const string Boiler1_InputPipe_FlowTransmitter1_Output = "nsu=" + Namespaces.Uri + ";i=141";
        /// <remarks />
        public const string Boiler1_InputPipe_FlowTransmitter1_Output_EURange = "nsu=" + Namespaces.Uri + ";i=144";
        /// <remarks />
        public const string Boiler1_InputPipe_FlowTransmitter1_Output_EngineeringUnits = "nsu=" + Namespaces.Uri + ";i=146";
        /// <remarks />
        public const string Boiler1_InputPipe_Valve_Input = "nsu=" + Namespaces.Uri + ";i=148";
        /// <remarks />
        public const string Boiler1_InputPipe_Valve_Input_EURange = "nsu=" + Namespaces.Uri + ";i=151";
        /// <remarks />
        public const string Boiler1_InputPipe_Valve_EnergyConsumption = "nsu=" + Namespaces.Uri + ";i=50";
        /// <remarks />
        public const string Boiler1_InputPipe_Valve_EnergyConsumption_EngineeringUnits = "nsu=" + Namespaces.Uri + ";i=240";
        /// <remarks />
        public const string Boiler1_Drum_LevelIndicator_Output = "nsu=" + Namespaces.Uri + ";i=156";
        /// <remarks />
        public const string Boiler1_Drum_LevelIndicator_Output_EURange = "nsu=" + Namespaces.Uri + ";i=159";
        /// <remarks />
        public const string Boiler1_Drum_LevelIndicator_Output_EngineeringUnits = "nsu=" + Namespaces.Uri + ";i=161";
        /// <remarks />
        public const string Boiler1_OutputPipe_FlowTransmitter2_Output = "nsu=" + Namespaces.Uri + ";i=164";
        /// <remarks />
        public const string Boiler1_OutputPipe_FlowTransmitter2_Output_EURange = "nsu=" + Namespaces.Uri + ";i=167";
        /// <remarks />
        public const string Boiler1_OutputPipe_FlowTransmitter2_Output_EngineeringUnits = "nsu=" + Namespaces.Uri + ";i=169";
        /// <remarks />
        public const string Boiler1_FlowController_Measurement = "nsu=" + Namespaces.Uri + ";i=171";
        /// <remarks />
        public const string Boiler1_FlowController_SetPoint = "nsu=" + Namespaces.Uri + ";i=172";
        /// <remarks />
        public const string Boiler1_FlowController_ControlOut = "nsu=" + Namespaces.Uri + ";i=173";
        /// <remarks />
        public const string Boiler1_LevelController_Measurement = "nsu=" + Namespaces.Uri + ";i=175";
        /// <remarks />
        public const string Boiler1_LevelController_SetPoint = "nsu=" + Namespaces.Uri + ";i=176";
        /// <remarks />
        public const string Boiler1_LevelController_ControlOut = "nsu=" + Namespaces.Uri + ";i=177";
        /// <remarks />
        public const string Boiler1_CustomController_Input1 = "nsu=" + Namespaces.Uri + ";i=179";
        /// <remarks />
        public const string Boiler1_CustomController_Input2 = "nsu=" + Namespaces.Uri + ";i=180";
        /// <remarks />
        public const string Boiler1_CustomController_Input3 = "nsu=" + Namespaces.Uri + ";i=181";
        /// <remarks />
        public const string Boiler1_CustomController_ControlOut = "nsu=" + Namespaces.Uri + ";i=182";
        /// <remarks />
        public const string Boiler1_ChangeSetPoints_InputArguments = "nsu=" + Namespaces.Uri + ";i=210";
        /// <remarks />
        public const string Boiler1_ChangeSetPoints_OutputArguments = "nsu=" + Namespaces.Uri + ";i=211";
        /// <remarks />
        public const string Boiler1_EmergencyShutdown_InputArguments = "nsu=" + Namespaces.Uri + ";i=213";
        /// <remarks />
        public const string Boiler1_EmergencyShutdown_OutputArguments = "nsu=" + Namespaces.Uri + ";i=214";
        /// <remarks />
        public const string Boiler1_Restart_InputArguments = "nsu=" + Namespaces.Uri + ";i=216";
        /// <remarks />
        public const string Boiler1_Restart_OutputArguments = "nsu=" + Namespaces.Uri + ";i=217";
        /// <remarks />
        public const string Boiler_BinarySchema = "nsu=" + Namespaces.Uri + ";i=192";
        /// <remarks />
        public const string Boiler_BinarySchema_NamespaceUri = "nsu=" + Namespaces.Uri + ";i=194";
        /// <remarks />
        public const string Boiler_BinarySchema_Deprecated = "nsu=" + Namespaces.Uri + ";i=15001";
        /// <remarks />
        public const string Boiler_BinarySchema_EnergyConsumptionType = "nsu=" + Namespaces.Uri + ";i=242";
        /// <remarks />
        public const string Boiler_BinarySchema_ChangeSetPointsRequest = "nsu=" + Namespaces.Uri + ";i=224";
        /// <remarks />
        public const string Boiler_BinarySchema_ChangeSetPointsResponse = "nsu=" + Namespaces.Uri + ";i=227";
        /// <remarks />
        public const string Boiler_XmlSchema = "nsu=" + Namespaces.Uri + ";i=185";
        /// <remarks />
        public const string Boiler_XmlSchema_NamespaceUri = "nsu=" + Namespaces.Uri + ";i=187";
        /// <remarks />
        public const string Boiler_XmlSchema_Deprecated = "nsu=" + Namespaces.Uri + ";i=15002";
        /// <remarks />
        public const string Boiler_XmlSchema_EnergyConsumptionType = "nsu=" + Namespaces.Uri + ";i=246";
        /// <remarks />
        public const string Boiler_XmlSchema_ChangeSetPointsRequest = "nsu=" + Namespaces.Uri + ";i=232";
        /// <remarks />
        public const string Boiler_XmlSchema_ChangeSetPointsResponse = "nsu=" + Namespaces.Uri + ";i=235";

        /// <summary>
        /// Converts a value to a name for display.
        /// </summary>
        public static string ToName(string value)
        {
            foreach (var field in typeof(VariableIds).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.GetValue(null).Equals(value))
                {
                    return field.Name;
                }
            }

            return value.ToString();
        }
    }
    
}