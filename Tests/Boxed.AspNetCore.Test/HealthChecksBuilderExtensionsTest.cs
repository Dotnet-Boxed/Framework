namespace Boxed.AspNetCore.Test
{
    using System;
    using Boxed.AspNetCore;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Xunit;

    public sealed class HealthChecksBuilderExtensionsTest : IDisposable
    {
        private readonly Mock<IHealthChecksBuilder> healthChecksBuilderMock;

        public HealthChecksBuilderExtensionsTest() =>
            this.healthChecksBuilderMock = new Mock<IHealthChecksBuilder>(MockBehavior.Strict);

        [Fact]
        public void AddIf_TrueCondition_ActionCalled()
        {
            var actionCalled = false;

            this.healthChecksBuilderMock.Object.AddIf(
                true,
                healthChecksBuilder =>
                {
                    Assert.Same(this.healthChecksBuilderMock.Object, healthChecksBuilder);
                    actionCalled = true;
                    return healthChecksBuilder;
                });

            Assert.True(actionCalled);
        }

        [Fact]
        public void AddIf_FalseCondition_ActionCalled()
        {
            var actionCalled = false;

            this.healthChecksBuilderMock.Object.AddIf(
                false,
                healthChecksBuilder =>
                {
                    actionCalled = true;
                    return healthChecksBuilder;
                });

            Assert.False(actionCalled);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddIfElse_TrueCondition_ActionCalled(bool condition)
        {
            var ifActionCalled = false;
            var elseActionCalled = false;

            this.healthChecksBuilderMock.Object.AddIfElse(
                condition,
                healthChecksBuilder =>
                {
                    Assert.Same(this.healthChecksBuilderMock.Object, healthChecksBuilder);
                    ifActionCalled = true;
                    return healthChecksBuilder;
                },
                healthChecksBuilder =>
                {
                    Assert.Same(this.healthChecksBuilderMock.Object, healthChecksBuilder);
                    elseActionCalled = true;
                    return healthChecksBuilder;
                });

            Assert.Equal(ifActionCalled, condition);
            Assert.NotEqual(elseActionCalled, condition);
        }

        public void Dispose() => Mock.VerifyAll(this.healthChecksBuilderMock);
    }
}
