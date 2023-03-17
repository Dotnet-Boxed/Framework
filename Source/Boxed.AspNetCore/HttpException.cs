namespace Boxed.AspNetCore;

using System;
using System.Net;

/// <summary>
/// Describes an exception that occurred during the processing of HTTP requests.
/// </summary>
/// <seealso cref="Exception" />
#pragma warning disable CA1032 // Implement standard exception constructors
public class HttpException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException" /> class.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    public HttpException(int httpStatusCode) =>
        this.StatusCode = httpStatusCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException" /> class.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    public HttpException(HttpStatusCode httpStatusCode) =>
        this.StatusCode = (int)httpStatusCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException" /> class.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    /// <param name="message">The exception message.</param>
    public HttpException(int httpStatusCode, string message)
        : base(message) =>
        this.StatusCode = httpStatusCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException" /> class.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    /// <param name="message">The exception message.</param>
    public HttpException(HttpStatusCode httpStatusCode, string message)
        : base(message) =>
        this.StatusCode = (int)httpStatusCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException" /> class.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    /// <param name="message">The exception message.</param>
    /// <param name="inner">The inner exception.</param>
    public HttpException(int httpStatusCode, string message, Exception inner)
        : base(message, inner) =>
        this.StatusCode = httpStatusCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException" /> class.
    /// </summary>
    /// <param name="httpStatusCode">The HTTP status code.</param>
    /// <param name="message">The exception message.</param>
    /// <param name="inner">The inner exception.</param>
    public HttpException(HttpStatusCode httpStatusCode, string message, Exception inner)
        : base(message, inner) =>
        this.StatusCode = (int)httpStatusCode;

    /// <summary>
    /// Gets the HTTP status code.
    /// </summary>
    /// <value>
    /// The HTTP status code.
    /// </value>
    public int StatusCode { get; }
}
