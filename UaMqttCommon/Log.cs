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
using System.Globalization;

namespace UaMqttCommon
{
    public static class Log
    {
        public static event Func<LogMessageEventArgs, Task> LogMessage;

        public static void Info(string message, params object[] args)
        {
            if (LogMessage != null)
            {
                LogMessage(new LogMessageEventArgs(message, LogSeverity.Information, args));
            }
        }

        public static void Warning(string message, params object[] args)
        {
            if (LogMessage != null)
            {
                LogMessage(new LogMessageEventArgs(message, LogSeverity.Warning, args));
            }
        }

        public static void Control(string message, params object[] args)
        {
            if (LogMessage != null)
            {
                LogMessage(new LogMessageEventArgs(message, LogSeverity.Control, args));
            }
        }

        public static void Error(string message, params object[] args)
        {
            if (LogMessage != null)
            {
                LogMessage(new LogMessageEventArgs(message, LogSeverity.Error, args));
            }
        }

        public static void Debug(string message, params object[] args)
        {
            if (LogMessage != null)
            {
                LogMessage(new LogMessageEventArgs(message, LogSeverity.Debug, args));
            }
        }

        public static void Exception(Exception e, bool showstack = false)
        {
            Log.Error($"[{e.GetType().Name}] {e.Message}");

            if (e is AggregateException ae)
            {
                foreach (var ie in ae.InnerExceptions)
                {
                    Log.Debug($">>> [{ie.GetType().Name}] {ie.Message}");
                }
            }
            else
            {
                Exception ie = e.InnerException;

                while (ie != null)
                {
                    Log.Debug($">>> [{ie.GetType().Name}] {ie.Message}");
                    ie = ie.InnerException;
                }
            }

            if (showstack)
            {
                var trace = e.StackTrace.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                Log.Debug(new string('=', 80));

                foreach (var line in trace)
                {
                    Log.Debug(line?.Trim());
                }

                Log.Debug(new string('=', 80));
            }
        }

        public static async Task<bool> CheckForKeyPress(int timeout, Func<bool> callback)
        {
            for (int ii = 0; ii < timeout / 100; ii++)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(false);

                    if (key.Key == ConsoleKey.Enter)
                    {
                        return true;
                    }
                }

                await Task.Delay(100);
            }

            if (callback != null)
            {
                if (callback())
                {
                    return true;
                }
            }

            return false;
        }

        public static async Task LogToConsole(LogMessageEventArgs e)
        {
            var color = Console.ForegroundColor;

            switch (e.Severity)
            {
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Control:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                default:
                case LogSeverity.Information:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
            }

            var message = String.Format(CultureInfo.InvariantCulture, e.Message, e.Args);
            await Console.Out.WriteLineAsync($"[{DateTime.Now:HH:mm:ss.fff}] {message}").ConfigureAwait(false);
            Console.ForegroundColor = color;
        }
    }

    public class LogMessageEventArgs
    {
        public LogMessageEventArgs(string message, LogSeverity severity, params object[] args)
        {
            Message = message;
            Args = args;
            Severity = severity;
        }

        public string Message { get; private set; }

        public object[] Args { get; private set; }

        public LogSeverity Severity { get; private set; }
    }

    public enum LogSeverity
    {
        Debug = 100,
        Information = 200,
        Warning = 300,
        Control = 400,
        Error = 500
    }
}
