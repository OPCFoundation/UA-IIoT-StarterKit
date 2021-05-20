using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Text;
using System.Threading;
using Range = Opc.Ua.Range;

namespace MqttAgent.Server
{
    public class GPIOGateMonitor : IDisposable
    {
        private object m_lock;
        private Timer m_simulationTimer;
        private GpioController m_controller;
        private BaseVariableState m_state;
        private BaseVariableState m_cycleTime;
        private BaseVariableState m_cycleCount;
        private DateTime m_nextUpdate;
        private bool m_disposed;

        public GPIOGateMonitor(SystemContext context, object parentLock)
        {
            SystemContext = context;
            m_lock = parentLock;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                    if (m_simulationTimer != null)
                    {
                        m_simulationTimer.Dispose();
                        m_simulationTimer = null;
                    }

                    if (m_controller != null)
                    {
                        m_controller.Dispose();
                        m_controller = null;
                    }
                }

                m_disposed = true;
            }
        }

        public SystemContext SystemContext { get; }

        public NodeState CreateAddressSpace(string baseName, NodeFactory factory)
        {
            BaseObjectState root = factory.CreateObject(null, "", baseName);

            factory.AddRootReference(root, ReferenceTypes.Organizes, true, ObjectIds.ObjectsFolder);
            factory.AddRootNotifier(root);

            List <BaseDataVariableState> variables = new List<BaseDataVariableState>();

            try
            {
                m_state = factory.CreateTwoStateDiscreteItemVariable(
                    root,
                    $"{baseName}.State",
                    "State",
                    "On",
                    "Off");

                m_cycleTime = factory.CreateAnalogItemVariable(
                    root,
                    $"{baseName}.CycleTime",
                    "CycleTime",
                    DataTypeIds.UInt32,
                    ValueRanks.Scalar,
                    (uint)10,
                    new Range() { High = 120, Low = 1 },
                    new EUInformation()
                    {
                        UnitId = 5457219,
                        DisplayName = "s",
                        Description = "second",
                        NamespaceUri = "http://www.opcfoundation.org/UA/units/un/cefact"
                    });

                m_cycleCount = factory.CreateVariable(
                    root,
                    $"{baseName}.CycleCount",
                    "CycleCount",
                    DataTypeIds.UInt32,
                    ValueRanks.Scalar);
            }
            catch (Exception e)
            {
                Utils.Trace(e, "Error creating conveyor belt");
            }

            if (factory.UseGPIO)
            {
                try
                {
                    m_controller = new GpioController();
                    m_controller.OpenPin(17, PinMode.Output);
                    Utils.Trace("GPIO initialized.");
                }
                catch (Exception e)
                {
                    Utils.Trace(e, "GPIO not supported.");
                }
            }

            m_simulationTimer = new Timer(DoSimulation, null, 1000, 1000);

            return root;
        }

        public void DeleteAddressSpace()
        {
            if (m_simulationTimer != null)
            {
                m_simulationTimer.Dispose();
                m_simulationTimer = null;
            }

            if (m_controller != null)
            {
                m_controller.Dispose();
                m_controller = null;
            }
        }

        private void DoSimulation(object state)
        {
            try
            {
                if (m_nextUpdate > DateTime.UtcNow)
                {
                    return;
                }

                lock (m_lock)
                {
                    var newState = !(bool)m_state.Value;

                    if (m_controller != null)
                    {
                        m_controller.Write(17, (newState) ? PinValue.High : PinValue.Low);
                    }

                    m_state.Value = newState;
                    m_state.Timestamp = DateTime.UtcNow;
                    m_state.ClearChangeMasks(SystemContext, false);

                    m_cycleCount.Value = (uint)((uint)m_cycleCount.Value + 1);
                    m_cycleCount.ClearChangeMasks(SystemContext, false);

                    m_nextUpdate = DateTime.UtcNow.AddSeconds((uint)m_cycleTime.Value);
                }
            }
            catch (Exception e)
            {
                Utils.Trace(e, "Unexpected error doing simulation.");
            }
        }
    }
}
