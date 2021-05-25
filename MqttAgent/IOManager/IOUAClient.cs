using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MqttAgent
{
    public class IOUAClient : IIOManager
    {
        private object m_lock = new object();
        private Session m_session;
        private Subscription m_subscription;
        private Dictionary<NodeId, MonitoredItem> m_monitoredItems;

        public ushort DataSetId { get; }

        public PublishedDataSetDataType DataSet { get; }

        public void Start()
        {
            StartAsync().Wait();
        }

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

                if (notification != null)
                {
                    return notification.Value;
                }
            }

            return null;
        }

        public void WritePublishedDataItem(NodeId nodeId, uint attributeId = 13, DataValue dataValue = null)
        {
            throw new NotImplementedException();
        }

        public IOUAClient(ushort datasetId, DataSetMetaDataType metadata)
        {
            DataSetId = datasetId;
            m_monitoredItems = new Dictionary<NodeId, MonitoredItem>();

            DataSet = new PublishedDataSetDataType()
            {
                Name = metadata.Name,
                DataSetMetaData = metadata
            };

            string json = File.ReadAllText($"config/sources/{metadata.Name}.json");

            var pdi = new PublishedDataItemsDataType();

            using (var decoder = new JsonDecoder(json, ServiceMessageContext.GlobalContext))
            {
                pdi.Decode(decoder);
            }

            PublishedDataItemsDataType source = new PublishedDataItemsDataType();
            source.PublishedData = new PublishedVariableDataTypeCollection();

            for (int ii = 0; ii < metadata.Fields.Count; ii++)
            {
                var field = metadata.Fields[ii];

                if (ii < pdi.PublishedData.Count)
                {
                    var item = pdi.PublishedData[ii];

                    var variable = new PublishedVariableDataType()
                    {
                        PublishedVariable = new NodeId(field.Name, datasetId),
                        AttributeId = item.AttributeId,
                        SamplingIntervalHint = item.SamplingIntervalHint,
                        SubstituteValue = item.SubstituteValue
                    };

                    source.PublishedData.Add(variable);

                    var monitoredItem = new MonitoredItem()
                    {
                        StartNodeId = item.PublishedVariable,
                        AttributeId = item.AttributeId,
                        IndexRange = item.IndexRange,
                        MonitoringMode = MonitoringMode.Reporting,
                        SamplingInterval = (int)(item.SamplingIntervalHint/2),
                        Handle = field
                    };

                    m_monitoredItems[variable.PublishedVariable] = monitoredItem;
                }
                else
                {
                    source.PublishedData.Add(
                        new PublishedVariableDataType()
                        {
                            PublishedVariable = new NodeId(field.Name, datasetId),
                            AttributeId = Attributes.Value,
                        });
                }
            }

            DataSet.DataSetSource = new ExtensionObject(source);
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
            );

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
