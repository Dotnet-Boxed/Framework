namespace Boxed.AspNetCore.Test;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Xunit;

public class RedirectToCanonicalUrlRuleTest
{
    [Fact]
    public void ApplyRule_NullContext_ThrowsArgumentNullException() =>
        Assert.Throws<ArgumentNullException>(
            () => new RedirectToCanonicalUrlRule(true, true).ApplyRule(null!));

    [Theory]
    [InlineData("CONNECT")]
    [InlineData("PUT")]
    [InlineData("POST")]
    [InlineData("PATCH")]
    [InlineData("TRACE")]
    [InlineData("HEAD")]
    [InlineData("DELETE")]
    [InlineData("OPTIONS")]
    public void ApplyRule_HttpMethodNotGet_DoNothing(string httpMethod)
    {
        var rule = new RedirectToCanonicalUrlRule(true, true);
        var context = new RewriteContext()
        {
            HttpContext = new DefaultHttpContext(),
        };
        context.HttpContext.Request.Method = httpMethod;

        rule.ApplyRule(context);

        Assert.Equal(RuleResult.ContinueRules, context.Result);
        Assert.Equal(StatusCodes.Status200OK, context.HttpContext.Response.StatusCode);
    }

    [Theory]
    [InlineData(true, true, "https://example.com")]
    [InlineData(true, true, "https://example.com/")]
    [InlineData(true, true, "https://example.com?bar=baz")]
    [InlineData(true, true, "https://example.com/?bar=baz")]
    [InlineData(true, true, "https://example.com/foo/")]
    [InlineData(true, true, "https://example.com/foo/?bar=baz")]

    [InlineData(false, false, "https://example.com")]
    [InlineData(false, false, "https://example.com/")]
    [InlineData(false, false, "https://example.com?bar=baz")]
    [InlineData(false, false, "https://example.com/?bar=baz")]
    [InlineData(false, false, "https://example.com/foo")]
    [InlineData(false, false, "https://example.com/foo?bar=baz")]
    public void ApplyRule_WithCanonicalUrl_DoNothing(
        bool appendTrailingSlash,
        bool lowercaseUrls,
        string url)
    {
        var rule = new RedirectToCanonicalUrlRule(appendTrailingSlash, lowercaseUrls);
        var context = new RewriteContext()
        {
            HttpContext = new DefaultHttpContext(),
        };
        context.HttpContext.SetEndpoint(new Endpoint(x => Task.CompletedTask, new EndpointMetadataCollection(), "Name"));
        var request = context.HttpContext.Request;
        request.Method = HttpMethods.Get;
        SetUrl(url, request);

        rule.ApplyRule(context);

        Assert.Equal(RuleResult.ContinueRules, context.Result);
        Assert.Equal(StatusCodes.Status200OK, context.HttpContext.Response.StatusCode);
    }

    [Theory]
    [InlineData(true, true, "https://example.com?BAR=BAZ", "https://example.com/?bar=baz")]
    [InlineData(true, true, "https://example.com/?BAR=BAZ", "https://example.com/?bar=baz")]
    [InlineData(true, true, "https://example.com/foo", "https://example.com/foo/")]
    [InlineData(true, true, "https://example.com/FOO/", "https://example.com/foo/")]
    [InlineData(true, true, "https://example.com/FOO", "https://example.com/foo/")]
    [InlineData(true, true, "https://example.com/FOO?BAR=BAZ", "https://example.com/foo/?bar=baz")]

    [InlineData(false, false, "https://example.com/foo/", "https://example.com/foo")]
    [InlineData(false, false, "https://example.com/FOO/", "https://example.com/FOO")]
    [InlineData(false, false, "https://example.com/FOO/?BAR=BAZ", "https://example.com/FOO?BAR=BAZ")]
    public void ApplyRule_WithoutCanonicalUrl_301PermanentRedirectToCanonicalUrl(
        bool appendTrailingSlash,
        bool lowercaseUrls,
        string url,
        string canonicalUrl)
    {
        var rule = new RedirectToCanonicalUrlRule(appendTrailingSlash, lowercaseUrls);
        var context = new RewriteContext()
        {
            HttpContext = new DefaultHttpContext(),
        };
        context.HttpContext.SetEndpoint(new Endpoint(x => Task.CompletedTask, new EndpointMetadataCollection(), "Name"));
        var request = context.HttpContext.Request;
        var response = context.HttpContext.Response;
        request.Method = HttpMethods.Get;
        SetUrl(url, request);

        rule.ApplyRule(context);

        Assert.Equal(RuleResult.EndResponse, context.Result);
        Assert.Equal(StatusCodes.Status301MovedPermanently, response.StatusCode);
        Assert.Equal(canonicalUrl, response.Headers.Location.ToString());
    }

    private static void SetUrl(string url, HttpRequest request)
    {
        var urlBuilder = new UriBuilder(url);
        request.Scheme = urlBuilder.Scheme;
        request.Host = new HostString(urlBuilder.Host);
        request.Path = new PathString(urlBuilder.Path);
        request.QueryString = new QueryString(urlBuilder.Query);
    }
}
