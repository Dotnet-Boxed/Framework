namespace Boilerplate.AspNetCore.Swagger.SchemaFilters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Shows an example of a <see cref="ModelStateDictionary"/> containing errors.
    /// </summary>
    /// <seealso cref="ISchemaFilter" />
    public class ModelStateDictionarySchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// Applies the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="context">The context.</param>
        public void Apply(Schema model, SchemaFilterContext context)
        {
            if (context.SystemType == typeof(ModelStateDictionary))
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError("Property1", "Error message 1");
                modelState.AddModelError("Property1", "Error message 2");
                modelState.AddModelError("Property2", "Error message 3");
                var serializableError = new SerializableError(modelState);

                model.Default = serializableError;
                model.Example = serializableError;
            }
        }
    }
}