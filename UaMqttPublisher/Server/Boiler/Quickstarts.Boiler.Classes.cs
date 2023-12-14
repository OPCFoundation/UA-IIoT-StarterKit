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
            return Opc.Ua.NodeId.Create(Quickstarts.Boiler.ObjectTypes.GenericControllerType, Quickstarts.Boiler.Namespaces.Boiler, namespaceUris);
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
           "AQAAACsAAAB0YWc6b3BjdWEub3JnLDIwMjMtMTE6aW90LXN0YXJ0ZXJraXQ6Ym9pbGVy/////wRggAIB" +
           "AAAAAQAdAAAAR2VuZXJpY0NvbnRyb2xsZXJUeXBlSW5zdGFuY2UBAQMAAQEDAAMAAAD/////AwAAABVg" +
           "iQoCAAAAAQALAAAATWVhc3VyZW1lbnQBAQQAAC4ARAQAAAAAC/////8BAf////8AAAAAFWCJCgIAAAAB" +
           "AAgAAABTZXRQb2ludAEBBQAALgBEBQAAAAAL/////wMD/////wAAAAAVYIkKAgAAAAEACgAAAENvbnRy" +
           "b2xPdXQBAQYAAC4ARAYAAAAAC/////8BAf////8AAAAA";
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
                case Quickstarts.Boiler.BrowseNames.Measurement:
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

                case Quickstarts.Boiler.BrowseNames.SetPoint:
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

                case Quickstarts.Boiler.BrowseNames.ControlOut:
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
            return Opc.Ua.NodeId.Create(Quickstarts.Boiler.ObjectTypes.GenericSensorType, Quickstarts.Boiler.Namespaces.Boiler, namespaceUris);
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
           "AQAAACsAAAB0YWc6b3BjdWEub3JnLDIwMjMtMTE6aW90LXN0YXJ0ZXJraXQ6Ym9pbGVy/////wRggAIB" +
           "AAAAAQAZAAAAR2VuZXJpY1NlbnNvclR5cGVJbnN0YW5jZQEBBwABAQcABwAAAP////8BAAAAFWCJCgIA" +
           "AAABAAYAAABPdXRwdXQBAQgAAC8BAEAJCAAAAAAL/////wEB/////wEAAAAVYIkKAgAAAAAABwAAAEVV" +
           "UmFuZ2UBAQsAAC4ARAsAAAABAHQD/////wEB/////wAAAAA=";
        #endregion
#endif
        #endregion

        #region Public Properties
        /// <remarks />
        public AnalogItemState<double> Output
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
                case Quickstarts.Boiler.BrowseNames.Output:
                {
                    if (createOrReplace)
                    {
                        if (Output == null)
                        {
                            if (replacement == null)
                            {
                                Output = new AnalogItemState<double>(this);
                            }
                            else
                            {
                                Output = (AnalogItemState<double>)replacement;
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
        private AnalogItemState<double> m_output;
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
            return Opc.Ua.NodeId.Create(Quickstarts.Boiler.ObjectTypes.GenericActuatorType, Quickstarts.Boiler.Namespaces.Boiler, namespaceUris);
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
           "AQAAACsAAAB0YWc6b3BjdWEub3JnLDIwMjMtMTE6aW90LXN0YXJ0ZXJraXQ6Ym9pbGVy/////wRggAIB" +
           "AAAAAQAbAAAAR2VuZXJpY0FjdHVhdG9yVHlwZUluc3RhbmNlAQEOAAEBDgAOAAAA/////wEAAAAVYIkK" +
           "AgAAAAEABQAAAElucHV0AQEPAAAvAQBACQ8AAAAAC/////8CAv////8BAAAAFWCJCgIAAAAAAAcAAABF" +
           "VVJhbmdlAQESAAAuAEQSAAAAAQB0A/////8BAf////8AAAAA";
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
                case Quickstarts.Boiler.BrowseNames.Input:
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
            return Opc.Ua.NodeId.Create(Quickstarts.Boiler.ObjectTypes.CustomControllerType, Quickstarts.Boiler.Namespaces.Boiler, namespaceUris);
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
           "AQAAACsAAAB0YWc6b3BjdWEub3JnLDIwMjMtMTE6aW90LXN0YXJ0ZXJraXQ6Ym9pbGVy/////wRggAIB" +
           "AAAAAQAcAAAAQ3VzdG9tQ29udHJvbGxlclR5cGVJbnN0YW5jZQEBFQABARUAFQAAAP////8EAAAAFWCJ" +
           "CgIAAAABAAYAAABJbnB1dDEBARYAAC4ARBYAAAAAC/////8CAv////8AAAAAFWCJCgIAAAABAAYAAABJ" +
           "bnB1dDIBARcAAC4ARBcAAAAAC/////8CAv////8AAAAAFWCJCgIAAAABAAYAAABJbnB1dDMBARgAAC4A" +
           "RBgAAAAAC/////8CAv////8AAAAAFWCJCgIAAAABAAoAAABDb250cm9sT3V0AQEZAAAuAEQZAAAAAAv/" +
           "////AQH/////AAAAAA==";
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
                case Quickstarts.Boiler.BrowseNames.Input1:
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

                case Quickstarts.Boiler.BrowseNames.Input2:
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

                case Quickstarts.Boiler.BrowseNames.Input3:
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

                case Quickstarts.Boiler.BrowseNames.ControlOut:
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
            return Opc.Ua.NodeId.Create(Quickstarts.Boiler.ObjectTypes.ValveType, Quickstarts.Boiler.Namespaces.Boiler, namespaceUris);
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
           "AQAAACsAAAB0YWc6b3BjdWEub3JnLDIwMjMtMTE6aW90LXN0YXJ0ZXJraXQ6Ym9pbGVy/////wRggAIB" +
           "AAAAAQARAAAAVmFsdmVUeXBlSW5zdGFuY2UBARoAAQEaABoAAAD/////AQAAABVgiQoCAAAAAQAFAAAA" +
           "SW5wdXQCAQBBQg8AAC8BAEAJQUIPAAAL/////wIC/////wEAAAAVYIkKAgAAAAAABwAAAEVVUmFuZ2UC" +
           "AQBFQg8AAC4AREVCDwABAHQD/////wEB/////wAAAAA=";
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
            return Opc.Ua.NodeId.Create(Quickstarts.Boiler.ObjectTypes.LevelControllerType, Quickstarts.Boiler.Namespaces.Boiler, namespaceUris);
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
           "AQAAACsAAAB0YWc6b3BjdWEub3JnLDIwMjMtMTE6aW90LXN0YXJ0ZXJraXQ6Ym9pbGVy/////wRggAIB" +
           "AAAAAQAbAAAATGV2ZWxDb250cm9sbGVyVHlwZUluc3RhbmNlAQEhAAEBIQAhAAAA/////wMAAAAVYIkK" +
           "AgAAAAEACwAAAE1lYXN1cmVtZW50AgEAR0IPAAAuAERHQg8AAAv/////AQH/////AAAAABVgiQoCAAAA" +
           "AQAIAAAAU2V0UG9pbnQCAQBIQg8AAC4AREhCDwAAC/////8DA/////8AAAAAFWCJCgIAAAABAAoAAABD" +
           "b250cm9sT3V0AgEASUIPAAAuAERJQg8AAAv/////AQH/////AAAAAA==";
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
            return Opc.Ua.NodeId.Create(Quickstarts.Boiler.ObjectTypes.FlowControllerType, Quickstarts.Boiler.Namespaces.Boiler, namespaceUris);
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
           "AQAAACsAAAB0YWc6b3BjdWEub3JnLDIwMjMtMTE6aW90LXN0YXJ0ZXJraXQ6Ym9pbGVy/////wRggAIB" +
           "AAAAAQAaAAAARmxvd0NvbnRyb2xsZXJUeXBlSW5zdGFuY2UBASUAAQElACUAAAD/////AwAAABVgiQoC" +
           "AAAAAQALAAAATWVhc3VyZW1lbnQCAQBKQg8AAC4AREpCDwAAC/////8BAf////8AAAAAFWCJCgIAAAAB" +
           "AAgAAABTZXRQb2ludAIBAEtCDwAALgBES0IPAAAL/////wMD/////wAAAAAVYIkKAgAAAAEACgAAAENv" +
           "bnRyb2xPdXQCAQBMQg8AAC4ARExCDwAAC/////8BAf////8AAAAA";
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
            return Opc.Ua.NodeId.Create(Quickstarts.Boiler.ObjectTypes.LevelIndicatorType, Quickstarts.Boiler.Namespaces.Boiler, namespaceUris);
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
           "AQAAACsAAAB0YWc6b3BjdWEub3JnLDIwMjMtMTE6aW90LXN0YXJ0ZXJraXQ6Ym9pbGVy/////wRggAIB" +
           "AAAAAQAaAAAATGV2ZWxJbmRpY2F0b3JUeXBlSW5zdGFuY2UBASkAAQEpACkAAAD/////AQAAABVgiQoC" +
           "AAAAAQAGAAAAT3V0cHV0AgEATUIPAAAvAQBACU1CDwAAC/////8BAf////8BAAAAFWCJCgIAAAAAAAcA" +
           "AABFVVJhbmdlAgEAUUIPAAAuAERRQg8AAQB0A/////8BAf////8AAAAA";
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
            return Opc.Ua.NodeId.Create(Quickstarts.Boiler.ObjectTypes.FlowTransmitterType, Quickstarts.Boiler.Namespaces.Boiler, namespaceUris);
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
           "AQAAACsAAAB0YWc6b3BjdWEub3JnLDIwMjMtMTE6aW90LXN0YXJ0ZXJraXQ6Ym9pbGVy/////wRggAIB" +
           "AAAAAQAbAAAARmxvd1RyYW5zbWl0dGVyVHlwZUluc3RhbmNlAQEwAAEBMAAwAAAA/////wEAAAAVYIkK" +
           "AgAAAAEABgAAAE91dHB1dAIBAFNCDwAALwEAQAlTQg8AAAv/////AQH/////AQAAABVgiQoCAAAAAAAH" +
           "AAAARVVSYW5nZQIBAFdCDwAALgBEV0IPAAEAdAP/////AQH/////AAAAAA==";
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
            return Opc.Ua.NodeId.Create(Quickstarts.Boiler.ObjectTypes.BoilerInputPipeType, Quickstarts.Boiler.Namespaces.Boiler, namespaceUris);
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
           "AQAAACsAAAB0YWc6b3BjdWEub3JnLDIwMjMtMTE6aW90LXN0YXJ0ZXJraXQ6Ym9pbGVy/////wRggAIB" +
           "AAAAAQAbAAAAQm9pbGVySW5wdXRQaXBlVHlwZUluc3RhbmNlAQFJAAEBSQBJAAAAAQAAAAAwAAEBSgAC" +
           "AAAAhGDACgEAAAAQAAAARmxvd1RyYW5zbWl0dGVyMQEABgAAAEZUWDAwMQEBSgAALwEBMABKAAAAAQEA" +
           "AAAAMAEBAUkAAQAAABVgiQoCAAAAAQAGAAAAT3V0cHV0AQFLAAAvAQBACUsAAAAAC/////8BAf////8B" +
           "AAAAFWCJCgIAAAAAAAcAAABFVVJhbmdlAQFOAAAuAEROAAAAAQB0A/////8BAf////8AAAAAhGDACgEA" +
           "AAAFAAAAVmFsdmUBAAkAAABWYWx2ZVgwMDEBAVEAAC8BARoAUQAAAAH/////AQAAABVgiQoCAAAAAQAF" +
           "AAAASW5wdXQBAVIAAC8BAEAJUgAAAAAL/////wIC/////wEAAAAVYIkKAgAAAAAABwAAAEVVUmFuZ2UB" +
           "AVUAAC4ARFUAAAABAHQD/////wEB/////wAAAAA=";
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
                case Quickstarts.Boiler.BrowseNames.FlowTransmitter1:
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

                case Quickstarts.Boiler.BrowseNames.Valve:
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
            return Opc.Ua.NodeId.Create(Quickstarts.Boiler.ObjectTypes.BoilerDrumType, Quickstarts.Boiler.Namespaces.Boiler, namespaceUris);
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
           "AQAAACsAAAB0YWc6b3BjdWEub3JnLDIwMjMtMTE6aW90LXN0YXJ0ZXJraXQ6Ym9pbGVy/////wRggAIB" +
           "AAAAAQAWAAAAQm9pbGVyRHJ1bVR5cGVJbnN0YW5jZQEBWAABAVgAWAAAAAEAAAAAMAABAVkAAQAAAIRg" +
           "wAoBAAAADgAAAExldmVsSW5kaWNhdG9yAQAGAAAATElYMDAxAQFZAAAvAQEpAFkAAAABAQAAAAAwAQEB" +
           "WAABAAAAFWCJCgIAAAABAAYAAABPdXRwdXQBAVoAAC8BAEAJWgAAAAAL/////wEB/////wEAAAAVYIkK" +
           "AgAAAAAABwAAAEVVUmFuZ2UBAV0AAC4ARF0AAAABAHQD/////wEB/////wAAAAA=";
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
                case Quickstarts.Boiler.BrowseNames.LevelIndicator:
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
            return Opc.Ua.NodeId.Create(Quickstarts.Boiler.ObjectTypes.BoilerOutputPipeType, Quickstarts.Boiler.Namespaces.Boiler, namespaceUris);
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
           "AQAAACsAAAB0YWc6b3BjdWEub3JnLDIwMjMtMTE6aW90LXN0YXJ0ZXJraXQ6Ym9pbGVy/////wRggAIB" +
           "AAAAAQAcAAAAQm9pbGVyT3V0cHV0UGlwZVR5cGVJbnN0YW5jZQEBYAABAWAAYAAAAAEAAAAAMAABAWEA" +
           "AQAAAIRgwAoBAAAAEAAAAEZsb3dUcmFuc21pdHRlcjIBAAYAAABGVFgwMDIBAWEAAC8BATAAYQAAAAEB" +
           "AAAAADABAQFgAAEAAAAVYIkKAgAAAAEABgAAAE91dHB1dAEBYgAALwEAQAliAAAAAAv/////AQH/////" +
           "AQAAABVgiQoCAAAAAAAHAAAARVVSYW5nZQEBZQAALgBEZQAAAAEAdAP/////AQH/////AAAAAA==";
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
                case Quickstarts.Boiler.BrowseNames.FlowTransmitter2:
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
           "AQAAACsAAAB0YWc6b3BjdWEub3JnLDIwMjMtMTE6aW90LXN0YXJ0ZXJraXQ6Ym9pbGVy/////wRhggoE" +
           "AAAAAQAZAAAAQ2hhbmdlU2V0UG9pbnRzTWV0aG9kVHlwZQEBQgAALwEBQgBCAAAAAQH/////AgAAABdg" +
           "qQoCAAAAAAAOAAAASW5wdXRBcmd1bWVudHMBAUMAAC4AREMAAACWAQAAAAEAKgEBGAAAAAcAAABDaGFu" +
           "Z2VzAQHcAP////8AAAAAAAEAKAEBAAAAAQAAAAEAAAABAf////8AAAAAF2CpCgIAAAAAAA8AAABPdXRw" +
           "dXRBcmd1bWVudHMBAUQAAC4AREQAAACWAgAAAAEAKgEBHgAAAA8AAABPcGVyYXRpb25SZXN1bHQAE///" +
           "//8AAAAAAAEAKgEBHgAAAA0AAABDdXJyZW50VmFsdWVzAQHdAP////8AAAAAAAEAKAEBAAAAAQAAAAIA" +
           "AAABAf////8AAAAA";
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
           "AQAAACsAAAB0YWc6b3BjdWEub3JnLDIwMjMtMTE6aW90LXN0YXJ0ZXJraXQ6Ym9pbGVy/////wRhggoE" +
           "AAAAAQAbAAAARW1lcmdlbmN5U2h1dGRvd25NZXRob2RUeXBlAQFFAAAvAQFFAEUAAAABAf////8CAAAA" +
           "F2CpCgIAAAAAAA4AAABJbnB1dEFyZ3VtZW50cwEBRgAALgBERgAAAJYBAAAAAQAqAQEVAAAABgAAAFJl" +
           "YXNvbgAM/////wAAAAAAAQAoAQEAAAABAAAAAQAAAAEB/////wAAAAAXYKkKAgAAAAAADwAAAE91dHB1" +
           "dEFyZ3VtZW50cwEBRwAALgBERwAAAJYBAAAAAQAqAQEeAAAADQAAAEVzdGltYXRlZFRpbWUBACIB////" +
           "/wAAAAAAAQAoAQEAAAABAAAAAQAAAAEB/////wAAAAA=";
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
           "AQAAACsAAAB0YWc6b3BjdWEub3JnLDIwMjMtMTE6aW90LXN0YXJ0ZXJraXQ6Ym9pbGVy/////wRhggoE" +
           "AAAAAQARAAAAUmVzdGFydE1ldGhvZFR5cGUBAUgAAC8BAUgASAAAAAEB/////wIAAAAXYKkKAgAAAAAA" +
           "DgAAAElucHV0QXJndW1lbnRzAQHGAAAuAETGAAAAlgEAAAABACoBARUAAAAGAAAAUmVhc29uAAz/////" +
           "AAAAAAABACgBAQAAAAEAAAABAAAAAQH/////AAAAABdgqQoCAAAAAAAPAAAAT3V0cHV0QXJndW1lbnRz" +
           "AQHHAAAuAETHAAAAlgEAAAABACoBAR4AAAANAAAARXN0aW1hdGVkVGltZQEAIgH/////AAAAAAABACgB" +
           "AQAAAAEAAAABAAAAAQH/////AAAAAA==";
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
            return Opc.Ua.NodeId.Create(Quickstarts.Boiler.ObjectTypes.BoilerType, Quickstarts.Boiler.Namespaces.Boiler, namespaceUris);
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
           "AQAAACsAAAB0YWc6b3BjdWEub3JnLDIwMjMtMTE6aW90LXN0YXJ0ZXJraXQ6Ym9pbGVy/////4RggAIB" +
           "AAAAAQASAAAAQm9pbGVyVHlwZUluc3RhbmNlAQE3AAEBNwA3AAAAAQMAAAAAMAABATgAADAAAQE5AAAw" +
           "AAEBQQAJAAAAhGDACgEAAAAJAAAASW5wdXRQaXBlAQAIAAAAUGlwZVgwMDEBATgAAC8BAUkAOAAAAAED" +
           "AAAAADABAQE3AAAwAAEBaAABAQEAAAEBOQACAAAAhGDACgEAAAAQAAAARmxvd1RyYW5zbWl0dGVyMQEA" +
           "BgAAAEZUWDAwMQEBaAAALwEBMABoAAAAAQEAAAAAMAEBATgAAQAAABVgiQoCAAAAAQAGAAAAT3V0cHV0" +
           "AQFpAAAvAQBACWkAAAAAC/////8BAQIAAAABAQIAAAEBfgABAQIAAAEBhwABAAAAFWCJCgIAAAAAAAcA" +
           "AABFVVJhbmdlAQFsAAAuAERsAAAAAQB0A/////8BAf////8AAAAAhGDACgEAAAAFAAAAVmFsdmUBAAkA" +
           "AABWYWx2ZVgwMDEBAW8AAC8BARoAbwAAAAH/////AQAAABVgiQoCAAAAAQAFAAAASW5wdXQBAXAAAC8B" +
           "AEAJcAAAAAAL/////wICAQAAAAEBAgABAQGAAAEAAAAVYIkKAgAAAAAABwAAAEVVUmFuZ2UBAXMAAC4A" +
           "RHMAAAABAHQD/////wEB/////wAAAACEYMAKAQAAAAQAAABEcnVtAQAIAAAARHJ1bVgwMDEBATkAAC8B" +
           "AVgAOQAAAAEEAAAAADABAQE3AAEBAQABAQE4AAAwAAEBOgABAQEAAAEBQQABAAAAhGDACgEAAAAOAAAA" +
           "TGV2ZWxJbmRpY2F0b3IBAAYAAABMSVgwMDEBAToAAC8BASkAOgAAAAEBAAAAADABAQE5AAEAAAAVYIkK" +
           "AgAAAAEABgAAAE91dHB1dAEBOwAALwEAQAk7AAAAAAv/////AQEBAAAAAQECAAABAYIAAQAAABVgiQoC" +
           "AAAAAAAHAAAARVVSYW5nZQEBPgAALgBEPgAAAAEAdAP/////AQH/////AAAAAIRgwAoBAAAACgAAAE91" +
           "dHB1dFBpcGUBAAgAAABQaXBlWDAwMgEBQQAALwEBYABBAAAAAQMAAAAAMAEBATcAAQEBAAEBATkAADAA" +
           "AQF2AAEAAACEYMAKAQAAABAAAABGbG93VHJhbnNtaXR0ZXIyAQAGAAAARlRYMDAyAQF2AAAvAQEwAHYA" +
           "AAABAQAAAAAwAQEBQQABAAAAFWCJCgIAAAABAAYAAABPdXRwdXQBAXcAAC8BAEAJdwAAAAAL/////wEB" +
           "AQAAAAEBAgAAAQGIAAEAAAAVYIkKAgAAAAAABwAAAEVVUmFuZ2UBAXoAAC4ARHoAAAABAHQD/////wEB" +
           "/////wAAAAAEYMAKAQAAAA4AAABGbG93Q29udHJvbGxlcgEABgAAAEZDWDAwMQEBfQAALwEBJQB9AAAA" +
           "/////wMAAAAVYIkKAgAAAAEACwAAAE1lYXN1cmVtZW50AQF+AAAuAER+AAAAAAv/////AQEBAAAAAQEC" +
           "AAEBAWkAAAAAABVgiQoCAAAAAQAIAAAAU2V0UG9pbnQBAX8AAC4ARH8AAAAAC/////8DAwEAAAABAQIA" +
           "AQEBiQAAAAAAFWCJCgIAAAABAAoAAABDb250cm9sT3V0AQGAAAAuAESAAAAAAAv/////AQEBAAAAAQEC" +
           "AAABAXAAAAAAAARgwAoBAAAADwAAAExldmVsQ29udHJvbGxlcgEABgAAAExDWDAwMQEBgQAALwEBIQCB" +
           "AAAA/////wMAAAAVYIkKAgAAAAEACwAAAE1lYXN1cmVtZW50AQGCAAAuAESCAAAAAAv/////AQEBAAAA" +
           "AQECAAEBATsAAAAAABVgiQoCAAAAAQAIAAAAU2V0UG9pbnQBAYMAAC4ARIMAAAAAC/////8DA/////8A" +
           "AAAAFWCJCgIAAAABAAoAAABDb250cm9sT3V0AQGEAAAuAESEAAAAAAv/////AQEBAAAAAQECAAABAYYA" +
           "AAAAAARgwAoBAAAAEAAAAEN1c3RvbUNvbnRyb2xsZXIBAAYAAABDQ1gwMDEBAYUAAC8BARUAhQAAAP//" +
           "//8EAAAAFWCJCgIAAAABAAYAAABJbnB1dDEBAYYAAC4ARIYAAAAAC/////8CAgEAAAABAQIAAQEBhAAA" +
           "AAAAFWCJCgIAAAABAAYAAABJbnB1dDIBAYcAAC4ARIcAAAAAC/////8CAgEAAAABAQIAAQEBaQAAAAAA" +
           "FWCJCgIAAAABAAYAAABJbnB1dDMBAYgAAC4ARIgAAAAAC/////8CAgEAAAABAQIAAQEBdwAAAAAAFWCJ" +
           "CgIAAAABAAoAAABDb250cm9sT3V0AQGJAAAuAESJAAAAAAv/////AQEBAAAAAQECAAABAX8AAAAAAARh" +
           "ggoEAAAAAQAPAAAAQ2hhbmdlU2V0UG9pbnRzAQHIAAAvAQHIAMgAAAABAf////8CAAAAF2CpCgIAAAAA" +
           "AA4AAABJbnB1dEFyZ3VtZW50cwEByQAALgBEyQAAAJYBAAAAAQAqAQEYAAAABwAAAENoYW5nZXMBAdwA" +
           "/////wAAAAAAAQAoAQEAAAABAAAAAQAAAAEB/////wAAAAAXYKkKAgAAAAAADwAAAE91dHB1dEFyZ3Vt" +
           "ZW50cwEBygAALgBEygAAAJYCAAAAAQAqAQEeAAAADwAAAE9wZXJhdGlvblJlc3VsdAAT/////wAAAAAA" +
           "AQAqAQEeAAAADQAAAEN1cnJlbnRWYWx1ZXMBAd0A/////wAAAAAAAQAoAQEAAAABAAAAAgAAAAEB////" +
           "/wAAAAAEYYIKBAAAAAEAEQAAAEVtZXJnZW5jeVNodXRkb3duAQHLAAAvAQHLAMsAAAABAf////8CAAAA" +
           "F2CpCgIAAAAAAA4AAABJbnB1dEFyZ3VtZW50cwEBzAAALgBEzAAAAJYBAAAAAQAqAQEVAAAABgAAAFJl" +
           "YXNvbgAM/////wAAAAAAAQAoAQEAAAABAAAAAQAAAAEB/////wAAAAAXYKkKAgAAAAAADwAAAE91dHB1" +
           "dEFyZ3VtZW50cwEBzQAALgBEzQAAAJYBAAAAAQAqAQEeAAAADQAAAEVzdGltYXRlZFRpbWUBACIB////" +
           "/wAAAAAAAQAoAQEAAAABAAAAAQAAAAEB/////wAAAAAEYYIKBAAAAAEABwAAAFJlc3RhcnQBAc4AAC8B" +
           "Ac4AzgAAAAEB/////wIAAAAXYKkKAgAAAAAADgAAAElucHV0QXJndW1lbnRzAQHPAAAuAETPAAAAlgEA" +
           "AAABACoBARUAAAAGAAAAUmVhc29uAAz/////AAAAAAABACgBAQAAAAEAAAABAAAAAQH/////AAAAABdg" +
           "qQoCAAAAAAAPAAAAT3V0cHV0QXJndW1lbnRzAQHQAAAuAETQAAAAlgEAAAABACoBAR4AAAANAAAARXN0" +
           "aW1hdGVkVGltZQEAIgH/////AAAAAAABACgBAQAAAAEAAAABAAAAAQH/////AAAAAA==";
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
                case Quickstarts.Boiler.BrowseNames.InputPipe:
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

                case Quickstarts.Boiler.BrowseNames.Drum:
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

                case Quickstarts.Boiler.BrowseNames.OutputPipe:
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

                case Quickstarts.Boiler.BrowseNames.FlowController:
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

                case Quickstarts.Boiler.BrowseNames.LevelController:
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

                case Quickstarts.Boiler.BrowseNames.CustomController:
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

                case Quickstarts.Boiler.BrowseNames.ChangeSetPoints:
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

                case Quickstarts.Boiler.BrowseNames.EmergencyShutdown:
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

                case Quickstarts.Boiler.BrowseNames.Restart:
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
