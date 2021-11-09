namespace Boxed.AspNetCore.TagHelpers.OpenGraph;

using System;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

/// <summary>
/// This object represents a 'radio' station of a stream of audio. The audio properties should be used to identify
/// the location of the stream itself. This object type is part of the Open Graph standard.
/// See http://ogp.me/
/// See https://developers.facebook.com/docs/reference/opengraph/object-type/music.radio_station/.
/// </summary>
[HtmlTargetElement(
    "open-graph-music-radio-station",
    Attributes = TitleAttributeName + "," + MainImageAttributeName,
    TagStructure = TagStructure.WithoutEndTag)]
public class OpenGraphMusicRadioStation : OpenGraphMetadata
{
    private const string CreatorUrlAttributeName = "creator-url";

    /// <summary>
    /// Gets or sets the URL to the page about the creator of the radio station. This URL must contain profile meta tags <see cref="OpenGraphProfile"/>.
    /// </summary>
    [HtmlAttributeName(CreatorUrlAttributeName)]
    public Uri? CreatorUrl { get; set; }

    /// <summary>
    /// Gets the namespace of this open graph type.
    /// </summary>
    public override string Namespace => "music: http://ogp.me/ns/music#";

    /// <summary>
    /// Gets the type of your object. Depending on the type you specify, other properties may also be required.
    /// </summary>
    public override OpenGraphType Type => OpenGraphType.MusicRadioStation;

    /// <summary>
    /// Appends a HTML-encoded string representing this instance to the <paramref name="stringBuilder"/> containing the Open Graph meta tags.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    public override void ToString(StringBuilder stringBuilder)
    {
        base.ToString(stringBuilder);

        stringBuilder.AppendMetaPropertyContentIfNotNull("music:creator", this.CreatorUrl);
    }
}
