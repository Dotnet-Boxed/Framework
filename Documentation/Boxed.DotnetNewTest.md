![.NET Boxed Banner](https://github.com/Dotnet-Boxed/Templates/blob/main/Images/Banner.png)

## Boxed.DotnetNewTest

[![Boxed.DotnetNewTest](https://img.shields.io/nuget/v/Boxed.DotnetNewTest.svg)](https://www.nuget.org/packages/Boxed.DotnetNewTest/) [![Boxed.DotnetNewTest package in dotnet-boxed feed in Azure Artifacts](https://feeds.dev.azure.com/dotnet-boxed/_apis/public/Packaging/Feeds/03bd56a4-9269-43f7-9f75-d82037c56a46/Packages/537173a7-9aba-493c-abd2-935fa9c14e27/Badge)](https://dev.azure.com/dotnet-boxed/Framework/_packaging?_a=package&feed=03bd56a4-9269-43f7-9f75-d82037c56a46&package=537173a7-9aba-493c-abd2-935fa9c14e27&preferRelease=true) [![Boxed.DotnetNewTest NuGet Package Downloads](https://img.shields.io/nuget/dt/Boxed.DotnetNewTest)](https://www.nuget.org/packages/Boxed.DotnetNewTest)

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
