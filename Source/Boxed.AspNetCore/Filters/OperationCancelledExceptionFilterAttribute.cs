namespace Boxed.AspNetCore.Filters
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Return a 499 Client Closed Request HTTP status code when the client application closes the TCP connection.
    /// </summary>
    public class OperationCancelledExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private const int ClientClosedRequestHttpStatusCode = 499;
        private readonly ILogger<OperationCancelledExceptionFilterAttribute> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCancelledExceptionFilterAttribute"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public OperationCancelledExceptionFilterAttribute(ILogger<OperationCancelledExceptionFilterAttribute> logger) =>
            this.logger = logger;

        /// <summary>
        /// Handle an exception when it occurs.
        /// </summary>
        /// <param name="context">The exception context.</param>
        public override void OnException(ExceptionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Exception is OperationCanceledException)
            {
                this.logger.LogInformation("Request was cancelled");
                context.ExceptionHandled = true;
                context.Result = new StatusCodeResult(ClientClosedRequestHttpStatusCode);
            }
        }
    }
}
