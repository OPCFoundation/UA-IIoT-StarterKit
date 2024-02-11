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
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using UaMqttCommon;

namespace UaMqttPublisher
{
    public class UAClient : IDisposable
    {
        #region Private Fields
        private readonly object m_lock = new();
        private readonly ApplicationConfiguration m_configuration;
        private SessionReconnectHandler m_reconnectHandler;
        private ISession m_session;
        private readonly ConnectionConfiguration m_pubSubConnection;
        #endregion

        #region Constructors
        public UAClient(ApplicationConfiguration configuration, ConnectionConfiguration settings)
        {
            m_configuration = configuration;
            m_pubSubConnection = settings;
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
        public IServiceMessageContext MessageContext => m_session?.MessageContext;

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

        public static async Task<UAClient> Run(ConnectionConfiguration pubSubConnection)
        {
            try
            {
                var application = new ApplicationInstance
                {
                    ApplicationName = pubSubConnection.Name,
                    ApplicationType = ApplicationType.Client,
                    ConfigSectionName = pubSubConnection.Name
                };

                string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var configurationFile = Path.Combine(folder, "config", "uaserver-configuration.xml");

                // load the application configuration.
                var configuration = await application.LoadApplicationConfiguration(configurationFile, false).ConfigureAwait(false);

                // delete old certificate
                if (pubSubConnection.RenewCertificate ?? false)
                {
                    await application.DeleteApplicationInstanceCertificate().ConfigureAwait(false);
                }

                // check the application certificate.
                bool haveAppCertificate = await application
                    .CheckApplicationInstanceCertificate(false, minimumKeySize: 0)
                    .ConfigureAwait(false);

                if (!haveAppCertificate)
                {
                    Log.Error("Application instance certificate invalid!");
                    return null;
                }

                var uaClient = new UAClient(configuration, pubSubConnection);

                bool connected = await uaClient.ConnectAsync().ConfigureAwait(false);

                if (connected)
                {
                    Log.Info("Connected!");

                    uaClient.ReconnectPeriod = 1000;
                    uaClient.ReconnectPeriodExponentialBackoff = 10000;
                    uaClient.Session.MinPublishRequestCount = 3;
                    uaClient.Session.TransferSubscriptionsOnReconnect = true;
                }
                else
                {
                    Log.Error("Could not connect to server!");
                }

                return uaClient;
            }
            catch (Exception e)
            {
                Log.Error($"Unexpected Exception [{e.GetType().Name}] {e.Message}");
                return null;
            }
        }

        #region Public Methods
        /// <summary>
        /// Creates a session with the UA server
        /// </summary>
        public async Task<bool> ConnectAsync()
        {
            try
            {
                if (m_session != null && m_session.Connected == true)
                {
                    Log.Warning("Session already connected!");
                    return true;
                }

                ITransportWaitingConnection connection = null;
                Log.Info("Connecting to... {0}", m_pubSubConnection.ServerUrl);

                var endpointDescription = CoreClientUtils.SelectEndpoint(
                    m_configuration,
                    m_pubSubConnection.ServerUrl,
                    !m_pubSubConnection.NoSecurity ?? false);

                var endpointConfiguration = EndpointConfiguration.Create(m_configuration);
                var endpoint = new ConfiguredEndpoint(null, endpointDescription, endpointConfiguration);
                var sessionFactory = TraceableSessionFactory.Instance;

                // set user identity
                if (!String.IsNullOrEmpty(m_pubSubConnection.UserName))
                {
                    UserIdentity = new UserIdentity(m_pubSubConnection.UserName, m_pubSubConnection.Password ?? string.Empty);
                }

                var session = await sessionFactory.CreateAsync(
                    m_configuration,
                    connection,
                    endpoint,
                    connection == null,
                    false,
                    m_configuration.ApplicationName,
                    m_pubSubConnection.SessionTimeout ?? 600_000,
                    UserIdentity,
                    null,
                    CancellationToken.None
                ).ConfigureAwait(false);

                // Assign the created session
                if (session != null && session.Connected)
                {
                    m_session = session;

                    // support transfer
                    m_session.DeleteSubscriptionsOnClose = false;
                    m_session.TransferSubscriptionsOnReconnect = true;

                    // set up keep alive callback.
                    m_session.KeepAlive += Session_KeepAlive;

                    // prepare a reconnect handler
                    m_reconnectHandler = new SessionReconnectHandler(true, ReconnectPeriodExponentialBackoff);
                }

                // Session created successfully.
                Log.Info("New Session Created with SessionName = {0}", m_session?.SessionName);
                return true;
            }
            catch (Exception ex)
            {
                // Log Error
                Log.Error("Create Session Error : {0}", ex.Message);
                return false;
            }
        }

        public bool Subscribe(IDictionary<string, SubscribedValue> cache)
        {
            try
            {
                if (m_session == null || m_session.Connected == false)
                {
                    Log.Warning("Session already connected!");
                    return false;
                }

                foreach (var group in m_pubSubConnection.Groups)
                {
                    foreach (var dataset in group.Writers)
                    {
                        foreach (var field in dataset.Fields)
                        {
                            var value = new SubscribedValue(field, cache);
                            value.StatusCode = StatusCodes.BadWaitingForInitialData;
                            value.Timestamp = DateTime.UtcNow;
                            cache[$"{group.Name}.{dataset.Name}.{field.Name}"] = value;

                            if (field.Properties?.Count > 0)
                            {
                                value.Properties = new();

                                foreach (var property in field.Properties)
                                {
                                    var subvalue = new SubscribedValue(property, cache);
                                    subvalue.StatusCode = StatusCodes.BadWaitingForInitialData;
                                    subvalue.Timestamp = DateTime.UtcNow;
                                    value.Properties.Add(subvalue);
                                    cache[$"{group.Name}.{dataset.Name}.{field.Name}.{property.Name}"] = subvalue;
                                }
                            }
                        }
                    }
                }

                foreach (var group in m_pubSubConnection.Groups)
                {
                    Subscription subscription = new(m_session.DefaultSubscription)
                    {
                        DisplayName = group.Name,
                        PublishingEnabled = group.Enabled ?? true,
                        PublishingInterval = group.PublishingInterval ?? 1000,
                        LifetimeCount = 0,
                        MinLifetimeInterval = 60_000,
                        KeepAliveCount = group.KeepAliveCount ?? 1
                    };

                    m_session.AddSubscription(subscription);

                    subscription.Create();
                    Log.Info("Subscription ({0}) created for Group '{1}'", subscription.Id, group.Name);

                    foreach (var dataset in group.Writers)
                    {
                        foreach (var field in dataset.Fields)
                        {
                            var value = cache[$"{group.Name}.{dataset.Name}.{field.Name}"];
                            value.CreateItem(subscription);

                            if (field.Properties?.Count > 0)
                            {
                                foreach (var property in field.Properties)
                                {
                                    var subvalue = cache[$"{group.Name}.{dataset.Name}.{field.Name}.{property.Name}"];
                                    subvalue.CreateItem(subscription);
                                }
                            }
                        }
                    }

                    subscription.ApplyChanges();


                }

                Log.Info("Subscriptions created for SessionName = {0}", m_session?.SessionName);
                return true;
            }
            catch (Exception ex)
            {
                // Log Error
                Log.Error("Create Session Error : {0}", ex.Message);
                return false;
            }
        }

        public void Disconnect()
        {
            try
            {
                if (m_session != null)
                {
                    Log.Info("Disconnecting...");

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
                    Log.Info("Session Disconnected.");
                }
                else
                {
                    Log.Error("Session not created!");
                }
            }
            catch (Exception ex)
            {
                // Log Error
                Log.Error($"Disconnect Error : {ex.Message}");
            }
        }

        private void Session_KeepAlive(ISession session, KeepAliveEventArgs e)
        {
            try
            {
                // start reconnect sequence on communication error.
                if (ServiceResult.IsBad(e.Status))
                {
                    if (m_session != null)
                    {
                        foreach (var subscription in m_session.Subscriptions)
                        {
                            foreach (var monitoredItem in subscription.MonitoredItems)
                            {
                                var value = monitoredItem.Handle as SubscribedValue;

                                if (value != null)
                                {
                                    value.SetConnectionError(StatusCodes.BadNoCommunication, e.CurrentTime);
                                }
                            }
                        }
                    }

                    if (ReconnectPeriod <= 0)
                    {
                        Log.Warning("KeepAlive status {0}, but reconnect is disabled.", e.Status);
                        return;
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
                        Log.Warning("--- RECONNECTED TO NEW SESSION --- {0}", m_reconnectHandler.Session.SessionId);
                        var session = m_session;
                        m_session = m_reconnectHandler.Session;
                        Utils.SilentDispose(session);
                    }
                    else
                    {
                        Log.Warning("--- REACTIVATED SESSION --- {0}", m_reconnectHandler.Session.SessionId);
                    }
                }
                else
                {
                    Log.Warning("--- RECONNECT KeepAlive recovered ---");
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
            Log.Error(error.ToString());

            if (error.StatusCode == StatusCodes.BadCertificateUntrusted && (m_pubSubConnection.AutoAccept ?? false))
            {
                certificateAccepted = true;
            }

            if (certificateAccepted)
            {
                Log.Info("Untrusted Certificate accepted. Subject = {0}", e.Certificate.Subject);
                e.Accept = true;
            }
            else
            {
                Log.Warning("Untrusted Certificate rejected. Subject = {0}", e.Certificate.Subject);
            }
        }
        #endregion
    }
}
