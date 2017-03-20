namespace Boilerplate.AspNetCore.Swagger.OperationFilters
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization.Infrastructure;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Adds claims from any authorization policy's <see cref="ClaimsAuthorizationRequirement"/>'s.
    /// </summary>
    /// <seealso cref="IOperationFilter" />
    public class ClaimsOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="context">The context.</param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var filterDescriptors = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var authorizationRequirements = filterDescriptors.GetPolicyRequirements();
            var claimTypes = authorizationRequirements
                .OfType<ClaimsAuthorizationRequirement>()
                .Select(x => x.ClaimType);
            if (claimTypes.Any())
            {
                operation.Security = new List<IDictionary<string, IEnumerable<string>>>()
                {
                    new Dictionary<string, IEnumerable<string>>()
                    {
                        { "oauth2", claimTypes }
                    }
                };
            }
        }
    }
}
