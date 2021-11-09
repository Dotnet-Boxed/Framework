namespace Boxed.AspNetCore.Swagger.Test.OperationFilters;

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

public class UnauthorizedResponseOperationFilterTest
{
    private readonly ApiDescription apiDescription;
    private readonly OpenApiOperation operation;
    private readonly OperationFilterContext context;
    private readonly UnauthorizedResponseOperationFilter operationFilter;

    public UnauthorizedResponseOperationFilterTest()
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
        this.operationFilter = new UnauthorizedResponseOperationFilter();
    }

    [Fact]
    public void Apply_HasDenyAnonymousAuthorizationRequirement_Adds401Response()
    {
        var requirement = new DenyAnonymousAuthorizationRequirement();
        var requirements = new List<IAuthorizationRequirement>() { requirement };
        var policy = new AuthorizationPolicy(requirements, new List<string>());
        var filterDescriptor = new FilterDescriptor(new AuthorizeFilter(policy), 30);
        this.context.ApiDescription.ActionDescriptor.FilterDescriptors.Add(filterDescriptor);

        this.operationFilter.Apply(this.operation, this.context);

        Assert.True(this.operation.Responses.ContainsKey("401"));
    }

    [Fact]
    public void Apply_DoesNotHaveDenyAnonymousAuthorizationRequirement_DoesNothing()
    {
        this.operationFilter.Apply(this.operation, this.context);

        Assert.Empty(this.operation.Responses);
    }

    [Fact]
    public void Apply_AlreadyHasForbiddenResponse_DoesNothing()
    {
        var response = new OpenApiResponse();
        this.operation.Responses.Add("401", response);
        this.operationFilter.Apply(this.operation, this.context);

        Assert.Single(this.operation.Responses);
        Assert.Same(response, this.operation.Responses["401"]);
    }
}
