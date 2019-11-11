namespace Boxed.AspNetCore.Test
{
    using Boxed.AspNetCore;
    using Microsoft.Extensions.Hosting;
    using Xunit;

    public class HostBuilderExtensionsTest
    {
        private readonly HostBuilder hostBuilder;

        public HostBuilderExtensionsTest() => this.hostBuilder = new HostBuilder();

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void UseIf_TrueCondition_ActionCalled(bool condition)
        {
            var actionCalled = false;

            this.hostBuilder.UseIf(
                condition,
                x =>
                {
                    actionCalled = true;
                    return x;
                });

            Assert.Equal(actionCalled, condition);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void UseIfElse_TrueCondition_ActionCalled(bool condition)
        {
            var ifActionCalled = false;
            var elseActionCalled = false;

            this.hostBuilder.UseIfElse(
                condition,
                x =>
                {
                    ifActionCalled = true;
                    return x;
                },
                x =>
                {
                    elseActionCalled = true;
                    return x;
                });

            Assert.Equal(ifActionCalled, condition);
            Assert.NotEqual(elseActionCalled, condition);
        }
    }
}
