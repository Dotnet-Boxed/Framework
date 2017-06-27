namespace Boilerplate.AspNetCore.Swagger.OperationFilters
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authorization.Infrastructure;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Adds a 403 Forbidden response to the Swagger response documentation when the authorization policy contains a
    /// <see cref="ClaimsAuthorizationRequirement"/>, <see cref="NameAuthorizationRequirement"/>,
    /// <see cref="RolesAuthorizationRequirement"/> or <see cref="AssertionRequirement"/>.
    /// </summary>
    /// <seealso cref="IOperationFilter" />
    public class ForbiddenResponseOperationFilter : IOperationFilter
    {
        private const string ForbiddenStatusCode = "403";
        private static readonly Response ForbiddenResponse = new Response()
        {
            Description = "Forbidden - The user does not have the necessary permissions to access the resource."
        };

        /// <summary>
        /// Applies the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="context">The context.</param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
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
                if (authorizationRequirement is ClaimsAuthorizationRequirement ||
                    authorizationRequirement is NameAuthorizationRequirement ||
                    authorizationRequirement is RolesAuthorizationRequirement ||
                    authorizationRequirement is AssertionRequirement)
                {
                    return true;
                }
            }

            return false;
        }
    }
}