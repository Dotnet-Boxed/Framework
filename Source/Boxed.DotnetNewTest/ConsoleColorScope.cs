namespace Boxed.DotnetNewTest
{
    using System;

    /// <summary>
    /// Sets the console foreground and/or background colour for the specified scope.
    /// </summary>
    /// <seealso cref="IDisposable" />
    internal class ConsoleColorScope : IDisposable
    {
        private readonly ConsoleColor backgroundColor;
        private readonly ConsoleColor foregroundColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleColorScope"/> class.
        /// </summary>
        /// <param name="foregroundColor">Color of the foreground.</param>
        public ConsoleColorScope(ConsoleColor foregroundColor)
            : this(Console.BackgroundColor, foregroundColor)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleColorScope"/> class.
        /// </summary>
        /// <param name="backgroundColor">Color of the background.</param>
        /// <param name="foregroundColor">Color of the foreground.</param>
        public ConsoleColorScope(
            ConsoleColor backgroundColor,
            ConsoleColor foregroundColor)
        {
            this.backgroundColor = Console.BackgroundColor;
            this.foregroundColor = Console.ForegroundColor;

            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
        }

        /// <summary>
        /// Resets the colour to the original values before the scope was initialized.
        /// </summary>
        public void Dispose()
        {
            Console.BackgroundColor = this.backgroundColor;
            Console.ForegroundColor = this.foregroundColor;
        }
    }
}
