namespace Boilerplate.AspNetCore
{
    using Microsoft.AspNetCore.Rewrite;

    /// <summary>
    /// <see cref="RewriteOptions"/> extension methods.
    /// </summary>
    public static class RewriteOptionsExtensions
    {
#pragma warning disable SA1625 // Element documentation must not be copied and pasted
        /// <summary>
        /// Redirect a request to the canonical URL.
        /// </summary>
        /// <param name="options">The <see cref="RewriteOptions" />.</param>
        /// <returns>The <see cref="RewriteOptions" />.</returns>
        public static RewriteOptions AddRedirectToCanonicalUrl(this RewriteOptions options)
#pragma warning restore SA1625 // Element documentation must not be copied and pasted
        {
            options.Rules.Add(new RedirectToCanonicalUrlRule());
            return options;
        }
    }
}
