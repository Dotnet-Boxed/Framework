namespace Boxed.AspNetCore.TagHelpers.OpenGraph
{
    /// <summary>
    /// <see cref="OpenGraphMusicReleaseType"/> extension methods.
    /// </summary>
    internal static class OpenGraphMusicReleaseTypeExtensions
    {
        /// <summary>
        /// Returns the lower-case <see cref="string" /> representation of the <see cref="OpenGraphMusicReleaseType" />.
        /// </summary>
        /// <param name="musicReleaseType">Type of the music release.</param>
        /// <returns>
        /// The lower-case <see cref="string" /> representation of the <see cref="OpenGraphMusicReleaseType" />.
        /// </returns>
        public static string ToLowercaseString(this OpenGraphMusicReleaseType musicReleaseType) =>
            musicReleaseType switch
            {
                OpenGraphMusicReleaseType.OriginalRelease => "original_release",
                OpenGraphMusicReleaseType.ReRelease => "re_release",
                OpenGraphMusicReleaseType.Anthology => "anthology",
                _ => string.Empty,
            };
    }
}
