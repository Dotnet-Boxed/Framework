namespace Boxed.AspNetCore.Test.Filters
{
    using System;
    using System.Collections.Generic;
    using Boxed.AspNetCore.Filters;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Primitives;
    using Xunit;

    public class RequestIdHttpHeaderAttributeTest
    {
        private readonly RequestIdHttpHeaderAttribute filter;

        public RequestIdHttpHeaderAttributeTest() =>
            this.filter = new RequestIdHttpHeaderAttribute();

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

        [Fact]
        public void OnHasHeader_UpdateTraceIdentifierIsTrue_TraceIdentifierSet()
        {
            var context = GetContext();
            var requestId = Guid.NewGuid().ToString();
            this.filter.UpdateTraceIdentifier = true;

            this.filter.OnHasHeader(context, requestId);

            Assert.Equal(context.HttpContext.TraceIdentifier, requestId);
        }

        [Fact]
        public void OnHasHeader_UpdateTraceIdentifierIsFalse_TraceIdentifierNotSet()
        {
            var context = GetContext();
            var requestId = Guid.NewGuid().ToString();
            this.filter.UpdateTraceIdentifier = false;

            this.filter.OnHasHeader(context, requestId);

            Assert.NotEqual(context.HttpContext.TraceIdentifier, requestId);
        }

        private static ActionExecutingContext GetContext() =>
            new ActionExecutingContext(
                new ActionContext(
                    new DefaultHttpContext(),
                    new RouteData(),
                    new ActionDescriptor()),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                null);

        private static StringValues GetStringValues(string value) =>
            value == null ? StringValues.Empty : new StringValues(value.Split(','));
    }
}
