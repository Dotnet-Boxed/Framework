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
        /// Gets or sets the write message function which defaults to writing to the debug output and console.
        /// </summary>
        public static Action<string> WriteMessage { get; set; }

        /// <summary>
        /// Writes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="color">The color.</param>
        internal static void Write(string message, ConsoleColor? color = null)
        {
            if (WriteMessage is null)
            {
                UseColor(() => DefaultWriteMessage(message), color);
            }
            else
            {
                WriteMessage(message);
            }
        }

        /// <summary>
        /// Writes a new line.
        /// </summary>
        internal static void WriteLine() => Write(Environment.NewLine);

        /// <summary>
        /// Writes the specified message followed by a new line.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="color">The color.</param>
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
