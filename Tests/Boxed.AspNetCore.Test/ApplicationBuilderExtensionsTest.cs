namespace Boxed.AspNetCore.Test
{
    using System;
    using System.Threading.Tasks;
    using Boxed.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Builder.Internal;
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UseIf_TrueCondition_ActionCalled(bool condition)
        {
            var actionCalled = false;

            this.applicationBuilder.UseIf(
                context => condition,
                application => application.Use(
                    (context, next) =>
                    {
                        Assert.NotSame(application, this.applicationBuilder);
                        actionCalled = true;
                        return next.Invoke();
                    }));
            await this.applicationBuilder.Build().Invoke(this.httpContext).ConfigureAwait(false);

            Assert.Equal(actionCalled, condition);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UseIfElse_TrueCondition_ActionCalled(bool condition)
        {
            var ifActionCalled = false;
            var elseActionCalled = false;

            this.applicationBuilder.UseIfElse(
                context => condition,
                application => application.Use(
                    (context, next) =>
                    {
                        Assert.NotSame(application, this.applicationBuilder);
                        ifActionCalled = true;
                        return next.Invoke();
                    }),
                application => application.Use(
                    (context, next) =>
                    {
                        Assert.NotSame(application, this.applicationBuilder);
                        elseActionCalled = true;
                        return next.Invoke();
                    }));
            await this.applicationBuilder.Build().Invoke(this.httpContext).ConfigureAwait(false);

            Assert.Equal(ifActionCalled, condition);
            Assert.NotEqual(elseActionCalled, condition);
        }

        public void Dispose() =>
            Mock.VerifyAll(
                this.serviceProviderMock);
    }
}
