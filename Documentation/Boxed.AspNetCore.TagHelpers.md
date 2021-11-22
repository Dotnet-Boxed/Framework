![.NET Boxed Banner](https://github.com/Dotnet-Boxed/Templates/blob/main/Images/Banner.png)

## Boxed.AspNetCore.TagHelpers

[![Boxed.AspNetCore.TagHelpers](https://img.shields.io/nuget/v/Boxed.AspNetCore.TagHelpers.svg)](https://www.nuget.org/packages/Boxed.AspNetCore.TagHelpers/) [![Boxed.AspNetCore.TagHelpers package in dotnet-boxed feed in Azure Artifacts](https://feeds.dev.azure.com/dotnet-boxed/_apis/public/Packaging/Feeds/03bd56a4-9269-43f7-9f75-d82037c56a46/Packages/0b0ed292-8769-4d42-9d89-b037e936633f/Badge)](https://dev.azure.com/dotnet-boxed/Framework/_packaging?_a=package&feed=03bd56a4-9269-43f7-9f75-d82037c56a46&package=0b0ed292-8769-4d42-9d89-b037e936633f&preferRelease=true) [![Boxed.AspNetCore.TagHelpers NuGet Package Downloads](https://img.shields.io/nuget/dt/Boxed.AspNetCore.TagHelpers)](https://www.nuget.org/packages/Boxed.AspNetCore.TagHelpers)

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
