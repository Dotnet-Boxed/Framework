namespace Boxed.AspNetCore.TagHelpers
{
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Hosting;

    /// <inheritdoc />
    [HtmlTargetElement(Attributes = HrefAttributeName + "," + SubresourceIntegrityHrefAttributeName)]
    public class HrefSubresourceIntegrityTagHelper : SubresourceIntegrityTagHelper
    {
        private const string HrefAttributeName = "href";
        private const string SubresourceIntegrityHrefAttributeName = "asp-subresource-integrity-href";

        /// <summary>
        /// Initializes a new instance of the <see cref="HrefSubresourceIntegrityTagHelper" /> class.
        /// </summary>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="hostEnvironment">The host environment.</param>
        /// <param name="actionContextAccessor">The MVC action context accessor.</param>
        /// <param name="urlHelperFactory">The URL helper factory.</param>
        public HrefSubresourceIntegrityTagHelper(
            IDistributedCache distributedCache,
            IHostEnvironment hostEnvironment,
            IActionContextAccessor actionContextAccessor,
            IUrlHelperFactory urlHelperFactory)
            : base(distributedCache, hostEnvironment, actionContextAccessor, urlHelperFactory)
        {
        }

        /// <inheritdoc />
        [HtmlAttributeName(SubresourceIntegrityHrefAttributeName)]
        public override string Source
        {
            get;
            set;
        }

        /// <inheritdoc />
        protected override string UrlAttributeName => HrefAttributeName;
    }
}
