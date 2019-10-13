![.NET Boxed Banner](https://raw.githubusercontent.com/Dotnet-Boxed/Templates/master/Images/Banner.png)

[![Twitter URL](https://img.shields.io/twitter/url/http/shields.io.svg?style=social)](https://twitter.com/RehanSaeedUK) [![Twitter Follow](https://img.shields.io/twitter/follow/rehansaeeduk.svg?style=social&label=Follow)](https://twitter.com/RehanSaeedUK)
 
.NET Core Extensions and Helper NuGet packages. If you are looking for the .NET Boxed project templates, you can find them [here](https://github.com/Dotnet-Boxed/Templates).

## Boxed.Mapping

[![Boxed.Mapping](https://img.shields.io/nuget/v/Boxed.Mapping.svg)](https://www.nuget.org/packages/Boxed.Mapping/) [![Boxed.Mapping package in dotnet-boxed feed in Azure Artifacts](https://feeds.dev.azure.com/dotnet-boxed/_apis/public/Packaging/Feeds/03bd56a4-9269-43f7-9f75-d82037c56a46/Packages/5ed2eb60-9538-4890-b90e-5d4d4cbb2a7a/Badge)](https://dev.azure.com/dotnet-boxed/Framework/_packaging?_a=package&feed=03bd56a4-9269-43f7-9f75-d82037c56a46&package=5ed2eb60-9538-4890-b90e-5d4d4cbb2a7a&preferRelease=true)

A simple and fast (fastest?) object to object mapper that does not use reflection. Read [A Simple and Fast Object Mapper](https://rehansaeed.com/a-simple-and-fast-object-mapper/) for more information.

```c#
public class MapFrom
{
    public bool BooleanFrom { get; set; }
    public int IntegerFrom { get; set; }
    public List<MapFromChild> ChildrenFrom { get; set; }
}
public class MapFromChild
{
    public DateTimeOffset DateTimeOffsetFrom { get; set; }
    public string StringFrom { get; set; }
}
 
public class MapTo
{
    public bool BooleanTo { get; set; }
    public int IntegerTo { get; set; }
    public List<MapToChild> ChildrenTo { get; set; }
}
public class MapToChild
{
    public DateTimeOffset DateTimeOffsetTo { get; set; }
    public string StringTo { get; set; }
}

public class DemoMapper : IMapper<MapFrom, MapTo>
{
    private readonly IMapper<MapFromChild, MapToChild> childMapper;
    
    public DemoMapper(IMapper<MapFromChild, MapToChild> childMapper) => this.childMapper = childMapper;
    
    public void Map(MapFrom source, MapTo destination)
    {
        destination.BooleanTo = source.BooleanFrom;
        destination.IntegerTo = source.IntegerFrom;
        destination.ChildrenTo = childMapper.MapList(source.ChildrenFrom);
    }
}

public class DemoChildMapper : IMapper<MapFromChild, MapToChild>
{
    public void Map(MapFromChild source, MapToChild destination)
    {
        destination.DateTimeOffsetTo = source.DateTimeOffsetFrom;
        destination.StringTo = source.StringFrom;
    }
}

public class UsageExample
{
    private readonly IMapper<MapFrom, MapTo> mapper = new DemoMapper();
    
    public MapTo MapOneObject(MapFrom source) => this.mapper.Map(source);
    
    public MapTo[] MapArray(List<MapFrom> source) => this.mapper.MapArray(source);
    
    public List<MapTo> MapList(List<MapFrom> source) => this.mapper.MapList(source);
}
```

## Boxed.AspNetCore

[![Boxed.AspNetCore](https://img.shields.io/nuget/v/Boxed.AspNetCore.svg)](https://www.nuget.org/packages/Boxed.AspNetCore/) [![Boxed.AspNetCore package in dotnet-boxed feed in Azure Artifacts](https://feeds.dev.azure.com/dotnet-boxed/_apis/public/Packaging/Feeds/03bd56a4-9269-43f7-9f75-d82037c56a46/Packages/47bb453b-3fe4-44c1-8b82-3c64a2a9009a/Badge)](https://dev.azure.com/dotnet-boxed/Framework/_packaging?_a=package&feed=03bd56a4-9269-43f7-9f75-d82037c56a46&package=47bb453b-3fe4-44c1-8b82-3c64a2a9009a&preferRelease=true)

Provides ASP.NET Core middleware, MVC filters, extension methods and helper code for an ASP.NET Core project.

### Fluent Interface Extensions

- [ASP.NET Core Fluent Interface Extensions](https://rehansaeed.com/asp-net-core-fluent-interface-extensions/)

**ILoggerFactory Extensions**

```c#
loggerfactory
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

## Boxed.AspNetCore.Swagger

[![Boxed.AspNetCore.Swagger](https://img.shields.io/nuget/v/Boxed.AspNetCore.Swagger.svg)](https://www.nuget.org/packages/Boxed.AspNetCore.Swagger/) [![Boxed.AspNetCore.Swagger package in dotnet-boxed feed in Azure Artifacts](https://feeds.dev.azure.com/dotnet-boxed/_apis/public/Packaging/Feeds/03bd56a4-9269-43f7-9f75-d82037c56a46/Packages/be991e58-86c8-4de4-8117-4a301e530669/Badge)](https://dev.azure.com/dotnet-boxed/Framework/_packaging?_a=package&feed=03bd56a4-9269-43f7-9f75-d82037c56a46&package=be991e58-86c8-4de4-8117-4a301e530669&preferRelease=true)

Provides ASP.NET Core middleware, MVC filters, extension methods and helper code for an ASP.NET Core project implementing Swagger (OpenAPI).

## Boxed.AspNetCore.TagHelpers

[![Boxed.AspNetCore.TagHelpers](https://img.shields.io/nuget/v/Boxed.AspNetCore.TagHelpers.svg)](https://www.nuget.org/packages/Boxed.AspNetCore.TagHelpers/) [![Boxed.AspNetCore.TagHelpers package in dotnet-boxed feed in Azure Artifacts](https://feeds.dev.azure.com/dotnet-boxed/_apis/public/Packaging/Feeds/03bd56a4-9269-43f7-9f75-d82037c56a46/Packages/0b0ed292-8769-4d42-9d89-b037e936633f/Badge)](https://dev.azure.com/dotnet-boxed/Framework/_packaging?_a=package&feed=03bd56a4-9269-43f7-9f75-d82037c56a46&package=0b0ed292-8769-4d42-9d89-b037e936633f&preferRelease=true)

ASP.NET Core tag helpers for Subresource Integrity (SRI), Referrer meta tags, OpenGraph (Facebook) and Twitter social network meta tags. Read more at:

### Subresource Integrity (SRI)

- [Subresource Integrity (SRI) TagHelper using ASP.NET Core - Part 1](https://rehansaeed.com/subresource-integrity-taghelper-using-asp-net-core/)
- [Subresource Integrity (SRI) TagHelper using ASP.NET Core - Part 2](https://rehansaeed.com/subresource-integrity-taghelper-using-asp-net-core-part-2/)

```html
<script src="https://ajax.googleapis.com/ajax/libs/jquery/2.2.0/jquery.min.js" 
        asp-subresource-integrity-src="~/js/jquery.min.js"></script>
```

### Social Network Meta Tags

- [Social TagHelper's using ASP.NET Core](https://rehansaeed.com/social-taghelpers-for-asp-net-core/)

**Twitter Cards**

```html
<twitter-card-summary-large-image username="@@RehanSaeedUK">
```

**Open Graph (Facebook)**

```html
<open-graph-website site-name="My Website"
                    title="Page Title"
                    main-image="@(new OpenGraphImage(
                        Url.AbsoluteContent("~/img/1200x630.png"),
                        ContentType.Png,
                        1200,
                        630))"
                    determiner="OpenGraphDeterminer.Blank">
```

## Boxed.DotnetNewTest

[![Boxed.DotnetNewTest](https://img.shields.io/nuget/v/Boxed.DotnetNewTest.svg)](https://www.nuget.org/packages/Boxed.DotnetNewTest/) [![Boxed.DotnetNewTest package in dotnet-boxed feed in Azure Artifacts](https://feeds.dev.azure.com/dotnet-boxed/_apis/public/Packaging/Feeds/03bd56a4-9269-43f7-9f75-d82037c56a46/Packages/537173a7-9aba-493c-abd2-935fa9c14e27/Badge)](https://dev.azure.com/dotnet-boxed/Framework/_packaging?_a=package&feed=03bd56a4-9269-43f7-9f75-d82037c56a46&package=537173a7-9aba-493c-abd2-935fa9c14e27&preferRelease=true)

A unit test framework for project templates built using [dotnet new](https://docs.microsoft.com/en-us/dotnet/core/tools/custom-templates).

1. Install dotnet new based project templates from a directory.
2. Run `dotnet restore`, `dotnet build` and `dotnet publish` commands.
3. For ASP.NET Core project templates you can run `dotnet run` which gives you a `HttpClient` that you can use to call the app and run further tests.

```c#
public class ApiTemplateTest
{
    public ApiTemplateTest() => DotnetNew.Install<ApiTemplateTest>("ApiTemplate.sln").Wait();

    [Theory]
    [InlineData("StatusEndpointOn", "status-endpoint=true")]
    [InlineData("StatusEndpointOff", "status-endpoint=false")]
    public async Task RestoreAndBuild_CustomArguments_IsSuccessful(string name, params string[] arguments)
    {
        using (var tempDirectory = TempDirectory.NewTempDirectory())
        {
            var dictionary = arguments
                .Select(x => x.Split('=', StringSplitOptions.RemoveEmptyEntries))
                .ToDictionary(x => x.First(), x => x.Last());
            var project = await tempDirectory.DotnetNew("api", name, dictionary);
            await project.DotnetRestore();
            await project.DotnetBuild();
        }
    }

    [Fact]
    public async Task Run_DefaultArguments_IsSuccessful()
    {
        using (var tempDirectory = TempDirectory.NewTempDirectory())
        {
            var project = await tempDirectory.DotnetNew("api", "DefaultArguments");
            await project.DotnetRestore();
            await project.DotnetBuild();
            await project.DotnetRun(
                @"Source\DefaultArguments",
                async (httpClient, httpsClient) =>
                {
                    var httpResponse = await httpsClient.GetAsync("status");
                    Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
                });
        }
    }
}
```


## Continuous Integration

| Name         | Operating System | Status | History |
| :---         | :---             | :---   | :---    |
| Azure Pipelines | Ubuntu           | [![Azure Pipelines Ubuntu Build Status](https://dev.azure.com/dotnet-boxed/Framework/_apis/build/status/Dotnet-Boxed.Framework?branchName=master&stageName=Build&jobName=Build&configuration=Build%20Linux)](https://dev.azure.com/dotnet-boxed/Framework/_build/latest?definitionId=1&branchName=master) | |
| Azure Pipelines | Mac              | [![Azure Pipelines Mac Build Status](https://dev.azure.com/dotnet-boxed/Framework/_apis/build/status/Dotnet-Boxed.Framework?branchName=master&stageName=Build&jobName=Build&configuration=Build%20Mac)](https://dev.azure.com/dotnet-boxed/Framework/_build/latest?definitionId=1&branchName=master) | |
| Azure Pipelines | Windows          | [![Azure Pipelines Windows Build Status](https://dev.azure.com/dotnet-boxed/Framework/_apis/build/status/Dotnet-Boxed.Framework?branchName=master&stageName=Build&jobName=Build&configuration=Build%20Windows)](https://dev.azure.com/dotnet-boxed/Framework/_build/latest?definitionId=1&branchName=master) | |
| Azure Pipelines | Overall          | [![Azure Pipelines Overall Build Status](https://dev.azure.com/dotnet-boxed/Framework/_apis/build/status/Dotnet-Boxed.Framework?branchName=master)](https://dev.azure.com/dotnet-boxed/Framework/_build/latest?definitionId=1&branchName=master) | |
| AppVeyor     | Ubuntu & Windows | [![AppVeyor Build status](https://ci.appveyor.com/api/projects/status/aknwu9sil3dv3im0?svg=true)](https://ci.appveyor.com/project/RehanSaeed/framework) | [![AppVeyor Build history](https://buildstats.info/appveyor/chart/RehanSaeed/Framework?branch=master&includeBuildsFromPullRequest=false)](https://ci.appveyor.com/project/RehanSaeed/Framework) |

## Contributions and Thanks

Please view the [contributing guide](/.github/CONTRIBUTING.md) for more information.

- [VictorioBerra](https://github.com/VictorioBerra) - Helping to create the `Boxed.DotnetNewTest` NuGet package.
