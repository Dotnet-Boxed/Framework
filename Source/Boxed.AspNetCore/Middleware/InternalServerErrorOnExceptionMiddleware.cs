namespace Boxed.AspNetCore.Middleware
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    internal class InternalServerErrorOnExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
            {
                var factory = context.RequestServices.GetRequiredService<ILoggerFactory>();
                var logger = factory.CreateLogger<InternalServerErrorOnExceptionMiddleware>();
                logger.LogInformation(
                    "Executing InternalServerErrorOnExceptionMiddleware, setting HTTP status code 500.");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}
