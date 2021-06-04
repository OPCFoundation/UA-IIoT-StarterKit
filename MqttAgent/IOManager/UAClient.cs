using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MqttAgent
{
    public class UAClient
    {
        private object m_lock = new object();
        private Session m_session;
        private Subscription m_subscription;
        private Dictionary<NodeId, MonitoredItem> m_monitoredItems;

        public DataValue ReadPublishedDataItem(NodeId nodeId, uint attributeId = 13, bool deltaFrame = false)
        {
            if (nodeId == null)
            {
                return null;
            }

            lock (m_lock)
            { 
                MonitoredItem monitoredItem = null;

                if (!m_monitoredItems.TryGetValue(nodeId, out monitoredItem))
                {
                    return null;
                }
                
                MonitoredItemNotification notification = monitoredItem.LastValue as MonitoredItemNotification;

                if (notification == null)
                {
                    return new DataValue() 
                    { 
                        StatusCode = StatusCodes.BadWaitingForInitialData,
                        SourceTimestamp = DateTime.UtcNow 
                    };
                }

                return notification.Value;
            }
        }

        public UAClient()
        {
            m_monitoredItems = new Dictionary<NodeId, MonitoredItem>();
        }

        public void Add(NodeId identifier, PublishedVariableDataType variable, string browseName)
        {
            lock (m_lock)
            {
                var monitoredItem = new MonitoredItem()
                {
                    StartNodeId = variable.PublishedVariable,
                    RelativePath = browseName,
                    AttributeId = (browseName == null) ? variable.AttributeId : Attributes.Value,
                    IndexRange = variable.IndexRange,
                    MonitoringMode = MonitoringMode.Reporting,
                    SamplingInterval = (int)(variable.SamplingIntervalHint / 2)
                };

                m_monitoredItems[identifier] = monitoredItem;
            }
        }

        public async Task StartAsync()
        {
            ApplicationInstance application = new ApplicationInstance
            {
                ApplicationName = "GPIO Client",
                ApplicationType = ApplicationType.Client,
                ConfigSectionName = "GPIO"
            };

            // load the application configuration.
            var configuration = await application.LoadApplicationConfiguration("config/server-configuration.xml", false).ConfigureAwait(false);

            // check the application certificate.
            bool haveAppCertificate = await application.CheckApplicationInstanceCertificate(
                false,
                CertificateFactory.DefaultKeySize,
                CertificateFactory.DefaultLifeTime).ConfigureAwait(false);

            if (!haveAppCertificate)
            {
                throw new Exception("Application instance certificate invalid!");
            }

            if (!configuration.SecurityConfiguration.AutoAcceptUntrustedCertificates)
            {
                configuration.CertificateValidator.CertificateValidation
                    += new CertificateValidationEventHandler(
                        CertificateValidator_CertificateValidation);
            }

            EndpointDescription endpointDescription = CoreClientUtils.SelectEndpoint("opc.tcp://localhost:48040", false);
            EndpointConfiguration endpointConfiguration = EndpointConfiguration.Create(configuration);
            ConfiguredEndpoint endpoint = new ConfiguredEndpoint(null, endpointDescription, endpointConfiguration);

            Session session = await Session.Create(
                configuration,
                endpoint,
                false,
                false,
                configuration.ApplicationName,
                30 * 60 * 1000,
                new UserIdentity(),
                null
            ).ConfigureAwait(false);

            if (session != null && session.Connected)
            {
                m_session = session;
            }

            SubscribeToDataChanges();
        }

        public void Stop()
        {
            if (m_session != null)
            {
                m_session.Close();
                m_session.Dispose();
                m_session = null;
            }
        }

        public void SubscribeToDataChanges()
        {
            if (m_session == null || m_session.Connected == false)
            {
                return;
            }

            lock (m_lock)
            {
                Subscription subscription = new Subscription(m_session.DefaultSubscription);

                subscription.PublishingEnabled = true;
                subscription.PublishingInterval = 1000;

                m_session.AddSubscription(subscription);

                subscription.Create();

                subscription.AddItems(m_monitoredItems.Values);

                m_subscription = subscription;
            }

            m_subscription.ApplyChanges();
        }

        /// <summary>
        private void CertificateValidator_CertificateValidation(CertificateValidator sender, CertificateValidationEventArgs e)
        {
            e.Accept = true;
        }
    }
}
