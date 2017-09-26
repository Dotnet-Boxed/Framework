namespace Boilerplate.AspNetCore.Swagger.Test.SchemaFilters
{
    using Boilerplate.AspNetCore.Swagger.SchemaFilters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Newtonsoft.Json.Serialization;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Xunit;

    public class ModelStateDictionarySchemaFilterTest
    {
        private readonly Schema schema;
        private readonly ModelStateDictionarySchemaFilter schemaFilter;

        public ModelStateDictionarySchemaFilterTest()
        {
            this.schema = new Schema();
            this.schemaFilter = new ModelStateDictionarySchemaFilter();
        }

        [Fact]
        public void Apply_TypeIsNotModelStateDictionary_DoesNothing()
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
                new SchemaFilterContext(typeof(ModelStateDictionary), new JsonStringContract(typeof(ModelStateDictionary)), null));

            Assert.NotNull(this.schema.Default);
            var serializableError = Assert.IsType<SerializableError>(this.schema.Default);
            Assert.NotEmpty(serializableError);
        }
    }
}
