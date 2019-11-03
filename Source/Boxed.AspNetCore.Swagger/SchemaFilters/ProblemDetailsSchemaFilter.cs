namespace Boxed.AspNetCore.Swagger.SchemaFilters
{
    using System;
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
        private static readonly OpenApiObject ProblemDetails = new OpenApiObject()
        {
            { "type", new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.4") },
            { "title", new OpenApiString("Error description") },
            { "status", new OpenApiInteger(400) },
            { "traceId", new OpenApiString("00-fbe064c2e2f29548b7f810d792145bee-148546d7edef1143-00") },
        };

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
                schema.Default = ProblemDetails;
                schema.Example = ProblemDetails;
            }
        }
    }
}
