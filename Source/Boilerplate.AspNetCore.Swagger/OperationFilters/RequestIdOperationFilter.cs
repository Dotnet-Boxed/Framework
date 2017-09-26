namespace Boilerplate.AspNetCore.Swagger.OperationFilters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Boilerplate.AspNetCore.Filters;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Adds a Swashbuckle <see cref="NonBodyParameter"/> if the action method has a
    /// <see cref="RequestIdHttpHeaderAttribute"/> filter applied to it. It adds a full description of the X-Request-ID
    /// HTTP header and adds a default value.
    /// </summary>
    /// <seealso cref="IOperationFilter" />
    public class RequestIdOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="context">The context.</param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var filter = context.ApiDescription.ActionDescriptor.FilterDescriptors
                .Select(x => x.Filter)
                .OfType<RequestIdHttpHeaderAttribute>()
                .FirstOrDefault();
            if (filter != null)
            {
                if (operation.Parameters == null)
                {
                    operation.Parameters = new List<IParameter>();
                }

                var description = "Used to uniquely identify the HTTP request. This ID is used to correlate the HTTP request between a client and server.";
                if (filter.Forward)
                {
                    description += " The ID is also mirrored in the response HTTP header.";
                }

                operation.Parameters.Add(
                    new NonBodyParameter()
                    {
                        Default = Guid.NewGuid(),
                        Description = description,
                        In = "header",
                        Name = "X-Request-ID",
                        Required = filter.Required,
                        Type = "string"
                    });
            }
        }
    }
}
