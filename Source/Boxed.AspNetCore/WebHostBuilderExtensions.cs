namespace Boxed.AspNetCore
{
    using System;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// <see cref="IWebHostBuilder"/> extension methods.
    /// </summary>
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// Executes the specified action if the specified <paramref name="condition"/> is <c>true</c> which can be
        /// used to conditionally add to the web host builder.
        /// </summary>
        /// <param name="webHostBuilder">The web host builder.</param>
        /// <param name="condition">If set to <c>true</c> the action is executed.</param>
        /// <param name="action">The action used to add to the web host builder.</param>
        /// <returns>The same web host builder.</returns>
        public static IWebHostBuilder UseIf(
            this IWebHostBuilder webHostBuilder,
            bool condition,
            Func<IWebHostBuilder, IWebHostBuilder> action)
        {
            ArgumentNullException.ThrowIfNull(webHostBuilder);
            ArgumentNullException.ThrowIfNull(action);

            if (condition)
            {
                webHostBuilder = action(webHostBuilder);
            }

            return webHostBuilder;
        }

        /// <summary>
        /// Executes the specified action if the specified <paramref name="condition"/> is <c>true</c> which can be
        /// used to conditionally add to the web host builder.
        /// </summary>
        /// <param name="webHostBuilder">The web host builder.</param>
        /// <param name="condition">If <c>true</c> is returned the action is executed.</param>
        /// <param name="action">The action used to add to the web host builder.</param>
        /// <returns>The same web host builder.</returns>
        public static IWebHostBuilder UseIf(
            this IWebHostBuilder webHostBuilder,
            Func<IWebHostBuilder, bool> condition,
            Func<IWebHostBuilder, IWebHostBuilder> action)
        {
            ArgumentNullException.ThrowIfNull(webHostBuilder);
            ArgumentNullException.ThrowIfNull(condition);
            ArgumentNullException.ThrowIfNull(action);

            if (condition(webHostBuilder))
            {
                webHostBuilder = action(webHostBuilder);
            }

            return webHostBuilder;
        }

        /// <summary>
        /// Executes the specified <paramref name="ifAction"/> if the specified <paramref name="condition"/> is
        /// <c>true</c>, otherwise executes the <paramref name="elseAction"/>. This can be used to conditionally add to
        /// the web host builder.
        /// </summary>
        /// <param name="webHostBuilder">The web host builder.</param>
        /// <param name="condition">If set to <c>true</c> the <paramref name="ifAction"/> is executed, otherwise the
        /// <paramref name="elseAction"/> is executed.</param>
        /// <param name="ifAction">The action used to add to the web host builder if the condition is <c>true</c>.</param>
        /// <param name="elseAction">The action used to add to the web host builder if the condition is <c>false</c>.</param>
        /// <returns>The same web host builder.</returns>
        public static IWebHostBuilder UseIfElse(
            this IWebHostBuilder webHostBuilder,
            bool condition,
            Func<IWebHostBuilder, IWebHostBuilder> ifAction,
            Func<IWebHostBuilder, IWebHostBuilder> elseAction)
        {
            ArgumentNullException.ThrowIfNull(webHostBuilder);
            ArgumentNullException.ThrowIfNull(ifAction);
            ArgumentNullException.ThrowIfNull(elseAction);

            if (condition)
            {
                webHostBuilder = ifAction(webHostBuilder);
            }
            else
            {
                webHostBuilder = elseAction(webHostBuilder);
            }

            return webHostBuilder;
        }

        /// <summary>
        /// Executes the specified <paramref name="ifAction"/> if the specified <paramref name="condition"/> is
        /// <c>true</c>, otherwise executes the <paramref name="elseAction"/>. This can be used to conditionally add to
        /// the web host builder.
        /// </summary>
        /// <param name="webHostBuilder">The web host builder.</param>
        /// <param name="condition">If <c>true</c> is returned the <paramref name="ifAction"/> is executed, otherwise the
        /// <paramref name="elseAction"/> is executed.</param>
        /// <param name="ifAction">The action used to add to the web host builder if the condition is <c>true</c>.</param>
        /// <param name="elseAction">The action used to add to the web host builder if the condition is <c>false</c>.</param>
        /// <returns>The same web host builder.</returns>
        public static IWebHostBuilder UseIfElse(
            this IWebHostBuilder webHostBuilder,
            Func<IWebHostBuilder, bool> condition,
            Func<IWebHostBuilder, IWebHostBuilder> ifAction,
            Func<IWebHostBuilder, IWebHostBuilder> elseAction)
        {
            ArgumentNullException.ThrowIfNull(webHostBuilder);
            ArgumentNullException.ThrowIfNull(condition);
            ArgumentNullException.ThrowIfNull(ifAction);
            ArgumentNullException.ThrowIfNull(elseAction);

            if (condition(webHostBuilder))
            {
                webHostBuilder = ifAction(webHostBuilder);
            }
            else
            {
                webHostBuilder = elseAction(webHostBuilder);
            }

            return webHostBuilder;
        }
    }
}
