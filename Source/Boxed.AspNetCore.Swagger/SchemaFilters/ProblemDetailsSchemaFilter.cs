namespace Boxed.AspNetCore.Swagger.SchemaFilters
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Shows an example of a <see cref="ProblemDetails"/> containing errors.
    /// </summary>
    /// <seealso cref="ISchemaFilter" />
    public class ProblemDetailsSchemaFilter : ISchemaFilter
    {
#pragma warning disable
        private static readonly OpenApiObject Status400ProblemDetails = new OpenApiObject()
        {
            ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.1"),
            ["title"] = new OpenApiString("Bad Request"),
            ["status"] = new OpenApiInteger(StatusCodes.Status400BadRequest),
            ["traceId"] = new OpenApiString("00-982607166a542147b435be3a847ddd71-fc75498eb9f09d48-00"),
            ["errors"] = new OpenApiObject()
            {
                ["property1"] = new OpenApiArray()
                {
                    new OpenApiString("The property field is required"),
                },
            },
        };

        private static readonly OpenApiObject Status401ProblemDetails = new OpenApiObject()
        {
            ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7235#section-3.1"),
            ["title"] = new OpenApiString("Unauthorized"),
            ["status"] = new OpenApiInteger(StatusCodes.Status401Unauthorized),
            ["traceId"] = new OpenApiString("00-982607166a542147b435be3a847ddd71-fc75498eb9f09d48-00"),
        };

        private static readonly OpenApiObject Status403ProblemDetails = new OpenApiObject()
        {
            ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.3"),
            ["title"] = new OpenApiString("Forbidden"),
            ["status"] = new OpenApiInteger(StatusCodes.Status403Forbidden),
            ["traceId"] = new OpenApiString("00-982607166a542147b435be3a847ddd71-fc75498eb9f09d48-00"),
        };

        private static readonly OpenApiObject Status404ProblemDetails = new OpenApiObject()
        {
            ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.4"),
            ["title"] = new OpenApiString("Not Found"),
            ["status"] = new OpenApiInteger(StatusCodes.Status404NotFound),
            ["traceId"] = new OpenApiString("00-982607166a542147b435be3a847ddd71-fc75498eb9f09d48-00"),
        };

        private static readonly OpenApiObject Status406ProblemDetails = new OpenApiObject()
        {
            ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.6"),
            ["title"] = new OpenApiString("Not Acceptable"),
            ["status"] = new OpenApiInteger(StatusCodes.Status406NotAcceptable),
            ["traceId"] = new OpenApiString("00-982607166a542147b435be3a847ddd71-fc75498eb9f09d48-00"),
        };

        private static readonly OpenApiObject Status409ProblemDetails = new OpenApiObject()
        {
            ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.8"),
            ["title"] = new OpenApiString("Conflict"),
            ["status"] = new OpenApiInteger(StatusCodes.Status409Conflict),
            ["traceId"] = new OpenApiString("00-982607166a542147b435be3a847ddd71-fc75498eb9f09d48-00"),
        };

        private static readonly OpenApiObject Status415ProblemDetails = new OpenApiObject()
        {
            ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.13"),
            ["title"] = new OpenApiString("Unsupported Media Type"),
            ["status"] = new OpenApiInteger(StatusCodes.Status415UnsupportedMediaType),
            ["traceId"] = new OpenApiString("00-982607166a542147b435be3a847ddd71-fc75498eb9f09d48-00"),
        };

        private static readonly OpenApiObject Status422ProblemDetails = new OpenApiObject()
        {
            ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc4918#section-11.2"),
            ["title"] = new OpenApiString("Unprocessable Entity"),
            ["status"] = new OpenApiInteger(StatusCodes.Status422UnprocessableEntity),
            ["traceId"] = new OpenApiString("00-982607166a542147b435be3a847ddd71-fc75498eb9f09d48-00"),
        };

        private static readonly OpenApiObject Status500ProblemDetails = new OpenApiObject()
        {
            ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.6.1"),
            ["title"] = new OpenApiString("Internal Server Error"),
            ["status"] = new OpenApiInteger(StatusCodes.Status500InternalServerError),
            ["traceId"] = new OpenApiString("00-982607166a542147b435be3a847ddd71-fc75498eb9f09d48-00"),
        };
#pragma warning restore

        /// <inheritdoc/>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema == null)
            {
                throw new ArgumentNullException(nameof(schema));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.ApiModel.Type == typeof(ProblemDetails))
            {
                // TODO: Set the default and example based on the status code.
                // schema.Default = ProblemDetails;
                // schema.Example = ProblemDetails;
            }
        }
    }
}
