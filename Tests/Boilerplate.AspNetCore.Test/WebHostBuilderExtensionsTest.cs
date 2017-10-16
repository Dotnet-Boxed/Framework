namespace Boilerplate.AspNetCore.Test
{
    using Boilerplate.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Xunit;

    public class WebHostBuilderExtensionsTest
    {
        private readonly WebHostBuilder webHostBuilder;

        public WebHostBuilderExtensionsTest() =>
            this.webHostBuilder = new WebHostBuilder();

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void UseIf_TrueCondition_ActionCalled(bool condition)
        {
            var actionCalled = false;

            this.webHostBuilder.UseIf(
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

            this.webHostBuilder.UseIfElse(
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
