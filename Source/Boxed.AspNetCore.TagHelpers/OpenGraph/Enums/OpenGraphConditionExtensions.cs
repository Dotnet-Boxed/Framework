namespace Boxed.AspNetCore.TagHelpers.OpenGraph;

/// <summary>
/// <see cref="OpenGraphCondition"/> extension methods.
/// </summary>
internal static class OpenGraphConditionExtensions
{
    /// <summary>
    /// Returns the lower-case <see cref="string" /> representation of the <see cref="OpenGraphCondition" />.
    /// </summary>
    /// <param name="openGraphCondition">The open graph condition of the item.</param>
    /// <returns>
    /// The lower-case <see cref="string" /> representation of the <see cref="OpenGraphCondition" />.
    /// </returns>
    public static string ToLowercaseString(this OpenGraphCondition openGraphCondition) =>
        openGraphCondition switch
        {
            OpenGraphCondition.New => "new",
            OpenGraphCondition.Refurbished => "refurbished",
            OpenGraphCondition.Used => "used",
            _ => string.Empty,
        };
}
