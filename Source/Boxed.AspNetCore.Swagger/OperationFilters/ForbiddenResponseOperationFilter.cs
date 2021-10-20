namespace Boxed.AspNetCore.Swagger.OperationFilters
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authorization.Infrastructure;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Adds a 403 Forbidden response to the Swagger response documentation when the authorization policy contains a
    /// <see cref="ClaimsAuthorizationRequirement"/>, <see cref="NameAuthorizationRequirement"/>,
    /// <see cref="OperationAuthorizationRequirement"/>, <see cref="RolesAuthorizationRequirement"/> or
    /// <see cref="AssertionRequirement"/>.
    /// </summary>
    /// <seealso cref="IOperationFilter" />
    public class ForbiddenResponseOperationFilter : IOperationFilter
    {
        private const string ForbiddenStatusCode = "403";
        private static readonly OpenApiResponse ForbiddenResponse = new()
        {
            Description = "Forbidden - The user does not have the necessary permissions to access the resource.",
        };

        /// <inheritdoc/>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            ArgumentNullException.ThrowIfNull(operation);
            ArgumentNullException.ThrowIfNull(context);

            var filterDescriptors = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var authorizationRequirements = filterDescriptors.GetPolicyRequirements();
            if (!operation.Responses.ContainsKey(ForbiddenStatusCode) &&
                HasAuthorizationRequirement(authorizationRequirements))
            {
                operation.Responses.Add(ForbiddenStatusCode, ForbiddenResponse);
            }
        }

        private static bool HasAuthorizationRequirement(
            IEnumerable<IAuthorizationRequirement> authorizationRequirements)
        {
            foreach (var authorizationRequirement in authorizationRequirements)
            {
                if (authorizationRequirement is ClaimsAuthorizationRequirement or
                    NameAuthorizationRequirement or
                    OperationAuthorizationRequirement or
                    RolesAuthorizationRequirement or
                    AssertionRequirement)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
