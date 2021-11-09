namespace Boxed.AspNetCore.TagHelpers.OpenGraph;

using System;

/// <summary>
/// <see cref="OpenGraphMedia"/> with a height and width, like a video or image.
/// </summary>
public abstract class OpenGraphSizedMedia : OpenGraphMedia
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenGraphSizedMedia"/> class.
    /// </summary>
    /// <param name="mediaUrl">The media URL.</param>
    /// <exception cref="ArgumentNullException">Thrown if mediaUrl is <c>null</c>.</exception>
    protected OpenGraphSizedMedia(Uri mediaUrl)
        : base(mediaUrl)
    {
    }

    /// <summary>
    /// Gets or sets the height of the media in pixels. This is optional.
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// Gets or sets the width of the media in pixels. This is optional.
    /// </summary>
    public int? Width { get; set; }
}
