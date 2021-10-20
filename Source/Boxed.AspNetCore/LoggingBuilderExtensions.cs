namespace Boxed.AspNetCore
{
    using System;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// <see cref="ILoggingBuilder"/> extension methods.
    /// </summary>
    public static class LoggingBuilderExtensions
    {
        /// <summary>
        /// Executes the specified action if the specified <paramref name="condition"/> is <c>true</c> which can be
        /// used to conditionally add to the logging builder.
        /// </summary>
        /// <param name="loggingBuilder">The logging builder.</param>
        /// <param name="condition">If set to <c>true</c> the action is executed.</param>
        /// <param name="action">The action used to add to the logging builder.</param>
        /// <returns>The same logging builder.</returns>
        public static ILoggingBuilder AddIf(
            this ILoggingBuilder loggingBuilder,
            bool condition,
            Func<ILoggingBuilder, ILoggingBuilder> action)
        {
            ArgumentNullException.ThrowIfNull(loggingBuilder);
            ArgumentNullException.ThrowIfNull(action);

            if (condition)
            {
                loggingBuilder = action(loggingBuilder);
            }

            return loggingBuilder;
        }

        /// <summary>
        /// Executes the specified <paramref name="ifAction"/> if the specified <paramref name="condition"/> is
        /// <c>true</c>, otherwise executes the <paramref name="elseAction"/>. This can be used to conditionally add to
        /// the logging builder.
        /// </summary>
        /// <param name="loggingBuilder">The logging builder.</param>
        /// <param name="condition">If set to <c>true</c> the <paramref name="ifAction"/> is executed, otherwise the
        /// <paramref name="elseAction"/> is executed.</param>
        /// <param name="ifAction">The action used to add to the logging builder if the condition is <c>true</c>.</param>
        /// <param name="elseAction">The action used to add to the logging builder if the condition is <c>false</c>.</param>
        /// <returns>The same logging builder.</returns>
        public static ILoggingBuilder AddIfElse(
            this ILoggingBuilder loggingBuilder,
            bool condition,
            Func<ILoggingBuilder, ILoggingBuilder> ifAction,
            Func<ILoggingBuilder, ILoggingBuilder> elseAction)
        {
            ArgumentNullException.ThrowIfNull(loggingBuilder);
            ArgumentNullException.ThrowIfNull(ifAction);
            ArgumentNullException.ThrowIfNull(elseAction);

            if (condition)
            {
                loggingBuilder = ifAction(loggingBuilder);
            }
            else
            {
                loggingBuilder = elseAction(loggingBuilder);
            }

            return loggingBuilder;
        }
    }
}
