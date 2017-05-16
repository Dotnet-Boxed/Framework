namespace Boilerplate.AspNetCore.Swagger.Test.OperationFilters
{
    using System.Collections.Generic;
    using Boilerplate.AspNetCore.Swagger.OperationFilters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authorization.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Moq;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Xunit;

    public class ForbiddenResponseOperationFilterTest
    {
        private readonly ApiDescription apiDescription;
        private readonly Operation operation;
        private readonly ForbiddenResponseOperationFilter operationFilter;
        private readonly OperationFilterContext operationFilterContext;

        public ForbiddenResponseOperationFilterTest()
        {
            this.apiDescription = new ApiDescription()
            {
                ActionDescriptor = new ActionDescriptor()
                {
                    FilterDescriptors = new List<FilterDescriptor>()
                }
            };
            this.operation = new Operation()
            {
                Responses = new Dictionary<string, Response>()
            };
            this.operationFilter = new ForbiddenResponseOperationFilter();
            this.operationFilterContext = new OperationFilterContext(
                this.apiDescription,
                new Mock<ISchemaRegistry>().Object);
        }

        [Fact]
        public void Apply_HasClaimsAuthorizationRequirement_Adds403Response() =>
            this.Apply_HasRequirement_Adds403Response(new ClaimsAuthorizationRequirement("Type", new string[0]));

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
            this.operationFilterContext.ApiDescription.ActionDescriptor.FilterDescriptors.Add(filterDescriptor);

            this.operationFilter.Apply(this.operation, this.operationFilterContext);

            Assert.Empty(this.operation.Responses);
        }

        [Fact]
        public void Apply_DoesNotHaveRequirement_DoesNothing()
        {
            this.operationFilter.Apply(this.operation, this.operationFilterContext);

            Assert.Empty(this.operation.Responses);
        }

        [Fact]
        public void Apply_AlreadyHasForbiddenResponse_DoesNothing()
        {
            var response = new Response();
            this.operation.Responses.Add("403", response);
            this.operationFilter.Apply(this.operation, this.operationFilterContext);

            Assert.Equal(1, this.operation.Responses.Count);
            Assert.Same(response, this.operation.Responses["403"]);
        }

        private void Apply_HasRequirement_Adds403Response(IAuthorizationRequirement authorizationRequirement)
        {
            var requirements = new List<IAuthorizationRequirement>() { authorizationRequirement };
            var policy = new AuthorizationPolicy(requirements, new List<string>());
            var filterDescriptor = new FilterDescriptor(new AuthorizeFilter(policy), 30);
            this.operationFilterContext.ApiDescription.ActionDescriptor.FilterDescriptors.Add(filterDescriptor);

            this.operationFilter.Apply(this.operation, this.operationFilterContext);

            Assert.True(this.operation.Responses.ContainsKey("403"));
        }
    }
}
