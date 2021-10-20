namespace Boxed.AspNetCore.Middleware
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Measures the time the request takes to process and returns this in a Server-Timing trailing HTTP header. It is
    /// used to surface any back-end server timing metrics (e.g. database read/write, CPU time, file system access,
    /// etc.) to the developer tools in the user's browser.
    /// </summary>
    /// <seealso cref="IMiddleware" />
    public class ServerTimingMiddleware : IMiddleware
    {
        private const string ServerTimingHttpHeader = "Server-Timing";

        /// <inheritdoc/>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(next);

            if (context.Response.SupportsTrailers())
            {
                context.Response.DeclareTrailer(ServerTimingHttpHeader);
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                await next(context).ConfigureAwait(false);

                stopWatch.Stop();
                context.Response.AppendTrailer(ServerTimingHttpHeader, $"app;dur={stopWatch.ElapsedMilliseconds}.0");
            }
            else
            {
                await next(context).ConfigureAwait(false);
            }
        }
    }
}
