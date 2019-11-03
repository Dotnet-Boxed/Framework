namespace Boxed.AspNetCore.Test
{
    using System;
    using System.Threading.Tasks;
    using Boxed.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;

    public class ApplicationBuilderExtensionsTest : IDisposable
    {
        private readonly Mock<IServiceProvider> serviceProviderMock;
        private readonly ApplicationBuilder applicationBuilder;
        private readonly HttpContext httpContext;

        public ApplicationBuilderExtensionsTest()
        {
            this.serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);
            this.applicationBuilder = new ApplicationBuilder(this.serviceProviderMock.Object);
            this.httpContext = new DefaultHttpContext();
        }

        [Fact]
        public async Task UseIf_TrueCondition_ActionCalled()
        {
            var actionCalled = false;

            this.applicationBuilder.UseIf(
                true,
                application =>
                {
                    Assert.Same(this.applicationBuilder, application);
                    actionCalled = true;
                    return application;
                });
            await this.applicationBuilder.Build().Invoke(this.httpContext).ConfigureAwait(false);

            Assert.True(actionCalled);
        }

        [Fact]
        public async Task UseIf_FalseCondition_ActionCalled()
        {
            var actionCalled = false;

            this.applicationBuilder.UseIf(
                false,
                application =>
                {
                    actionCalled = true;
                    return application;
                });
            await this.applicationBuilder.Build().Invoke(this.httpContext).ConfigureAwait(false);

            Assert.False(actionCalled);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UseIfElse_TrueCondition_ActionCalled(bool condition)
        {
            var ifActionCalled = false;
            var elseActionCalled = false;

            this.applicationBuilder.UseIfElse(
                condition,
                application =>
                {
                    Assert.Same(application, this.applicationBuilder);
                    ifActionCalled = true;
                    return application;
                },
                application =>
                {
                    Assert.Same(application, this.applicationBuilder);
                    elseActionCalled = true;
                    return application;
                });
            await this.applicationBuilder.Build().Invoke(this.httpContext).ConfigureAwait(false);

            Assert.Equal(ifActionCalled, condition);
            Assert.NotEqual(elseActionCalled, condition);
        }

        public void Dispose() => Mock.VerifyAll(this.serviceProviderMock);
    }
}
