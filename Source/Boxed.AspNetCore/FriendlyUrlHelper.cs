namespace Boxed.AspNetCore
{
    using System;
    using System.Text;

    /// <summary>
    /// Helps convert <see cref="string"/> title text to URL friendly <see cref="string"/>'s that can safely be
    /// displayed in a URL.
    /// </summary>
    public static class FriendlyUrlHelper
    {
        /// <summary>
        /// Converts the specified title so that it is more human and search engine readable e.g.
        /// http://example.com/product/123/this-is-the-seo-and-human-friendly-product-title. Note that the ID of the
        /// product is still included in the URL, to avoid having to deal with two titles with the same name. Search
        /// Engine Optimization (SEO) friendly URL's gives your site a boost in search rankings by including keywords
        /// in your URL's. They are also easier to read by users and can give them an indication of what they are
        /// clicking on when they look at a URL. Refer to the code example below to see how this helper can be used.
        /// Go to definition on this method to see a code example. To learn more about friendly URL's see
        /// https://moz.com/blog/15-seo-best-practices-for-structuring-urls.
        /// To learn more about how this was implemented see
        /// http://stackoverflow.com/questions/25259/how-does-stack-overflow-generate-its-seo-friendly-urls/25486.
        /// </summary>
        /// <param name="title">The title of the URL.</param>
        /// <param name="remapToAscii">if set to <c>true</c>, remaps special UTF8 characters like 'è' to their ASCII
        /// equivalent 'e'. All modern browsers except Internet Explorer display the 'è' correctly. Older browsers and
        /// Internet Explorer percent encode these international characters so they are displayed as'%C3%A8'. What you
        /// set this to depends on whether your target users are English speakers or not.</param>
        /// <param name="maxlength">The maximum allowed length of the title.</param>
        /// <returns>The SEO and human friendly title.</returns>
        /// <code>
        /// <c>
        /// [HttpGet("product/{id}/{title}", Name = "GetDetails")]
        /// public IActionResult Product(int id, string title)
        /// {
        ///     // Get the product as indicated by the ID from a database or some repository.
        ///     var product = ProductRepository.Find(id);
        ///
        ///     // If a product with the specified ID was not found, return a 404 Not Found response.
        ///     if (product is null)
        ///     {
        ///         return this.HttpNotFound();
        ///     }
        ///
        ///     // Get the actual friendly version of the title.
        ///     var friendlyTitle = FriendlyUrlHelper.GetFriendlyTitle(product.Title);
        ///
        ///     // Compare the title with the friendly title.
        ///     if (!string.Equals(friendlyTitle, title, StringComparison.Ordinal))
        ///     {
        ///         // If the title is null, empty or does not match the friendly title, return a 301 Permanent
        ///         // Redirect to the correct friendly URL.
        ///         return this.RedirectToRoutePermanent("GetProduct", new { id = id, title = friendlyTitle });
        ///     }
        ///
        ///     // The URL the client has browsed to is correct, show them the view containing the product.
        ///     return this.View(product);
        /// }
        /// </c>
        /// .
        /// </code>
        public static string GetFriendlyTitle(string title, bool remapToAscii = false, int maxlength = 80)
        {
            if (title is null)
            {
                return string.Empty;
            }

            var length = title.Length;
            var prevdash = false;
            var stringBuilder = new StringBuilder(length);
            char c;

            for (var i = 0; i < length; ++i)
            {
                c = title[i];
                if (c is (>= 'a' and <= 'z') or (>= '0' and <= '9'))
                {
                    stringBuilder.Append(c);
                    prevdash = false;
                }
                else if (c is >= 'A' and <= 'Z')
                {
                    // tricky way to convert to lower-case
                    stringBuilder.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c is ' ' or ',' or '.' or '/' or '\\' or '-' or '_' or '=')
                {
                    if (!prevdash && (stringBuilder.Length > 0))
                    {
                        stringBuilder.Append('-');
                        prevdash = true;
                    }
                }
                else if (c >= 128)
                {
                    var previousLength = stringBuilder.Length;

                    if (remapToAscii)
                    {
                        stringBuilder.Append(RemapInternationalCharToAscii(c));
                    }
                    else
                    {
                        stringBuilder.Append(c);
                    }

                    if (previousLength != stringBuilder.Length)
                    {
                        prevdash = false;
                    }
                }

                if (stringBuilder.Length >= maxlength)
                {
                    break;
                }
            }

            if (prevdash || stringBuilder.Length > maxlength)
            {
                return stringBuilder.ToString()[..(stringBuilder.Length - 1)];
            }
            else
            {
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Remaps the international character to their equivalent ASCII characters. See
        /// http://meta.stackexchange.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696.
        /// </summary>
        /// <param name="character">The character to remap to its ASCII equivalent.</param>
        /// <returns>The remapped character.</returns>
        private static string RemapInternationalCharToAscii(char character)
        {
#pragma warning disable CA1308 // Normalize strings to uppercase
            var s = new string(character, 1).ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase
            if ("àåáâäãåąā".Contains(s, StringComparison.Ordinal))
            {
                return "a";
            }
            else if ("èéêěëę".Contains(s, StringComparison.Ordinal))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s, StringComparison.Ordinal))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s, StringComparison.Ordinal))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s, StringComparison.Ordinal))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s, StringComparison.Ordinal))
            {
                return "c";
            }
            else if ("żźž".Contains(s, StringComparison.Ordinal))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s, StringComparison.Ordinal))
            {
                return "s";
            }
            else if ("ñń".Contains(s, StringComparison.Ordinal))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s, StringComparison.Ordinal))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s, StringComparison.Ordinal))
            {
                return "g";
            }
            else if ("ŕř".Contains(s, StringComparison.Ordinal))
            {
                return "r";
            }
            else if ("ĺľł".Contains(s, StringComparison.Ordinal))
            {
                return "l";
            }
            else if ("úů".Contains(s, StringComparison.Ordinal))
            {
                return "u";
            }
            else if ("đď".Contains(s, StringComparison.Ordinal))
            {
                return "d";
            }
            else if (character == 'ť')
            {
                return "t";
            }
            else if (character == 'ž')
            {
                return "z";
            }
            else if (character == 'ß')
            {
                return "ss";
            }
            else if (character == 'Þ')
            {
                return "th";
            }
            else if (character == 'ĥ')
            {
                return "h";
            }
            else if (character == 'ĵ')
            {
                return "j";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
