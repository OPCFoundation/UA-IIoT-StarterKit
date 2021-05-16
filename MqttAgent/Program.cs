using System;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Opc.Ua;
using MQTTnet.Extensions.ManagedClient;
using System.Device.Gpio;
using Microsoft.Extensions.CommandLineUtils;

namespace MqttAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MqttAgentApplication.Run(args);
            }
            catch (CommandParsingException e)
            {
                Console.WriteLine($"[{e.GetType().Name}] {e.Message} ({e.Command})");
            }
            catch (Exception e)
            {
                Console.WriteLine($"[{e.GetType().Name}] {e.Message}");
            }

            Console.ReadLine();
        }
    }
}
