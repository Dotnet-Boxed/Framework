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
public class HttpExceptionMiddleware : IMiddleware
{
    private readonly RequestDelegate next;
    private readonly HttpExceptionMiddlewareOptions options;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpExceptionMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next.</param>
    /// <param name="options">The options.</param>
    public HttpExceptionMiddleware(RequestDelegate next, HttpExceptionMiddlewareOptions options)
    {
        this.next = next;
        this.options = options;
    }

    /// <inheritdoc/>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

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
