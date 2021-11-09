namespace Boxed.AspNetCore;

using System;
using Microsoft.Extensions.Logging;

/// <summary>
/// <see cref="ILogger"/> extension methods. Helps log messages using strongly typing and source generators.
/// </summary>
internal static partial class LoggerExtensions
{
    [LoggerMessage(
        EventId = 4000,
        Level = LogLevel.Information,
        Message = "Executing HttpExceptionMiddleware, setting HTTP status code {StatusCode}.")]
    public static partial void SettingHttpStatusCode(this ILogger logger, Exception exception, int statusCode);
}
