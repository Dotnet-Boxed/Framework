namespace Boxed.AspNetCore.Middleware;

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
#if NET7_0_OR_GREATER
            var startingTimestamp = Stopwatch.GetTimestamp();
#else
            var stopWatch = Stopwatch.StartNew();
#endif

            await next(context).ConfigureAwait(false);

#if NET7_0_OR_GREATER
            var elapsedMilliseconds = Stopwatch.GetElapsedTime(startingTimestamp).TotalMilliseconds;
#else
            stopWatch.Stop();
            var elapsedMilliseconds = stopWatch.ElapsedMilliseconds;
#endif
            context.Response.AppendTrailer(ServerTimingHttpHeader, $"app;dur={elapsedMilliseconds}.0");
        }
        else
        {
            await next(context).ConfigureAwait(false);
        }
    }
}
