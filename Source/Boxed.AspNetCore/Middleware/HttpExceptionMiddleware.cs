namespace Boxed.AspNetCore.Middleware;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

/// <summary>
/// The <see cref="HttpException"/> handling middleware.
/// </summary>
/// <seealso cref="IMiddleware" />
public class HttpExceptionMiddleware
{
    private readonly RequestDelegate next;
    private readonly HttpExceptionMiddlewareOptions options;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpExceptionMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware.</param>
    /// <param name="options">The options.</param>
    public HttpExceptionMiddleware(RequestDelegate next, HttpExceptionMiddlewareOptions options)
    {
        this.next = next;
        this.options = options;
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
            await this.next.Invoke(context).ConfigureAwait(false);
        }
        catch (HttpException httpException)
        {
            var factory = context.RequestServices.GetRequiredService<ILoggerFactory>();
            var logger = factory.CreateLogger<HttpExceptionMiddleware>();
            logger.SettingHttpStatusCode(httpException, httpException.StatusCode);

            context.Response.StatusCode = httpException.StatusCode;
            if (this.options.IncludeReasonPhraseInResponse)
            {
                var responseFeature = context.Features.Get<IHttpResponseFeature>();
                if (responseFeature is not null)
                {
                    responseFeature.ReasonPhrase = httpException.Message;
                }
            }
        }
    }
}
