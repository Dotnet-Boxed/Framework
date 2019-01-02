namespace Boxed.AspNetCore.TagHelpers
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// <see cref="HttpContext"/> extension methods.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Gets UrlHelper instance using <see cref="IUrlHelperFactory"/> and <see cref="IActionContextAccessor"/>.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns><see cref="UrlHelper"/> instance for current request.</returns>
        public static IUrlHelper GetUrlHelper(this HttpContext httpContext)
        {
            var services = httpContext
                           .Features
                           .Get<IServiceProvidersFeature>()
                           .RequestServices;
            var actionContext = services
                                .GetRequiredService<IActionContextAccessor>()
                                .ActionContext;
            var urlHelper = services
                            .GetRequiredService<IUrlHelperFactory>()
                            .GetUrlHelper(actionContext);
            return urlHelper;
        }
    }
}
