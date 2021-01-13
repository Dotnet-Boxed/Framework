namespace Boxed.AspNetCore.TagHelpers
{
    /// <summary>
    /// <see cref="ReferrerMode"/> extension methods.
    /// </summary>
    internal static class ReferrerModeExtensions
    {
        /// <summary>
        /// Returns the lower-case <see cref="string" /> representation of the <see cref="ReferrerMode" />.
        /// </summary>
        /// <param name="referrerMode">The referrer mode.</param>
        /// <returns>
        /// The lower-case <see cref="string" /> representation of the <see cref="ReferrerMode" />.
        /// </returns>
        public static string ToLowercaseString(this ReferrerMode referrerMode) =>
            referrerMode switch
            {
                ReferrerMode.None => "none",
                ReferrerMode.NoneWhenDowngrade => "none-when-downgrade",
                ReferrerMode.Origin => "origin",
                ReferrerMode.OriginWhenCrossOrigin => "origin-when-crossorigin",
                ReferrerMode.UnsafeUrl => "unsafe-url",
                _ => string.Empty,
            };
    }
}
