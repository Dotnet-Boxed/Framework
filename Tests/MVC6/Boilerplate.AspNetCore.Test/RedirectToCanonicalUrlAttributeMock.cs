namespace Boilerplate.AspNetCore.Test
{
    using System.Diagnostics;
    using Boilerplate.AspNetCore.Filters;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Options;

    [DebuggerStepThrough]
    public class RedirectToCanonicalUrlAttributeMock : RedirectToCanonicalUrlAttribute
    {
        public RedirectToCanonicalUrlAttributeMock(IOptions<RouteOptions> options)
            : base(options)
        {
        }

        public RedirectToCanonicalUrlAttributeMock(bool appendTrailingSlash, bool lowercaseUrls)
            : base(appendTrailingSlash, lowercaseUrls)
        {
        }

        public new bool TryGetCanonicalUrl(ResourceExecutingContext context, out string canonicalUrl) =>
            base.TryGetCanonicalUrl(context, out canonicalUrl);
    }
}
