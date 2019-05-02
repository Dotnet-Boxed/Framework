namespace Boxed.AspNetCore.Test.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Boxed.AspNetCore.Filters;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Internal;
    using Moq;
    using Xunit;

    public class OperationCancelledExceptionFilterAttributeTest : IDisposable
    {
        private readonly Mock<ILogger<OperationCancelledExceptionFilterAttribute>> loggerMock;
        private readonly OperationCancelledExceptionFilterAttribute filter;

        public OperationCancelledExceptionFilterAttributeTest()
        {
            this.loggerMock = new Mock<ILogger<OperationCancelledExceptionFilterAttribute>>(MockBehavior.Strict);
            this.filter = new OperationCancelledExceptionFilterAttribute(this.loggerMock.Object);
        }

        [Theory]
        [InlineData(typeof(AggregateException))]
        [InlineData(typeof(Exception))]
        public void OnException_NonCancellationExceptionType_DoesNothing(Type type)
        {
            var context = GetExceptionContext(type);

            this.filter.OnException(context);
        }

        [Theory]
        [InlineData(typeof(OperationCanceledException))]
        [InlineData(typeof(TaskCanceledException))]
        public void OnException_OperationCanceledException_LogsInfoAndReturnsHttp499ClientClosedRequest(Type type)
        {
            var context = GetExceptionContext(type);
            this.loggerMock.Setup(x => x.Log(
                LogLevel.Information,
                0,
                It.IsAny<FormattedLogValues>(),
                null,
                It.IsAny<Func<object, Exception, string>>()));

            this.filter.OnException(context);
        }

        public void Dispose() => Mock.VerifyAll(this.loggerMock);

        private static ExceptionContext GetExceptionContext(Type exceptionType) =>
            new ExceptionContext(
                new ActionContext(
                    new DefaultHttpContext(),
                    new RouteData(),
                    new ActionDescriptor()),
                new List<IFilterMetadata>())
            {
                Exception = (Exception)Activator.CreateInstance(exceptionType)
            };
    }
}
