using Opc.Ua;
using Opc.Ua.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttAgent.Server
{
    public class GPIO
    {
        private GPIOServerManager m_server;

        public async Task Start(bool useGPIO)
        {
            ApplicationInstance application = new ApplicationInstance
            {
                ApplicationName = "GPIO Server",
                ApplicationType = ApplicationType.Server,
                ConfigSectionName = "GPIO"
            };

            // load the application configuration.
            var config = await application.LoadApplicationConfiguration("config/server-configuration.xml", false).ConfigureAwait(false);

            // check the application certificate.
            bool haveAppCertificate = await application.CheckApplicationInstanceCertificate(
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
            await application.Start(m_server).ConfigureAwait(false);

            // print endpoint info
            var endpoints = application.Server
                .GetEndpoints()
                .Select(e => e.EndpointUrl)
                .Distinct();

            foreach (var endpoint in endpoints)
            {
                Console.WriteLine(endpoint);
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
