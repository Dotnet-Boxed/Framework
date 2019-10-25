namespace Boxed.AspNetCore.TagHelpers
{
    using System.Text.Encodings.Web;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Caching.Distributed;

    /// <inheritdoc />
    [HtmlTargetElement(Attributes = SrcAttributeName + "," + SubresourceIntegritySrcAttributeName)]
    public class SrcSubresourceIntegrityTagHelper : SubresourceIntegrityTagHelper
    {
        private const string SrcAttributeName = "src";
        private const string SubresourceIntegritySrcAttributeName = "asp-subresource-integrity-src";

        /// <summary>
        /// Initializes a new instance of the <see cref="SrcSubresourceIntegrityTagHelper"/> class.
        /// </summary>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        /// <param name="actionContextAccessor">The MVC action context accessor.</param>
        /// <param name="urlHelperFactory">The URL helper factory.</param>
        /// <param name="htmlEncoder">The <see cref="HtmlEncoder"/>.</param>
        public SrcSubresourceIntegrityTagHelper(
            IDistributedCache distributedCache,
            IHostingEnvironment hostingEnvironment,
            IActionContextAccessor actionContextAccessor,
            IUrlHelperFactory urlHelperFactory,
            HtmlEncoder htmlEncoder)
            : base(distributedCache, hostingEnvironment, actionContextAccessor, urlHelperFactory, htmlEncoder)
        {
        }

        /// <inheritdoc />
        [HtmlAttributeName(SubresourceIntegritySrcAttributeName)]
        public override string Source
        {
            get;
            set;
        }

        /// <inheritdoc />
        protected override string UrlAttributeName => SrcAttributeName;
    }
}
