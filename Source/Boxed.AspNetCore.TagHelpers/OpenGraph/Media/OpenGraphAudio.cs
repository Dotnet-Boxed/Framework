namespace Boxed.AspNetCore.TagHelpers.OpenGraph;

using System;
using System.Text;

/// <summary>
/// A audio file that complements this object.
/// </summary>
public class OpenGraphAudio : OpenGraphMedia
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenGraphAudio"/> class.
    /// </summary>
    /// <param name="audioUrl">The audio URL.</param>
    /// <exception cref="ArgumentNullException">Thrown if audioUrl is <c>null</c>.</exception>
    public OpenGraphAudio(Uri audioUrl)
        : base(audioUrl)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenGraphAudio"/> class.
    /// </summary>
    /// <param name="mediaUrl">The media URL.</param>
    /// <param name="type">The MIME type of the media e.g. media/ogg. This is optional if your media URL ends with
    /// a file extension, otherwise it is recommended.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="mediaUrl"/> is <c>null</c>.</exception>
    public OpenGraphAudio(Uri mediaUrl, string type)
        : this(mediaUrl) =>
        this.Type = type;

    /// <summary>
    /// Appends a HTML-encoded string representing this instance to the <paramref name="stringBuilder"/> containing
    /// the Open Graph meta tags.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    public override void ToString(StringBuilder stringBuilder)
    {
        ArgumentNullException.ThrowIfNull(stringBuilder);

        stringBuilder.AppendMetaPropertyContent("og:audio", this.Url);
        stringBuilder.AppendMetaPropertyContentIfNotNull("og:audio:secure_url", this.UrlSecure);
        stringBuilder.AppendMetaPropertyContentIfNotNull("og:audio:type", this.Type);
    }
}
