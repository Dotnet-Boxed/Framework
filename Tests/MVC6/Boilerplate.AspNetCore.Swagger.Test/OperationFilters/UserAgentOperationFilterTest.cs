namespace Boilerplate.AspNetCore.Swagger.Test.OperationFilters
{
    using System.Collections.Generic;
    using System.Linq;
    using Boilerplate.AspNetCore.Filters;
    using Boilerplate.AspNetCore.Swagger.OperationFilters;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Swashbuckle.Swagger.Model;
    using Swashbuckle.SwaggerGen.Generator;
    using Xunit;

    public class UserAgentOperationFilterTest
    {
        private readonly Operation operation;
        private readonly UserAgentOperationFilter operationFilter;
        private readonly OperationFilterContext context;

        public UserAgentOperationFilterTest()
        {
            this.operation = new Operation();
            this.context = new OperationFilterContext(
                new ApiDescription()
                {
                    ActionDescriptor = new ActionDescriptor()
                    {
                        FilterDescriptors = new List<FilterDescriptor>()
                    }
                },
                null);
            this.operationFilter = new UserAgentOperationFilter();
        }

        [Fact]
        public void Apply_UserAgentFilterMissing_AddsRequestIdNonBodyParameter()
        {
            this.operationFilter.Apply(this.operation, this.context);

            Assert.Null(this.operation.Parameters);
        }

        [Fact]
        public void Apply_UserAgentNotRequired_AddsRequestIdNonBodyParameter()
        {
            this.context.ApiDescription.ActionDescriptor.FilterDescriptors.Add(
                new FilterDescriptor(new UserAgentHttpHeaderAttribute() { Required = false }, 0));

            this.operationFilter.Apply(this.operation, this.context);

            Assert.Equal(1, this.operation.Parameters.Count);
        }

        [Fact]
        public void Apply_NullParameters_AddsRequestIdNonBodyParameter()
        {
            this.context.ApiDescription.ActionDescriptor.FilterDescriptors.Add(
                new FilterDescriptor(new UserAgentHttpHeaderAttribute() { Required = true }, 0));

            this.operationFilter.Apply(this.operation, this.context);

            Assert.Equal(1, this.operation.Parameters.Count);
            Assert.IsType<NonBodyParameter>(this.operation.Parameters.First());
            var parameter = (NonBodyParameter)this.operation.Parameters.First();
            Assert.Equal("Application-Name/1.0.0 (Operating System Name 1.0.0)", parameter.Default);
            Assert.Equal("Used to identify the application making the HTTP request.", parameter.Description);
            Assert.Equal("header", parameter.In);
            Assert.Equal("User-Agent", parameter.Name);
            Assert.True(parameter.Required);
            Assert.Equal("string", parameter.Type);
        }
    }
}
