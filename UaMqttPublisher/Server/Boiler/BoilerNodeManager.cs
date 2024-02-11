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

using System.Reflection;
using Opc.Ua;
using Opc.Ua.Server;
using UaMqttPublisher.Server;

namespace Quickstarts.Boiler.Server
{
    /// <summary>
    /// A node manager for a server that exposes several variables.
    /// </summary>
    public class BoilerNodeManager : CustomNodeManager2
    {
        #region Constructors
        /// <summary>
        /// Initializes the node manager.
        /// </summary>
        public BoilerNodeManager(IServerInternal server, ApplicationConfiguration configuration)
        :
            base(server, configuration)
        {
            SystemContext.NodeIdFactory = this;

            // set one namespace for the type model and one names for dynamically created nodes.
            string[] namespaceUrls = new string[2];
            namespaceUrls[0] = Namespaces.Boiler;
            namespaceUrls[1] = Namespaces.Boiler + ":instance";
            SetNamespaces(namespaceUrls);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// An overrideable version of the Dispose.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_simulationTimer != null)
                {
                    Utils.SilentDispose(m_simulationTimer);
                    m_simulationTimer = null;
                }
            }
        }
        #endregion

        #region INodeIdFactory Members
        /// <summary>
        /// Creates the NodeId for the specified node.
        /// </summary>
        public override NodeId New(ISystemContext context, NodeState node)
        {
            // generate a new numeric id in the instance namespace.
            return new NodeId(++m_nodeIdCounter, NamespaceIndexes[1]);
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Loads a node set from a file or resource and addes them to the set of predefined nodes.
        /// </summary>
        protected override NodeStateCollection LoadPredefinedNodes(ISystemContext context)
        {
            NodeStateCollection predefinedNodes = new NodeStateCollection();
            predefinedNodes.LoadFromBinaryResource(context,
                "UaMqttPublisher.Server.Boiler.Quickstarts.Boiler.PredefinedNodes.uanodes",
                typeof(BoilerNodeManager).GetTypeInfo().Assembly,
                true);
            return predefinedNodes;
        }
        #endregion

        #region INodeManager Members
        /// <summary>
        /// Does any initialization required before the address space can be used.
        /// </summary>
        /// <remarks>
        /// The externalReferences is an out parameter that allows the node manager to link to nodes
        /// in other node managers. For example, the 'Objects' node is managed by the CoreNodeManager and
        /// should have a reference to the root folder node(s) exposed by this node manager.  
        /// </remarks>
        public override void CreateAddressSpace(IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            lock (Lock)
            {
                LoadPredefinedNodes(SystemContext, externalReferences);

                // find the untyped Boiler1 node that was created when the model was loaded.
                BaseObjectState passiveNode = (BaseObjectState)FindPredefinedNode(new NodeId(Objects.Boiler1, NamespaceIndexes[0]), typeof(BaseObjectState));

                // convert the untyped node to a typed node that can be manipulated within the server.
                m_boiler1 = new BoilerState(null);
                m_boiler1.Create(SystemContext, passiveNode);

                // replaces the untyped predefined nodes with their strongly typed versions.
                AddPredefinedNode(SystemContext, m_boiler1);

                m_boiler1.ChangeSetPoints.OnCallMethod2 = OnChangeSetPoints;
                m_boiler1.EmergencyShutdown.OnCallMethod2 = OnEmergencyShutdown;
                m_boiler1.Restart.OnCallMethod2 = OnRestart;

                // create a boiler node.
                m_boiler2 = new BoilerState(null);

                // initialize it from the type model and assign unique node ids.
                m_boiler2.Create(
                    SystemContext,
                    null,
                    new QualifiedName("Boiler #2", NamespaceIndexes[1]),
                    null,
                    true);

                // link root to objects folder.
                IList<IReference> references = null;

                if (!externalReferences.TryGetValue(Opc.Ua.ObjectIds.ObjectsFolder, out references))
                {
                    externalReferences[Opc.Ua.ObjectIds.ObjectsFolder] = references = new List<IReference>();
                }

                references.Add(new NodeStateReference(Opc.Ua.ReferenceTypeIds.Organizes, false, m_boiler2.NodeId));

                // store it and all of its children in the pre-defined nodes dictionary for easy look up.
                AddPredefinedNode(SystemContext, m_boiler2);

                m_boiler2.ChangeSetPoints.OnCallMethod2 = OnChangeSetPoints;
                m_boiler2.EmergencyShutdown.OnCallMethod2 = OnEmergencyShutdown;
                m_boiler2.Restart.OnCallMethod2 = OnRestart;

                NodeFactory factory = new NodeFactory()
                {
                    ExternalReferences = externalReferences,
                    NamespaceIndex = NamespaceIndex,
                    SystemContext = SystemContext,
                    TypeTree = Server.TypeTree
                };

                UpdateValues(m_boiler1, 100, 0);
                UpdateValues(m_boiler2, 200, 0);

                m_boiler1.Handle = true;
                m_boiler2.Handle = true;

                // start a simulation that changes the values of the nodes.
                m_simulationTimer = new Timer(DoSimulation, null, 1000, 1000);
            }
        }

        /// <summary>
        /// Frees any resources allocated for the address space.
        /// </summary>
        public override void DeleteAddressSpace()
        {
            lock (Lock)
            {
                base.DeleteAddressSpace();
            }
        }

        /// <summary>
        /// Returns a unique handle for the node.
        /// </summary>
        protected override NodeHandle GetManagerHandle(ServerSystemContext context, NodeId nodeId, IDictionary<NodeId, NodeState> cache)
        {
            lock (Lock)
            {
                // quickly exclude nodes that are not in the namespace.
                if (!IsNodeIdInNamespace(nodeId))
                {
                    return null;
                }

                // check for predefined nodes.
                if (PredefinedNodes != null)
                {
                    NodeState node = null;

                    if (PredefinedNodes.TryGetValue(nodeId, out node))
                    {
                        NodeHandle handle = new NodeHandle();

                        handle.NodeId = nodeId;
                        handle.Validated = true;
                        handle.Node = node;

                        return handle;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Verifies that the specified node exists.
        /// </summary>
        protected override NodeState ValidateNode(
            ServerSystemContext context,
            NodeHandle handle,
            IDictionary<NodeId, NodeState> cache)
        {
            // not valid if no root.
            if (handle == null)
            {
                return null;
            }

            // check if previously validated.
            if (handle.Validated)
            {
                return handle.Node;
            }

            // TBD

            return null;
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Does the simulation.
        /// </summary>
        /// <param name="state">The state.</param>
        private void DoSimulation(object state)
        {
            try
            {
                UpdateValues(m_boiler1);
                m_boiler1.ClearChangeMasks(SystemContext, true);

                UpdateValues(m_boiler2);
                m_boiler2.ClearChangeMasks(SystemContext, true);
            }
            catch (Exception e)
            {
                Utils.Trace(e, "Unexpected error during simulation.");
            }
        }

        private void UpdateValues(BoilerState boiler, double setPoint, double delta)
        {
            // set by user (0, 300)
            boiler.LevelController.SetPoint.Value = setPoint;

            if (boiler.Drum.LevelIndicator.Output.WrappedValue == Variant.Null)
            {
                boiler.Drum.LevelIndicator.Output.Value = setPoint;
            }

            var ratio = (boiler.Drum.LevelIndicator.Output.Value + delta)/300.0;

            // sets CustomController.Input1 (1, 100)
            boiler.LevelController.ControlOut.Value = NewValue(ratio, 1, 100);

            // set by LevelController.ControlOut (1, 100)
            boiler.CustomController.Input1.Value = boiler.LevelController.ControlOut.Value;

            // sets FlowController.SetPoint (1, 100)
            boiler.CustomController.ControlOut.Value = NewValue(ratio, 1, 100);

            // set by CustomController.ControlOut (1, 100)
            boiler.FlowController.SetPoint.Value = boiler.CustomController.ControlOut.Value;

            // sets InputPipe.Valve.Input (0, 10)
            boiler.FlowController.ControlOut.Value = NewValue(ratio, 0, 10);

            // set by FlowController.ControlOut (0, 10)
            boiler.InputPipe.Valve.Input.Value = boiler.FlowController.ControlOut.Value;

            // sets FlowController.Measurement (0, 20)
            // sets CustomController.Input2 (0, 20)
            boiler.InputPipe.FlowTransmitter1.Output.Value = NewValue(ratio, 0, 20); 

            // set by InputPipe.FlowTransmitter1.Output (0, 20)
            boiler.FlowController.Measurement.Value = boiler.InputPipe.FlowTransmitter1.Output.Value;

            // set by InputPipe.FlowTransmitter1.Output (0, 20)
            boiler.CustomController.Input2.Value = boiler.InputPipe.FlowTransmitter1.Output.Value;

            // sets LevelController.Measurement (0, 300)
            boiler.Drum.LevelIndicator.Output.Value = NewValue(ratio, 0, 300);

            // set by Drum.LevelIndicator.Output (0, 300)
            boiler.LevelController.Measurement.Value = boiler.Drum.LevelIndicator.Output.Value;

            // sets CustomController.Input3 (100, 10000)
            boiler.OutputPipe.FlowTransmitter2.Output.Value =  NewValue(ratio, 100, 10000); 

            // set by OutputPipe.FlowTransmitter2.Output (100, 10000)
            boiler.CustomController.Input3.Value = boiler.OutputPipe.FlowTransmitter2.Output.Value;
        }

        private double NewValue(double value, double low, double high)
        {
            return Math.Round(((value * (high - low) + low) + ((m_random.NextDouble() - 0.5) * ((high-low) * 0.01))), 2);
        }

        private void UpdateValues(BoilerState boiler)
        {
            var running = boiler.Handle as bool?;

            if (running == null || !running.Value)
            {
                return;
            }

            var delta = (boiler.LevelController.SetPoint.Value - boiler.Drum.LevelIndicator.Output.Value) * 0.10;
            UpdateValues(boiler, boiler.LevelController.SetPoint.Value, delta);
        }

        public ServiceResult OnEmergencyShutdown(
            ISystemContext context,
            MethodState method,
            NodeId objectId,
            IList<object> inputArguments,
            IList<object> outputArguments)
        {

            if (objectId != m_boiler1.NodeId && objectId != m_boiler2.NodeId)
            {
                return StatusCodes.BadMethodInvalid;
            }

            var boiler = (objectId == m_boiler2.NodeId) ? m_boiler2 : m_boiler1;
            boiler.Handle = false;
            UpdateValues(boiler, 0, 0);
            outputArguments[0] = 1000.0;

            return ServiceResult.Good;
        }

        public ServiceResult OnRestart(
            ISystemContext context,
            MethodState method,
            NodeId objectId,
            IList<object> inputArguments,
            IList<object> outputArguments)
        {

            if (objectId != m_boiler1.NodeId && objectId != m_boiler2.NodeId)
            {
                return StatusCodes.BadMethodInvalid;
            }

            var boiler = (objectId == m_boiler2.NodeId) ? m_boiler2 : m_boiler1;
            boiler.Handle = true;
            outputArguments[0] = 10000000.0;

            return ServiceResult.Good;
        }

        public ServiceResult OnChangeSetPoints(
            ISystemContext context,
            MethodState method,
            NodeId objectId,
            IList<object> inputArguments,
            IList<object> outputArguments)
        {

            if (objectId != m_boiler1.NodeId && objectId != m_boiler2.NodeId)
            {
                return StatusCodes.BadMethodInvalid;
            }

            var boiler = (objectId == m_boiler2.NodeId) ? m_boiler2 : m_boiler1;

            UpdateAndClampValue(boiler.LevelController.SetPoint, inputArguments[0] as double?);
            UpdateAndClampValue(boiler.FlowController.SetPoint, inputArguments[1] as double?);

            outputArguments[0] = boiler.LevelController.SetPoint.Value;
            outputArguments[1] = boiler.LevelController.Measurement.Value;
            outputArguments[2] = boiler.FlowController.SetPoint.Value;
            outputArguments[3] = boiler.FlowController.Measurement.Value;

            return ServiceResult.Good;
        }

        private void UpdateAndClampValue(BaseVariableState variable, double? value)
        {
            if (value == null || value.Value <= 0)
            {
                return;
            }

            if (value.Value > 100)
            {
                value = 100;
            }

            variable.Value = value.Value;
        }
        #endregion

        #region Private Fields
        private BoilerState m_boiler1;
        private BoilerState m_boiler2;
        private uint m_nodeIdCounter;
        private Timer m_simulationTimer;
        private Random m_random = new Random();
        #endregion
    }
}
