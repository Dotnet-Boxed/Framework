namespace Boxed.AspNetCore.Swagger.Test.SchemaFilters
{
    using Boxed.AspNetCore.Swagger.SchemaFilters;
    using Microsoft.AspNetCore.JsonPatch;
    using Newtonsoft.Json.Serialization;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Xunit;
    using Operation = Microsoft.AspNetCore.JsonPatch.Operations.Operation;

    public class JsonPatchDocumentSchemaFilterTest
    {
        private readonly Schema schema;
        private readonly JsonPatchDocumentSchemaFilter schemaFilter;

        public JsonPatchDocumentSchemaFilterTest()
        {
            this.schema = new Schema();
            this.schemaFilter = new JsonPatchDocumentSchemaFilter();
        }

        [Fact]
        public void Apply_TypeIsNotJsonPatchDocument_DoesNothing()
        {
            this.schemaFilter.Apply(
                this.schema,
                new SchemaFilterContext(typeof(string), new JsonStringContract(typeof(string)), null));
        }

        [Fact]
        public void Apply_TypeIsModelStateDictionary_DoesNothing()
        {
            this.schemaFilter.Apply(
                this.schema,
                new SchemaFilterContext(typeof(JsonPatchDocument<Model>), new JsonStringContract(typeof(JsonPatchDocument<Model>)), null));

            Assert.NotNull(this.schema.Default);
            var operations = Assert.IsType<Operation[]>(this.schema.Default);
            Assert.NotEmpty(operations);
            Assert.NotNull(this.schema.ExternalDocs);
            Assert.Equal("JSON Patch Documentation", this.schema.ExternalDocs.Description);
            Assert.Equal("http://jsonpatch.com/", this.schema.ExternalDocs.Url);
        }

        public class Model
        {
        }
    }
}
