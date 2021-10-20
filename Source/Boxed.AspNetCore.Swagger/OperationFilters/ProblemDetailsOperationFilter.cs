namespace Boxed.AspNetCore.Swagger.OperationFilters
{
    using System;
    using Boxed.AspNetCore;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Shows an example of a <see cref="ProblemDetails"/> containing errors.
    /// </summary>
    /// <seealso cref="IOperationFilter" />
    public class ProblemDetailsOperationFilter : IOperationFilter
    {
        private const string StatusCode400 = "400";
        private const string StatusCode401 = "401";
        private const string StatusCode403 = "403";
        private const string StatusCode404 = "404";
        private const string StatusCode406 = "406";
        private const string StatusCode409 = "409";
        private const string StatusCode415 = "415";
        private const string StatusCode422 = "422";
        private const string StatusCode500 = "500";

        /// <summary>
        /// Gets the 400 Bad Request example of a <see cref="ProblemDetails"/> response.
        /// </summary>
        public static OpenApiObject Status400ProblemDetails { get; } =
            new()
            {
                ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.1"),
                ["title"] = new OpenApiString("Bad Request"),
                ["status"] = new OpenApiInteger(StatusCodes.Status400BadRequest),
                ["traceId"] = new OpenApiString("00-982607166a542147b435be3a847ddd71-fc75498eb9f09d48-00"),
                ["errors"] = new OpenApiObject()
                {
                    ["exampleProperty1"] = new OpenApiArray()
                    {
                        new OpenApiString("The property field is required"),
                    },
                },
            };

        /// <summary>
        /// Gets the 401 Unauthorized example of a <see cref="ProblemDetails"/> response.
        /// </summary>
        public static OpenApiObject Status401ProblemDetails { get; } =
            new()
            {
                ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7235#section-3.1"),
                ["title"] = new OpenApiString("Unauthorized"),
                ["status"] = new OpenApiInteger(StatusCodes.Status401Unauthorized),
                ["traceId"] = new OpenApiString("00-982607166a542147b435be3a847ddd71-fc75498eb9f09d48-00"),
            };

        /// <summary>
        /// Gets the 403 Forbidden example of a <see cref="ProblemDetails"/> response.
        /// </summary>
        public static OpenApiObject Status403ProblemDetails { get; } =
            new()
            {
                ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.3"),
                ["title"] = new OpenApiString("Forbidden"),
                ["status"] = new OpenApiInteger(StatusCodes.Status403Forbidden),
                ["traceId"] = new OpenApiString("00-982607166a542147b435be3a847ddd71-fc75498eb9f09d48-00"),
            };

        /// <summary>
        /// Gets the 404 Not Found example of a <see cref="ProblemDetails"/> response.
        /// </summary>
        public static OpenApiObject Status404ProblemDetails { get; } =
            new()
            {
                ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.4"),
                ["title"] = new OpenApiString("Not Found"),
                ["status"] = new OpenApiInteger(StatusCodes.Status404NotFound),
                ["traceId"] = new OpenApiString("00-982607166a542147b435be3a847ddd71-fc75498eb9f09d48-00"),
            };

        /// <summary>
        /// Gets the 406 Not Acceptable example of a <see cref="ProblemDetails"/> response.
        /// </summary>
        public static OpenApiObject Status406ProblemDetails { get; } =
            new()
            {
                ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.6"),
                ["title"] = new OpenApiString("Not Acceptable"),
                ["status"] = new OpenApiInteger(StatusCodes.Status406NotAcceptable),
                ["traceId"] = new OpenApiString("00-982607166a542147b435be3a847ddd71-fc75498eb9f09d48-00"),
            };

        /// <summary>
        /// Gets the 409 Conflict example of a <see cref="ProblemDetails"/> response.
        /// </summary>
        public static OpenApiObject Status409ProblemDetails { get; } =
            new()
            {
                ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.8"),
                ["title"] = new OpenApiString("Conflict"),
                ["status"] = new OpenApiInteger(StatusCodes.Status409Conflict),
                ["traceId"] = new OpenApiString("00-982607166a542147b435be3a847ddd71-fc75498eb9f09d48-00"),
            };

        /// <summary>
        /// Gets the 415 Unsupported Media Type example of a <see cref="ProblemDetails"/> response.
        /// </summary>
        public static OpenApiObject Status415ProblemDetails { get; } =
            new()
            {
                ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.13"),
                ["title"] = new OpenApiString("Unsupported Media Type"),
                ["status"] = new OpenApiInteger(StatusCodes.Status415UnsupportedMediaType),
                ["traceId"] = new OpenApiString("00-982607166a542147b435be3a847ddd71-fc75498eb9f09d48-00"),
            };

        /// <summary>
        /// Gets the 422 Unprocessable Entity example of a <see cref="ProblemDetails"/> response.
        /// </summary>
        public static OpenApiObject Status422ProblemDetails { get; } =
            new()
            {
                ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc4918#section-11.2"),
                ["title"] = new OpenApiString("Unprocessable Entity"),
                ["status"] = new OpenApiInteger(StatusCodes.Status422UnprocessableEntity),
                ["traceId"] = new OpenApiString("00-982607166a542147b435be3a847ddd71-fc75498eb9f09d48-00"),
            };

        /// <summary>
        /// Gets the 500 Internal Server Error example of a <see cref="ProblemDetails"/> response.
        /// </summary>
        public static OpenApiObject Status500ProblemDetails { get; } =
            new()
            {
                ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.6.1"),
                ["title"] = new OpenApiString("Internal Server Error"),
                ["status"] = new OpenApiInteger(StatusCodes.Status500InternalServerError),
                ["traceId"] = new OpenApiString("00-982607166a542147b435be3a847ddd71-fc75498eb9f09d48-00"),
            };

        /// <inheritdoc/>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            ArgumentNullException.ThrowIfNull(operation);
            ArgumentNullException.ThrowIfNull(context);

            foreach (var keyValuePair in operation.Responses)
            {
                switch (keyValuePair.Key)
                {
                    case StatusCode400:
                        SetDefaultAndExample(keyValuePair.Value, Status400ProblemDetails);
                        break;
                    case StatusCode401:
                        SetDefaultAndExample(keyValuePair.Value, Status401ProblemDetails);
                        break;
                    case StatusCode403:
                        SetDefaultAndExample(keyValuePair.Value, Status403ProblemDetails);
                        break;
                    case StatusCode404:
                        SetDefaultAndExample(keyValuePair.Value, Status404ProblemDetails);
                        break;
                    case StatusCode406:
                        SetDefaultAndExample(keyValuePair.Value, Status406ProblemDetails);
                        break;
                    case StatusCode409:
                        SetDefaultAndExample(keyValuePair.Value, Status409ProblemDetails);
                        break;
                    case StatusCode415:
                        SetDefaultAndExample(keyValuePair.Value, Status415ProblemDetails);
                        break;
                    case StatusCode422:
                        SetDefaultAndExample(keyValuePair.Value, Status422ProblemDetails);
                        break;
                    case StatusCode500:
                        SetDefaultAndExample(keyValuePair.Value, Status500ProblemDetails);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void SetDefaultAndExample(OpenApiResponse value, OpenApiObject status500ProblemDetails)
        {
            if (value.Content is not null)
            {
                if (value.Content.TryGetValue(ContentType.ProblemJson, out var problemJsonMediaType))
                {
                    problemJsonMediaType.Example = status500ProblemDetails;
                }

                if (value.Content.TryGetValue(ContentType.ProblemXml, out var problemXmlMediaType))
                {
                    problemXmlMediaType.Example = status500ProblemDetails;
                }
            }
        }
    }
}
