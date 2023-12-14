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
using System.Diagnostics;
using McMaster.Extensions.CommandLineUtils;
using Opc.Ua;
using UaMqttCommon;
using UaMqttPublisher;

try
{
    if (Debugger.IsAttached)
    {
        args = new string[]
        {
            "publish",
            "--config",
            "config\\uapublisher-config.json",
            "--broker",
            "IopGateway",
            "--connection",
            "InternalServer"
        };
    }

    Log.LogMessage += Log.LogToConsole;
    Application.Run(args);
}
catch (CommandParsingException e)
{
    Log.Exception(e);
    Environment.Exit(3);
}
catch (Exception e)
{
    Log.Exception(e, showstack: true);
    Environment.Exit(3);
}

public static partial class Application
{
    public static void Run(string[] args)
    {
        var app = new CommandLineApplication();
        app.Name = "UaMqttPublisher";
        app.Description = "An application that produces OPC UA PubSub messages sends them to an MQTT broker.";
        app.HelpOption("-?|-h|--help");

        EncodeableFactory.GlobalFactory.AddEncodeableTypes(System.Reflection.Assembly.GetExecutingAssembly());
        Utils.SetTraceMask(Utils.TraceMasks.Information);
        Utils.SetTraceOutput(Utils.TraceOutput.DebugAndFile);

        app.Command("publish", (e) => Publish(e));

        app.OnExecute(() =>
        {
            app.ShowHelp();
            return 0;
        });

        app.Execute(args);
    }

    private static void Publish(CommandLineApplication app)
    {
        app.Description = "Produces OPC UA PubSub messages sends them to an MQTT broker.";
        app.HelpOption("-?|-h|--help");

        AddOptions(app);

        app.OnExecuteAsync(async (token) =>
        {
            var options = GetOptions(app);
            Publisher publisher = new();
            await publisher.Start(options.ConfigFilePath, options.BrokerName, options.ConnectionName);
        });
    }

    private static string GetOption(this CommandLineApplication application, string name, string defaultValue = "")
    {
        var option = application.Options.Where((ii) => (ii.ShortName == name || ii.LongName == name) && ii.HasValue()).FirstOrDefault();

        if (option == null)
        {
            return defaultValue;
        }

        return option.Value();
    }

    private static bool GetOption(this CommandLineApplication application, string name, bool notSetValue = false)
    {
        var option = application.Options.Where((ii) => (ii.ShortName == name || ii.LongName == name) && ii.HasValue()).FirstOrDefault();

        if (option == null)
        {
            return notSetValue;
        }

        return !notSetValue;
    }

    static class OptionsNames
    {

        public const string LogFilePath = "log";
        public const string ConfigFilePath = "config";
        public const string BrokerName = "broker";
        public const string ConnectionName = "connection";
        public const string GroupName = "group";
    }

    class Options
    {
        public string LogFilePath { get; internal set; }
        public string ConfigFilePath { get; internal set; }
        public string BrokerName { get; internal set; }
        public string ConnectionName { get; internal set; }
        public string GroupName { get; internal set; }
    }

    private static void AddOptions(CommandLineApplication app)
    {
        app.Option(
            $"--{OptionsNames.LogFilePath}",
            "The path to the log file.",
            CommandOptionType.SingleValue);

        app.Option(
            $"--{OptionsNames.ConfigFilePath}",
            "The path to the configuration file.",
            CommandOptionType.SingleValue);

        app.Option(
            $"--{OptionsNames.BrokerName}",
            "The name of the broker configuration to use.",
            CommandOptionType.SingleValue);

        app.Option(
            $"--{OptionsNames.ConnectionName}",
            "The name of the connection configuration to use.",
            CommandOptionType.SingleValue);

        app.Option(
            $"--{OptionsNames.GroupName}",
            "The name of the dataset group configuration to use.",
            CommandOptionType.SingleValue);
    }

    private static Options GetOptions(CommandLineApplication app)
    {
        var options = new Options()
        {
            LogFilePath = app.GetOption(OptionsNames.LogFilePath, null),
            ConfigFilePath = app.GetOption(OptionsNames.ConfigFilePath, null),
            BrokerName = app.GetOption(OptionsNames.BrokerName, null),
            ConnectionName = app.GetOption(OptionsNames.ConnectionName, null),
            GroupName = app.GetOption(OptionsNames.GroupName, null),
        };

        return options;
    }
}
