namespace Boxed.AspNetCore.TagHelpers.OpenGraph;

/// <summary>
/// <see cref="OpenGraphAvailability"/> extension methods.
/// </summary>
internal static class OpenGraphAvailabilityExtensions
{
    /// <summary>
    /// Returns the lower-case <see cref="string" /> representation of the <see cref="OpenGraphAvailability" />.
    /// </summary>
    /// <param name="openGraphAvailability">The open graph availability.</param>
    /// <returns>
    /// The lower-case <see cref="string" /> representation of the <see cref="OpenGraphAvailability" />.
    /// </returns>
    public static string ToLowercaseString(this OpenGraphAvailability openGraphAvailability) =>
        openGraphAvailability switch
        {
            OpenGraphAvailability.InStock => "instock",
            OpenGraphAvailability.OutOfStock => "oos",
            OpenGraphAvailability.Pending => "pending",
            _ => string.Empty,
        };
}
