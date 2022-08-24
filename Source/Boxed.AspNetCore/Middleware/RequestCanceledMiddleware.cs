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
public class RequestCanceledMiddleware
{
    private readonly ILogger<RequestCanceledMiddleware> logger;
    private readonly RequestDelegate next;
    private readonly RequestCanceledMiddlewareOptions options;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestCanceledMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware.</param>
    /// <param name="options">The middleware options.</param>
    /// <param name="logger">A logger.</param>
    public RequestCanceledMiddleware(
        RequestDelegate next,
        RequestCanceledMiddlewareOptions options,
        ILogger<RequestCanceledMiddleware> logger)
    {
        this.next = next;
        this.options = options;
        this.logger = logger;
    }

    /// <summary>
    /// Request handling method.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
    /// <returns>A <see cref="Task"/> that represents the execution of this middleware.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            await this.next(context).ConfigureAwait(false);
        }
        catch (OperationCanceledException operationCanceledException)
        when (operationCanceledException.CancellationToken == context.RequestAborted)
        {
            this.logger.RequestCanceled();
            context.Response.StatusCode = this.options.StatusCode;
        }
    }
}
