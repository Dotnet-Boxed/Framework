namespace Boxed.AspNetCore.Middleware;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

/// <summary>
/// A middleware which handles <see cref="OperationCanceledException"/> caused by the HTTP request being aborted, then
/// shortcuts and returns an error status code.
/// </summary>
/// <seealso cref="IMiddleware" />
public class RequestCanceledMiddleware : IMiddleware
{
    private readonly ILogger<RequestCanceledMiddleware> logger;
    private readonly RequestCanceledMiddlewareOptions options;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestCanceledMiddleware"/> class.
    /// </summary>
    /// <param name="options">The middleware options.</param>
    /// <param name="logger">A logger.</param>
    public RequestCanceledMiddleware(
        RequestCanceledMiddlewareOptions options,
        ILogger<RequestCanceledMiddleware> logger)
    {
        this.options = options;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        try
        {
            await next(context).ConfigureAwait(false);
        }
        catch (OperationCanceledException operationCanceledException)
        when (operationCanceledException.CancellationToken == context.RequestAborted)
        {
            this.logger.RequestCancelled();
            context.Response.StatusCode = this.options.StatusCode;
        }
    }
}
