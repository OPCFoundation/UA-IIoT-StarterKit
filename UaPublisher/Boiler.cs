using BoilerModel = Boiler;
using Opc.Ua;
using Range = Opc.Ua.Range;

namespace UaPublisher
{
    internal class Boiler : PublisherSource
    {
        public Boiler(int id)
        {
            Id = id;
            Name = $"Boiler{Id}";
            m_metadata = BuildMetaData();
        }

        public Drum Drum = new Drum() { Name = "Drum{0}001" };
        public OutputPipe OutputPipe = new OutputPipe() { Name = "Pipe{0}002" };
        public InputPipe InputPipe = new InputPipe() { Name = "Pipe{0}001" };
        public Controller LevelController = new Controller() { Name = "LC{0}001" };
        public Controller FlowController = new Controller() { Name = "FC{0}001" };
        public CustomController CustomController = new CustomController() { Name = "CC{0}001" };

        public override List<KeyValuePair<FieldMetaData, DataValue>> ReadChangedFields(
            IDictionary<string, Variant> cache,
            bool isKeyFrame)
        {
            lock (this)
            {
                List<KeyValuePair<FieldMetaData, DataValue>> changes = new();

                ReadFieldValue(
                    changes,
                    cache,
                    GetPath(OutputPipe.Name, OutputPipe.FlowIndicator.Name, nameof(Sensor.Output)),
                    OutputPipe.FlowIndicator.Output,
                    m_lastScanTime,
                    isKeyFrame);

                ReadFieldValue(
                    changes,
                    cache,
                    GetPath(Drum.Name, Drum.LevelIndicator.Name, nameof(Sensor.Output)),
                    Drum.LevelIndicator.Output,
                    m_lastScanTime,
                    isKeyFrame);

                ReadFieldValue(
                    changes,
                    cache,
                    GetPath(InputPipe.Name, InputPipe.FlowIndicator.Name, nameof(Sensor.Output)),
                    InputPipe.FlowIndicator.Output,
                    m_lastScanTime,
                    isKeyFrame);

                ReadFieldValue(
                    changes,
                    cache,
                    GetPath(LevelController.Name, nameof(Controller.SetPoint)),
                    LevelController.SetPoint,
                    LevelController.SetPointUpdateTime,
                    isKeyFrame);

                ReadFieldValue(
                    changes,
                    cache,
                    GetPath(FlowController.Name, nameof(Controller.SetPoint)),
                    FlowController.SetPoint,
                    FlowController.SetPointUpdateTime,
                    isKeyFrame);

                ReadFieldValue(
                    changes,
                    cache,
                    GetPath(InputPipe.Name, InputPipe.Valve.Name, nameof(Actuator.EnergyConsumption)),
                    new ExtensionObject(
                        BoilerModel.DataTypeIds.EnergyConsumptionType,
                        InputPipe.Valve.EnergyConsumption.Value
                    ),
                    m_lastScanTime,
                    isKeyFrame);

                return changes;
            }
        }

        public override DataSetMetaDataType BuildMetaData()
        {
            DataSetMetaDataType metadata = new();

            metadata.Name = $"Boiler{Id}";

            metadata.Namespaces = new string[]
            {
                BoilerModel.Namespaces.Boiler
            };

            metadata.StructureDataTypes = new StructureDescriptionCollection()
            {
                 new StructureDescription()
                 {
                      Name = BoilerModel.BrowseNames.EnergyConsumptionType,
                      DataTypeId = new NodeId(BoilerModel.DataTypes.EnergyConsumptionType, 1),
                      StructureDefinition = new StructureDefinition()
                      {
                           StructureType = StructureType.Structure,
                           Fields = new StructureFieldCollection()
                           {
                                new StructureField()
                                {
                                    Name = "Period",
                                    DataType = DataTypeIds.UInt32,
                                    ValueRank = ValueRanks.Scalar
                                },
                                new StructureField()
                                {
                                    Name = "MaxPower",
                                    DataType = DataTypeIds.Double,
                                    ValueRank = ValueRanks.Scalar
                                },
                                new StructureField()
                                {
                                    Name = "AveragePower",
                                    DataType = DataTypeIds.Double,
                                    ValueRank = ValueRanks.Scalar
                                },
                                new StructureField()
                                {
                                    Name = "Consumption",
                                    DataType = DataTypeIds.Double,
                                    ValueRank = ValueRanks.Scalar
                                },
                           }
                       }
                 }
            };

            metadata.ConfigurationVersion = new ConfigurationVersionDataType()
            {
                MajorVersion = GetVersionTime(),
                MinorVersion = GetVersionTime()
            };

            metadata.Fields = new FieldMetaDataCollection();

            string path = null;
            FieldMetaData field = null;

            path = GetPath(OutputPipe.Name, OutputPipe.FlowIndicator.Name, nameof(Sensor.Output));

            lock (m_lock)
            {
                field = m_fields[path] = new FieldMetaData()
                {
                    Name = path,
                    DataType = DataTypeIds.Double,
                    ValueRank = ValueRanks.Scalar,
                    BuiltInType = (byte)BuiltInType.Double,
                    Properties = new KeyValuePairCollection()
                    {
                        new Opc.Ua.KeyValuePair()
                        {
                            Key = BrowseNames.EURange,
                            Value = new ExtensionObject(DataTypeIds.Range, OutputPipe.FlowIndicator.EURange)
                        },
                        new Opc.Ua.KeyValuePair()
                        {
                            Key = BrowseNames.EngineeringUnits,
                            Value = new ExtensionObject(DataTypeIds.EUInformation, OutputPipe.FlowIndicator.EngineeringUnits)
                        }
                    }
                };
            }

            metadata.Fields.Add(field);

            path = GetPath(Drum.Name, Drum.LevelIndicator.Name, nameof(Sensor.Output));

            lock (m_lock)
            {
                field = m_fields[path] = new FieldMetaData()
                {
                    Name = path,
                    DataType = DataTypeIds.Double,
                    ValueRank = ValueRanks.Scalar,
                    BuiltInType = (byte)BuiltInType.Double,
                    Properties = new KeyValuePairCollection()
                    {
                        new Opc.Ua.KeyValuePair()
                        {
                            Key = BrowseNames.EURange,
                            Value = new ExtensionObject(DataTypeIds.Range, Drum.LevelIndicator.EURange)
                        },
                        new Opc.Ua.KeyValuePair()
                        {
                            Key = BrowseNames.EngineeringUnits,
                            Value = new ExtensionObject(DataTypeIds.EUInformation, Drum.LevelIndicator.EngineeringUnits)
                        }
                    }
                };
            }

            metadata.Fields.Add(field);

            path = GetPath(InputPipe.Name, InputPipe.FlowIndicator.Name, nameof(Sensor.Output));

            lock (m_lock)
            {
                field = m_fields[path] = new FieldMetaData()
                {
                    Name = path,
                    DataType = DataTypeIds.Double,
                    ValueRank = ValueRanks.Scalar,
                    BuiltInType = (byte)BuiltInType.Double,
                    Properties = new KeyValuePairCollection()
                    {
                        new Opc.Ua.KeyValuePair()
                        {
                            Key = BrowseNames.EURange,
                            Value = new ExtensionObject(DataTypeIds.Range, InputPipe.FlowIndicator.EURange)
                        },
                        new Opc.Ua.KeyValuePair()
                        {
                            Key = BrowseNames.EngineeringUnits,
                            Value = new ExtensionObject(DataTypeIds.EUInformation, InputPipe.FlowIndicator.EngineeringUnits)
                        }
                    }
                };
            }

            metadata.Fields.Add(field);

            path = GetPath(LevelController.Name, nameof(Controller.SetPoint));

            lock (m_lock)
            {
                field = m_fields[path] = new FieldMetaData()
                {
                    Name = path,
                    DataType = DataTypeIds.Double,
                    ValueRank = ValueRanks.Scalar,
                    BuiltInType = (byte)BuiltInType.Double
                };
            }

            metadata.Fields.Add(field);

            path = GetPath(FlowController.Name, nameof(Controller.SetPoint));

            lock (m_lock)
            {
                field = m_fields[path] = new FieldMetaData()
                {
                    Name = path,
                    DataType = DataTypeIds.Double,
                    ValueRank = ValueRanks.Scalar,
                    BuiltInType = (byte)BuiltInType.Double
                };
            }

            metadata.Fields.Add(field);

            path = GetPath(InputPipe.Name, InputPipe.Valve.Name, nameof(Actuator.EnergyConsumption));

            lock (m_lock)
            {
                field = m_fields[path] = new FieldMetaData()
                {
                    Name = path,
                    DataType = new NodeId(BoilerModel.DataTypes.EnergyConsumptionType, 1),
                    ValueRank = ValueRanks.Scalar,
                    BuiltInType = (byte)BuiltInType.ExtensionObject,
                    Properties = new KeyValuePairCollection()
                    {
                        new Opc.Ua.KeyValuePair()
                        {
                            Key = BrowseNames.EngineeringUnits,
                            Value = new ExtensionObject(DataTypeIds.EUInformation, InputPipe.Valve.EnergyConsumption.Value)
                        }
                    }
                };
            }

            metadata.Fields.Add(field);

            return metadata;
        }

        public override Task Update()
        {
            lock (m_lock)
            {
                if (DateTime.UtcNow - m_lastScanTime > new TimeSpan(0, 0, 5))
                {
                    var delta = (LevelController.SetPoint - Drum.LevelIndicator.Output) * 0.10;
                    UpdateValues(LevelController.SetPoint, delta);
                    m_lastScanTime = DateTime.UtcNow;
                }
            }

            return Task.CompletedTask;
        }

        private void UpdateValues(double setPoint, double delta)
        {
            var consumption = InputPipe.Valve.EnergyConsumption.Value;

            InputPipe.Valve.EnergyConsumption.Value = new BoilerModel.EnergyConsumptionType()
            {
                Period = consumption.Period,
                AveragePower = PerturbValue(5, 20),
                MaxPower = PerturbValue(50, 200)
            };

            InputPipe.Valve.EnergyConsumption.Value.Consumption = Math.Round((InputPipe.Valve.EnergyConsumption.Value.AveragePower * 3600) / 1000, 2);

            // set by user (0, 300)
            LevelController.SetPoint = setPoint;

            var ratio = (Drum.LevelIndicator.Output + delta) / 300.0;

            // sets CustomController.Input1 (1, 100)
            LevelController.ControlOut = NewValue(ratio, 1, 100);

            // set by LevelController.ControlOut (1, 100)
            CustomController.Input1 = LevelController.ControlOut;

            // sets FlowController.SetPoint (1, 100)
            CustomController.ControlOut = NewValue(ratio, 1, 100);

            // set by CustomController.ControlOut (1, 100)
            FlowController.SetPoint = CustomController.ControlOut;

            // sets InputPipe.Valve.Input (0, 10)
            FlowController.ControlOut = NewValue(ratio, 0, 10);

            // set by FlowController.ControlOut (0, 10)
            InputPipe.Valve.Input = FlowController.ControlOut;

            // sets FlowController.Measurement (0, 20)
            // sets CustomController.Input2 (0, 20)
            InputPipe.FlowIndicator.Output = NewValue(ratio, 0, 20);

            // set by InputPipe.FlowTransmitter1.Output (0, 20)
            FlowController.Measurement = InputPipe.FlowIndicator.Output;

            // set by InputPipe.FlowTransmitter1.Output (0, 20)
            CustomController.Input2 = InputPipe.FlowIndicator.Output;

            // sets LevelController.Measurement (0, 300)
            Drum.LevelIndicator.Output = NewValue(ratio, 0, 300);

            // set by Drum.LevelIndicator.Output (0, 300)
            LevelController.Measurement = Drum.LevelIndicator.Output;

            // sets CustomController.Input3 (100, 10000)
            OutputPipe.FlowIndicator.Output = NewValue(ratio, 100, 10000);

            // set by OutputPipe.FlowTransmitter2.Output (100, 10000)
            CustomController.Input3 = OutputPipe.FlowIndicator.Output;
        }

        private double PerturbValue(double low, double high)
        {
            return Math.Round(Random.Shared.NextDouble() * (high - low) + low);
        }

        private double NewValue(double value, double low, double high)
        {
            return Math.Round(((value * (high - low) + low) + ((Random.Shared.NextDouble() - 0.5) * ((high - low) * 0.01))), 2);
        }

        public List<Variant> Reset(ActionTarget target, List<Variant> inputArguments)
        {
            var newSetPoint = (double)TypeInfo.Cast(inputArguments?[0], BuiltInType.Double);

            var oldSetPoint = LevelController.SetPoint;

            LevelController.SetPoint = newSetPoint;

            LevelController.Measurement = 0;
            FlowController.Measurement = 0;
            InputPipe.FlowIndicator.Output = 0;
            OutputPipe.FlowIndicator.Output = 0;
            Drum.LevelIndicator.Output = 0;

            return new List<Variant>([oldSetPoint]);
        }
    }

    public class Drum
    {
        public string Name;

        public Sensor LevelIndicator = new Sensor()
        {
            Name = "LI{0}001",
            Output = 0.0,
            EURange = new Range() { Low = 0.0, High = 300.0 },
            EngineeringUnits = new EUInformation
            {
                NamespaceUri = "http://www.opcfoundation.org/UA/units/un/cefact",
                UnitId = 4410708,
                DisplayName = "cm",
                Description = "centimetre",
            }
        };
    }

    public class OutputPipe
    {
        public string Name;

        public Sensor FlowIndicator = new Sensor()
        {
            Name = "FT{0}002",
            Output = 0.0,
            EURange = new Range() { Low = 100.0, High = 10000.0 },
            EngineeringUnits = new EUInformation
            {
                NamespaceUri = "http://www.opcfoundation.org/UA/units/un/cefact",
                UnitId = 5259596,
                DisplayName = "Pa",
                Description = "pascal",
            }
        };
    }

    public class InputPipe
    {
        public string Name;

        public Sensor FlowIndicator = new Sensor()
        {
            Name = "FT{0}001",
            Output = 0.0,
            EURange = new Range() { Low = 0.0, High = 20.0 },
            EngineeringUnits = new EUInformation
            {
                NamespaceUri = "http://www.opcfoundation.org/UA/units/un/cefact",
                UnitId = 4666673,
                DisplayName = "l/s",
                Description = "litre per second",
            }
        };

        public Actuator Valve = new Actuator()
        {
            Name = "Valve{0}001",
            Input = 0.0,
            EURange = new Range() { Low = 0, High = 10 },
            EnergyConsumption = new EnergyCalculator()
            {
                Name = "EnergyConsumption",
                EngineeringUnits = new EUInformation
                {
                    NamespaceUri = "http://www.opcfoundation.org/UA/units/un/cefact",
                    UnitId = 4937544,
                    DisplayName = "kW·h",
                    Description = "kilowatt hour",
                }
            }
        };
    }

    public class Sensor
    {
        public string Name;
        public double Output;
        public Range EURange;
        public EUInformation EngineeringUnits;
    }

    public class Actuator
    {
        public string Name;
        public double Input;
        public Range EURange;
        public EnergyCalculator EnergyConsumption;
    }

    public class Controller
    {
        public string Name;
        public double Measurement;
        public double SetPoint;
        public DateTime SetPointUpdateTime;
        public double ControlOut;
    }

    public class CustomController
    {
        public string Name;
        public double Input1;
        public double Input2;
        public double Input3;
        public double ControlOut;
    }

    public class EnergyCalculator
    {
        public string Name;
        public EUInformation EngineeringUnits;
        public BoilerModel.EnergyConsumptionType Value = new()
        {
            Consumption = 0,
            AveragePower = 0,
            MaxPower = 0,
            Period = 3600
        };
    }
}
