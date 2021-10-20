namespace Boxed.AspNetCore.TagHelpers.OpenGraph
{
    using System;
    using System.Text;

    /// <summary>
    /// An media which should represent your object within the graph.
    /// </summary>
    public abstract class OpenGraphMedia
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGraphMedia"/> class.
        /// </summary>
        /// <param name="mediaUrl">The media URL.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="mediaUrl"/> is <c>null</c>.</exception>
        protected OpenGraphMedia(Uri mediaUrl)
        {
            ArgumentNullException.ThrowIfNull(mediaUrl);

            if (string.Equals(mediaUrl.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
            {
                this.Url = new UriBuilder(mediaUrl)
                {
                    Port = -1,
                    Scheme = Uri.UriSchemeHttps,
                }.Uri;
                this.UrlSecure = mediaUrl;
            }
            else
            {
                this.Url = mediaUrl;
            }
        }

        /// <summary>
        /// Gets or sets the MIME type of the media e.g. media/jpeg. This is optional if your media URL ends with a file extension,
        /// otherwise it is recommended.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Gets the absolute HTTP media URL which should represent your object within the graph.
        /// </summary>
        public Uri Url { get; }

        /// <summary>
        /// Gets the absolute HTTPS media URL which should represent your object within the graph.
        /// </summary>
        public Uri? UrlSecure { get; }

        /// <summary>
        /// Appends a HTML-encoded string representing this instance to the <paramref name="stringBuilder"/> containing the Open Graph meta tags.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        public abstract void ToString(StringBuilder stringBuilder);
    }
}
