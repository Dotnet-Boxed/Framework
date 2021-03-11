namespace Boxed.AspNetCore
{
    using System;
    using Boxed.AspNetCore.Middleware;
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// <see cref="IApplicationBuilder"/> extension methods.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Allows the use of <see cref="HttpException"/> as an alternative method of returning an error result.
        /// </summary>
        /// <param name="application">The application builder.</param>
        /// <returns>The same application builder.</returns>
        public static IApplicationBuilder UseHttpException(this IApplicationBuilder application) =>
            UseHttpException(application, null);

        /// <summary>
        /// Allows the use of <see cref="HttpException"/> as an alternative method of returning an error result.
        /// </summary>
        /// <param name="application">The application builder.</param>
        /// <param name="configureOptions">The middleware options.</param>
        /// <returns>The same application builder.</returns>
        public static IApplicationBuilder UseHttpException(
            this IApplicationBuilder application,
            Action<HttpExceptionMiddlewareOptions>? configureOptions)
        {
            if (application is null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            var options = new HttpExceptionMiddlewareOptions();
            configureOptions?.Invoke(options);
            return application.UseMiddleware<HttpExceptionMiddleware>(options);
        }

        /// <summary>
        /// Measures the time the request takes to process and returns this in a Server-Timing trailing HTTP header.
        /// It is used to surface any back-end server timing metrics (e.g. database read/write, CPU time, file system
        /// access, etc.) to the developer tools in the user's browser.
        /// </summary>
        /// <param name="application">The application builder.</param>
        /// <returns>The same application builder.</returns>
        public static IApplicationBuilder UseServerTiming(this IApplicationBuilder application)
        {
            if (application is null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return application.UseMiddleware<ServerTimingMiddleware>();
        }

        /// <summary>
        /// Executes the specified action if the specified <paramref name="condition"/> is <c>true</c> which can be
        /// used to conditionally add to the request execution pipeline.
        /// </summary>
        /// <param name="application">The application builder.</param>
        /// <param name="condition">If set to <c>true</c> the action is executed.</param>
        /// <param name="action">The action used to add to the request execution pipeline.</param>
        /// <returns>The same application builder.</returns>
        public static IApplicationBuilder UseIf(
            this IApplicationBuilder application,
            bool condition,
            Func<IApplicationBuilder, IApplicationBuilder> action)
        {
            if (application is null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (condition)
            {
                application = action(application);
            }

            return application;
        }

        /// <summary>
        /// Executes the specified <paramref name="ifAction"/> if the specified <paramref name="condition"/> is
        /// <c>true</c>, otherwise executes the <paramref name="elseAction"/>. This can be used to conditionally add to
        /// the request execution pipeline.
        /// </summary>
        /// <param name="application">The application builder.</param>
        /// <param name="condition">If set to <c>true</c> the <paramref name="ifAction"/> is executed, otherwise the
        /// <paramref name="elseAction"/> is executed.</param>
        /// <param name="ifAction">The action used to add to the request execution pipeline if the condition is
        /// <c>true</c>.</param>
        /// <param name="elseAction">The action used to add to the request execution pipeline if the condition is
        /// <c>false</c>.</param>
        /// <returns>The same application builder.</returns>
        public static IApplicationBuilder UseIfElse(
            this IApplicationBuilder application,
            bool condition,
            Func<IApplicationBuilder, IApplicationBuilder> ifAction,
            Func<IApplicationBuilder, IApplicationBuilder> elseAction)
        {
            if (application is null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            if (ifAction is null)
            {
                throw new ArgumentNullException(nameof(ifAction));
            }

            if (elseAction is null)
            {
                throw new ArgumentNullException(nameof(elseAction));
            }

            if (condition)
            {
                application = ifAction(application);
            }
            else
            {
                application = elseAction(application);
            }

            return application;
        }
    }
}
