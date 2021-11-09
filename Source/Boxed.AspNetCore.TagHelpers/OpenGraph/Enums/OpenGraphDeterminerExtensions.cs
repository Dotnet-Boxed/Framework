namespace Boxed.AspNetCore.TagHelpers.OpenGraph;

/// <summary>
/// <see cref="OpenGraphDeterminer"/> extension methods.
/// </summary>
internal static class OpenGraphDeterminerExtensions
{
    /// <summary>
    /// Returns the lower-case <see cref="string" /> representation of the <see cref="OpenGraphDeterminer" />.
    /// </summary>
    /// <param name="determiner">The determiner word to display before the title.</param>
    /// <returns>
    /// The lower-case <see cref="string" /> representation of the <see cref="OpenGraphDeterminer" />.
    /// </returns>
    public static string ToLowercaseString(this OpenGraphDeterminer determiner) =>
        determiner switch
        {
            OpenGraphDeterminer.A => "a",
            OpenGraphDeterminer.An => "an",
            OpenGraphDeterminer.Auto => "auto",
            OpenGraphDeterminer.The => "the",
            OpenGraphDeterminer.Blank => string.Empty,
            _ => string.Empty,
        };
}
