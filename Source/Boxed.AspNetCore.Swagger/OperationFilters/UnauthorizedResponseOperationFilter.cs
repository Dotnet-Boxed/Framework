namespace Boxed.AspNetCore.Swagger.OperationFilters;

using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// Adds a 401 Unauthorized response to the Swagger response documentation when the authorization policy contains a
/// <see cref="DenyAnonymousAuthorizationRequirement"/>.
/// </summary>
/// <seealso cref="IOperationFilter" />
public class UnauthorizedResponseOperationFilter : IOperationFilter
{
    private const string UnauthorizedStatusCode = "401";
    private static readonly OpenApiResponse UnauthorizedResponse = new()
    {
        Description = "Unauthorized - The user has not supplied the necessary credentials to access the resource.",
    };

    /// <inheritdoc/>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentNullException.ThrowIfNull(context);

        var filterDescriptors = context.ApiDescription.ActionDescriptor.FilterDescriptors;
        var authorizationRequirements = filterDescriptors.GetPolicyRequirements();
        if (!operation.Responses.ContainsKey(UnauthorizedStatusCode) &&
            authorizationRequirements.OfType<DenyAnonymousAuthorizationRequirement>().Any())
        {
            operation.Responses.Add(UnauthorizedStatusCode, UnauthorizedResponse);
        }
    }
}
