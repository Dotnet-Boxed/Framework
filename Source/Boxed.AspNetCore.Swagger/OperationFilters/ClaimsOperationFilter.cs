namespace Boxed.AspNetCore.Swagger.OperationFilters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization.Infrastructure;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Adds claims from any authorization policy's <see cref="ClaimsAuthorizationRequirement"/>'s.
    /// </summary>
    /// <seealso cref="IOperationFilter" />
    public class ClaimsOperationFilter : IOperationFilter
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

            var filterDescriptors = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var authorizationRequirements = filterDescriptors.GetPolicyRequirements();
            var claimTypes = authorizationRequirements
                .OfType<ClaimsAuthorizationRequirement>()
                .Select(x => x.ClaimType);
            if (claimTypes.Any())
            {
                operation.Security = new List<OpenApiSecurityRequirement>()
                {
                    new OpenApiSecurityRequirement()
                    {
                        // { new OpenApiSecurityScheme() { "oauth2" }, claimTypes }
                    }
                };
            }
        }
    }
}
