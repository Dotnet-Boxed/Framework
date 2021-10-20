namespace Boxed.AspNetCore
{
    using System;
    using System.Net;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// <see cref="HttpRequest"/> extension methods.
    /// </summary>
    public static class HttpRequestExtensions
    {
        private const string XmlHttpRequest = "XMLHttpRequest";

        /// <summary>
        /// Determines whether the specified HTTP request is an AJAX request.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <returns><c>true</c> if the specified HTTP request is an AJAX request; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="request"/> parameter is <c>null</c>.</exception>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (request.Headers is not null)
            {
                return request.Headers.XRequestedWith == XmlHttpRequest;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified HTTP request is a local request where the IP address of the request
        /// originator was 127.0.0.1.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <returns><c>true</c> if the specified HTTP request is a local request; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="request"/> parameter is <c>null</c>.</exception>
        public static bool IsLocalRequest(this HttpRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var connection = request.HttpContext.Connection;
            if (connection.RemoteIpAddress is not null)
            {
                if (connection.LocalIpAddress is not null)
                {
                    return connection.RemoteIpAddress.Equals(connection.LocalIpAddress);
                }
                else
                {
                    return IPAddress.IsLoopback(connection.RemoteIpAddress);
                }
            }

            // for in memory TestServer or when dealing with default connection info
            if (connection.RemoteIpAddress is null && connection.LocalIpAddress is null)
            {
                return true;
            }

            return false;
        }
    }
}
