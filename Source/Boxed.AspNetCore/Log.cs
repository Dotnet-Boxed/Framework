namespace Boxed.AspNetCore
{
    using System;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Logs messages using strongly typing and source generators.
    /// </summary>
    internal static partial class Log
    {
        [LoggerMessage(
            EventId = 5000,
            Level = LogLevel.Information,
            Message = "Executing HttpExceptionMiddleware, setting HTTP status code {StatusCode}.")]
        public static partial void SettingHttpStatusCode(this ILogger logger, Exception exception, int statusCode);
    }
}
