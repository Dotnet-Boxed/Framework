![.NET Boxed Banner](https://github.com/Dotnet-Boxed/Templates/blob/main/Images/Banner.png)

## Boxed.AspNetCore

[![Boxed.AspNetCore](https://img.shields.io/nuget/v/Boxed.AspNetCore.svg)](https://www.nuget.org/packages/Boxed.AspNetCore/) [![Boxed.AspNetCore package in dotnet-boxed feed in Azure Artifacts](https://feeds.dev.azure.com/dotnet-boxed/_apis/public/Packaging/Feeds/03bd56a4-9269-43f7-9f75-d82037c56a46/Packages/47bb453b-3fe4-44c1-8b82-3c64a2a9009a/Badge)](https://dev.azure.com/dotnet-boxed/Framework/_packaging?_a=package&feed=03bd56a4-9269-43f7-9f75-d82037c56a46&package=47bb453b-3fe4-44c1-8b82-3c64a2a9009a&preferRelease=true) [![Boxed.AspNetCore NuGet Package Downloads](https://img.shields.io/nuget/dt/Boxed.AspNetCore)](https://www.nuget.org/packages/Boxed.AspNetCore)

Provides ASP.NET Core middleware, MVC filters, extension methods and helper code for an ASP.NET Core project.

### Fluent Interface Extensions

- [ASP.NET Core Fluent Interface Extensions](https://rehansaeed.com/asp-net-core-fluent-interface-extensions/)

**ILoggingBuilder Extensions**

```c#
loggingBuilder
    .AddIfElse(
        hostingEnvironment.IsDevelopment(),
        x => x.AddConsole(...).AddDebug(),
        x => x.AddSerilog(...));
```

**IConfiguration Extensions**

```c#
this.configuration = new ConfigurationBuilder()
    .SetBasePath(hostingEnvironment.ContentRootPath)
    .AddJsonFile("config.json")
    .AddJsonFile($"config.{hostingEnvironment.EnvironmentName}.json", optional: true)
    .AddIf(
        hostingEnvironment.IsDevelopment(),
        x => x.AddUserSecrets())
    .AddEnvironmentVariables()
    .AddApplicationInsightsSettings(developerMode: !hostingEnvironment.IsProduction())
    .Build();
```

**IApplicationBuilder Extensions**

```c#
application
    .UseIfElse(
        environment.IsDevelopment(),
        x => x.UseDeveloperExceptionPage(),
        x => x.UseStatusCodePagesWithReExecute("/error/{0}/"))
    .UseIf(
        environment.IsStaging(),
        x => x.UseStagingSpecificMiddleware())
    .UseStaticFiles()
    .UseMvc();
```

### SEO Friendly URL's

- [SEO Friendly URL's for ASP.NET Core](https://rehansaeed.com/seo-friendly-urls-asp-net-core/)

```c#
[HttpGet("product/{id}/{title}", Name = "GetProduct")]
public IActionResult GetProduct(int id, string title)
{
    var product = this.productRepository.Find(id);
    if (product == null)
    {
        return this.NotFound();
    }

    // Get the actual friendly version of the title.
    string friendlyTitle = FriendlyUrlHelper.GetFriendlyTitle(product.Title);

    // Compare the title with the friendly title.
    if (!string.Equals(friendlyTitle, title, StringComparison.Ordinal))
    {
        // If the title is null, empty or does not match the friendly title, return a 301 Permanent
        // Redirect to the correct friendly URL.
        return this.RedirectToRoutePermanent("GetProduct", new { id = id, title = friendlyTitle });
    }

    // The URL the client has browsed to is correct, show them the view containing the product.
    return this.View(product);
}
```

### Canonical URL's

- [Canonical URL's for ASP.NET Core](https://rehansaeed.com/canonical-urls-for-asp-net-mvc/)
