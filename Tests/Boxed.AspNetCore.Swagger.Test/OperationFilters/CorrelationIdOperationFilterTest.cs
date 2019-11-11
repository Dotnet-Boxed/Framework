namespace Boxed.AspNetCore.Swagger.Test.OperationFilters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Boxed.AspNetCore.Swagger.OperationFilters;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Moq;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Xunit;
    using FilterDescriptor = Microsoft.AspNetCore.Mvc.Filters.FilterDescriptor;

    public class CorrelationIdOperationFilterTest
    {
        private readonly ApiDescription apiDescription;
        private readonly OpenApiOperation operation;
        private readonly OperationFilterContext context;
        private readonly CorrelationIdOperationFilter operationFilter;

        public CorrelationIdOperationFilterTest()
        {
            this.operation = new OpenApiOperation()
            {
                Responses = new OpenApiResponses(),
            };
            this.apiDescription = new ApiDescription()
            {
                ActionDescriptor = new ActionDescriptor()
                {
                    FilterDescriptors = new List<FilterDescriptor>()
                }
            };
            this.context = new OperationFilterContext(
                this.apiDescription,
                new Mock<ISchemaGenerator>().Object,
                new SchemaRepository(),
                this.GetType().GetMethods().First());
            this.operationFilter = new CorrelationIdOperationFilter();
        }

        [Fact]
        public void Apply_Default_AddsParameter()
        {
            this.operationFilter.Apply(this.operation, this.context);

            var parameter = Assert.Single(this.operation.Parameters);
            Assert.Equal(ParameterLocation.Header, parameter.In);
            Assert.Equal("X-Correlation-ID", parameter.Name);
            Assert.False(parameter.Required);
            var defaultValue = Assert.IsType<OpenApiString>(parameter.Schema.Default);
            Assert.True(Guid.TryParse(defaultValue.Value, out _));
            Assert.Equal("string", parameter.Schema.Type);
        }
    }
}
