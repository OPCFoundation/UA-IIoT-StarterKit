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
using Opc.Ua.Configuration;

namespace UaMqttPublisher
{
    public class UAClient : IDisposable
    {
        #region Private Fields
        private readonly object m_lock = new();
        private readonly ApplicationConfiguration m_configuration;
        private SessionReconnectHandler m_reconnectHandler;
        private ISession m_session;
        private readonly TextWriter m_output;
        private readonly UAClientSettings m_settings;
        #endregion

        #region Constructors
        public UAClient(ApplicationConfiguration configuration, UAClientSettings settings)
        {
            m_configuration = configuration;
            m_settings = settings;
            m_output = settings.OutputWriter;
            m_configuration.CertificateValidator.CertificateValidation += CertificateValidation;
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            Utils.SilentDispose(m_session);
            m_configuration.CertificateValidator.CertificateValidation -= CertificateValidation;
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the client session.
        /// </summary>
        public ISession Session => m_session;

        /// <summary>
        /// The user identity to use to connect to the server.
        /// </summary>
        public IUserIdentity UserIdentity { get; set; } = new UserIdentity();

        /// <summary>
        /// The reconnect period to be used in ms.
        /// </summary>
        public int ReconnectPeriod { get; set; } = 5000;

        /// <summary>
        /// The reconnect period exponential backoff to be used in ms.
        /// </summary>
        public int ReconnectPeriodExponentialBackoff { get; set; } = 15000;
        #endregion

        public static async Task<UAClient> Run(UAClientSettings settings, IEnumerable<CachedValue> values)
        {
            try
            {
                var application = new ApplicationInstance
                {
                    ApplicationName = "PubSub.Quickstart",
                    ApplicationType = ApplicationType.Client,
                    ConfigSectionName = "PubSub.Quickstart"
                };

                // load the application configuration.
                var configuration = await application.LoadApplicationConfiguration(silent: false).ConfigureAwait(false);

                // delete old certificate
                if (settings.RenewCertificate)
                {
                    await application.DeleteApplicationInstanceCertificate().ConfigureAwait(false);
                }

                // check the application certificate.
                bool haveAppCertificate = await application.CheckApplicationInstanceCertificate(false, minimumKeySize: 0).ConfigureAwait(false);

                if (!haveAppCertificate)
                {
                    settings.OutputWriter.WriteLine("Application instance certificate invalid!");
                    return null;
                }

                var uaClient = new UAClient(configuration, settings);

                bool connected = await uaClient.ConnectAsync(values).ConfigureAwait(false);

                if (connected)
                {
                    settings.OutputWriter.WriteLine("Connected!");

                    uaClient.ReconnectPeriod = 1000;
                    uaClient.ReconnectPeriodExponentialBackoff = 10000;
                    uaClient.Session.MinPublishRequestCount = 3;
                    uaClient.Session.TransferSubscriptionsOnReconnect = true;
                }
                else
                {
                    settings.OutputWriter.WriteLine("Could not connect to server!");
                }

                return uaClient;
            }
            catch (Exception e)
            {
                settings.OutputWriter.WriteLine($"Unexpected Exception [{e.GetType().Name}] {e.Message}");
                return null;
            }
        }

        #region Public Methods
        /// <summary>
        /// Creates a session with the UA server
        /// </summary>
        public async Task<bool> ConnectAsync(IEnumerable<CachedValue> values)
        {
            try
            {
                if (m_session != null && m_session.Connected == true)
                {
                    m_output.WriteLine("Session already connected!");
                }
                else
                {
                    ITransportWaitingConnection connection = null;
                    m_output.WriteLine("Connecting to... {0}", m_settings.ServerUrl);
                    var endpointDescription = CoreClientUtils.SelectEndpoint(m_configuration, m_settings.ServerUrl, !m_settings.NoSecurity);

                    var endpointConfiguration = EndpointConfiguration.Create(m_configuration);
                    var endpoint = new ConfiguredEndpoint(null, endpointDescription, endpointConfiguration);
                    var sessionFactory = TraceableSessionFactory.Instance;

                    // set user identity
                    if (!String.IsNullOrEmpty(m_settings.UserName))
                    {
                        UserIdentity = new UserIdentity(m_settings.UserName, m_settings.Password ?? string.Empty);
                    }

                    var session = await sessionFactory.CreateAsync(
                        m_configuration,
                        connection,
                        endpoint,
                        connection == null,
                        false,
                        m_configuration.ApplicationName,
                        m_settings.SessionLifeTime,
                        UserIdentity,
                        null,
                        m_settings.CancellationToken
                    ).ConfigureAwait(false);

                    // Assign the created session
                    if (session != null && session.Connected)
                    {
                        m_session = session;

                        // override keep alive interval
                        m_session.KeepAliveInterval = m_settings.KeepAliveInterval;

                        // support transfer
                        m_session.DeleteSubscriptionsOnClose = false;
                        m_session.TransferSubscriptionsOnReconnect = true;

                        // set up keep alive callback.
                        m_session.KeepAlive += Session_KeepAlive;

                        // prepare a reconnect handler
                        m_reconnectHandler = new SessionReconnectHandler(true, ReconnectPeriodExponentialBackoff);

                        // Define Subscription parameters
                        Subscription subscription = new(session.DefaultSubscription)
                        {
                            DisplayName = "Console ReferenceClient Subscription",
                            PublishingEnabled = true,
                            PublishingInterval = 1000,
                            LifetimeCount = 0,
                            MinLifetimeInterval = 6000,
                        };

                        session.AddSubscription(subscription);

                        // Create the subscription on Server side
                        subscription.Create();
                        m_output.WriteLine("New Subscription created with SubscriptionId = {0}.", subscription.Id);

                        foreach (var value in values)
                        {
                            MonitoredItem monitoredItem = value.CreateItem(subscription);
                            monitoredItem.Notification += OnMonitoredItemNotification;

                            subscription.AddItem(monitoredItem);

                            var propertyItems = value.CreatePropertyItems(subscription);

                            foreach (var propertyItem in propertyItems)
                            {
                                propertyItem.Notification += OnMonitoredItemNotification;
                                subscription.AddItem(propertyItem);
                            }
                        }

                        // Create the monitored items on Server side
                        subscription.ApplyChanges();
                    }

                    // Session created successfully.
                    m_output.WriteLine("New Session Created with SessionName = {0}", m_session?.SessionName);
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log Error
                m_output.WriteLine("Create Session Error : {0}", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Disconnects the session.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (m_session != null)
                {
                    m_output.WriteLine("Disconnecting...");

                    lock (m_lock)
                    {
                        m_session.KeepAlive -= Session_KeepAlive;
                        m_reconnectHandler?.Dispose();
                        m_reconnectHandler = null;
                    }

                    m_session.Close();
                    m_session.Dispose();
                    m_session = null;

                    // Log Session Disconnected event
                    m_output.WriteLine("Session Disconnected.");
                }
                else
                {
                    m_output.WriteLine("Session not created!");
                }
            }
            catch (Exception ex)
            {
                // Log Error
                m_output.WriteLine($"Disconnect Error : {ex.Message}");
            }
        }

        /// <summary>
        /// Handle DataChange notifications from Server
        /// </summary>
        private void OnMonitoredItemNotification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            try
            {
                var value = monitoredItem.Handle as CachedValue;

                lock (value.Lock)
                {
                    MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;

                    value.Value = notification.Value.WrappedValue;
                    value.Timestamp = notification.Value.ServerTimestamp;
                    value.StatusCode = (uint)notification.Value.StatusCode;
                    value.IsDirty = true;
                }
            }
            catch (Exception ex)
            {
                m_output.WriteLine("OnMonitoredItemNotification error: {0}", ex.Message);
            }
        }


        /// <summary>
        /// Handles a keep alive event from a session and triggers a reconnect if necessary.
        /// </summary>
        private void Session_KeepAlive(ISession session, KeepAliveEventArgs e)
        {
            try
            {
                // start reconnect sequence on communication error.
                if (ServiceResult.IsBad(e.Status))
                {
                    if (ReconnectPeriod <= 0)
                    {
                        Utils.LogWarning("KeepAlive status {0}, but reconnect is disabled.", e.Status);
                        return;
                    }

                    if (m_session != null)
                    {
                        foreach (var subscription in m_session.Subscriptions)
                        {
                            foreach (var monitoredItem in subscription.MonitoredItems)
                            {
                                var value = monitoredItem.Handle as CachedValue;

                                lock (value.Lock)
                                {
                                    value.Value = Variant.Null;
                                    value.Timestamp = DateTime.UtcNow;
                                    value.StatusCode = StatusCodes.BadNotConnected;
                                    value.IsDirty = true;
                                }
                            }
                        }
                    }

                    if (m_session != null && m_reconnectHandler != null)
                    {
                        var state = m_reconnectHandler.BeginReconnect(m_session, null, ReconnectPeriod, Client_ReconnectComplete);

                        if (state == SessionReconnectHandler.ReconnectState.Triggered)
                        {
                            Utils.LogInfo("KeepAlive status {0}, reconnect status {1}, reconnect period {2}ms.", e.Status, state, ReconnectPeriod);
                        }
                        else
                        {
                            Utils.LogInfo("KeepAlive status {0}, reconnect status {1}.", e.Status, state);
                        }
                    }

                    return;
                }
            }
            catch (Exception exception)
            {
                Utils.LogError(exception, "Error in OnKeepAlive.");
            }
        }

        /// <summary>
        /// Called when the reconnect attempt was successful.
        /// </summary>
        private void Client_ReconnectComplete(object sender, EventArgs e)
        {
            // ignore callbacks from discarded objects.
            if (!Object.ReferenceEquals(sender, m_reconnectHandler))
            {
                return;
            }

            lock (m_lock)
            {
                // if session recovered, Session property is null
                if (m_reconnectHandler?.Session != null)
                {
                    // ensure only a new instance is disposed
                    // after reactivate, the same session instance may be returned
                    if (!Object.ReferenceEquals(m_session, m_reconnectHandler.Session))
                    {
                        m_output.WriteLine("--- RECONNECTED TO NEW SESSION --- {0}", m_reconnectHandler.Session.SessionId);
                        var session = m_session;
                        m_session = m_reconnectHandler.Session;
                        Utils.SilentDispose(session);
                    }
                    else
                    {
                        m_output.WriteLine("--- REACTIVATED SESSION --- {0}", m_reconnectHandler.Session.SessionId);
                    }
                }
                else
                {
                    m_output.WriteLine("--- RECONNECT KeepAlive recovered ---");
                }
            }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Handles the certificate validation event.
        /// This event is triggered every time an untrusted certificate is received from the server.
        /// </summary>
        protected virtual void CertificateValidation(CertificateValidator sender, CertificateValidationEventArgs e)
        {
            bool certificateAccepted = false;

            // ****
            // Implement a custom logic to decide if the certificate should be
            // accepted or not and set certificateAccepted flag accordingly.
            // The certificate can be retrieved from the e.Certificate field
            // ***

            ServiceResult error = e.Error;
            m_output.WriteLine(error);
            if (error.StatusCode == StatusCodes.BadCertificateUntrusted && m_settings.AutoAccept)
            {
                certificateAccepted = true;
            }

            if (certificateAccepted)
            {
                m_output.WriteLine("Untrusted Certificate accepted. Subject = {0}", e.Certificate.Subject);
                e.Accept = true;
            }
            else
            {
                m_output.WriteLine("Untrusted Certificate rejected. Subject = {0}", e.Certificate.Subject);
            }
        }
        #endregion
    }

    public class UAClientSettings
    {
        /// <summary>
        /// The session keepalive interval to be used in ms.
        /// </summary>
        public bool RenewCertificate { get; set; } = false;

        /// <summary>
        /// The session keepalive interval to be used in ms.
        /// </summary>
        public int KeepAliveInterval { get; set; } = 5000;

        /// <summary>
        /// The session lifetime.
        /// </summary>
        public uint SessionLifeTime { get; set; } = 60 * 1000;

        /// <summary>
        /// Auto accept untrusted certificates.
        /// </summary>
        public bool AutoAccept { get; set; } = false;

        /// <summary>
        /// The file to use for log output.
        /// </summary>
        public string LogFile { get; set; }

        /// <summary>
        /// True if security should not be used.
        /// </summary>
        public bool NoSecurity { get; set; } = false;

        /// <summary>
        /// The username to use to when connecting to the server.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The password to use to when connecting to the server.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The URL of the server to connect to.
        /// </summary>
        public string ServerUrl { get; set; } = "opc.tcp://localhost:62541/";

        /// <summary>
        /// The cancellation token to use.
        /// </summary>
        public CancellationToken CancellationToken { get; set; } = default;

        /// <summary>
        /// The output writer to use.
        /// </summary>
        public TextWriter OutputWriter { get; set; } = Console.Out;
    }

    public class CachedValue
    {
        public CachedValue(object @lock)
        {
            Lock = @lock ?? new object();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string NodeId { get; set; }
        public Variant Value { get; set; }
        public DateTime Timestamp { get; set; }
        public uint StatusCode { get; set; }
        public bool IsDirty { get; set; } = false;
        public List<CachedValue> Properties { get; set; }
        public object Lock { get; set; }

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
                    SamplingInterval = 1000,
                    QueueSize = 0,
                    DiscardOldest = true,
                    Handle = this
                };

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
                            SamplingInterval = 1000,
                            QueueSize = 0,
                            DiscardOldest = true,
                            Handle = property
                        };

                        items.Add(monitoredItem);
                    }
                }

                return items;
            }
        }
    }
}
