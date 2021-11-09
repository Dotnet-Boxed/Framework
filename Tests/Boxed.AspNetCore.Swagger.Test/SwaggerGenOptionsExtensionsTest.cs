namespace Boxed.AspNetCore.Swagger.Test;

using System;
using System.IO;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

public class SwaggerGenOptionsExtensionsTest
{
    [Fact]
    public void IncludeXmlCommentsIfExists_NullOptions_ThrowsArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() =>
            SwaggerGenOptionsExtensions.IncludeXmlCommentsIfExists(
                null!,
                typeof(SwaggerGenOptionsExtensionsTest).GetTypeInfo().Assembly));

    [Fact]
    public void IncludeXmlCommentsIfExists_NullAssembly_ThrowsArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(() =>
            SwaggerGenOptionsExtensions.IncludeXmlCommentsIfExists(
                new SwaggerGenOptions(),
                (Assembly)null!));

    [Fact]
    public void IncludeXmlCommentsIfExists_XmlFileExistsWithAssemblyLocation_XmlCommentsFileAdded()
    {
        var assembly = typeof(SwaggerGenOptionsExtensionsTest).GetTypeInfo().Assembly;
        var xmlFilePath = Path.ChangeExtension(assembly.Location, ".xml");
        File.WriteAllText(xmlFilePath, "<?xml version=\"1.0\"?><doc></doc>");
        var options = new SwaggerGenOptions();

        try
        {
            var actualOptions = SwaggerGenOptionsExtensions.IncludeXmlCommentsIfExists(
                options,
                assembly);

            Assert.Same(options, actualOptions);
        }
        finally
        {
            EnsureFileDeleted(xmlFilePath);
        }
    }

    private static void EnsureFileDeleted(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
