namespace Boxed.AspNetCore.Middleware;

/// <summary>
/// Options controlling <see cref="RequestCanceledMiddleware"/>.
/// </summary>
public class RequestCanceledMiddlewareOptions
{
    /// <summary>
    /// The non-standard 499 status code 'Client Closed Request' used by NGINX to signify an aborted/cancelled request.
    /// </summary>
    public const int ClientClosedRequest = 499;

    /// <summary>
    /// Gets or sets the status code to return for a cancelled request. The default is the non-standard 499
    /// 'Client Closed Request' used by NGINX.
    /// See https://stackoverflow.com/questions/46234679/what-is-the-correct-http-status-code-for-a-cancelled-request.
    /// </summary>
    public int StatusCode { get; set; } = ClientClosedRequest;
}
