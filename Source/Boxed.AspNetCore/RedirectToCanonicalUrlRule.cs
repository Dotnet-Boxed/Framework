namespace Boxed.AspNetCore;

using System;
using System.Diagnostics.CodeAnalysis;
using Boxed.AspNetCore.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

/// <summary>
/// To improve Search Engine Optimization SEO, there should only be a single URL for each resource. Case
/// differences and/or URL's with/without trailing slashes are treated as different URL's by search engines. This
/// rewrite rule redirects all non-canonical URL's based on the settings specified to their canonical equivalent.
/// Note: Non-canonical URL's are not generated by this site template, it is usually external sites which are
/// linking to your site but have changed the URL case or added/removed trailing slashes.
/// (See Google's comments at http://googlewebmastercentral.blogspot.co.uk/2010/04/to-slash-or-not-to-slash.html
/// and Bing's at http://blogs.bing.com/webmaster/2012/01/26/moving-content-think-301-not-relcanonical).
/// </summary>
public class RedirectToCanonicalUrlRule : IRule
{
    private const char SlashCharacter = '/';

    /// <summary>
    /// Initializes a new instance of the <see cref="RedirectToCanonicalUrlRule"/> class.
    /// </summary>
    /// <param name="options">The route options.</param>
    public RedirectToCanonicalUrlRule(IOptions<RouteOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        this.AppendTrailingSlash = options.Value.AppendTrailingSlash;
        this.LowercaseUrls = options.Value.LowercaseUrls;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RedirectToCanonicalUrlRule" /> class.
    /// </summary>
    /// <param name="appendTrailingSlash">If set to <c>true</c> append trailing slashes, otherwise strip trailing
    /// slashes.</param>
    /// <param name="lowercaseUrls">If set to <c>true</c> lower-case all URL's.</param>
    public RedirectToCanonicalUrlRule(
        bool appendTrailingSlash,
        bool lowercaseUrls)
    {
        this.AppendTrailingSlash = appendTrailingSlash;
        this.LowercaseUrls = lowercaseUrls;
    }

    /// <summary>
    /// Gets a value indicating whether to append trailing slashes.
    /// </summary>
    /// <value>
    /// <c>true</c> if appending trailing slashes; otherwise, strip trailing slashes.
    /// </value>
    public bool AppendTrailingSlash { get; }

    /// <summary>
    /// Gets a value indicating whether to lower-case all URL's.
    /// </summary>
    /// <value>
    /// <c>true</c> if lower-casing URL's; otherwise, <c>false</c>.
    /// </value>
    public bool LowercaseUrls { get; }

    /// <inheritdoc/>
    public void ApplyRule(RewriteContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (HttpMethods.IsGet(context.HttpContext.Request.Method))
        {
            if (!this.TryGetCanonicalUrl(context, out var canonicalUrl))
            {
                this.HandleNonCanonicalRequest(context, canonicalUrl);
            }
        }
    }

    /// <summary>
    /// Determines whether the specified URl is canonical and if it is not, outputs the canonical URL.
    /// </summary>
    /// <param name="context">The <see cref="RewriteContext" />.</param>
    /// <param name="canonicalUrl">The canonical URL.</param>
    /// <returns><c>true</c> if the URL is canonical, otherwise <c>false</c>.</returns>
    protected virtual bool TryGetCanonicalUrl(RewriteContext context, [NotNullWhen(false)] out Uri? canonicalUrl)
    {
        ArgumentNullException.ThrowIfNull(context);

        var isCanonical = true;

        var request = context.HttpContext.Request;
        var hasPath = request.Path.HasValue && (request.Path.Value!.Length > 1);

        // If we are not dealing with the home page. Note, the home page is a special case and it doesn't matter
        // if there is a trailing slash or not. Both will be treated as the same by search engines.
        if (hasPath)
        {
            var hasTrailingSlash = request.Path.Value![^1] == SlashCharacter;

            if (this.AppendTrailingSlash)
            {
                // Append a trailing slash to the end of the URL.
                if (!hasTrailingSlash && !this.HasAttribute<NoTrailingSlashAttribute>(context))
                {
                    request.Path = new PathString(request.Path.Value + SlashCharacter);
                    isCanonical = false;
                }
            }
            else
            {
                // Trim a trailing slash from the end of the URL.
                if (hasTrailingSlash)
                {
                    request.Path = new PathString(request.Path.Value.TrimEnd(SlashCharacter));
                    isCanonical = false;
                }
            }
        }

        if (hasPath || request.QueryString.HasValue)
        {
            if (this.LowercaseUrls && !this.HasAttribute<NoTrailingSlashAttribute>(context))
            {
                foreach (var character in request.Path.Value!)
                {
                    if (char.IsUpper(character))
                    {
#pragma warning disable CA1308 // Normalize strings to uppercase
                        request.Path = new PathString(request.Path.Value.ToLowerInvariant());
#pragma warning restore CA1308 // Normalize strings to uppercase
                        isCanonical = false;
                        break;
                    }
                }

                if (request.QueryString.HasValue && !this.HasAttribute<NoLowercaseQueryStringAttribute>(context))
                {
                    foreach (var character in request.QueryString.Value!)
                    {
                        if (char.IsUpper(character))
                        {
#pragma warning disable CA1308 // Normalize strings to uppercase
                            request.QueryString = new QueryString(request.QueryString.Value.ToLowerInvariant());
#pragma warning restore CA1308 // Normalize strings to uppercase
                            isCanonical = false;
                            break;
                        }
                    }
                }
            }
        }

        if (isCanonical)
        {
            canonicalUrl = null;
        }
        else
        {
            canonicalUrl = new Uri(UriHelper.GetEncodedUrl(request), UriKind.Absolute);
        }

        return isCanonical;
    }

    /// <summary>
    /// Handles HTTP requests for URL's that are not canonical. Performs a 301 Permanent Redirect to the canonical URL.
    /// </summary>
    /// <param name="context">The <see cref="RewriteContext" />.</param>
    /// <param name="canonicalUrl">The canonical URL.</param>
    protected virtual void HandleNonCanonicalRequest(RewriteContext context, Uri canonicalUrl)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(canonicalUrl);

        var response = context.HttpContext.Response;
        response.StatusCode = StatusCodes.Status301MovedPermanently;
        context.Result = RuleResult.EndResponse;
        response.Headers.Location = canonicalUrl.ToString();
    }

    /// <summary>
    /// Determines whether the specified action or its controller has the attribute with the specified type
    /// <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the attribute.</typeparam>
    /// <param name="context">The <see cref="RewriteContext" />.</param>
    /// <returns><c>true</c> if a <typeparamref name="T"/> attribute is specified, otherwise <c>false</c>.</returns>
    protected virtual bool HasAttribute<T>(RewriteContext context)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(context);

        var endpoint = context.HttpContext.GetEndpoint();
        return endpoint?.Metadata?.GetMetadata<T>() is not null;
    }
}
