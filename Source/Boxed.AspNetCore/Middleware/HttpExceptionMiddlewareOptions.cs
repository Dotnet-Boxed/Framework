namespace Boxed.AspNetCore.Middleware
{
    /// <summary>
    /// Options controlling <see cref="HttpExceptionMiddleware"/>.
    /// </summary>
    public class HttpExceptionMiddlewareOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether ReasonPhrase should be included in HttpResponseMessage.
        /// </summary>
        public bool IncludeReasonPhraseInResponse { get; set; } = false;
    }
}
