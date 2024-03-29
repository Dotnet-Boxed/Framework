namespace Boxed.AspNetCore.Test.Middleware;

using System;
using System.Threading.Tasks;
using Boxed.AspNetCore.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Xunit;

public class ServerTimingMiddlewareTest
{
    private readonly DefaultHttpContext context;
    private readonly RequestDelegate next;

    public ServerTimingMiddlewareTest()
    {
        this.context = new DefaultHttpContext();
        this.next = x => Task.CompletedTask;
    }

    [Fact]
    public Task InvokeAsync_NullContext_ThrowsArgumentNullExceptionAsync() =>
        Assert.ThrowsAsync<ArgumentNullException>(() => new ServerTimingMiddleware().InvokeAsync(null!, this.next));

    [Fact]
    public Task InvokeAsync_NullNext_ThrowsArgumentNullExceptionAsync() =>
        Assert.ThrowsAsync<ArgumentNullException>(() => new ServerTimingMiddleware().InvokeAsync(this.context, null!));

    [Fact]
    public async Task InvokeAsync_DoesNotSupportTrailingHeaders_DoNotAddServerTimingHttpHeaderAsync()
    {
        var responseTrailersFeature = this.context.Features.Get<IHttpResponseTrailersFeature>();

        await new ServerTimingMiddleware().InvokeAsync(this.context, this.next).ConfigureAwait(false);

        Assert.Null(responseTrailersFeature);
    }

    [Fact]
    public async Task InvokeAsync_SupportsTrailingHeaders_AddsServerTimingHttpHeaderAsync()
    {
        this.context.Features.Set<IHttpResponseTrailersFeature>(new ResponseTrailersFeature());
        var responseTrailersFeature = this.context.Features.Get<IHttpResponseTrailersFeature>()!;

        await new ServerTimingMiddleware().InvokeAsync(this.context, this.next).ConfigureAwait(false);

        var header = Assert.Single(responseTrailersFeature.Trailers);
        Assert.Equal("Server-Timing", header.Key);
        Assert.StartsWith("app;dur=0.", header.Value.ToString(), StringComparison.Ordinal);
    }

    internal sealed class ResponseTrailersFeature : IHttpResponseTrailersFeature
    {
        public IHeaderDictionary Trailers { get; set; } = new HeaderDictionary();
    }
}
