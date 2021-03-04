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
        private const string OAuth2OpenApiReferenceId = "oauth2";
        private static readonly OpenApiSecurityScheme OAuth2OpenApiSecurityScheme = new()
        {
            Reference = new OpenApiReference()
            {
                Id = OAuth2OpenApiReferenceId,
                Type = ReferenceType.SecurityScheme,
            },
        };

        /// <inheritdoc/>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation is null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var filterDescriptors = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var authorizationRequirements = filterDescriptors.GetPolicyRequirements();
            var claimTypes = authorizationRequirements
                .OfType<ClaimsAuthorizationRequirement>()
                .Select(x => x.ClaimType)
                .ToList();
            if (claimTypes.Any())
            {
                operation.Security = new List<OpenApiSecurityRequirement>()
                {
                    new OpenApiSecurityRequirement()
                    {
                        { OAuth2OpenApiSecurityScheme, claimTypes },
                    },
                };
            }
        }
    }
}
