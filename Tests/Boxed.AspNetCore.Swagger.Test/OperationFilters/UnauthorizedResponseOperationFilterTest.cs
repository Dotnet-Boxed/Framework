namespace Boxed.AspNetCore.Swagger.Test.OperationFilters
{
    using System.Collections.Generic;
    using System.Linq;
    using Boxed.AspNetCore.Swagger.OperationFilters;
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

    public class UnauthorizedResponseOperationFilterTest
    {
        private readonly ApiDescription apiDescription;
        private readonly Operation operation;
        private readonly UnauthorizedResponseOperationFilter operationFilter;
        private readonly OperationFilterContext operationFilterContext;

        public UnauthorizedResponseOperationFilterTest()
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
            this.operationFilter = new UnauthorizedResponseOperationFilter();
            this.operationFilterContext = new OperationFilterContext(
                this.apiDescription,
                new Mock<ISchemaRegistry>().Object,
                this.GetType().GetMethods().First());
        }

        [Fact]
        public void Apply_HasDenyAnonymousAuthorizationRequirement_Adds401Response()
        {
            var requirement = new DenyAnonymousAuthorizationRequirement();
            var requirements = new List<IAuthorizationRequirement>() { requirement };
            var policy = new AuthorizationPolicy(requirements, new List<string>());
            var filterDescriptor = new FilterDescriptor(new AuthorizeFilter(policy), 30);
            this.operationFilterContext.ApiDescription.ActionDescriptor.FilterDescriptors.Add(filterDescriptor);

            this.operationFilter.Apply(this.operation, this.operationFilterContext);

            Assert.True(this.operation.Responses.ContainsKey("401"));
        }

        [Fact]
        public void Apply_DoesNotHaveDenyAnonymousAuthorizationRequirement_DoesNothing()
        {
            this.operationFilter.Apply(this.operation, this.operationFilterContext);

            Assert.Empty(this.operation.Responses);
        }

        [Fact]
        public void Apply_AlreadyHasForbiddenResponse_DoesNothing()
        {
            var response = new Response();
            this.operation.Responses.Add("401", response);
            this.operationFilter.Apply(this.operation, this.operationFilterContext);

            Assert.Equal(1, this.operation.Responses.Count);
            Assert.Same(response, this.operation.Responses["401"]);
        }
    }
}
