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
using System.Device.Gpio;
using Opc.Ua;
using Range = Opc.Ua.Range;

namespace UaMqttPublisher.Server
{
    public class GPIOGateMonitor : IDisposable
    {
        private const int GPIO_PIN = 17;

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
            BaseObjectState root = factory.CreateObject(null, baseName, baseName);

            factory.AddRootReference(root, ReferenceTypes.Organizes, true, ObjectIds.ObjectsFolder);
            factory.AddRootNotifier(root);

            List<BaseDataVariableState> variables = new List<BaseDataVariableState>();

            try
            {
                m_state = factory.CreateTwoStateDiscreteItemVariable(
                    root,
                    $"{baseName}_State",
                    "State",
                    "Closed",
                    "Open");

                m_cycleTime = factory.CreateAnalogItemVariable(
                    root,
                    $"{baseName}_CycleTime",
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
                    $"{baseName}_CycleCount",
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
                    m_controller.OpenPin(GPIO_PIN, PinMode.Output);
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
                        m_controller.Write(GPIO_PIN, (newState) ? PinValue.Low : PinValue.High);
                    }

                    m_state.Value = newState;
                    m_state.Timestamp = DateTime.UtcNow;
                    m_state.ClearChangeMasks(SystemContext, false);

                    m_cycleCount.Value = (uint)m_cycleCount.Value + 1;
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
