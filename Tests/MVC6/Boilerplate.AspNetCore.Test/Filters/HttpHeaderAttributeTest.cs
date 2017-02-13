namespace Boilerplate.AspNetCore.Test.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Boilerplate.AspNetCore.Filters;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;
    using Moq;
    using Xunit;

    public class HttpHeaderAttributeTest : IDisposable
    {
        private const string HttpHeaderName = "X-Request-ID";
        private readonly ActionExecutingContext actionExecutingContext;
        private readonly Mock<HttpContext> httpContextMock;
        private readonly Mock<HttpRequest> httpRequestMock;
        private readonly Mock<HttpResponse> httpResponseMock;
        private readonly Mock<ILoggerFactory> loggerFactoryMock;
        private readonly Mock<ILogger> loggerMock;
        private readonly Mock<IServiceProvider> serviceProviderMock;
        private readonly HttpHeaderAttributeStub filter;

        public HttpHeaderAttributeTest()
        {
            this.httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
            this.httpRequestMock = new Mock<HttpRequest>(MockBehavior.Strict);
            this.httpResponseMock = new Mock<HttpResponse>(MockBehavior.Strict);
            this.loggerFactoryMock = new Mock<ILoggerFactory>(MockBehavior.Strict);
            this.loggerMock = new Mock<ILogger>(MockBehavior.Strict);
            this.serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            var actionContext = new ActionContext(
                this.httpContextMock.Object,
                new RouteData(),
                new ActionDescriptor(),
                new ModelStateDictionary());
            this.actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new ControllerMock());

            this.filter = new HttpHeaderAttributeStub(HttpHeaderName);
        }

        [Fact]
        public void OnActionExecuting_ForwardRequiredAndValidAndHasHeader_HeaderAddedToResponse()
        {
            var requestHeaders = new HeaderDictionary()
            {
                { HttpHeaderName, "Hello" }
            };
            var responseHeaders = new HeaderDictionary();
            this.httpContextMock.SetupGet(x => x.Request).Returns(this.httpRequestMock.Object);
            this.httpContextMock.SetupGet(x => x.Response).Returns(this.httpResponseMock.Object);
            this.httpRequestMock.SetupGet(x => x.Headers).Returns(requestHeaders);
            this.httpResponseMock.SetupGet(x => x.Headers).Returns(responseHeaders);
            this.filter.Forward = true;
            this.filter.Required = true;
            this.filter.Valid = true;

            this.filter.OnActionExecuting(this.actionExecutingContext);

            Assert.True(responseHeaders.ContainsKey(HttpHeaderName));
            Assert.Equal(1, responseHeaders[HttpHeaderName].Count);
            Assert.Equal("Hello", responseHeaders[HttpHeaderName].First());
        }

        [Fact]
        public void OnActionExecuting_NotForwardRequiredAndValidAndHasHeader_NoChange()
        {
            var requestHeaders = new HeaderDictionary()
            {
                { HttpHeaderName, "Hello" }
            };
            var responseHeaders = new HeaderDictionary();
            this.httpContextMock.SetupGet(x => x.Request).Returns(this.httpRequestMock.Object);
            this.httpRequestMock.SetupGet(x => x.Headers).Returns(requestHeaders);
            this.filter.Forward = false;
            this.filter.Required = true;
            this.filter.Valid = true;

            this.filter.OnActionExecuting(this.actionExecutingContext);

            Assert.False(responseHeaders.ContainsKey(HttpHeaderName));
        }

        [Fact]
        public void OnActionExecuting_ForwardNotRequiredAndValidAndDoesNotHaveHeader_NoChange()
        {
            var requestHeaders = new HeaderDictionary();
            var responseHeaders = new HeaderDictionary();
            this.httpContextMock.SetupGet(x => x.Request).Returns(this.httpRequestMock.Object);
            this.httpRequestMock.SetupGet(x => x.Headers).Returns(requestHeaders);
            this.filter.Forward = true;
            this.filter.Required = false;
            this.filter.Valid = true;

            this.filter.OnActionExecuting(this.actionExecutingContext);

            Assert.False(responseHeaders.ContainsKey(HttpHeaderName));
        }

        [Fact]
        public void OnActionExecuting_RequiredAndValidAndDoesNotHaveHeader_Returns400BadRequest()
        {
            var requestHeaders = new HeaderDictionary();
            var responseHeaders = new HeaderDictionary();
            this.httpContextMock.SetupGet(x => x.Request).Returns(this.httpRequestMock.Object);
            this.httpContextMock.SetupGet(x => x.RequestServices).Returns(this.serviceProviderMock.Object);
            this.httpRequestMock.SetupGet(x => x.Headers).Returns(requestHeaders);
            this.serviceProviderMock.Setup(x => x.GetService(typeof(ILoggerFactory))).Returns(this.loggerFactoryMock.Object);
            this.loggerFactoryMock.Setup(x => x.CreateLogger(typeof(HttpHeaderAttribute).FullName)).Returns(this.loggerMock.Object);
            this.loggerMock.Setup(x => x.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<object>(), null, It.IsAny<Func<object, Exception, string>>()));
            this.filter.Forward = true;
            this.filter.Required = true;
            this.filter.Valid = true;

            this.filter.OnActionExecuting(this.actionExecutingContext);

            Assert.False(responseHeaders.ContainsKey(HttpHeaderName));
            Assert.IsType<BadRequestObjectResult>(this.actionExecutingContext.Result);
            var badRequest = (BadRequestObjectResult)this.actionExecutingContext.Result;
            Assert.Equal($"{HttpHeaderName} HTTP header is required.", badRequest.Value);
        }

        [Fact]
        public void OnActionExecuting_NotForwardAndRequiredAndInvalid_Returns400BadRequest()
        {
            var requestHeaders = new HeaderDictionary()
            {
                { HttpHeaderName, "Hello" }
            };
            var responseHeaders = new HeaderDictionary();
            this.httpContextMock.SetupGet(x => x.Request).Returns(this.httpRequestMock.Object);
            this.httpContextMock.SetupGet(x => x.RequestServices).Returns(this.serviceProviderMock.Object);
            this.httpRequestMock.SetupGet(x => x.Headers).Returns(requestHeaders);
            this.serviceProviderMock.Setup(x => x.GetService(typeof(ILoggerFactory))).Returns(this.loggerFactoryMock.Object);
            this.loggerFactoryMock.Setup(x => x.CreateLogger(typeof(HttpHeaderAttribute).FullName)).Returns(this.loggerMock.Object);
            this.loggerMock.Setup(x => x.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<object>(), null, It.IsAny<Func<object, Exception, string>>()));
            this.filter.Forward = false;
            this.filter.Required = true;
            this.filter.Valid = false;

            this.filter.OnActionExecuting(this.actionExecutingContext);

            Assert.False(responseHeaders.ContainsKey(HttpHeaderName));
            Assert.IsType<BadRequestObjectResult>(this.actionExecutingContext.Result);
            var badRequest = (BadRequestObjectResult)this.actionExecutingContext.Result;
            Assert.Equal($"{HttpHeaderName} HTTP header value 'Hello' is invalid.", badRequest.Value);
        }

        public void Dispose()
        {
            Mock.VerifyAll(
                this.httpContextMock,
                this.httpRequestMock,
                this.httpResponseMock,
                this.loggerFactoryMock,
                this.loggerMock,
                this.serviceProviderMock);
        }

        public class ControllerMock : Controller
        {
        }

        public class HttpHeaderAttributeStub : HttpHeaderAttribute
        {
            public HttpHeaderAttributeStub(string httpHeaderName)
                : base(httpHeaderName)
            {
            }

            public bool Valid { get; set; }

            public override bool IsValid(StringValues headerValues)
            {
                return this.Valid;
            }
        }
    }
}
