namespace Boilerplate.AspNetCore.Test.Filters
{
    using Boilerplate.AspNetCore.Filters;
    using Microsoft.Extensions.Primitives;
    using Xunit;

    public class UserAgentHttpHeaderAttributeTest
    {
        private readonly UserAgentHttpHeaderAttribute filter;

        public UserAgentHttpHeaderAttributeTest() =>
            this.filter = new UserAgentHttpHeaderAttribute();

        [Theory]
        [InlineData("ApplicationName")]
        [InlineData("Application Name/1.0.0.0")]
        [InlineData("ApplicationName/1.0.0.0")]
        [InlineData("Application-Name/1.0.0.0")]
        [InlineData("ApplicationName/1.0.0.0 (OS Name 1.0.0.0)")]
        [InlineData("Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Mobile Safari/537.36")]
        public void IsValid_ValidUserAgent_ReturnsTrue(string headerValue)
        {
            bool isValid = this.filter.IsValid(GetStringValues(headerValue));

            Assert.True(isValid, headerValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("ApplicationName/1.0.0.0,(OS Name 1.0.0.0)")]
        [InlineData("Application-Name/1.0.0.0,(OS Name 1.0.0.0)")]
        public void IsValid_InvalidUserAgent_ReturnsFalse(string headerValue)
        {
            bool isValid = this.filter.IsValid(GetStringValues(headerValue));

            Assert.False(isValid, headerValue);
        }

        private static StringValues GetStringValues(string value) =>
            value == null ? StringValues.Empty : new StringValues(value.Split(','));
    }
}
