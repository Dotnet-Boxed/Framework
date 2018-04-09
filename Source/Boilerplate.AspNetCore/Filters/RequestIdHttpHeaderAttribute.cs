namespace Boilerplate.AspNetCore.Filters
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// Require the X-Request-ID HTTP header to be specified in a request optionally forward it in the response.
    /// </summary>
    /// <seealso cref="HttpHeaderAttribute" />
    public class RequestIdHttpHeaderAttribute : HttpHeaderAttribute
    {
        private const string RequestIdHttpHeaderName = "X-Request-ID";

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestIdHttpHeaderAttribute"/> class.
        /// </summary>
        public RequestIdHttpHeaderAttribute()
            : base(RequestIdHttpHeaderName)
        {
        }

        /// <summary>
        /// Gets or sets whether to set the trace identifier for the current request.
        /// </summary>
        public bool UpdateTraceIdentifier { get; set; } = true;

        /// <summary>
        /// Returns <c>true</c> if the X-Request-ID HTTP header contains a GUID, otherwise <c>false</c>.
        /// </summary>
        /// <param name="headerValues">The header values.</param>
        /// <returns><c>true</c> if the X-Request-ID HTTP header values are valid; otherwise, <c>false</c>.</returns>
        public override bool IsValid(StringValues headerValues) =>
            !StringValues.IsNullOrEmpty(headerValues) && headerValues.All(x => Guid.TryParse(x, out Guid guid));

        /// <inheritdoc />
        public override void OnHasHeader(ActionExecutingContext context, StringValues headerValue)
        {
            if (this.UpdateTraceIdentifier)
            {
                context.HttpContext.TraceIdentifier = headerValue;
            }
        }
    }
}