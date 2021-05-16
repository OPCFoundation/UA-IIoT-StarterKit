using System;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Opc.Ua;
using MQTTnet.Extensions.ManagedClient;
using System.Device.Gpio;

namespace MqttAgent
{
    public class PublishingErrorEventArgs
    {
        public PublishingErrorEventArgs(string code, string text)
        {
            ErrorCode = code;
            ErrorText = text;
        }

        public string ErrorCode { get; }
        public string ErrorText { get; }
    }
}
