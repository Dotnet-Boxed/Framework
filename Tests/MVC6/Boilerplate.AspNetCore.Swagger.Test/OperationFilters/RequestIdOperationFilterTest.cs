namespace Framework.AspNetCore.Swagger.Test.OperationFilters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Framework.AspNetCore.Filters;
    using Framework.AspNetCore.Swagger.OperationFilters;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Swashbuckle.Swagger.Model;
    using Swashbuckle.SwaggerGen.Generator;
    using Xunit;

    public class RequestIdOperationFilterTest
    {
        private readonly Operation operation;
        private readonly RequestIdOperationFilter operationFilter;
        private readonly OperationFilterContext context;

        public RequestIdOperationFilterTest()
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
            this.operationFilter = new RequestIdOperationFilter();
        }

        [Fact]
        public void Apply_RequestIdFilterMissing_AddsRequestIdNonBodyParameter()
        {
            this.operationFilter.Apply(this.operation, this.context);

            Assert.Null(this.operation.Parameters);
        }

        [Fact]
        public void Apply_RequestIdNotRequired_AddsRequestIdNonBodyParameter()
        {
            this.context.ApiDescription.ActionDescriptor.FilterDescriptors.Add(
                new FilterDescriptor(new RequestIdHttpHeaderAttribute() { Required = false }, 0));

            this.operationFilter.Apply(this.operation, this.context);

            Assert.Equal(1, this.operation.Parameters.Count);
        }

        [Theory]
        [InlineData(false, "Used to uniquely identify the HTTP request. This ID is used to correlate the HTTP request between a client and server.")]
        [InlineData(true, "Used to uniquely identify the HTTP request. This ID is used to correlate the HTTP request between a client and server. The ID is also mirrored in the response HTTP header.")]
        public void Apply_RequestIdRequired_AddsRequestIdNonBodyParameter(bool forward, string expectedDescription)
        {
            var filter = new RequestIdHttpHeaderAttribute()
            {
                Forward = forward,
                Required = true
            };
            this.context.ApiDescription.ActionDescriptor.FilterDescriptors.Add(
                new FilterDescriptor(filter, 0));

            this.operationFilter.Apply(this.operation, this.context);

            Assert.Equal(1, this.operation.Parameters.Count);
            Assert.IsType<NonBodyParameter>(this.operation.Parameters.First());
            var parameter = (NonBodyParameter)this.operation.Parameters.First();
            Assert.IsType<Guid>(parameter.Default);
            Assert.Equal(expectedDescription, parameter.Description);
            Assert.Equal("header", parameter.In);
            Assert.Equal("X-Request-ID", parameter.Name);
            Assert.True(parameter.Required);
            Assert.Equal("string", parameter.Type);
        }
    }
}
