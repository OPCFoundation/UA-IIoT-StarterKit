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
using Opc.Ua.Client;
using UaMqttCommon;

namespace UaMqttPublisher
{
    public class SubscribedValue
    {
        private DataSetFieldBase m_field;

        public SubscribedValue(DataSetFieldBase field, object @lock)
        {
            m_field = field ?? throw new ArgumentNullException(nameof(field));
            Lock = @lock ?? new object();
        }

        public object Lock { get; set; }
        public string Name => m_field.Name;
        public string Description => m_field.Description;
        public string NodeId => m_field.Source;
        public int SamplingInterval => m_field.SamplingInterval ?? 1000;
        public List<SubscribedValue> Properties { get; set; }

        public Variant Value { get; set; }
        public DateTime Timestamp { get; set; }
        public uint StatusCode { get; set; }
        public bool IsDirty { get; set; }

        public MonitoredItem CreateItem(Subscription subscription)
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));

            lock (Lock)
            {
                MonitoredItem monitoredItem = new(subscription.DefaultItem)
                {
                    StartNodeId = ExpandedNodeId.Parse(NodeId, subscription.Session.NamespaceUris),
                    AttributeId = Attributes.Value,
                    DisplayName = Name,
                    SamplingInterval = SamplingInterval,
                    QueueSize = 0,
                    DiscardOldest = true,
                    Handle = this
                };

                monitoredItem.Notification += OnMonitoredItemNotification;
                subscription.AddItem(monitoredItem);

                return monitoredItem;
            }
        }

        public List<MonitoredItem> CreatePropertyItems(Subscription subscription)
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));

            lock (Lock)
            {
                List<MonitoredItem> items = new();

                if (Properties?.Count > 0)
                {
                    foreach (var property in Properties)
                    {
                        MonitoredItem monitoredItem = new(subscription.DefaultItem)
                        {
                            StartNodeId = ExpandedNodeId.Parse(property.NodeId, subscription.Session.NamespaceUris),
                            AttributeId = Attributes.Value,
                            DisplayName = property.Name,
                            SamplingInterval = property.SamplingInterval,
                            QueueSize = 0,
                            DiscardOldest = true,
                            Handle = property
                        };

                        monitoredItem.Notification += property.OnMonitoredItemNotification;
                        subscription.AddItem(monitoredItem);

                        items.Add(monitoredItem);
                    }
                }

                return items;
            }
        }

        public void SetConnectionError(uint error, DateTime timestamp)
        {
            lock (Lock)
            {
                Value = Variant.Null;
                Timestamp = timestamp;
                StatusCode = error;
                IsDirty = true;
            }
        }

        private void OnMonitoredItemNotification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            try
            {
                lock (Lock)
                {
                    if (e.NotificationValue is MonitoredItemNotification notification)
                    {
                        Value = notification.Value.WrappedValue;
                        Timestamp = notification.Value.ServerTimestamp;
                        StatusCode = (uint)notification.Value.StatusCode;
                        IsDirty = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Warning($"OnUpdate Error [{exception.GetType().Name}] {exception.Message}");
            }
        }
    }
}
