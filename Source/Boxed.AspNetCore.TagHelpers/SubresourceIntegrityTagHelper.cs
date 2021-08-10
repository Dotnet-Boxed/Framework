namespace Boxed.AspNetCore.TagHelpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Caching.Distributed;

    /// <summary>
    /// Adds Sub-resource Integrity (SRI) to a script tag. Sub-resource Integrity (SRI) is a security feature that
    /// enables browsers to verify that files they fetch (for example, from a CDN) are delivered without unexpected
    /// manipulation. It works by allowing you to provide a cryptographic hash that a fetched file must match.
    /// See https://developer.mozilla.org/en-US/docs/Web/Security/Subresource_Integrity and https://www.w3.org/TR/SRI/.
    /// The tag helper works by taking the URL from the script tag and checking the <see cref="IDistributedCache"/> for
    /// a matching SRI value. If one is found, it is used otherwise the file from the alternative source is read and
    /// the SRI is calculated and stored in the <see cref="IDistributedCache"/>.
    /// </summary>
    public abstract class SubresourceIntegrityTagHelper : TagHelper
    {
        private const string SubresourceIntegrityHashAlgorithmsAttributeName = "asp-subresource-integrity-hash-algorithms";
        private const string CrossOriginAttributeName = "crossorigin";
        private const string IntegrityAttributeName = "integrity";

        private readonly IDistributedCache distributedCache;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IUrlHelper urlHelper;
        private readonly HtmlEncoder htmlEncoder;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubresourceIntegrityTagHelper"/> class.
        /// </summary>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="webHostEnvironment">The web host environment.</param>
        /// <param name="actionContextAccessor">The MVC action context accessor.</param>
        /// <param name="urlHelperFactory">The URL helper factory.</param>
        /// <param name="htmlEncoder">The <see cref="HtmlEncoder"/>.</param>
        protected SubresourceIntegrityTagHelper(
            IDistributedCache distributedCache,
            IWebHostEnvironment webHostEnvironment,
            IActionContextAccessor actionContextAccessor,
            IUrlHelperFactory urlHelperFactory,
            HtmlEncoder htmlEncoder)
        {
            this.distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
            this.webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            this.htmlEncoder = htmlEncoder ?? throw new ArgumentNullException(nameof(htmlEncoder));

            if (actionContextAccessor is null)
            {
                throw new ArgumentNullException(nameof(actionContextAccessor));
            }

            if (urlHelperFactory is null)
            {
                throw new ArgumentNullException(nameof(urlHelperFactory));
            }

            this.urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        /// <summary>
        /// Gets or sets the one or more hashing algorithms to be used. This is a required property.
        /// </summary>
        [HtmlAttributeName(SubresourceIntegrityHashAlgorithmsAttributeName)]
        public SubresourceIntegrityHashAlgorithm HashAlgorithms { get; set; } =
            SubresourceIntegrityHashAlgorithm.SHA512;

        /// <summary>
        /// Gets or sets the source file.
        /// </summary>
        /// <value>
        /// The source file.
        /// </value>
        public virtual string? Source { get; set; }

        /// <summary>
        /// Gets the name of the attribute which contains the URL to the resource.
        /// </summary>
        /// <value>
        /// The name of the attribute containing the URL to the resource.
        /// </value>
#pragma warning disable CA1056 // Uri properties should not be strings
        protected abstract string UrlAttributeName { get; }
#pragma warning restore CA1056 // Uri properties should not be strings

        /// <summary>
        /// Asynchronously executes the <see cref="TagHelper" /> with the given context and output.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        /// <returns>A task representing the operation.</returns>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output is null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var url = this.GetEncodedStringValue(output.Attributes[this.UrlAttributeName].Value);

            if (!string.IsNullOrWhiteSpace(url) && !string.IsNullOrWhiteSpace(this.Source))
            {
                var sri = await this.GetCachedSriAsync(url).ConfigureAwait(false);
                if (sri is null)
                {
                    sri = this.GetSubresourceIntegrityFromContentFile(this.Source, this.HashAlgorithms);
                    await this.SetCachedSriAsync(url, sri).ConfigureAwait(false);
                }

                output.Attributes.SetAttribute(CrossOriginAttributeName, "anonymous");
                output.Attributes.SetAttribute(IntegrityAttributeName, new HtmlString(sri));
            }
        }

        /// <summary>
        /// Gets the key used to retrieve the SRI value from the distributed cache.
        /// </summary>
        /// <param name="url">The URL to the resource.</param>
        /// <returns>A key value for the URL.</returns>
#pragma warning disable CA1054 // Uri parameters should not be strings
        protected virtual string GetSriKey(string url) => "SRI:" + url;
#pragma warning restore CA1054 // Uri parameters should not be strings

        /// <summary>
        /// Reads all bytes from the file with the specified path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The files bytes.</returns>
        protected virtual byte[] ReadAllBytes(string filePath) => File.ReadAllBytes(filePath);

        private static IEnumerable<SubresourceIntegrityHashAlgorithm> GetFlags(SubresourceIntegrityHashAlgorithm enumeration)
        {
            foreach (var value in (IEnumerable<SubresourceIntegrityHashAlgorithm>)Enum.GetValues(enumeration.GetType()))
            {
                if (enumeration.HasFlag(value))
                {
                    yield return value;
                }
            }
        }

        private static HashAlgorithm CreateHashAlgorithm<T>()
            where T : HashAlgorithm
        {
            var type = typeof(T);
            if (type == typeof(SHA256))
            {
                return SHA256.Create();
            }
            else if (type == typeof(SHA384))
            {
                return SHA384.Create();
            }
            else if (type == typeof(SHA512))
            {
                return SHA512.Create();
            }
            else
            {
                throw new InvalidOperationException(
                    FormattableString.Invariant($"Hash algorithm not recognized. Type<{type}>."));
            }
        }

        private static string GetHash<T>(byte[] bytes)
            where T : HashAlgorithm
        {
            using (var hashAlgorithm = CreateHashAlgorithm<T>())
            {
                var hashedBytes = hashAlgorithm.ComputeHash(bytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private static void AppendSri(
            StringBuilder stringBuilder,
            byte[] bytes,
            SubresourceIntegrityHashAlgorithm hashAlgorithm)
        {
            switch (hashAlgorithm)
            {
                case SubresourceIntegrityHashAlgorithm.SHA256:
                    stringBuilder.Append("sha256-");
                    stringBuilder.Append(GetHash<SHA256>(bytes));
                    break;
                case SubresourceIntegrityHashAlgorithm.SHA384:
                    stringBuilder.Append("sha384-");
                    stringBuilder.Append(GetHash<SHA384>(bytes));
                    break;
                case SubresourceIntegrityHashAlgorithm.SHA512:
                    stringBuilder.Append("sha512-");
                    stringBuilder.Append(GetHash<SHA512>(bytes));
                    break;
                default:
                    throw new ArgumentException(
                        FormattableString.Invariant($"Hash algorithm not recognized. HashAlgorithm<{hashAlgorithm}>."),
                        nameof(hashAlgorithm));
            }
        }

        private static string GetSpaceDelimetedSri(byte[] bytes, SubresourceIntegrityHashAlgorithm hashAlgorithms)
        {
            var stringBuilder = new StringBuilder();
            foreach (var hashAlgorithm in GetFlags(hashAlgorithms))
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(' ');
                }

                AppendSri(stringBuilder, bytes, hashAlgorithm);
            }

            return stringBuilder.ToString();
        }

        private Task<string> GetCachedSriAsync(string url)
        {
            var key = this.GetSriKey(url);
            return this.distributedCache.GetStringAsync(key);
        }

        private Task SetCachedSriAsync(string url, string value)
        {
            var key = this.GetSriKey(url);
            return this.distributedCache.SetStringAsync(key, value);
        }

        private string GetSubresourceIntegrityFromContentFile(
            string contentPath,
            SubresourceIntegrityHashAlgorithm hashAlgorithms)
        {
            var filePath = Path.Combine(
                this.webHostEnvironment.ContentRootPath,
                this.urlHelper.Content(contentPath)?.TrimStart('/') ?? string.Empty);
            var bytes = this.ReadAllBytes(filePath);
            return GetSpaceDelimetedSri(bytes, hashAlgorithms);
        }

        private string GetEncodedStringValue(object attributeValue)
        {
            if (attributeValue is string stringValue)
            {
                var encodedStringValue = this.htmlEncoder.Encode(stringValue);
                return encodedStringValue;
            }
            else
            {
                if (attributeValue is IHtmlContent htmlContent)
                {
                    if (htmlContent is HtmlString htmlString)
                    {
                        // No need for a StringWriter in this case.
                        stringValue = htmlString.ToString();
                    }
                    else
                    {
                        using (var writer = new StringWriter())
                        {
                            htmlContent.WriteTo(writer, this.htmlEncoder);
                            stringValue = writer.ToString();
                        }
                    }

                    return stringValue;
                }
            }

            return attributeValue.ToString()!;
        }
    }
}
