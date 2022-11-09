namespace Boxed.AspNetCore.Test.Middleware;

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boxed.AspNetCore.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class RequestCanceledMiddlewareTest
{
    private readonly DefaultHttpContext context;
    private RequestDelegate next;

    public RequestCanceledMiddlewareTest()
    {
        this.context = new DefaultHttpContext();
        this.next = x => Task.CompletedTask;
    }

    [Fact]
    public async Task InvokeAsync_NullContext_ThrowsArgumentNullExceptionAsync() =>
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            new RequestCanceledMiddleware(
                this.next,
                new RequestCanceledMiddlewareOptions(),
                new Mock<ILogger<RequestCanceledMiddleware>>().Object)
            .InvokeAsync(null!))
            .ConfigureAwait(false);

    [Fact]
    public async Task InvokeAsync_RequestNotCanceled_RunsNextMiddlewareAsync()
    {
        await new RequestCanceledMiddleware(
            this.next,
            new RequestCanceledMiddlewareOptions(),
            new Mock<ILogger<RequestCanceledMiddleware>>().Object)
        .InvokeAsync(this.context)
        .ConfigureAwait(false);

        Assert.Equal(200, this.context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_OperationCanceledExceptionThrownNotCanceled_RunsNextMiddlewareAsync()
    {
        using var cancellationTokenSource1 = new CancellationTokenSource();
        using var cancellationTokenSource2 = new CancellationTokenSource();
        cancellationTokenSource2.Cancel();
        this.context.RequestAborted = cancellationTokenSource1.Token;
        this.next = x => Task.FromException(new OperationCanceledException(cancellationTokenSource2.Token));

        await Assert
            .ThrowsAsync<OperationCanceledException>(() =>
                new RequestCanceledMiddleware(
                    this.next,
                    new RequestCanceledMiddlewareOptions(),
                    new Mock<ILogger<RequestCanceledMiddleware>>().Object)
                .InvokeAsync(this.context))
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task InvokeAsync_TaskCanceledExceptionThrownNotCanceled_RunsNextMiddlewareAsync()
    {
        using var cancellationTokenSource1 = new CancellationTokenSource();
        using var cancellationTokenSource2 = new CancellationTokenSource();
        cancellationTokenSource2.Cancel();
        this.context.RequestAborted = cancellationTokenSource1.Token;
        this.next = x => Task.FromException(new TaskCanceledException(Task.FromCanceled(cancellationTokenSource2.Token)));

        await Assert
            .ThrowsAsync<TaskCanceledException>(() =>
                new RequestCanceledMiddleware(
                    this.next,
                    new RequestCanceledMiddlewareOptions(),
                    new Mock<ILogger<RequestCanceledMiddleware>>().Object)
                .InvokeAsync(this.context))
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task InvokeAsync_RequestCanceled_Returns499ClientClosedRequestAsync()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var activityFeature = new TestActivityFeature(new Activity("test"));

        activityFeature.Activity.Start();
        cancellationTokenSource.Cancel();

        this.context.Features.Set<IHttpActivityFeature>(activityFeature);
        this.context.RequestAborted = cancellationTokenSource.Token;
        this.next = x => Task.FromCanceled(cancellationTokenSource.Token);

        await new RequestCanceledMiddleware(
            this.next,
            new RequestCanceledMiddlewareOptions(),
            new Mock<ILogger<RequestCanceledMiddleware>>().Object)
        .InvokeAsync(this.context)
        .ConfigureAwait(false);

        Assert.Single(Activity.Current!.Events);
        Assert.Equal("Client cancelled the request.", Activity.Current!.Events.SingleOrDefault().Name);
        Assert.Equal(RequestCanceledMiddlewareOptions.ClientClosedRequest, this.context.Response.StatusCode);

        activityFeature.Activity.Dispose();
    }

    private sealed class TestActivityFeature : IHttpActivityFeature
    {
        public TestActivityFeature(Activity activity) => this.Activity = activity;

        public Activity Activity { get; set; }
    }
}
