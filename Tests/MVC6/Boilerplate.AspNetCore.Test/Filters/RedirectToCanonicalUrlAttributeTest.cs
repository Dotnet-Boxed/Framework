namespace Boilerplate.AspNetCore.Test.Filters
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Routing;
    using Xunit;

    public class RedirectToCanonicalUrlAttributeTest
    {
        [Theory]
        [InlineData(true, true, "http://example.com")]
        [InlineData(true, true, "http://example.com/")]
        [InlineData(true, true, "http://example.com/?foo=bar")]
        [InlineData(true, true, "http://example.com/path/")]
        [InlineData(true, true, "http://example.com/path/?foo=bar")]

        [InlineData(true, false, "http://example.com")]
        [InlineData(true, false, "http://example.com/")]
        [InlineData(true, false, "http://example.com/?foo=BAR")]
        [InlineData(true, false, "http://example.com/PATH/")]
        [InlineData(true, false, "http://example.com/PATH/?foo=BAR")]

        [InlineData(false, true, "http://example.com")]
        [InlineData(false, true, "http://example.com/")]
        [InlineData(false, true, "http://example.com?foo=bar")]
        [InlineData(false, true, "http://example.com/path")]
        [InlineData(false, true, "http://example.com/path?foo=bar")]

        [InlineData(false, false, "http://example.com")]
        [InlineData(false, false, "http://example.com/")]
        [InlineData(false, false, "http://example.com?foo=BAR")]
        [InlineData(false, false, "http://example.com/PATH")]
        [InlineData(false, false, "http://example.com/PATH?foo=BAR")]
        public void TryGetCanonicalUrl_CanonicalUrl_ReturnsTrue(
            bool appendTrailingSlash,
            bool lowercaseUrls,
            string requestUrl)
        {
            var context = GetResourceExecutingContext(requestUrl);
            var filter = new RedirectToCanonicalUrlAttributeMock(appendTrailingSlash, lowercaseUrls);

            var isCanonical = filter.TryGetCanonicalUrl(context, out string canonicalUrl);

            Assert.True(
                isCanonical,
                $"{nameof(appendTrailingSlash)}={appendTrailingSlash} {nameof(lowercaseUrls)}={lowercaseUrls} {nameof(requestUrl)}={requestUrl}");
            Assert.Null(canonicalUrl);
        }

        [Theory]
        [InlineData(true, true, "http://example.com?FOO=BAR", "http://example.com/?foo=bar")]
        [InlineData(true, true, "http://example.com/?FOO=BAR", "http://example.com/?foo=bar")]
        [InlineData(true, true, "http://example.com/path", "http://example.com/path/")]
        [InlineData(true, true, "http://example.com/PATH/", "http://example.com/path/")]
        [InlineData(true, true, "http://example.com/PATH", "http://example.com/path/")]
        [InlineData(true, true, "http://example.com/PATH?FOO=BAR", "http://example.com/path/?foo=bar")]

        [InlineData(true, false, "http://example.com/path", "http://example.com/path/")]
        [InlineData(true, false, "http://example.com/PATH", "http://example.com/PATH/")]
        [InlineData(true, false, "http://example.com/PATH?FOO=BAR", "http://example.com/PATH/?FOO=BAR")]

        [InlineData(false, true, "http://example.com?FOO=BAR", "http://example.com/?foo=bar")]
        [InlineData(false, true, "http://example.com/?FOO=BAR", "http://example.com/?foo=bar")]
        [InlineData(false, true, "http://example.com/PATH", "http://example.com/path")]
        [InlineData(false, true, "http://example.com/PATH/", "http://example.com/path")]
        [InlineData(false, true, "http://example.com/PATH?FOO=BAR", "http://example.com/path?foo=bar")]

        [InlineData(false, false, "http://example.com/PATH/", "http://example.com/PATH")]
        [InlineData(false, false, "http://example.com/PATH/?FOO=BAR", "http://example.com/PATH?FOO=BAR")]
        public void TryGetCanonicalUrl_AppendTrailingSlashAndLowercaseUrlsAndIsNotCanonicalUrl_ReturnsFalseAndCanonicalUrl(
            bool appendTrailingSlash,
            bool lowercaseUrls,
            string requestUrl,
            string expectedCanonicalUrl)
        {
            var context = GetResourceExecutingContext(requestUrl);
            var filter = new RedirectToCanonicalUrlAttributeMock(appendTrailingSlash, lowercaseUrls);

            var isCanonical = filter.TryGetCanonicalUrl(context, out string canonicalUrl);

            Assert.False(
                isCanonical,
                $"{nameof(appendTrailingSlash)}={appendTrailingSlash} {nameof(lowercaseUrls)}={lowercaseUrls} {nameof(requestUrl)}={requestUrl}");
            Assert.Equal(expectedCanonicalUrl, canonicalUrl);
        }

        private static ResourceExecutingContext GetResourceExecutingContext(string requestUrl)
        {
            var uri = new Uri(requestUrl);
            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Scheme = uri.Scheme;
            request.Host = new HostString(uri.Host);
            request.Path = new PathString(uri.LocalPath);
            request.QueryString = new QueryString(uri.Query);
            return new ResourceExecutingContext(
                new ActionContext(
                    httpContext,
                    new RouteData(),
                    new ActionDescriptor()),
                new List<IFilterMetadata>(),
                new List<IValueProviderFactory>());
        }
    }
}
