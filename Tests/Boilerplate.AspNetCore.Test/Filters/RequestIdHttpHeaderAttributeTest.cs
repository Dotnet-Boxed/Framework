namespace Boilerplate.AspNetCore.Test.Filters
{
    using Boilerplate.AspNetCore.Filters;
    using Microsoft.Extensions.Primitives;
    using Xunit;

    public class RequestIdHttpHeaderAttributeTest
    {
        private readonly RequestIdHttpHeaderAttribute filter;

        public RequestIdHttpHeaderAttributeTest()
        {
            this.filter = new RequestIdHttpHeaderAttribute();
        }

        [Theory]
        [InlineData("75b552c1-5a00-4872-b07f-e6bef735a7b7")]
        [InlineData("75B552C1-5A00-4872-B07F-E6BEF735A7B7")]
        public void IsValid_ValidGuid_ReturnsTrue(string headerValue)
        {
            bool isValid = this.filter.IsValid(GetStringValues(headerValue));

            Assert.True(isValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("x75b552c1-5a00-4872-b07f-e6bef735a7b7")]
        public void IsValid_InvalidGuid_ReturnsFalse(string headerValue)
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
