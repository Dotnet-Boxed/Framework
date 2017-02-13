namespace Boilerplate.AspNetCore.Swagger.SchemaFilters
{
    using Microsoft.AspNetCore.JsonPatch;
    using Swashbuckle.Swagger.Model;
    using Swashbuckle.SwaggerGen.Generator;
    using Operation = Microsoft.AspNetCore.JsonPatch.Operations.Operation;

    /// <summary>
    /// Shows an example of a <see cref="JsonPatchDocument"/> containing all the different patch operations you can do
    /// and a link to http://jsonpatch.com for convenience.
    /// </summary>
    /// <seealso cref="Swashbuckle.SwaggerGen.Generator.ISchemaFilter" />
    public class JsonPatchDocumentSchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// Applies the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="context">The context.</param>
        public void Apply(Schema model, SchemaFilterContext context)
        {
            if (context.SystemType.GenericTypeArguments.Length > 0 &&
                context.SystemType.GetGenericTypeDefinition() == typeof(JsonPatchDocument<>))
            {
                model.Default = GetExample();
                model.ExternalDocs = new ExternalDocs()
                {
                    Description = "JSON Patch Documentation",
                    Url = "http://jsonpatch.com/"
                };
            }
        }

        private static Operation[] GetExample()
        {
            return new Operation[]
            {
                new Operation()
                {
                    op = "replace",
                    path = "/property",
                    value = "New Value"
                },
                new Operation()
                {
                    op = "add",
                    path = "/property",
                    value = "New Value"
                },
                new Operation()
                {
                    op = "remove",
                    path = "/property"
                },
                new Operation()
                {
                    op = "copy",
                    from = "/fromProperty",
                    path = "/toProperty"
                },
                new Operation()
                {
                    op = "move",
                    from = "/fromProperty",
                    path = "/toProperty"
                },
                new Operation()
                {
                    op = "test",
                    path = "/property",
                    value = "Has Value"
                },
                new Operation()
                {
                    op = "replace",
                    path = "/arrayProperty/0",
                    value = "Replace First Array Item"
                },
                new Operation()
                {
                    op = "replace",
                    path = "/arrayProperty/-",
                    value = "Replace Last Array Item"
                }
            };
        }
    }
}