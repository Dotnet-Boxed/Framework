namespace Framework.AspNetCore.Test.Filters
{
    using Framework.AspNetCore.Filters;
    using Microsoft.Extensions.Primitives;
    using Xunit;

    public class UserAgentHttpHeaderAttributeTest
    {
        private readonly UserAgentHttpHeaderAttribute filter;

        public UserAgentHttpHeaderAttributeTest()
        {
            this.filter = new UserAgentHttpHeaderAttribute();
        }

        [Theory]
        [InlineData("ApplicationName/1.0.0.0")]
        [InlineData("Application-Name/1.0.0.0")]
        [InlineData("ApplicationName/1.0.0.0,(OS Name 1.0.0.0)")]
        [InlineData("Application-Name/1.0.0.0,(OS Name 1.0.0.0)")]
        [InlineData("ApplicationName/1.0.0.0 (OS Name 1.0.0.0)")]
        public void IsValid_ValidUserAgent_ReturnsTrue(string headerValue)
        {
            bool isValid = this.filter.IsValid(GetStringValues(headerValue));

            Assert.True(isValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("ApplicationName")] // Version must be specified.
        [InlineData("Application Name/1.0.0.0")] // Spaces not allowed in Application Name.
        public void IsValid_InvalidUserAgent_ReturnsFalse(string headerValue)
        {
            bool isValid = this.filter.IsValid(GetStringValues(headerValue));

            Assert.False(isValid);
        }

        private static StringValues GetStringValues(string value)
        {
            return value == null ? StringValues.Empty : new StringValues(value.Split(','));
        }
    }
}
