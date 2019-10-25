namespace Boxed.AspNetCore.TagHelpers.Test
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Caching.Distributed;
    using Moq;
    using Xunit;

    public class SubresourceIntegrityTagHelperTest
    {
        private readonly Mock<IDistributedCache> distributedCacheMock;
        private readonly Mock<IHostingEnvironment> hostingEnvironmentMock;
        private readonly Mock<IActionContextAccessor> actionContextAccessor;
        private readonly Mock<IUrlHelperFactory> urlHelperFactoryMock;
        private readonly Mock<IUrlHelper> urlHelperMock;
        private readonly SubresourceIntegrityTagHelper tagHelper;
        private readonly Mock<HtmlEncoder> htmlEncoderMock;

        public SubresourceIntegrityTagHelperTest()
        {
            this.distributedCacheMock = new Mock<IDistributedCache>(MockBehavior.Strict);
            this.hostingEnvironmentMock = new Mock<IHostingEnvironment>(MockBehavior.Strict);
            this.actionContextAccessor = new Mock<IActionContextAccessor>(MockBehavior.Strict);
            this.urlHelperFactoryMock = new Mock<IUrlHelperFactory>(MockBehavior.Strict);
            this.urlHelperMock = new Mock<IUrlHelper>(MockBehavior.Strict);
            this.htmlEncoderMock = new Mock<HtmlEncoder>(MockBehavior.Strict);

            this.actionContextAccessor.SetupGet(x => x.ActionContext).Returns((ActionContext)null);
            this.urlHelperFactoryMock.Setup(x => x.GetUrlHelper(this.actionContextAccessor.Object.ActionContext)).Returns(this.urlHelperMock.Object);
            this.htmlEncoderMock.Setup(x => x.Encode(It.IsAny<string>())).Returns((string s) => s);

            this.tagHelper = new TestSubresourceIntegrityTagHelper(
                this.distributedCacheMock.Object,
                this.hostingEnvironmentMock.Object,
                this.actionContextAccessor.Object,
                this.urlHelperFactoryMock.Object,
                this.htmlEncoderMock.Object);
        }

        [Fact]
        public async Task ProcessAsync_SriAlreadyCached_ReturnsCachedSri()
        {
            this.tagHelper.Source = "/foo.js";
            var attributes = new TagHelperAttributeList
            {
                { "src", "/foo.js" }
            };
            var context = new TagHelperContext(attributes, new Dictionary<object, object>(), Guid.NewGuid().ToString());
            var output = new TagHelperOutput("script", attributes, (x, y) => throw new ArgumentException());
            this.distributedCacheMock
                .Setup(x => x.GetAsync("SRI:/foo.js", CancellationToken.None))
                .ReturnsAsync(Encoding.UTF8.GetBytes("SRI Value"));

            await this.tagHelper.ProcessAsync(context, output);

            Assert.True(attributes.ContainsName("crossorigin"));
            Assert.Equal("anonymous", attributes["crossorigin"].Value);
            Assert.True(attributes.ContainsName("integrity"));
            var htmlString = Assert.IsType<HtmlString>(attributes["integrity"].Value);
            Assert.Equal("SRI Value", htmlString.Value);
        }

        [Fact]
        public async Task ProcessAsync_Sha512SriNotCached_CachesSriAndReturnsIt()
        {
            var expectedSri = "sha512-NhX4DJ0pPtdAJof5SyLVjlKbjMeRb4+sf933+9WvTPd309eVp6AKFr9+fz+5Vh7puq5IDan+ehh2nnGIawPzFQ==";
            this.tagHelper.Source = "/foo.js";
            var attributes = new TagHelperAttributeList
            {
                { "src", "/foo.js" }
            };
            var context = new TagHelperContext(attributes, new Dictionary<object, object>(), Guid.NewGuid().ToString());
            var output = new TagHelperOutput("script", attributes, (x, y) => throw new ArgumentException());
            this.distributedCacheMock
                .Setup(x => x.GetAsync("SRI:/foo.js", CancellationToken.None))
                .ReturnsAsync((byte[])null);
            this.hostingEnvironmentMock.SetupGet(x => x.WebRootPath).Returns(@"C:\Foo\wwwroot");
            this.urlHelperMock.Setup(x => x.Content("/foo.js")).Returns(@"C:\Foo\wwwroot\foo.js");
            this.distributedCacheMock
                .Setup(x => x.SetAsync(
                    "SRI:/foo.js",
                    It.Is<byte[]>(y => string.Equals(Encoding.UTF8.GetString(y), expectedSri, StringComparison.Ordinal)),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    CancellationToken.None))
                .Returns(Task.CompletedTask);

            await this.tagHelper.ProcessAsync(context, output);

            Assert.True(attributes.ContainsName("crossorigin"));
            Assert.Equal("anonymous", attributes["crossorigin"].Value);
            Assert.True(attributes.ContainsName("integrity"));
            var htmlString = Assert.IsType<HtmlString>(attributes["integrity"].Value);
            Assert.Equal(expectedSri, htmlString.Value);
        }

        [Fact]
        public async Task ProcessAsync_Sha256And384And512SriNotCached_CachesSriAndReturnsIt()
        {
            var expectedSri = "sha256-GF+NsyJx/iX1Yab8k4suJkMG7DBO2lGAB9F2SCY4GWk= " +
                "sha384-NRn+WtLFlu/j4nam81G4/AsD24YXgkkNRfdZjr0Ktf1VIO0QLzjEpeyDTphmgDX8 " +
                "sha512-NhX4DJ0pPtdAJof5SyLVjlKbjMeRb4+sf933+9WvTPd309eVp6AKFr9+fz+5Vh7puq5IDan+ehh2nnGIawPzFQ==";
            this.tagHelper.HashAlgorithms =
                SubresourceIntegrityHashAlgorithm.SHA256 |
                SubresourceIntegrityHashAlgorithm.SHA384 |
                SubresourceIntegrityHashAlgorithm.SHA512;
            this.tagHelper.Source = "/foo.js";
            var attributes = new TagHelperAttributeList
            {
                { "src", "/foo.js" }
            };
            var context = new TagHelperContext(attributes, new Dictionary<object, object>(), Guid.NewGuid().ToString());
            var output = new TagHelperOutput("script", attributes, (x, y) => throw new ArgumentException());
            this.distributedCacheMock
                .Setup(x => x.GetAsync("SRI:/foo.js", CancellationToken.None))
                .ReturnsAsync((byte[])null);
            this.hostingEnvironmentMock.SetupGet(x => x.WebRootPath).Returns(@"C:\Foo\wwwroot");
            this.urlHelperMock.Setup(x => x.Content("/foo.js")).Returns(@"C:\Foo\wwwroot\foo.js");
            this.distributedCacheMock
                .Setup(x => x.SetAsync(
                    "SRI:/foo.js",
                    It.Is<byte[]>(y => string.Equals(Encoding.UTF8.GetString(y), expectedSri, StringComparison.Ordinal)),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    CancellationToken.None))
                .Returns(Task.CompletedTask);

            await this.tagHelper.ProcessAsync(context, output);

            Assert.True(attributes.ContainsName("crossorigin"));
            Assert.Equal("anonymous", attributes["crossorigin"].Value);
            Assert.True(attributes.ContainsName("integrity"));
            var htmlString = Assert.IsType<HtmlString>(attributes["integrity"].Value);
            Assert.Equal(expectedSri, htmlString.Value);
        }

        public class TestSubresourceIntegrityTagHelper : SubresourceIntegrityTagHelper
        {
            public TestSubresourceIntegrityTagHelper(
                IDistributedCache distributedCache,
                IHostingEnvironment hostingEnvironment,
                IActionContextAccessor actionContextAccessor,
                IUrlHelperFactory urlHelperFactory,
                HtmlEncoder htmlEncoder)
                : base(distributedCache, hostingEnvironment, actionContextAccessor, urlHelperFactory, htmlEncoder)
            {
            }

            protected override string UrlAttributeName => "src";

            protected override byte[] ReadAllBytes(string filePath) => Encoding.UTF8.GetBytes("Hello");
        }
    }
}
