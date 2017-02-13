namespace Boilerplate.AspNetCore.Swagger.OperationFilters
{
    using System.Collections.Generic;
    using System.Linq;
    using Boilerplate.AspNetCore.Filters;
    using Swashbuckle.Swagger.Model;
    using Swashbuckle.SwaggerGen.Generator;

    /// <summary>
    /// Adds a Swashbuckle <see cref="NonBodyParameter"/> if the action method has a
    /// <see cref="UserAgentHttpHeaderAttribute"/> filter applied to it. It adds a full description of the User-Agent
    /// HTTP header and adds a default value.
    /// </summary>
    /// <seealso cref="Swashbuckle.SwaggerGen.Generator.IOperationFilter" />
    public class UserAgentOperationFilter : IOperationFilter
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
                .OfType<UserAgentHttpHeaderAttribute>()
                .FirstOrDefault();
            if (filter != null)
            {
                if (operation.Parameters == null)
                {
                    operation.Parameters = new List<IParameter>();
                }

                operation.Parameters.Add(
                    new NonBodyParameter()
                    {
                        Default = "Application-Name/1.0.0 (Operating System Name 1.0.0)",
                        Description = "Used to identify the application making the HTTP request.",
                        In = "header",
                        Name = "User-Agent",
                        Required = filter.Required,
                        Type = "string"
                    });
                }
        }
    }
}
