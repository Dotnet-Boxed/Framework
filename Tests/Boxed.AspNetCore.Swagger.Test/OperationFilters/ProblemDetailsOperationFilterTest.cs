namespace Boxed.AspNetCore.Swagger.Test.OperationFilters
{
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

    public class ProblemDetailsOperationFilterTest
    {
        private readonly ApiDescription apiDescription;
        private readonly OpenApiOperation operation;
        private readonly OperationFilterContext context;
        private readonly ProblemDetailsOperationFilter operationFilter;

        public ProblemDetailsOperationFilterTest()
        {
            this.operation = new OpenApiOperation();
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
            this.operationFilter = new ProblemDetailsOperationFilter();
        }

        [Fact]
        public void Apply_200NoProblemDetails_DoesNothing()
        {
            this.operation.Responses.Add("200", new OpenApiResponse());

            this.operationFilter.Apply(this.operation, this.context);

            Assert.Empty(this.operation.Responses["200"].Content);
        }

        [Fact]
        public void Apply_200WithProblemDetails_DoesNothing()
        {
            this.operation.Responses.Add("200", GetResponse());

            this.operationFilter.Apply(this.operation, this.context);

            Assert.Null(this.operation.Responses["200"].Content["application/problem+json"].Example);
            Assert.Null(this.operation.Responses["200"].Content["application/problem+xml"].Example);
        }

        [Theory]
        [InlineData("400")]
        [InlineData("401")]
        [InlineData("403")]
        [InlineData("404")]
        [InlineData("406")]
        [InlineData("409")]
        [InlineData("415")]
        [InlineData("422")]
        [InlineData("500")]
        public void Apply_WithProblemDetails_AddsExample(string statusCode)
        {
            this.operation.Responses.Add(statusCode, GetResponse());

            this.operationFilter.Apply(this.operation, this.context);

            var jsonMediaType = this.operation.Responses[statusCode].Content["application/problem+json"];
            var xmlMediaType = this.operation.Responses[statusCode].Content["application/problem+xml"];

            var jsonActualExample = Assert.IsType<OpenApiObject>(jsonMediaType.Example);
            var xmlActualExample = Assert.IsType<OpenApiObject>(xmlMediaType.Example);

            var expectedExample = (OpenApiObject?)typeof(ProblemDetailsOperationFilter)
                .GetProperty($"Status{statusCode}ProblemDetails")
                ?.GetValue(null);
            Assert.Same(expectedExample, jsonActualExample);
            Assert.Same(expectedExample, xmlActualExample);
        }

        private static OpenApiResponse GetResponse() =>
            new()
            {
                Content = new Dictionary<string, OpenApiMediaType>()
                {
                    { "application/problem+json", new OpenApiMediaType() },
                    { "application/problem+xml", new OpenApiMediaType() },
                },
            };
    }
}
