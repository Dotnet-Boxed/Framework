namespace Boxed.AspNetCore.TagHelpers.OpenGraph;

using System;

/// <summary>
/// Represents an actor in a video.
/// </summary>
public class OpenGraphActor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenGraphActor"/> class.
    /// </summary>
    /// <param name="actorUrl">The URL to the page about the actor. This URL must contain profile meta tags <see cref="OpenGraphProfile"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="actorUrl"/> is <c>null</c>.</exception>
    public OpenGraphActor(Uri actorUrl)
    {
        ArgumentNullException.ThrowIfNull(actorUrl);

        this.ActorUrl = actorUrl;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenGraphActor"/> class.
    /// </summary>
    /// <param name="actorUrl">The URL to the page about the actor. This URL must contain profile meta tags <see cref="OpenGraphProfile"/>.</param>
    /// <param name="role">The role the actor played.</param>
    /// <exception cref="ArgumentNullException"><paramref name="actorUrl"/> is <c>null</c>.</exception>
    public OpenGraphActor(Uri actorUrl, string role)
        : this(actorUrl) =>
        this.Role = role;

    /// <summary>
    /// Gets the URL to the page about the actor. This URL must contain profile meta tags <see cref="OpenGraphProfile"/>.
    /// </summary>
    public Uri ActorUrl { get; }

    /// <summary>
    /// Gets or sets the role the actor played.
    /// </summary>
    public string? Role { get; set; }
}
