namespace Boxed.DotnetNewTest
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// The dotnet new logger used to log output from dotnet new commands.
    /// </summary>
    public static class TestLogger
    {
        private static readonly Action<string> DefaultWriteMessage =
            (message) =>
            {
                Debug.Write(message);
                Console.Write(message);
            };

        /// <summary>
        /// Gets the write message function which defaults to writing to the debug output and console.
        /// </summary>
        public static Action<string> WriteMessage { get; set; }

        internal static void Write(string message, ConsoleColor? color = null)
        {
            if (WriteMessage == null)
            {
                UseColor(() => DefaultWriteMessage(message), color);
            }
            else
            {
                WriteMessage(message);
            }
        }

        internal static void WriteLine() => Write(Environment.NewLine);

        internal static void WriteLine(string message, ConsoleColor? color = null) =>
            Write(message + Environment.NewLine, color);

        private static void UseColor(Action action, ConsoleColor? color = null)
        {
            if (color.HasValue)
            {
                using (new ConsoleColorScope(color.Value))
                {
                    action();
                }
            }
            else
            {
                action();
            }
        }
    }
}
