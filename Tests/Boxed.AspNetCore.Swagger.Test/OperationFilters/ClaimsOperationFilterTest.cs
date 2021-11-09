namespace Boxed.AspNetCore.Swagger.Test.OperationFilters;

using System;
using System.Collections.Generic;
using System.Linq;
using Boxed.AspNetCore.Swagger.OperationFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Moq;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;
using FilterDescriptor = Microsoft.AspNetCore.Mvc.Filters.FilterDescriptor;

public class ClaimsOperationFilterTest
{
    private readonly ApiDescription apiDescription;
    private readonly OpenApiOperation operation;
    private readonly OperationFilterContext context;
    private readonly ClaimsOperationFilter operationFilter;

    public ClaimsOperationFilterTest()
    {
        this.operation = new OpenApiOperation()
        {
            Responses = new OpenApiResponses(),
        };
        this.apiDescription = new ApiDescription()
        {
            ActionDescriptor = new ActionDescriptor()
            {
                FilterDescriptors = new List<FilterDescriptor>(),
            },
        };
        this.context = new OperationFilterContext(
            this.apiDescription,
            new Mock<ISchemaGenerator>().Object,
            new SchemaRepository(),
            this.GetType().GetMethods().First());
        this.operationFilter = new ClaimsOperationFilter();
    }

    [Fact]
    public void Apply_HasClaimsAuthorizationRequirements_AddsClaimsToOperation()
    {
        var requirement = new ClaimsAuthorizationRequirement("Type", Array.Empty<string>());
        var requirements = new List<IAuthorizationRequirement>() { requirement };
        var policy = new AuthorizationPolicy(requirements, new List<string>());
        var filterDescriptor = new FilterDescriptor(new AuthorizeFilter(policy), 30);
        this.context.ApiDescription.ActionDescriptor.FilterDescriptors.Add(filterDescriptor);

        this.operationFilter.Apply(this.operation, this.context);

        Assert.NotNull(this.operation.Security);
        Assert.Equal(1, this.operation.Security.Count);
        Assert.Single(this.operation.Security.First());
        Assert.Equal("oauth2", this.operation.Security.First().First().Key.Reference.Id);
        Assert.Equal(new string[] { "Type" }, this.operation.Security.First().First().Value);
    }

    [Fact]
    public void Apply_HasPolicyWithNoClaimsAuthorizationRequirements_DoesNothing()
    {
        var requirement = new DenyAnonymousAuthorizationRequirement();
        var requirements = new List<IAuthorizationRequirement>() { requirement };
        var policy = new AuthorizationPolicy(requirements, new List<string>());
        var filterDescriptor = new FilterDescriptor(new AuthorizeFilter(policy), 30);
        this.context.ApiDescription.ActionDescriptor.FilterDescriptors.Add(filterDescriptor);

        this.operationFilter.Apply(this.operation, this.context);

        Assert.Empty(this.operation.Security);
    }

    [Fact]
    public void Apply_DoesNotHaveClaimsAuthorizationRequirement_DoesNothing()
    {
        this.operationFilter.Apply(this.operation, this.context);

        Assert.Empty(this.operation.Security);
    }
}
