namespace Boxed.AspNetCore
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Net.Http.Headers;

    /// <summary>
    /// <see cref="HttpContext"/> extension methods.
    /// </summary>
    public static class HttpContextExtensions
    {
        private const string NoCache = "no-cache";
        private const string NoCacheMaxAge = "no-cache,max-age=";
        private const string NoStore = "no-store";
        private const string NoStoreNoCache = "no-store,no-cache";
        private const string PublicMaxAge = "public,max-age=";
        private const string PrivateMaxAge = "private,max-age=";

        /// <summary>
        /// Adds the Cache-Control and Pragma HTTP headers by applying the specified cache profile to the HTTP context.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="cacheProfile">The cache profile.</param>
        /// <returns>The same HTTP context.</returns>
        /// <exception cref="System.ArgumentNullException">context or cacheProfile.</exception>
        public static HttpContext ApplyCacheProfile(this HttpContext context, CacheProfile cacheProfile)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (cacheProfile == null)
            {
                throw new ArgumentNullException(nameof(cacheProfile));
            }

            var headers = context.Response.Headers;

            if (!string.IsNullOrEmpty(cacheProfile.VaryByHeader))
            {
                headers[HeaderNames.Vary] = cacheProfile.VaryByHeader;
            }

            if (cacheProfile.NoStore == true)
            {
                // Cache-control: no-store, no-cache is valid.
                if (cacheProfile.Location == ResponseCacheLocation.None)
                {
                    headers[HeaderNames.CacheControl] = NoStoreNoCache;
                    headers[HeaderNames.Pragma] = NoCache;
                }
                else
                {
                    headers[HeaderNames.CacheControl] = NoStore;
                }
            }
            else
            {
                string cacheControlValue;
                var duration = cacheProfile.Duration.GetValueOrDefault().ToString(CultureInfo.InvariantCulture);
                switch (cacheProfile.Location)
                {
                    case ResponseCacheLocation.Any:
                        cacheControlValue = PublicMaxAge + duration;
                        break;
                    case ResponseCacheLocation.Client:
                        cacheControlValue = PrivateMaxAge + duration;
                        break;
                    case ResponseCacheLocation.None:
                        cacheControlValue = NoCacheMaxAge + duration;
                        headers[HeaderNames.Pragma] = NoCache;
                        break;
                    default:
                        var exception = new NotImplementedException(FormattableString.Invariant($"Unknown {nameof(ResponseCacheLocation)}: {cacheProfile.Location}"));
                        Debug.Fail(exception.ToString());
                        throw exception;
                }

                headers[HeaderNames.CacheControl] = cacheControlValue;
            }

            return context;
        }
    }
}
