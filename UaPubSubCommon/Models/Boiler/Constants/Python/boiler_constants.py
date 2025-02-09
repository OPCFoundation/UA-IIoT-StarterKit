from enum import Enum

class Namespaces(Enum):
     Uri = "urn:opcua.org:2025-01:iiot-starterkit:boiler"

class BrowseNames(Enum):
    Boiler_BinarySchema = "Boiler"
    Boiler_XmlSchema = "Boiler"
    Boiler1 = "Boiler #1"
    BoilerDrumType = "BoilerDrumType"
    BoilerInputPipeType = "BoilerInputPipeType"
    BoilerOutputPipeType = "BoilerOutputPipeType"
    BoilerType = "BoilerType"
    ChangeSetPoints = "ChangeSetPoints"
    ChangeSetPointsRequest = "ChangeSetPointsRequest"
    ChangeSetPointsResponse = "ChangeSetPointsResponse"
    ControlOut = "ControlOut"
    CustomController = "CCX001"
    CustomControllerType = "CustomControllerType"
    Drum = "DrumX001"
    EmergencyShutdown = "EmergencyShutdown"
    EnergyConsumption = "EnergyConsumption"
    EnergyConsumptionType = "EnergyConsumptionType"
    FlowController = "FCX001"
    FlowControllerType = "FlowControllerType"
    FlowTo = "FlowTo"
    FlowTransmitter1 = "FTX001"
    FlowTransmitter2 = "FTX002"
    FlowTransmitterType = "FlowTransmitterType"
    GenericActuatorType = "GenericActuatorType"
    GenericControllerType = "GenericControllerType"
    GenericSensorType = "GenericSensorType"
    Input = "Input"
    Input1 = "Input1"
    Input2 = "Input2"
    Input3 = "Input3"
    InputPipe = "PipeX001"
    LevelController = "LCX001"
    LevelControllerType = "LevelControllerType"
    LevelIndicator = "LIX001"
    LevelIndicatorType = "LevelIndicatorType"
    Measurement = "Measurement"
    Output = "Output"
    OutputPipe = "PipeX002"
    Restart = "Restart"
    SetPoint = "SetPoint"
    SetPointMask = "SetPointMask"
    SignalTo = "SignalTo"
    Valve = "ValveX001"
    ValveType = "ValveType"

class DataTypeIds(Enum):
    EnergyConsumptionType = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=27"
    SetPointMask = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=218"
    ChangeSetPointsRequest = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=220"
    ChangeSetPointsResponse = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=221"

def get_DataTypeIds_name(value: str) -> str:
    try:
        return DataTypeIds(value).name
    except ValueError:
        return None


class MethodIds(Enum):
    BoilerType_ChangeSetPoints = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=200"
    BoilerType_EmergencyShutdown = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=203"
    BoilerType_Restart = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=206"
    Boiler1_ChangeSetPoints = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=209"
    Boiler1_EmergencyShutdown = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=212"
    Boiler1_Restart = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=215"

def get_MethodIds_name(value: str) -> str:
    try:
        return MethodIds(value).name
    except ValueError:
        return None


class ObjectIds(Enum):
    BoilerInputPipeType_FlowTransmitter1 = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=74"
    BoilerInputPipeType_Valve = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=81"
    BoilerDrumType_LevelIndicator = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=89"
    BoilerOutputPipeType_FlowTransmitter2 = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=97"
    BoilerType_InputPipe = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=56"
    BoilerType_InputPipe_FlowTransmitter1 = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=104"
    BoilerType_InputPipe_Valve = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=111"
    BoilerType_Drum = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=57"
    BoilerType_Drum_LevelIndicator = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=58"
    BoilerType_OutputPipe = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=65"
    BoilerType_OutputPipe_FlowTransmitter2 = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=118"
    BoilerType_FlowController = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=125"
    BoilerType_LevelController = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=129"
    BoilerType_CustomController = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=133"
    Boiler1 = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=138"
    Boiler1_InputPipe = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=139"
    Boiler1_InputPipe_FlowTransmitter1 = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=140"
    Boiler1_InputPipe_Valve = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=147"
    Boiler1_Drum = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=154"
    Boiler1_Drum_LevelIndicator = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=155"
    Boiler1_OutputPipe = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=162"
    Boiler1_OutputPipe_FlowTransmitter2 = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=163"
    Boiler1_FlowController = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=170"
    Boiler1_LevelController = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=174"
    Boiler1_CustomController = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=178"
    EnergyConsumptionType_Encoding_DefaultBinary = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=241"
    ChangeSetPointsRequest_Encoding_DefaultBinary = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=222"
    ChangeSetPointsResponse_Encoding_DefaultBinary = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=223"
    EnergyConsumptionType_Encoding_DefaultXml = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=245"
    ChangeSetPointsRequest_Encoding_DefaultXml = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=230"
    ChangeSetPointsResponse_Encoding_DefaultXml = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=231"
    EnergyConsumptionType_Encoding_DefaultJson = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=249"
    ChangeSetPointsRequest_Encoding_DefaultJson = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=238"
    ChangeSetPointsResponse_Encoding_DefaultJson = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=239"

def get_ObjectIds_name(value: str) -> str:
    try:
        return ObjectIds(value).name
    except ValueError:
        return None


class ObjectTypeIds(Enum):
    GenericControllerType = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=3"
    GenericSensorType = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=7"
    GenericActuatorType = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=14"
    CustomControllerType = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=21"
    ValveType = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=26"
    LevelControllerType = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=33"
    FlowControllerType = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=37"
    LevelIndicatorType = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=41"
    FlowTransmitterType = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=48"
    BoilerInputPipeType = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=73"
    BoilerDrumType = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=88"
    BoilerOutputPipeType = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=96"
    BoilerType = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=55"

def get_ObjectTypeIds_name(value: str) -> str:
    try:
        return ObjectTypeIds(value).name
    except ValueError:
        return None


class ReferenceTypeIds(Enum):
    FlowTo = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=1"
    SignalTo = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=2"

def get_ReferenceTypeIds_name(value: str) -> str:
    try:
        return ReferenceTypeIds(value).name
    except ValueError:
        return None


class VariableIds(Enum):
    GenericControllerType_Measurement = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=4"
    GenericControllerType_SetPoint = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=5"
    GenericControllerType_ControlOut = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=6"
    GenericSensorType_Output = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=8"
    GenericSensorType_Output_EURange = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=11"
    GenericSensorType_Output_EngineeringUnits = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=13"
    GenericActuatorType_Input = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=15"
    GenericActuatorType_Input_EURange = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=18"
    GenericActuatorType_EnergyConsumption = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=28"
    GenericActuatorType_EnergyConsumption_EngineeringUnits = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=34"
    CustomControllerType_Input1 = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=22"
    CustomControllerType_Input2 = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=23"
    CustomControllerType_Input3 = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=24"
    CustomControllerType_ControlOut = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=25"
    BoilerInputPipeType_FlowTransmitter1_Output = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=75"
    BoilerInputPipeType_FlowTransmitter1_Output_EURange = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=78"
    BoilerInputPipeType_FlowTransmitter1_Output_EngineeringUnits = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=80"
    BoilerInputPipeType_Valve_Input = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=82"
    BoilerInputPipeType_Valve_Input_EURange = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=85"
    BoilerInputPipeType_Valve_EnergyConsumption = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=35"
    BoilerInputPipeType_Valve_EnergyConsumption_EngineeringUnits = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=42"
    BoilerDrumType_LevelIndicator_Output = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=90"
    BoilerDrumType_LevelIndicator_Output_EURange = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=93"
    BoilerDrumType_LevelIndicator_Output_EngineeringUnits = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=95"
    BoilerOutputPipeType_FlowTransmitter2_Output = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=98"
    BoilerOutputPipeType_FlowTransmitter2_Output_EURange = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=101"
    BoilerOutputPipeType_FlowTransmitter2_Output_EngineeringUnits = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=103"
    SetPointMask_OptionSetValues = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=219"
    BoilerType_InputPipe_FlowTransmitter1_Output = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=105"
    BoilerType_InputPipe_FlowTransmitter1_Output_EURange = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=108"
    BoilerType_InputPipe_FlowTransmitter1_Output_EngineeringUnits = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=110"
    BoilerType_InputPipe_Valve_Input = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=112"
    BoilerType_InputPipe_Valve_Input_EURange = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=115"
    BoilerType_InputPipe_Valve_EnergyConsumption = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=43"
    BoilerType_InputPipe_Valve_EnergyConsumption_EngineeringUnits = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=49"
    BoilerType_Drum_LevelIndicator_Output = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=59"
    BoilerType_Drum_LevelIndicator_Output_EURange = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=62"
    BoilerType_Drum_LevelIndicator_Output_EngineeringUnits = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=64"
    BoilerType_OutputPipe_FlowTransmitter2_Output = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=119"
    BoilerType_OutputPipe_FlowTransmitter2_Output_EURange = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=122"
    BoilerType_OutputPipe_FlowTransmitter2_Output_EngineeringUnits = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=124"
    BoilerType_FlowController_Measurement = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=126"
    BoilerType_FlowController_SetPoint = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=127"
    BoilerType_FlowController_ControlOut = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=128"
    BoilerType_LevelController_Measurement = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=130"
    BoilerType_LevelController_SetPoint = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=131"
    BoilerType_LevelController_ControlOut = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=132"
    BoilerType_CustomController_Input1 = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=134"
    BoilerType_CustomController_Input2 = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=135"
    BoilerType_CustomController_Input3 = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=136"
    BoilerType_CustomController_ControlOut = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=137"
    BoilerType_ChangeSetPoints_InputArguments = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=201"
    BoilerType_ChangeSetPoints_OutputArguments = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=202"
    BoilerType_EmergencyShutdown_InputArguments = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=204"
    BoilerType_EmergencyShutdown_OutputArguments = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=205"
    BoilerType_Restart_InputArguments = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=207"
    BoilerType_Restart_OutputArguments = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=208"
    Boiler1_InputPipe_FlowTransmitter1_Output = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=141"
    Boiler1_InputPipe_FlowTransmitter1_Output_EURange = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=144"
    Boiler1_InputPipe_FlowTransmitter1_Output_EngineeringUnits = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=146"
    Boiler1_InputPipe_Valve_Input = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=148"
    Boiler1_InputPipe_Valve_Input_EURange = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=151"
    Boiler1_InputPipe_Valve_EnergyConsumption = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=50"
    Boiler1_InputPipe_Valve_EnergyConsumption_EngineeringUnits = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=240"
    Boiler1_Drum_LevelIndicator_Output = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=156"
    Boiler1_Drum_LevelIndicator_Output_EURange = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=159"
    Boiler1_Drum_LevelIndicator_Output_EngineeringUnits = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=161"
    Boiler1_OutputPipe_FlowTransmitter2_Output = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=164"
    Boiler1_OutputPipe_FlowTransmitter2_Output_EURange = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=167"
    Boiler1_OutputPipe_FlowTransmitter2_Output_EngineeringUnits = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=169"
    Boiler1_FlowController_Measurement = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=171"
    Boiler1_FlowController_SetPoint = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=172"
    Boiler1_FlowController_ControlOut = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=173"
    Boiler1_LevelController_Measurement = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=175"
    Boiler1_LevelController_SetPoint = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=176"
    Boiler1_LevelController_ControlOut = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=177"
    Boiler1_CustomController_Input1 = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=179"
    Boiler1_CustomController_Input2 = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=180"
    Boiler1_CustomController_Input3 = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=181"
    Boiler1_CustomController_ControlOut = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=182"
    Boiler1_ChangeSetPoints_InputArguments = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=210"
    Boiler1_ChangeSetPoints_OutputArguments = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=211"
    Boiler1_EmergencyShutdown_InputArguments = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=213"
    Boiler1_EmergencyShutdown_OutputArguments = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=214"
    Boiler1_Restart_InputArguments = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=216"
    Boiler1_Restart_OutputArguments = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=217"
    Boiler_BinarySchema = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=192"
    Boiler_BinarySchema_NamespaceUri = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=194"
    Boiler_BinarySchema_Deprecated = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=15001"
    Boiler_BinarySchema_EnergyConsumptionType = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=242"
    Boiler_BinarySchema_ChangeSetPointsRequest = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=224"
    Boiler_BinarySchema_ChangeSetPointsResponse = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=227"
    Boiler_XmlSchema = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=185"
    Boiler_XmlSchema_NamespaceUri = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=187"
    Boiler_XmlSchema_Deprecated = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=15002"
    Boiler_XmlSchema_EnergyConsumptionType = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=246"
    Boiler_XmlSchema_ChangeSetPointsRequest = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=232"
    Boiler_XmlSchema_ChangeSetPointsResponse = "nsu=urn:opcua.org:2025-01:iiot-starterkit:boiler;i=235"

def get_VariableIds_name(value: str) -> str:
    try:
        return VariableIds(value).name
    except ValueError:
        return None

