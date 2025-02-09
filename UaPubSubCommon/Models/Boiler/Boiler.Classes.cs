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
    #region GenericControllerState Class
#if (!OPCUA_EXCLUDE_GenericControllerState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class GenericControllerState : BaseObjectState
    {
        #region Constructors
        /// <remarks />
        public GenericControllerState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(Boiler.ObjectTypes.GenericControllerType, Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAACwAAAB1cm46b3BjdWEub3JnOjIwMjUtMDE6aWlvdC1zdGFydGVya2l0OmJvaWxlcv////8EYIAC" +
           "AQAAAAEAHQAAAEdlbmVyaWNDb250cm9sbGVyVHlwZUluc3RhbmNlAQEDAAEBAwADAAAA/////wMAAAAV" +
           "YIkKAgAAAAEACwAAAE1lYXN1cmVtZW50AQEEAAAuAEQEAAAAAAv/////AQH/////AAAAABVgiQoCAAAA" +
           "AQAIAAAAU2V0UG9pbnQBAQUAAC4ARAUAAAAAC/////8DA/////8AAAAAFWCJCgIAAAABAAoAAABDb250" +
           "cm9sT3V0AQEGAAAuAEQGAAAAAAv/////AQH/////AAAAAA==";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public PropertyState<double> Measurement
        {
            get
            {
                return m_measurement;
            }

            set
            {
                if (!Object.ReferenceEquals(m_measurement, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_measurement = value;
            }
        }

        /// <remarks />
        public PropertyState<double> SetPoint
        {
            get
            {
                return m_setPoint;
            }

            set
            {
                if (!Object.ReferenceEquals(m_setPoint, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_setPoint = value;
            }
        }

        /// <remarks />
        public PropertyState<double> ControlOut
        {
            get
            {
                return m_controlOut;
            }

            set
            {
                if (!Object.ReferenceEquals(m_controlOut, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_controlOut = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <remarks />
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_measurement != null)
            {
                children.Add(m_measurement);
            }

            if (m_setPoint != null)
            {
                children.Add(m_setPoint);
            }

            if (m_controlOut != null)
            {
                children.Add(m_controlOut);
            }

            base.GetChildren(context, children);
        }
            
        /// <remarks />
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case Boiler.BrowseNames.Measurement:
                {
                    if (createOrReplace)
                    {
                        if (Measurement == null)
                        {
                            if (replacement == null)
                            {
                                Measurement = new PropertyState<double>(this);
                            }
                            else
                            {
                                Measurement = (PropertyState<double>)replacement;
                            }
                        }
                    }

                    instance = Measurement;
                    break;
                }

                case Boiler.BrowseNames.SetPoint:
                {
                    if (createOrReplace)
                    {
                        if (SetPoint == null)
                        {
                            if (replacement == null)
                            {
                                SetPoint = new PropertyState<double>(this);
                            }
                            else
                            {
                                SetPoint = (PropertyState<double>)replacement;
                            }
                        }
                    }

                    instance = SetPoint;
                    break;
                }

                case Boiler.BrowseNames.ControlOut:
                {
                    if (createOrReplace)
                    {
                        if (ControlOut == null)
                        {
                            if (replacement == null)
                            {
                                ControlOut = new PropertyState<double>(this);
                            }
                            else
                            {
                                ControlOut = (PropertyState<double>)replacement;
                            }
                        }
                    }

                    instance = ControlOut;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private PropertyState<double> m_measurement;
        private PropertyState<double> m_setPoint;
        private PropertyState<double> m_controlOut;
        #endregion
    }
    #endif
    #endregion

    #region GenericSensorState Class
    #if (!OPCUA_EXCLUDE_GenericSensorState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class GenericSensorState : BaseObjectState
    {
        #region Constructors
        /// <remarks />
        public GenericSensorState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(Boiler.ObjectTypes.GenericSensorType, Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAACwAAAB1cm46b3BjdWEub3JnOjIwMjUtMDE6aWlvdC1zdGFydGVya2l0OmJvaWxlcv////8EYIAC" +
           "AQAAAAEAGQAAAEdlbmVyaWNTZW5zb3JUeXBlSW5zdGFuY2UBAQcAAQEHAAcAAAD/////AQAAABVgiQoC" +
           "AAAAAQAGAAAAT3V0cHV0AQEIAAAvAQCiRAgAAAAAC/////8BAf////8CAAAAFWCpCgIAAAAAAAcAAABF" +
           "VVJhbmdlAQELAAAuAEQLAAAAFgEAdgMBEAAAAAAAAAAAAAAAAAAAAAAAWUABAHQD/////wEB/////wAA" +
           "AAAVYKkKAgAAAAAAEAAAAEVuZ2luZWVyaW5nVW5pdHMBAQ0AAC4ARA0AAAAWAQB5AwFUAAAALwAAAGh0" +
           "dHA6Ly93d3cub3BjZm91bmRhdGlvbi5vcmcvVUEvdW5pdHMvdW4vY2VmYWN0U1RNAAIDAAAAbS9zAhAA" +
           "AABtZXRyZSBwZXIgc2Vjb25kAQB3A/////8DA/////8AAAAA";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public AnalogUnitRangeState<double> Output
        {
            get
            {
                return m_output;
            }

            set
            {
                if (!Object.ReferenceEquals(m_output, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_output = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <remarks />
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_output != null)
            {
                children.Add(m_output);
            }

            base.GetChildren(context, children);
        }
            
        /// <remarks />
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case Boiler.BrowseNames.Output:
                {
                    if (createOrReplace)
                    {
                        if (Output == null)
                        {
                            if (replacement == null)
                            {
                                Output = new AnalogUnitRangeState<double>(this);
                            }
                            else
                            {
                                Output = (AnalogUnitRangeState<double>)replacement;
                            }
                        }
                    }

                    instance = Output;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private AnalogUnitRangeState<double> m_output;
        #endregion
    }
    #endif
    #endregion

    #region GenericActuatorState Class
    #if (!OPCUA_EXCLUDE_GenericActuatorState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class GenericActuatorState : BaseObjectState
    {
        #region Constructors
        /// <remarks />
        public GenericActuatorState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(Boiler.ObjectTypes.GenericActuatorType, Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAACwAAAB1cm46b3BjdWEub3JnOjIwMjUtMDE6aWlvdC1zdGFydGVya2l0OmJvaWxlcv////8EYIAC" +
           "AQAAAAEAGwAAAEdlbmVyaWNBY3R1YXRvclR5cGVJbnN0YW5jZQEBDgABAQ4ADgAAAP////8CAAAAFWCJ" +
           "CgIAAAABAAUAAABJbnB1dAEBDwAALwEAQAkPAAAAAAv/////AgL/////AQAAABVgqQoCAAAAAAAHAAAA" +
           "RVVSYW5nZQEBEgAALgBEEgAAABYBAHYDARAAAAAAAAAAAAAAAAAAAAAAACRAAQB0A/////8BAf////8A" +
           "AAAAFWCJCgIAAAABABEAAABFbmVyZ3lDb25zdW1wdGlvbgEBHAAALwEAWUQcAAAAAQEbAP////8BAf//" +
           "//8BAAAAFWCpCgIAAAAAABAAAABFbmdpbmVlcmluZ1VuaXRzAQEiAAAuAEQiAAAAFgEAeQMBQgAAAC8A" +
           "AABodHRwOi8vd3d3Lm9wY2ZvdW5kYXRpb24ub3JnL1VBL3VuaXRzL3VuL2NlZmFjdEhXSwACBQAAAGtX" +
           "wrdoAAEAdwP/////AQH/////AAAAAA==";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public AnalogItemState<double> Input
        {
            get
            {
                return m_input;
            }

            set
            {
                if (!Object.ReferenceEquals(m_input, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_input = value;
            }
        }

        /// <remarks />
        public AnalogUnitState<EnergyConsumptionType> EnergyConsumption
        {
            get
            {
                return m_energyConsumption;
            }

            set
            {
                if (!Object.ReferenceEquals(m_energyConsumption, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_energyConsumption = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <remarks />
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_input != null)
            {
                children.Add(m_input);
            }

            if (m_energyConsumption != null)
            {
                children.Add(m_energyConsumption);
            }

            base.GetChildren(context, children);
        }
            
        /// <remarks />
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case Boiler.BrowseNames.Input:
                {
                    if (createOrReplace)
                    {
                        if (Input == null)
                        {
                            if (replacement == null)
                            {
                                Input = new AnalogItemState<double>(this);
                            }
                            else
                            {
                                Input = (AnalogItemState<double>)replacement;
                            }
                        }
                    }

                    instance = Input;
                    break;
                }

                case Boiler.BrowseNames.EnergyConsumption:
                {
                    if (createOrReplace)
                    {
                        if (EnergyConsumption == null)
                        {
                            if (replacement == null)
                            {
                                EnergyConsumption = new AnalogUnitState<EnergyConsumptionType>(this);
                            }
                            else
                            {
                                EnergyConsumption = (AnalogUnitState<EnergyConsumptionType>)replacement;
                            }
                        }
                    }

                    instance = EnergyConsumption;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private AnalogItemState<double> m_input;
        private AnalogUnitState<EnergyConsumptionType> m_energyConsumption;
        #endregion
    }
    #endif
    #endregion

    #region CustomControllerState Class
    #if (!OPCUA_EXCLUDE_CustomControllerState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class CustomControllerState : BaseObjectState
    {
        #region Constructors
        /// <remarks />
        public CustomControllerState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(Boiler.ObjectTypes.CustomControllerType, Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAACwAAAB1cm46b3BjdWEub3JnOjIwMjUtMDE6aWlvdC1zdGFydGVya2l0OmJvaWxlcv////8EYIAC" +
           "AQAAAAEAHAAAAEN1c3RvbUNvbnRyb2xsZXJUeXBlSW5zdGFuY2UBARUAAQEVABUAAAD/////BAAAABVg" +
           "iQoCAAAAAQAGAAAASW5wdXQxAQEWAAAuAEQWAAAAAAv/////AgL/////AAAAABVgiQoCAAAAAQAGAAAA" +
           "SW5wdXQyAQEXAAAuAEQXAAAAAAv/////AgL/////AAAAABVgiQoCAAAAAQAGAAAASW5wdXQzAQEYAAAu" +
           "AEQYAAAAAAv/////AgL/////AAAAABVgiQoCAAAAAQAKAAAAQ29udHJvbE91dAEBGQAALgBEGQAAAAAL" +
           "/////wEB/////wAAAAA=";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public PropertyState<double> Input1
        {
            get
            {
                return m_input1;
            }

            set
            {
                if (!Object.ReferenceEquals(m_input1, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_input1 = value;
            }
        }

        /// <remarks />
        public PropertyState<double> Input2
        {
            get
            {
                return m_input2;
            }

            set
            {
                if (!Object.ReferenceEquals(m_input2, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_input2 = value;
            }
        }

        /// <remarks />
        public PropertyState<double> Input3
        {
            get
            {
                return m_input3;
            }

            set
            {
                if (!Object.ReferenceEquals(m_input3, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_input3 = value;
            }
        }

        /// <remarks />
        public PropertyState<double> ControlOut
        {
            get
            {
                return m_controlOut;
            }

            set
            {
                if (!Object.ReferenceEquals(m_controlOut, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_controlOut = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <remarks />
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_input1 != null)
            {
                children.Add(m_input1);
            }

            if (m_input2 != null)
            {
                children.Add(m_input2);
            }

            if (m_input3 != null)
            {
                children.Add(m_input3);
            }

            if (m_controlOut != null)
            {
                children.Add(m_controlOut);
            }

            base.GetChildren(context, children);
        }
            
        /// <remarks />
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case Boiler.BrowseNames.Input1:
                {
                    if (createOrReplace)
                    {
                        if (Input1 == null)
                        {
                            if (replacement == null)
                            {
                                Input1 = new PropertyState<double>(this);
                            }
                            else
                            {
                                Input1 = (PropertyState<double>)replacement;
                            }
                        }
                    }

                    instance = Input1;
                    break;
                }

                case Boiler.BrowseNames.Input2:
                {
                    if (createOrReplace)
                    {
                        if (Input2 == null)
                        {
                            if (replacement == null)
                            {
                                Input2 = new PropertyState<double>(this);
                            }
                            else
                            {
                                Input2 = (PropertyState<double>)replacement;
                            }
                        }
                    }

                    instance = Input2;
                    break;
                }

                case Boiler.BrowseNames.Input3:
                {
                    if (createOrReplace)
                    {
                        if (Input3 == null)
                        {
                            if (replacement == null)
                            {
                                Input3 = new PropertyState<double>(this);
                            }
                            else
                            {
                                Input3 = (PropertyState<double>)replacement;
                            }
                        }
                    }

                    instance = Input3;
                    break;
                }

                case Boiler.BrowseNames.ControlOut:
                {
                    if (createOrReplace)
                    {
                        if (ControlOut == null)
                        {
                            if (replacement == null)
                            {
                                ControlOut = new PropertyState<double>(this);
                            }
                            else
                            {
                                ControlOut = (PropertyState<double>)replacement;
                            }
                        }
                    }

                    instance = ControlOut;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private PropertyState<double> m_input1;
        private PropertyState<double> m_input2;
        private PropertyState<double> m_input3;
        private PropertyState<double> m_controlOut;
        #endregion
    }
    #endif
    #endregion

    #region ValveState Class
    #if (!OPCUA_EXCLUDE_ValveState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class ValveState : GenericActuatorState
    {
        #region Constructors
        /// <remarks />
        public ValveState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(Boiler.ObjectTypes.ValveType, Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAACwAAAB1cm46b3BjdWEub3JnOjIwMjUtMDE6aWlvdC1zdGFydGVya2l0OmJvaWxlcv////8EYIAC" +
           "AQAAAAEAEQAAAFZhbHZlVHlwZUluc3RhbmNlAQEaAAEBGgAaAAAA/////wIAAAAVYIkKAgAAAAEABQAA" +
           "AElucHV0AgEAQUIPAAAvAQBACUFCDwAAC/////8CAv////8BAAAAFWCpCgIAAAAAAAcAAABFVVJhbmdl" +
           "AgEARUIPAAAuAERFQg8AFgEAdgMBEAAAAAAAAAAAAAAAAAAAAAAAJEABAHQD/////wEB/////wAAAAAV" +
           "YIkKAgAAAAEAEQAAAEVuZXJneUNvbnN1bXB0aW9uAgEAR0IPAAAvAQBZREdCDwABARsA/////wEB////" +
           "/wEAAAAVYKkKAgAAAAAAEAAAAEVuZ2luZWVyaW5nVW5pdHMCAQBMQg8AAC4ARExCDwAWAQB5AwFCAAAA" +
           "LwAAAGh0dHA6Ly93d3cub3BjZm91bmRhdGlvbi5vcmcvVUEvdW5pdHMvdW4vY2VmYWN0SFdLAAIFAAAA" +
           "a1fCt2gAAQB3A/////8BAf////8AAAAA";
        #endregion
        #endif
        #endregion

        #region Public Properties
        #endregion

        #region Overridden Methods
        #endregion

        #region Private Fields
        #endregion
    }
    #endif
    #endregion

    #region LevelControllerState Class
    #if (!OPCUA_EXCLUDE_LevelControllerState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class LevelControllerState : GenericControllerState
    {
        #region Constructors
        /// <remarks />
        public LevelControllerState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(Boiler.ObjectTypes.LevelControllerType, Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAACwAAAB1cm46b3BjdWEub3JnOjIwMjUtMDE6aWlvdC1zdGFydGVya2l0OmJvaWxlcv////8EYIAC" +
           "AQAAAAEAGwAAAExldmVsQ29udHJvbGxlclR5cGVJbnN0YW5jZQEBIQABASEAIQAAAP////8DAAAAFWCJ" +
           "CgIAAAABAAsAAABNZWFzdXJlbWVudAIBAE1CDwAALgBETUIPAAAL/////wEB/////wAAAAAVYIkKAgAA" +
           "AAEACAAAAFNldFBvaW50AgEATkIPAAAuAEROQg8AAAv/////AwP/////AAAAABVgiQoCAAAAAQAKAAAA" +
           "Q29udHJvbE91dAIBAE9CDwAALgBET0IPAAAL/////wEB/////wAAAAA=";
        #endregion
        #endif
        #endregion

        #region Public Properties
        #endregion

        #region Overridden Methods
        #endregion

        #region Private Fields
        #endregion
    }
    #endif
    #endregion

    #region FlowControllerState Class
    #if (!OPCUA_EXCLUDE_FlowControllerState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class FlowControllerState : GenericControllerState
    {
        #region Constructors
        /// <remarks />
        public FlowControllerState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(Boiler.ObjectTypes.FlowControllerType, Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAACwAAAB1cm46b3BjdWEub3JnOjIwMjUtMDE6aWlvdC1zdGFydGVya2l0OmJvaWxlcv////8EYIAC" +
           "AQAAAAEAGgAAAEZsb3dDb250cm9sbGVyVHlwZUluc3RhbmNlAQElAAEBJQAlAAAA/////wMAAAAVYIkK" +
           "AgAAAAEACwAAAE1lYXN1cmVtZW50AgEAUEIPAAAuAERQQg8AAAv/////AQH/////AAAAABVgiQoCAAAA" +
           "AQAIAAAAU2V0UG9pbnQCAQBRQg8AAC4ARFFCDwAAC/////8DA/////8AAAAAFWCJCgIAAAABAAoAAABD" +
           "b250cm9sT3V0AgEAUkIPAAAuAERSQg8AAAv/////AQH/////AAAAAA==";
        #endregion
        #endif
        #endregion

        #region Public Properties
        #endregion

        #region Overridden Methods
        #endregion

        #region Private Fields
        #endregion
    }
    #endif
    #endregion

    #region LevelIndicatorState Class
    #if (!OPCUA_EXCLUDE_LevelIndicatorState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class LevelIndicatorState : GenericSensorState
    {
        #region Constructors
        /// <remarks />
        public LevelIndicatorState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(Boiler.ObjectTypes.LevelIndicatorType, Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAACwAAAB1cm46b3BjdWEub3JnOjIwMjUtMDE6aWlvdC1zdGFydGVya2l0OmJvaWxlcv////8EYIAC" +
           "AQAAAAEAGgAAAExldmVsSW5kaWNhdG9yVHlwZUluc3RhbmNlAQEpAAEBKQApAAAA/////wEAAAAVYIkK" +
           "AgAAAAEABgAAAE91dHB1dAIBAFNCDwAALwEAokRTQg8AAAv/////AQH/////AgAAABVgqQoCAAAAAAAH" +
           "AAAARVVSYW5nZQIBAFdCDwAALgBEV0IPABYBAHYDARAAAAAAAAAAAAAAAAAAAAAAAFlAAQB0A/////8B" +
           "Af////8AAAAAFWCpCgIAAAAAABAAAABFbmdpbmVlcmluZ1VuaXRzAgEAWEIPAAAuAERYQg8AFgEAeQMB" +
           "VAAAAC8AAABodHRwOi8vd3d3Lm9wY2ZvdW5kYXRpb24ub3JnL1VBL3VuaXRzL3VuL2NlZmFjdFNUTQAC" +
           "AwAAAG0vcwIQAAAAbWV0cmUgcGVyIHNlY29uZAEAdwP/////AwP/////AAAAAA==";
        #endregion
        #endif
        #endregion

        #region Public Properties
        #endregion

        #region Overridden Methods
        #endregion

        #region Private Fields
        #endregion
    }
    #endif
    #endregion

    #region FlowTransmitterState Class
    #if (!OPCUA_EXCLUDE_FlowTransmitterState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class FlowTransmitterState : GenericSensorState
    {
        #region Constructors
        /// <remarks />
        public FlowTransmitterState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(Boiler.ObjectTypes.FlowTransmitterType, Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAACwAAAB1cm46b3BjdWEub3JnOjIwMjUtMDE6aWlvdC1zdGFydGVya2l0OmJvaWxlcv////8EYIAC" +
           "AQAAAAEAGwAAAEZsb3dUcmFuc21pdHRlclR5cGVJbnN0YW5jZQEBMAABATAAMAAAAP////8BAAAAFWCJ" +
           "CgIAAAABAAYAAABPdXRwdXQCAQBZQg8AAC8BAKJEWUIPAAAL/////wEB/////wIAAAAVYKkKAgAAAAAA" +
           "BwAAAEVVUmFuZ2UCAQBdQg8AAC4ARF1CDwAWAQB2AwEQAAAAAAAAAAAAAAAAAAAAAABZQAEAdAP/////" +
           "AQH/////AAAAABVgqQoCAAAAAAAQAAAARW5naW5lZXJpbmdVbml0cwIBAF5CDwAALgBEXkIPABYBAHkD" +
           "AVQAAAAvAAAAaHR0cDovL3d3dy5vcGNmb3VuZGF0aW9uLm9yZy9VQS91bml0cy91bi9jZWZhY3RTVE0A" +
           "AgMAAABtL3MCEAAAAG1ldHJlIHBlciBzZWNvbmQBAHcD/////wMD/////wAAAAA=";
        #endregion
        #endif
        #endregion

        #region Public Properties
        #endregion

        #region Overridden Methods
        #endregion

        #region Private Fields
        #endregion
    }
    #endif
    #endregion

    #region BoilerInputPipeState Class
    #if (!OPCUA_EXCLUDE_BoilerInputPipeState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class BoilerInputPipeState : FolderState
    {
        #region Constructors
        /// <remarks />
        public BoilerInputPipeState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(Boiler.ObjectTypes.BoilerInputPipeType, Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAACwAAAB1cm46b3BjdWEub3JnOjIwMjUtMDE6aWlvdC1zdGFydGVya2l0OmJvaWxlcv////8EYIAC" +
           "AQAAAAEAGwAAAEJvaWxlcklucHV0UGlwZVR5cGVJbnN0YW5jZQEBSQABAUkASQAAAAEAAAAAMAABAUoA" +
           "AgAAAIRgwAoBAAAAEAAAAEZsb3dUcmFuc21pdHRlcjEBAAYAAABGVFgwMDEBAUoAAC8BATAASgAAAAEB" +
           "AAAAADABAQFJAAEAAAAVYIkKAgAAAAEABgAAAE91dHB1dAEBSwAALwEAokRLAAAAAAv/////AQH/////" +
           "AgAAABVgqQoCAAAAAAAHAAAARVVSYW5nZQEBTgAALgBETgAAABYBAHYDARAAAAAAAAAAAAAAAAAAAAAA" +
           "ADRAAQB0A/////8BAf////8AAAAAFWCpCgIAAAAAABAAAABFbmdpbmVlcmluZ1VuaXRzAQFQAAAuAERQ" +
           "AAAAFgEAeQMBVAAAAC8AAABodHRwOi8vd3d3Lm9wY2ZvdW5kYXRpb24ub3JnL1VBL3VuaXRzL3VuL2Nl" +
           "ZmFjdDE1RwACAwAAAGwvcwIQAAAAbGl0cmUgcGVyIHNlY29uZAEAdwP/////AwP/////AAAAAIRgwAoB" +
           "AAAABQAAAFZhbHZlAQAJAAAAVmFsdmVYMDAxAQFRAAAvAQEaAFEAAAAB/////wIAAAAVYIkKAgAAAAEA" +
           "BQAAAElucHV0AQFSAAAvAQBACVIAAAAAC/////8CAv////8BAAAAFWCpCgIAAAAAAAcAAABFVVJhbmdl" +
           "AQFVAAAuAERVAAAAFgEAdgMBEAAAAAAAAAAAAAAAAAAAAAAAJEABAHQD/////wEB/////wAAAAAVYIkK" +
           "AgAAAAEAEQAAAEVuZXJneUNvbnN1bXB0aW9uAQEjAAAvAQBZRCMAAAABARsA/////wEB/////wEAAAAV" +
           "YKkKAgAAAAAAEAAAAEVuZ2luZWVyaW5nVW5pdHMBASoAAC4ARCoAAAAWAQB5AwFCAAAALwAAAGh0dHA6" +
           "Ly93d3cub3BjZm91bmRhdGlvbi5vcmcvVUEvdW5pdHMvdW4vY2VmYWN0SFdLAAIFAAAAa1fCt2gAAQB3" +
           "A/////8BAf////8AAAAA";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public FlowTransmitterState FlowTransmitter1
        {
            get
            {
                return m_flowTransmitter1;
            }

            set
            {
                if (!Object.ReferenceEquals(m_flowTransmitter1, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_flowTransmitter1 = value;
            }
        }

        /// <remarks />
        public ValveState Valve
        {
            get
            {
                return m_valve;
            }

            set
            {
                if (!Object.ReferenceEquals(m_valve, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_valve = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <remarks />
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_flowTransmitter1 != null)
            {
                children.Add(m_flowTransmitter1);
            }

            if (m_valve != null)
            {
                children.Add(m_valve);
            }

            base.GetChildren(context, children);
        }
            
        /// <remarks />
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case Boiler.BrowseNames.FlowTransmitter1:
                {
                    if (createOrReplace)
                    {
                        if (FlowTransmitter1 == null)
                        {
                            if (replacement == null)
                            {
                                FlowTransmitter1 = new FlowTransmitterState(this);
                            }
                            else
                            {
                                FlowTransmitter1 = (FlowTransmitterState)replacement;
                            }
                        }
                    }

                    instance = FlowTransmitter1;
                    break;
                }

                case Boiler.BrowseNames.Valve:
                {
                    if (createOrReplace)
                    {
                        if (Valve == null)
                        {
                            if (replacement == null)
                            {
                                Valve = new ValveState(this);
                            }
                            else
                            {
                                Valve = (ValveState)replacement;
                            }
                        }
                    }

                    instance = Valve;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private FlowTransmitterState m_flowTransmitter1;
        private ValveState m_valve;
        #endregion
    }
    #endif
    #endregion

    #region BoilerDrumState Class
    #if (!OPCUA_EXCLUDE_BoilerDrumState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class BoilerDrumState : FolderState
    {
        #region Constructors
        /// <remarks />
        public BoilerDrumState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(Boiler.ObjectTypes.BoilerDrumType, Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAACwAAAB1cm46b3BjdWEub3JnOjIwMjUtMDE6aWlvdC1zdGFydGVya2l0OmJvaWxlcv////8EYIAC" +
           "AQAAAAEAFgAAAEJvaWxlckRydW1UeXBlSW5zdGFuY2UBAVgAAQFYAFgAAAABAAAAADAAAQFZAAEAAACE" +
           "YMAKAQAAAA4AAABMZXZlbEluZGljYXRvcgEABgAAAExJWDAwMQEBWQAALwEBKQBZAAAAAQEAAAAAMAEB" +
           "AVgAAQAAABVgiQoCAAAAAQAGAAAAT3V0cHV0AQFaAAAvAQCiRFoAAAAAC/////8BAf////8CAAAAFWCp" +
           "CgIAAAAAAAcAAABFVVJhbmdlAQFdAAAuAERdAAAAFgEAdgMBEAAAAAAAAAAAAAAAAAAAAADAckABAHQD" +
           "/////wEB/////wAAAAAVYKkKAgAAAAAAEAAAAEVuZ2luZWVyaW5nVW5pdHMBAV8AAC4ARF8AAAAWAQB5" +
           "AwFNAAAALwAAAGh0dHA6Ly93d3cub3BjZm91bmRhdGlvbi5vcmcvVUEvdW5pdHMvdW4vY2VmYWN0VE1D" +
           "AAICAAAAY20CCgAAAGNlbnRpbWV0cmUBAHcD/////wMD/////wAAAAA=";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public LevelIndicatorState LevelIndicator
        {
            get
            {
                return m_levelIndicator;
            }

            set
            {
                if (!Object.ReferenceEquals(m_levelIndicator, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_levelIndicator = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <remarks />
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_levelIndicator != null)
            {
                children.Add(m_levelIndicator);
            }

            base.GetChildren(context, children);
        }
            
        /// <remarks />
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case Boiler.BrowseNames.LevelIndicator:
                {
                    if (createOrReplace)
                    {
                        if (LevelIndicator == null)
                        {
                            if (replacement == null)
                            {
                                LevelIndicator = new LevelIndicatorState(this);
                            }
                            else
                            {
                                LevelIndicator = (LevelIndicatorState)replacement;
                            }
                        }
                    }

                    instance = LevelIndicator;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private LevelIndicatorState m_levelIndicator;
        #endregion
    }
    #endif
    #endregion

    #region BoilerOutputPipeState Class
    #if (!OPCUA_EXCLUDE_BoilerOutputPipeState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class BoilerOutputPipeState : FolderState
    {
        #region Constructors
        /// <remarks />
        public BoilerOutputPipeState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(Boiler.ObjectTypes.BoilerOutputPipeType, Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAACwAAAB1cm46b3BjdWEub3JnOjIwMjUtMDE6aWlvdC1zdGFydGVya2l0OmJvaWxlcv////8EYIAC" +
           "AQAAAAEAHAAAAEJvaWxlck91dHB1dFBpcGVUeXBlSW5zdGFuY2UBAWAAAQFgAGAAAAABAAAAADAAAQFh" +
           "AAEAAACEYMAKAQAAABAAAABGbG93VHJhbnNtaXR0ZXIyAQAGAAAARlRYMDAyAQFhAAAvAQEwAGEAAAAB" +
           "AQAAAAAwAQEBYAABAAAAFWCJCgIAAAABAAYAAABPdXRwdXQBAWIAAC8BAKJEYgAAAAAL/////wEB////" +
           "/wIAAAAVYKkKAgAAAAAABwAAAEVVUmFuZ2UBAWUAAC4ARGUAAAAWAQB2AwEQAAAAAAAAAAAAWUAAAAAA" +
           "AIjDQAEAdAP/////AQH/////AAAAABVgqQoCAAAAAAAQAAAARW5naW5lZXJpbmdVbml0cwEBZwAALgBE" +
           "ZwAAABYBAHkDAUkAAAAvAAAAaHR0cDovL3d3dy5vcGNmb3VuZGF0aW9uLm9yZy9VQS91bml0cy91bi9j" +
           "ZWZhY3RMQVAAAgIAAABQYQIGAAAAcGFzY2FsAQB3A/////8DA/////8AAAAA";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public FlowTransmitterState FlowTransmitter2
        {
            get
            {
                return m_flowTransmitter2;
            }

            set
            {
                if (!Object.ReferenceEquals(m_flowTransmitter2, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_flowTransmitter2 = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <remarks />
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_flowTransmitter2 != null)
            {
                children.Add(m_flowTransmitter2);
            }

            base.GetChildren(context, children);
        }
            
        /// <remarks />
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case Boiler.BrowseNames.FlowTransmitter2:
                {
                    if (createOrReplace)
                    {
                        if (FlowTransmitter2 == null)
                        {
                            if (replacement == null)
                            {
                                FlowTransmitter2 = new FlowTransmitterState(this);
                            }
                            else
                            {
                                FlowTransmitter2 = (FlowTransmitterState)replacement;
                            }
                        }
                    }

                    instance = FlowTransmitter2;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private FlowTransmitterState m_flowTransmitter2;
        #endregion
    }
    #endif
    #endregion

    #region ChangeSetPointsMethodState Class
    #if (!OPCUA_EXCLUDE_ChangeSetPointsMethodState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class ChangeSetPointsMethodState : MethodState
    {
        #region Constructors
        /// <remarks />
        public ChangeSetPointsMethodState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        public new static NodeState Construct(NodeState parent)
        {
            return new ChangeSetPointsMethodState(parent);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAACwAAAB1cm46b3BjdWEub3JnOjIwMjUtMDE6aWlvdC1zdGFydGVya2l0OmJvaWxlcv////8EYYIK" +
           "BAAAAAEAGQAAAENoYW5nZVNldFBvaW50c01ldGhvZFR5cGUBAUIAAC8BAUIAQgAAAAEB/////wIAAAAX" +
           "YKkKAgAAAAAADgAAAElucHV0QXJndW1lbnRzAQFDAAAuAERDAAAAlgEAAAABACoBARgAAAAHAAAAQ2hh" +
           "bmdlcwEB3AD/////AAAAAAABACgBAQAAAAEAAAABAAAAAQH/////AAAAABdgqQoCAAAAAAAPAAAAT3V0" +
           "cHV0QXJndW1lbnRzAQFEAAAuAEREAAAAlgIAAAABACoBAR4AAAAPAAAAT3BlcmF0aW9uUmVzdWx0ABP/" +
           "////AAAAAAABACoBAR4AAAANAAAAQ3VycmVudFZhbHVlcwEB3QD/////AAAAAAABACgBAQAAAAEAAAAC" +
           "AAAAAQH/////AAAAAA==";
        #endregion
        #endif
        #endregion

        #region Event Callbacks
        /// <remarks />
        public ChangeSetPointsMethodStateMethodCallHandler OnCall;
        #endregion

        #region Public Properties
        #endregion

        #region Overridden Methods
        /// <remarks />
        protected override ServiceResult Call(
            ISystemContext _context,
            NodeId _objectId,
            IList<object> _inputArguments,
            IList<object> _outputArguments)
        {
            if (OnCall == null)
            {
                return base.Call(_context, _objectId, _inputArguments, _outputArguments);
            }

            ServiceResult _result = null;

            ChangeSetPointsRequest changes = (ChangeSetPointsRequest)ExtensionObject.ToEncodeable((ExtensionObject)_inputArguments[0]);

            StatusCode operationResult = (StatusCode)_outputArguments[0];
            ChangeSetPointsResponse currentValues = (ChangeSetPointsResponse)_outputArguments[1];

            if (OnCall != null)
            {
                _result = OnCall(
                    _context,
                    this,
                    _objectId,
                    changes,
                    ref operationResult,
                    ref currentValues);
            }

            _outputArguments[0] = operationResult;
            _outputArguments[1] = currentValues;

            return _result;
        }
        #endregion

        #region Private Fields
        #endregion
    }

    /// <remarks />
    /// <exclude />
    public delegate ServiceResult ChangeSetPointsMethodStateMethodCallHandler(
        ISystemContext _context,
        MethodState _method,
        NodeId _objectId,
        ChangeSetPointsRequest changes,
        ref StatusCode operationResult,
        ref ChangeSetPointsResponse currentValues);
    #endif
    #endregion

    #region EmergencyShutdownMethodState Class
    #if (!OPCUA_EXCLUDE_EmergencyShutdownMethodState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class EmergencyShutdownMethodState : MethodState
    {
        #region Constructors
        /// <remarks />
        public EmergencyShutdownMethodState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        public new static NodeState Construct(NodeState parent)
        {
            return new EmergencyShutdownMethodState(parent);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAACwAAAB1cm46b3BjdWEub3JnOjIwMjUtMDE6aWlvdC1zdGFydGVya2l0OmJvaWxlcv////8EYYIK" +
           "BAAAAAEAGwAAAEVtZXJnZW5jeVNodXRkb3duTWV0aG9kVHlwZQEBRQAALwEBRQBFAAAAAQH/////AgAA" +
           "ABdgqQoCAAAAAAAOAAAASW5wdXRBcmd1bWVudHMBAUYAAC4AREYAAACWAQAAAAEAKgEBFQAAAAYAAABS" +
           "ZWFzb24ADP////8AAAAAAAEAKAEBAAAAAQAAAAEAAAABAf////8AAAAAF2CpCgIAAAAAAA8AAABPdXRw" +
           "dXRBcmd1bWVudHMBAUcAAC4AREcAAACWAQAAAAEAKgEBHgAAAA0AAABFc3RpbWF0ZWRUaW1lAQAiAf//" +
           "//8AAAAAAAEAKAEBAAAAAQAAAAEAAAABAf////8AAAAA";
        #endregion
        #endif
        #endregion

        #region Event Callbacks
        /// <remarks />
        public EmergencyShutdownMethodStateMethodCallHandler OnCall;
        #endregion

        #region Public Properties
        #endregion

        #region Overridden Methods
        /// <remarks />
        protected override ServiceResult Call(
            ISystemContext _context,
            NodeId _objectId,
            IList<object> _inputArguments,
            IList<object> _outputArguments)
        {
            if (OnCall == null)
            {
                return base.Call(_context, _objectId, _inputArguments, _outputArguments);
            }

            ServiceResult _result = null;

            string reason = (string)_inputArguments[0];

            double estimatedTime = (double)_outputArguments[0];

            if (OnCall != null)
            {
                _result = OnCall(
                    _context,
                    this,
                    _objectId,
                    reason,
                    ref estimatedTime);
            }

            _outputArguments[0] = estimatedTime;

            return _result;
        }
        #endregion

        #region Private Fields
        #endregion
    }

    /// <remarks />
    /// <exclude />
    public delegate ServiceResult EmergencyShutdownMethodStateMethodCallHandler(
        ISystemContext _context,
        MethodState _method,
        NodeId _objectId,
        string reason,
        ref double estimatedTime);
    #endif
    #endregion

    #region RestartMethodState Class
    #if (!OPCUA_EXCLUDE_RestartMethodState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class RestartMethodState : MethodState
    {
        #region Constructors
        /// <remarks />
        public RestartMethodState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        public new static NodeState Construct(NodeState parent)
        {
            return new RestartMethodState(parent);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AQAAACwAAAB1cm46b3BjdWEub3JnOjIwMjUtMDE6aWlvdC1zdGFydGVya2l0OmJvaWxlcv////8EYYIK" +
           "BAAAAAEAEQAAAFJlc3RhcnRNZXRob2RUeXBlAQFIAAAvAQFIAEgAAAABAf////8CAAAAF2CpCgIAAAAA" +
           "AA4AAABJbnB1dEFyZ3VtZW50cwEBxgAALgBExgAAAJYBAAAAAQAqAQEVAAAABgAAAFJlYXNvbgAM////" +
           "/wAAAAAAAQAoAQEAAAABAAAAAQAAAAEB/////wAAAAAXYKkKAgAAAAAADwAAAE91dHB1dEFyZ3VtZW50" +
           "cwEBxwAALgBExwAAAJYBAAAAAQAqAQEeAAAADQAAAEVzdGltYXRlZFRpbWUBACIB/////wAAAAAAAQAo" +
           "AQEAAAABAAAAAQAAAAEB/////wAAAAA=";
        #endregion
        #endif
        #endregion

        #region Event Callbacks
        /// <remarks />
        public RestartMethodStateMethodCallHandler OnCall;
        #endregion

        #region Public Properties
        #endregion

        #region Overridden Methods
        /// <remarks />
        protected override ServiceResult Call(
            ISystemContext _context,
            NodeId _objectId,
            IList<object> _inputArguments,
            IList<object> _outputArguments)
        {
            if (OnCall == null)
            {
                return base.Call(_context, _objectId, _inputArguments, _outputArguments);
            }

            ServiceResult _result = null;

            string reason = (string)_inputArguments[0];

            double estimatedTime = (double)_outputArguments[0];

            if (OnCall != null)
            {
                _result = OnCall(
                    _context,
                    this,
                    _objectId,
                    reason,
                    ref estimatedTime);
            }

            _outputArguments[0] = estimatedTime;

            return _result;
        }
        #endregion

        #region Private Fields
        #endregion
    }

    /// <remarks />
    /// <exclude />
    public delegate ServiceResult RestartMethodStateMethodCallHandler(
        ISystemContext _context,
        MethodState _method,
        NodeId _objectId,
        string reason,
        ref double estimatedTime);
    #endif
    #endregion

    #region BoilerState Class
    #if (!OPCUA_EXCLUDE_BoilerState)
    /// <remarks />
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class BoilerState : BaseObjectState
    {
        #region Constructors
        /// <remarks />
        public BoilerState(NodeState parent) : base(parent)
        {
        }

        /// <remarks />
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(Boiler.ObjectTypes.BoilerType, Boiler.Namespaces.Boiler, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <remarks />
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <remarks />
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <remarks />
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "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" +
           "IQCBAAAA/////wMAAAAVYIkKAgAAAAEACwAAAE1lYXN1cmVtZW50AQGCAAAuAESCAAAAAAv/////AQEB" +
           "AAAAAQECAAEBATsAAAAAADVgiQoCAAAAAQAIAAAAU2V0UG9pbnQBAYMAAwAAAAA1AAAAVGhlIHNldCBw" +
           "b2ludCBmb3IgdGhlIGxldmVsIG9mIHRoZSB3YXRlciBpbiB0aGUgZHJ1bS4ALgBEgwAAAAAL/////wMD" +
           "/////wAAAAAVYIkKAgAAAAEACgAAAENvbnRyb2xPdXQBAYQAAC4ARIQAAAAAC/////8BAQEAAAABAQIA" +
           "AAEBhgAAAAAABGDACgEAAAAQAAAAQ3VzdG9tQ29udHJvbGxlcgEABgAAAENDWDAwMQEBhQAALwEBFQCF" +
           "AAAA/////wQAAAAVYIkKAgAAAAEABgAAAElucHV0MQEBhgAALgBEhgAAAAAL/////wICAQAAAAEBAgAB" +
           "AQGEAAAAAAAVYIkKAgAAAAEABgAAAElucHV0MgEBhwAALgBEhwAAAAAL/////wICAQAAAAEBAgABAQFp" +
           "AAAAAAAVYIkKAgAAAAEABgAAAElucHV0MwEBiAAALgBEiAAAAAAL/////wICAQAAAAEBAgABAQF3AAAA" +
           "AAAVYIkKAgAAAAEACgAAAENvbnRyb2xPdXQBAYkAAC4ARIkAAAAAC/////8BAQEAAAABAQIAAAEBfwAA" +
           "AAAABGGCCgQAAAABAA8AAABDaGFuZ2VTZXRQb2ludHMBAcgAAC8BAcgAyAAAAAEB/////wIAAAAXYKkK" +
           "AgAAAAAADgAAAElucHV0QXJndW1lbnRzAQHJAAAuAETJAAAAlgEAAAABACoBARgAAAAHAAAAQ2hhbmdl" +
           "cwEB3AD/////AAAAAAABACgBAQAAAAEAAAABAAAAAQH/////AAAAABdgqQoCAAAAAAAPAAAAT3V0cHV0" +
           "QXJndW1lbnRzAQHKAAAuAETKAAAAlgIAAAABACoBAR4AAAAPAAAAT3BlcmF0aW9uUmVzdWx0ABP/////" +
           "AAAAAAABACoBAR4AAAANAAAAQ3VycmVudFZhbHVlcwEB3QD/////AAAAAAABACgBAQAAAAEAAAACAAAA" +
           "AQH/////AAAAAARhggoEAAAAAQARAAAARW1lcmdlbmN5U2h1dGRvd24BAcsAAC8BAcsAywAAAAEB////" +
           "/wIAAAAXYKkKAgAAAAAADgAAAElucHV0QXJndW1lbnRzAQHMAAAuAETMAAAAlgEAAAABACoBARUAAAAG" +
           "AAAAUmVhc29uAAz/////AAAAAAABACgBAQAAAAEAAAABAAAAAQH/////AAAAABdgqQoCAAAAAAAPAAAA" +
           "T3V0cHV0QXJndW1lbnRzAQHNAAAuAETNAAAAlgEAAAABACoBAR4AAAANAAAARXN0aW1hdGVkVGltZQEA" +
           "IgH/////AAAAAAABACgBAQAAAAEAAAABAAAAAQH/////AAAAAARhggoEAAAAAQAHAAAAUmVzdGFydAEB" +
           "zgAALwEBzgDOAAAAAQH/////AgAAABdgqQoCAAAAAAAOAAAASW5wdXRBcmd1bWVudHMBAc8AAC4ARM8A" +
           "AACWAQAAAAEAKgEBFQAAAAYAAABSZWFzb24ADP////8AAAAAAAEAKAEBAAAAAQAAAAEAAAABAf////8A" +
           "AAAAF2CpCgIAAAAAAA8AAABPdXRwdXRBcmd1bWVudHMBAdAAAC4ARNAAAACWAQAAAAEAKgEBHgAAAA0A" +
           "AABFc3RpbWF0ZWRUaW1lAQAiAf////8AAAAAAAEAKAEBAAAAAQAAAAEAAAABAf////8AAAAA";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public BoilerInputPipeState InputPipe
        {
            get
            {
                return m_inputPipe;
            }

            set
            {
                if (!Object.ReferenceEquals(m_inputPipe, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_inputPipe = value;
            }
        }

        /// <remarks />
        public BoilerDrumState Drum
        {
            get
            {
                return m_drum;
            }

            set
            {
                if (!Object.ReferenceEquals(m_drum, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_drum = value;
            }
        }

        /// <remarks />
        public BoilerOutputPipeState OutputPipe
        {
            get
            {
                return m_outputPipe;
            }

            set
            {
                if (!Object.ReferenceEquals(m_outputPipe, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_outputPipe = value;
            }
        }

        /// <remarks />
        public FlowControllerState FlowController
        {
            get
            {
                return m_flowController;
            }

            set
            {
                if (!Object.ReferenceEquals(m_flowController, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_flowController = value;
            }
        }

        /// <remarks />
        public LevelControllerState LevelController
        {
            get
            {
                return m_levelController;
            }

            set
            {
                if (!Object.ReferenceEquals(m_levelController, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_levelController = value;
            }
        }

        /// <remarks />
        public CustomControllerState CustomController
        {
            get
            {
                return m_customController;
            }

            set
            {
                if (!Object.ReferenceEquals(m_customController, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_customController = value;
            }
        }

        /// <remarks />
        public ChangeSetPointsMethodState ChangeSetPoints
        {
            get
            {
                return m_changeSetPointsMethod;
            }

            set
            {
                if (!Object.ReferenceEquals(m_changeSetPointsMethod, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_changeSetPointsMethod = value;
            }
        }

        /// <remarks />
        public EmergencyShutdownMethodState EmergencyShutdown
        {
            get
            {
                return m_emergencyShutdownMethod;
            }

            set
            {
                if (!Object.ReferenceEquals(m_emergencyShutdownMethod, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_emergencyShutdownMethod = value;
            }
        }

        /// <remarks />
        public RestartMethodState Restart
        {
            get
            {
                return m_restartMethod;
            }

            set
            {
                if (!Object.ReferenceEquals(m_restartMethod, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_restartMethod = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <remarks />
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_inputPipe != null)
            {
                children.Add(m_inputPipe);
            }

            if (m_drum != null)
            {
                children.Add(m_drum);
            }

            if (m_outputPipe != null)
            {
                children.Add(m_outputPipe);
            }

            if (m_flowController != null)
            {
                children.Add(m_flowController);
            }

            if (m_levelController != null)
            {
                children.Add(m_levelController);
            }

            if (m_customController != null)
            {
                children.Add(m_customController);
            }

            if (m_changeSetPointsMethod != null)
            {
                children.Add(m_changeSetPointsMethod);
            }

            if (m_emergencyShutdownMethod != null)
            {
                children.Add(m_emergencyShutdownMethod);
            }

            if (m_restartMethod != null)
            {
                children.Add(m_restartMethod);
            }

            base.GetChildren(context, children);
        }
            
        /// <remarks />
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case Boiler.BrowseNames.InputPipe:
                {
                    if (createOrReplace)
                    {
                        if (InputPipe == null)
                        {
                            if (replacement == null)
                            {
                                InputPipe = new BoilerInputPipeState(this);
                            }
                            else
                            {
                                InputPipe = (BoilerInputPipeState)replacement;
                            }
                        }
                    }

                    instance = InputPipe;
                    break;
                }

                case Boiler.BrowseNames.Drum:
                {
                    if (createOrReplace)
                    {
                        if (Drum == null)
                        {
                            if (replacement == null)
                            {
                                Drum = new BoilerDrumState(this);
                            }
                            else
                            {
                                Drum = (BoilerDrumState)replacement;
                            }
                        }
                    }

                    instance = Drum;
                    break;
                }

                case Boiler.BrowseNames.OutputPipe:
                {
                    if (createOrReplace)
                    {
                        if (OutputPipe == null)
                        {
                            if (replacement == null)
                            {
                                OutputPipe = new BoilerOutputPipeState(this);
                            }
                            else
                            {
                                OutputPipe = (BoilerOutputPipeState)replacement;
                            }
                        }
                    }

                    instance = OutputPipe;
                    break;
                }

                case Boiler.BrowseNames.FlowController:
                {
                    if (createOrReplace)
                    {
                        if (FlowController == null)
                        {
                            if (replacement == null)
                            {
                                FlowController = new FlowControllerState(this);
                            }
                            else
                            {
                                FlowController = (FlowControllerState)replacement;
                            }
                        }
                    }

                    instance = FlowController;
                    break;
                }

                case Boiler.BrowseNames.LevelController:
                {
                    if (createOrReplace)
                    {
                        if (LevelController == null)
                        {
                            if (replacement == null)
                            {
                                LevelController = new LevelControllerState(this);
                            }
                            else
                            {
                                LevelController = (LevelControllerState)replacement;
                            }
                        }
                    }

                    instance = LevelController;
                    break;
                }

                case Boiler.BrowseNames.CustomController:
                {
                    if (createOrReplace)
                    {
                        if (CustomController == null)
                        {
                            if (replacement == null)
                            {
                                CustomController = new CustomControllerState(this);
                            }
                            else
                            {
                                CustomController = (CustomControllerState)replacement;
                            }
                        }
                    }

                    instance = CustomController;
                    break;
                }

                case Boiler.BrowseNames.ChangeSetPoints:
                {
                    if (createOrReplace)
                    {
                        if (ChangeSetPoints == null)
                        {
                            if (replacement == null)
                            {
                                ChangeSetPoints = new ChangeSetPointsMethodState(this);
                            }
                            else
                            {
                                ChangeSetPoints = (ChangeSetPointsMethodState)replacement;
                            }
                        }
                    }

                    instance = ChangeSetPoints;
                    break;
                }

                case Boiler.BrowseNames.EmergencyShutdown:
                {
                    if (createOrReplace)
                    {
                        if (EmergencyShutdown == null)
                        {
                            if (replacement == null)
                            {
                                EmergencyShutdown = new EmergencyShutdownMethodState(this);
                            }
                            else
                            {
                                EmergencyShutdown = (EmergencyShutdownMethodState)replacement;
                            }
                        }
                    }

                    instance = EmergencyShutdown;
                    break;
                }

                case Boiler.BrowseNames.Restart:
                {
                    if (createOrReplace)
                    {
                        if (Restart == null)
                        {
                            if (replacement == null)
                            {
                                Restart = new RestartMethodState(this);
                            }
                            else
                            {
                                Restart = (RestartMethodState)replacement;
                            }
                        }
                    }

                    instance = Restart;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private BoilerInputPipeState m_inputPipe;
        private BoilerDrumState m_drum;
        private BoilerOutputPipeState m_outputPipe;
        private FlowControllerState m_flowController;
        private LevelControllerState m_levelController;
        private CustomControllerState m_customController;
        private ChangeSetPointsMethodState m_changeSetPointsMethod;
        private EmergencyShutdownMethodState m_emergencyShutdownMethod;
        private RestartMethodState m_restartMethod;
        #endregion
    }
    #endif
    #endregion
}