namespace Boxed.AspNetCore.Swagger.SchemaFilters
{
    using System;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Operation = Microsoft.AspNetCore.JsonPatch.Operations.Operation;

    /// <summary>
    /// Shows an example of a <see cref="JsonPatchDocument"/> containing all the different patch operations you can do
    /// and a link to http://jsonpatch.com for convenience.
    /// </summary>
    /// <seealso cref="ISchemaFilter" />
    public class JsonPatchDocumentSchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// Applies the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="context">The context.</param>
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.ApiModel.Type.GenericTypeArguments.Length > 0 &&
                context.ApiModel.Type.GetGenericTypeDefinition() == typeof(JsonPatchDocument<>))
            {
                var example = GetExample();

                // model.Default = example;
                // model.Example = example;
                model.ExternalDocs = new OpenApiExternalDocs()
                {
                    Description = "JSON Patch Documentation",
                    Url = new Uri("http://jsonpatch.com/", UriKind.Absolute),
                };
            }
        }

        private static Operation[] GetExample() =>
            new Operation[]
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
