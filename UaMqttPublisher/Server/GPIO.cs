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
using System.Text;
using Opc.Ua;
using Opc.Ua.Configuration;

namespace UaMqttPublisher.Server
{
    public class GPIO
    {
        private ApplicationInstance m_application;
        private GPIOServerManager m_server;

        public ApplicationDescription ApplicationDescription { get; private set; }

        public StringCollection ServerCapabilities { get; private set; }

        public EndpointDescriptionCollection PublisherEndpoints { get; private set; }

        public async Task Start(bool useGPIO, string port)
        {
            m_application = new ApplicationInstance
            {
                ApplicationName = "GPIO Server",
                ApplicationType = ApplicationType.Server,
                ConfigSectionName = "GPIO"
            };

            // load the application configuration.
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configurationFile = Path.Combine(folder, "config", "uaserver-configuration.xml");
            var xml = File.ReadAllText(configurationFile);

            if (!String.IsNullOrEmpty(port))
            {
                xml = xml.Replace("48040", port);
            }

            var istrm = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            var config = await m_application.LoadApplicationConfiguration(istrm, false).ConfigureAwait(false);
            istrm.Close();

            // check the application certificate.
            bool haveAppCertificate = await m_application.CheckApplicationInstanceCertificate(
                false,
                CertificateFactory.DefaultKeySize,
                CertificateFactory.DefaultLifeTime).ConfigureAwait(false);

            if (!haveAppCertificate)
            {
                throw new Exception("Application instance certificate invalid!");
            }

            if (!config.SecurityConfiguration.AutoAcceptUntrustedCertificates)
            {
                config.CertificateValidator.CertificateValidation
                    += new CertificateValidationEventHandler(
                        CertificateValidator_CertificateValidation);
            }

            // start the server.
            m_server = new GPIOServerManager(useGPIO);
            await m_application.Start(m_server).ConfigureAwait(false);

            // print endpoint info
            var endpoints = m_application.Server.GetEndpoints();

            PublisherEndpoints = new EndpointDescriptionCollection();

            foreach (var endpoint in endpoints)
            {
                if (ApplicationDescription == null)
                {
                    ApplicationDescription = new ApplicationDescription()
                    {
                        ApplicationName = endpoint.Server.ApplicationName,
                        ApplicationUri = endpoint.Server.ApplicationUri,
                        ApplicationType = endpoint.Server.ApplicationType,
                        ProductUri = endpoint.Server.ProductUri,
                        DiscoveryProfileUri = endpoint.Server.DiscoveryProfileUri,
                        GatewayServerUri = endpoint.Server.GatewayServerUri,
                        DiscoveryUrls = endpoint.Server.DiscoveryUrls
                    };

                    ServerCapabilities = config.ServerConfiguration.ServerCapabilities;
                }

                PublisherEndpoints.Add(endpoint);
            }
        }

        private void CertificateValidator_CertificateValidation(CertificateValidator sender, CertificateValidationEventArgs e)
        {
            e.Accept = true;
        }

        public Task Stop()
        {
            m_server.Stop();
            return Task.FromResult(0);
        }
    }
}
