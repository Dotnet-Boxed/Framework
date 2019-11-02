namespace Boxed.AspNetCore.Swagger.OperationFilters
{
    using System;
    using System.Collections.Generic;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Adds an Open API <see cref="OpenApiParameter"/> to all operations with a description of the X-Correlation-ID
    /// HTTP header and a default GUID value.
    /// </summary>
    /// <seealso cref="IOperationFilter" />
    public class CorrelationIdOperationFilter : IOperationFilter
    {
        /// <inheritdoc/>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(
                new OpenApiParameter()
                {
                    Description = "Used to uniquely identify the HTTP request. This ID is used to correlate the HTTP request between a client and server.",
                    In = ParameterLocation.Header,
                    Name = "X-Correlation-ID",
                    Required = false,
                    Schema = new OpenApiSchema()
                    {
                        Default = new OpenApiString(Guid.NewGuid().ToString()),
                        Type = "string",
                    },
                });
        }
    }
}
