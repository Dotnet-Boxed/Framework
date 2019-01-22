namespace Boxed.AspNetCore.Middleware
{
    /// <summary>
    /// Options controlling <see cref="HttpExceptionMiddleware"/>.
    /// </summary>
    public class HttpExceptionMiddlewareOptions
    {
        /// <summary>
        /// Property controlling if ReasonPhrase should be included in HttpResponseMessage.
        /// </summary>
        public bool IncludeReasonPhraseInResponse { get; set; } = false;
    }
}