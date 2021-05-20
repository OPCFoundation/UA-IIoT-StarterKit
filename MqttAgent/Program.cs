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
        }
    }
}
