namespace Boxed.AspNetCore.Swagger.Test.SchemaFilters
{
    using System;
    using Boxed.AspNetCore.Swagger.SchemaFilters;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Xunit;

    public class JsonPatchDocumentSchemaFilterTest
    {
        private readonly OpenApiSchema schema;
        private readonly JsonPatchDocumentSchemaFilter schemaFilter;

        public JsonPatchDocumentSchemaFilterTest()
        {
            this.schema = new OpenApiSchema();
            this.schemaFilter = new JsonPatchDocumentSchemaFilter();
        }

        [Fact]
        public void Apply_TypeIsNotJsonPatchDocument_DoesNothing() =>
            this.schemaFilter.Apply(
                this.schema,
                new SchemaFilterContext(
                    new ApiModel(typeof(JsonPatchDocument<Model>)),
                    new SchemaRepository(),
                    null));

        [Fact]
        public void Apply_TypeIsModelStateDictionary_DoesNothing()
        {
            this.schemaFilter.Apply(
                this.schema,
                new SchemaFilterContext(
                    new ApiModel(typeof(JsonPatchDocument<Model>)),
                    new SchemaRepository(),
                    null));

            Assert.NotNull(this.schema.Default);
            var defaultOpenApiArray = Assert.IsType<OpenApiArray>(this.schema.Default);
            Assert.NotEmpty(defaultOpenApiArray);
            Assert.NotNull(this.schema.Example);
            var exampleOpenApiArray = Assert.IsType<OpenApiArray>(this.schema.Example);
            Assert.NotEmpty(exampleOpenApiArray);
            Assert.NotNull(this.schema.ExternalDocs);
            Assert.Equal("JSON Patch Documentation", this.schema.ExternalDocs.Description);
            Assert.Equal(new Uri("http://jsonpatch.com/"), this.schema.ExternalDocs.Url);
        }

        public class Model
        {
        }
    }
}
