using System.Globalization;
using System.Text;

namespace UaPubSubCommon
{
    public static class Log
    {
        private static readonly Queue<Entry> m_buffer = new();
        private static bool m_enableBuffering = false;

        public static LogLevel LogLevel = LogLevel.Debug;

        public static bool EnableBuffering
        {
            get { return m_enableBuffering; }

            set
            {
                if (!value)
                {
                    lock (m_buffer)
                    {
                        while (m_buffer.Count > 0)
                        {
                            var entry = m_buffer.Dequeue();
                            InternalOut(entry.Level, entry.Message, entry.Args);
                        }
                    }
                }

                m_enableBuffering = value;
            }
        }

        private class Entry
        {
            public LogLevel Level;
            public string Message;
            public string[] Args;
        }

        public static void Error(string message, params string[] args) => Log.Out(LogLevel.Error, message, args);
        public static void Warning(string message, params string[] args) => Log.Out(LogLevel.Warning, message, args);
        public static void System(string message, params string[] args) => Log.Out(LogLevel.System, message, args);
        public static void Info(string message, params string[] args) => Log.Out(LogLevel.Info, message, args);
        public static void Verbose(string message, params string[] args) => Log.Out(LogLevel.Verbose, message, args);
        public static void Debug(string message, params string[] args) => Log.Out(LogLevel.Debug, message, args);
        public static void Prompt(string message, params string[] args) => Log.InternalOut(LogLevel.System, message, args);

        public static void Out(LogLevel level, string message, params string[] args)
        {
            if (m_enableBuffering)
            {
                lock (m_buffer)
                {
                    m_buffer.Enqueue(new Entry { Level = level, Message = message, Args = args });
                    return;
                }
            }

            InternalOut(level, message, args);
        }

        private static void InternalOut(LogLevel level, string message, params string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            lock (m_buffer)
            {
                var text = message;

                if (args?.Length > 0)
                {
                    text = String.Format(CultureInfo.InvariantCulture, message, args);
                }

                var color = Console.ForegroundColor;

                try
                {
                    switch (level)
                    {
                        default:
                        case LogLevel.Debug: { Console.ForegroundColor = ConsoleColor.Gray; break; }
                        case LogLevel.Verbose: { Console.ForegroundColor = ConsoleColor.DarkYellow; break; }
                        case LogLevel.Info: { Console.ForegroundColor = ConsoleColor.Green; break; }
                        case LogLevel.System: { Console.ForegroundColor = ConsoleColor.Cyan; break; }
                        case LogLevel.Warning: { Console.ForegroundColor = ConsoleColor.Yellow; break; }
                        case LogLevel.Error: { Console.ForegroundColor = ConsoleColor.Red; break; }
                    }

                    if (LogLevel >= level)
                    {
                        Console.WriteLine(text);
                    }
                }
                finally
                {
                    Console.ForegroundColor = color;
                }
            }
        }
    }

    public enum LogLevel
    {
        None,
        Error,
        Warning,
        System,
        Info,
        Verbose,
        Debug
    }
}
