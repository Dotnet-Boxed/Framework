namespace Boxed.AspNetCore.Swagger.Test.OperationFilters
{
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

    public class ForbiddenResponseOperationFilterTest
    {
        private readonly ApiDescription apiDescription;
        private readonly OpenApiOperation operation;
        private readonly OperationFilterContext context;
        private readonly ForbiddenResponseOperationFilter operationFilter;

        public ForbiddenResponseOperationFilterTest()
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
            this.operationFilter = new ForbiddenResponseOperationFilter();
        }

        [Fact]
        public void Apply_HasClaimsAuthorizationRequirement_Adds403Response() =>
            this.Apply_HasRequirement_Adds403Response(new ClaimsAuthorizationRequirement("Type", Array.Empty<string>()));

        [Fact]
        public void Apply_HasNameAuthorizationRequirement_Adds403Response() =>
            this.Apply_HasRequirement_Adds403Response(new NameAuthorizationRequirement("Name"));

        [Fact]
        public void Apply_HasRolesAuthorizationRequirement_Adds403Response() =>
            this.Apply_HasRequirement_Adds403Response(new RolesAuthorizationRequirement(new string[] { "Role" }));

        [Fact]
        public void Apply_HasAssertionRequirement_Adds403Response() =>
            this.Apply_HasRequirement_Adds403Response(new AssertionRequirement(x => true));

        [Fact]
        public void Apply_HasDenyAnonymousAuthorizationRequirement_DoesNothing()
        {
            var requirement = new DenyAnonymousAuthorizationRequirement();
            var requirements = new List<IAuthorizationRequirement>() { requirement };
            var policy = new AuthorizationPolicy(requirements, new List<string>());
            var filterDescriptor = new FilterDescriptor(new AuthorizeFilter(policy), 30);
            this.context.ApiDescription.ActionDescriptor.FilterDescriptors.Add(filterDescriptor);

            this.operationFilter.Apply(this.operation, this.context);

            Assert.Empty(this.operation.Responses);
        }

        [Fact]
        public void Apply_DoesNotHaveRequirement_DoesNothing()
        {
            this.operationFilter.Apply(this.operation, this.context);

            Assert.Empty(this.operation.Responses);
        }

        [Fact]
        public void Apply_AlreadyHasForbiddenResponse_DoesNothing()
        {
            var response = new OpenApiResponse();
            this.operation.Responses.Add("403", response);
            this.operationFilter.Apply(this.operation, this.context);

            Assert.Single(this.operation.Responses);
            Assert.Same(response, this.operation.Responses["403"]);
        }

        private void Apply_HasRequirement_Adds403Response(IAuthorizationRequirement authorizationRequirement)
        {
            var requirements = new List<IAuthorizationRequirement>() { authorizationRequirement };
            var policy = new AuthorizationPolicy(requirements, new List<string>());
            var filterDescriptor = new FilterDescriptor(new AuthorizeFilter(policy), 30);
            this.context.ApiDescription.ActionDescriptor.FilterDescriptors.Add(filterDescriptor);

            this.operationFilter.Apply(this.operation, this.context);

            Assert.True(this.operation.Responses.ContainsKey("403"));
        }
    }
}
