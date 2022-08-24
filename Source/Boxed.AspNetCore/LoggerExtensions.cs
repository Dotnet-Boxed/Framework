namespace Boxed.AspNetCore;

using System;
using Microsoft.Extensions.Logging;

/// <summary>
/// <see cref="ILogger"/> extension methods. Helps log messages using strongly typing and source generators.
/// </summary>
internal static partial class LoggerExtensions
{
    /// <summary>
    /// The request cancelled message.
    /// </summary>
    public const string RequestCanceledMessage = "Client cancelled the request.";

    [LoggerMessage(
        EventId = 4000,
        Level = LogLevel.Information,
        Message = "Executing HttpExceptionMiddleware, setting HTTP status code {StatusCode}.")]
    public static partial void SettingHttpStatusCode(this ILogger logger, Exception exception, int statusCode);

    [LoggerMessage(
        EventId = 4001,
        Level = LogLevel.Information,
        Message = RequestCanceledMessage)]
    public static partial void RequestCanceled(this ILogger logger);
}
