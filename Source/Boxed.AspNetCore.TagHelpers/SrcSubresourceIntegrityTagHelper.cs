namespace Boxed.AspNetCore.TagHelpers
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Hosting;

    /// <inheritdoc />
    [HtmlTargetElement(Attributes = SrcAttributeName + "," + SubresourceIntegritySrcAttributeName)]
    public class SrcSubresourceIntegrityTagHelper : SubresourceIntegrityTagHelper
    {
        private const string SrcAttributeName = "src";
        private const string SubresourceIntegritySrcAttributeName = "asp-subresource-integrity-src";

        /// <summary>
        /// Initializes a new instance of the <see cref="SrcSubresourceIntegrityTagHelper" /> class.
        /// </summary>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="webHostEnvironment">The web host environment.</param>
        /// <param name="actionContextAccessor">The MVC action context accessor.</param>
        /// <param name="urlHelperFactory">The URL helper factory.</param>
        public SrcSubresourceIntegrityTagHelper(
            IDistributedCache distributedCache,
            IWebHostEnvironment webHostEnvironment,
            IActionContextAccessor actionContextAccessor,
            IUrlHelperFactory urlHelperFactory)
            : base(distributedCache, webHostEnvironment, actionContextAccessor, urlHelperFactory)
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
