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
                var x = new DateTime(2021, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                var y = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

                var z = (x - y).TotalSeconds;

                MqttAgentApplication.Run(args);
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
