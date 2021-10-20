namespace Boxed.AspNetCore.Swagger
{
    using System;
    using System.IO;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// <see cref="SwaggerGenOptions"/>  extension methods.
    /// </summary>
    public static class SwaggerGenOptionsExtensions
    {
        /// <summary>
        /// Includes the XML comment file if it has the same name as the assembly, a .xml file extension and exists in
        /// the same directory as the assembly.
        /// </summary>
        /// <param name="options">The Swagger options.</param>
        /// <param name="assembly">The assembly.</param>
        /// <returns><c>true</c> if the comment file exists and was added, otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">options or assembly.</exception>
        public static SwaggerGenOptions IncludeXmlCommentsIfExists(this SwaggerGenOptions options, Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(assembly);

            if (!string.IsNullOrEmpty(assembly.Location))
            {
                var filePath = Path.ChangeExtension(assembly.Location, ".xml");
                IncludeXmlCommentsIfExists(options, filePath);
            }

            return options;
        }

        /// <summary>
        /// Includes the XML comment file if it exists at the specified file path.
        /// </summary>
        /// <param name="options">The Swagger options.</param>
        /// <param name="filePath">The XML comment file path.</param>
        /// <returns><c>true</c> if the comment file exists and was added, otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">options or filePath.</exception>
        public static bool IncludeXmlCommentsIfExists(this SwaggerGenOptions options, string filePath)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(filePath);

            if (File.Exists(filePath))
            {
                options.IncludeXmlComments(filePath);
                return true;
            }

            return false;
        }
    }
}
