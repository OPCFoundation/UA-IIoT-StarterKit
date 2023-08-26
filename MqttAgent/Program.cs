using System;
using Microsoft.Extensions.CommandLineUtils;

namespace MqttAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var args2 = new string[] { "publish", "-b=mqtt://127.0.0.1:1883", "-a=mydevice:one" };

                MqttAgentApplication.Run(args2);
            }
            catch (CommandParsingException e)
            {
                Console.WriteLine($"[{e.GetType().Name}] {e.Message} ({e.Command})");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[{e.GetType().Name}] {e.Message}");
                Console.ReadLine();
            }
        }
    }
}
