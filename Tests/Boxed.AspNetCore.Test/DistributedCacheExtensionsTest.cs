namespace Boxed.AspNetCore.Test;

using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Boxed.AspNetCore;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Xunit;

public sealed class DistributedCacheExtensionsTest : IDisposable
{
    private readonly Mock<IDistributedCache> distributedCacheMock;

    public DistributedCacheExtensionsTest() =>
        this.distributedCacheMock = new Mock<IDistributedCache>(MockBehavior.Strict);

    [Fact]
    public Task GetAsJsonAsync_NullDistributedCache_ThrowsArgumentNullExceptionAsync() =>
        Assert.ThrowsAsync<ArgumentNullException>(() => ((IDistributedCache)null!).GetAsJsonAsync<TestClass>("Key"));

    [Fact]
    public Task GetAsJsonAsync_NullKey_ThrowsArgumentNullExceptionAsync() =>
        Assert.ThrowsAsync<ArgumentNullException>(() => this.distributedCacheMock.Object.GetAsJsonAsync<TestClass>(null!));

    [Fact]
    public async Task GetAsJsonAsync_ValidValue_ReturnsDeserializedObjectAsync()
    {
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            this.distributedCacheMock
                .Setup(x => x.GetAsync("Key", cancellationTokenSource.Token))
                .ReturnsAsync(Encoding.UTF8.GetBytes("{\"Value\":1}"));

            var testClass = await this.distributedCacheMock.Object
                .GetAsJsonAsync<TestClass>("Key", null, cancellationTokenSource.Token)
                .ConfigureAwait(false);

            Assert.NotNull(testClass);
            Assert.Equal(1, testClass!.Value);
        }
    }

    [Fact]
    public Task SetAsJsonAsync_NullDistributedCache_ThrowsArgumentNullExceptionAsync() =>
        Assert.ThrowsAsync<ArgumentNullException>(
            () => ((IDistributedCache)null!).SetAsJsonAsync("Key", new TestClass()));

    [Fact]
    public Task SetAsJsonAsync_NullKey_ThrowsArgumentNullExceptionAsync() =>
        Assert.ThrowsAsync<ArgumentNullException>(
            () => this.distributedCacheMock.Object.SetAsJsonAsync(null!, new TestClass()));

    [Fact]
    public async Task SetAsJsonAsync_ValidValue_SavesSerializedValueAsync()
    {
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            this.distributedCacheMock
                .Setup(x => x.SetAsync(
                    "Key",
                    It.Is<byte[]>(y => y.SequenceEqual(Encoding.UTF8.GetBytes("{\"Value\":1}"))),
                    null,
                    cancellationTokenSource.Token))
                .Returns(Task.CompletedTask);

            await this.distributedCacheMock.Object
                .SetAsJsonAsync("Key", new TestClass() { Value = 1 }, null, null, cancellationTokenSource.Token)
                .ConfigureAwait(false);
        }
    }

    public void Dispose() => Mock.VerifyAll(this.distributedCacheMock);

    internal class TestClass
    {
        public int Value { get; set; }
    }
}
