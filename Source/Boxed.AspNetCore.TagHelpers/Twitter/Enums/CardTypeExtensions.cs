namespace Boxed.AspNetCore.TagHelpers.Twitter;

/// <summary>
/// <see cref="CardType"/> extension methods.
/// </summary>
internal static class CardTypeExtensions
{
    /// <summary>
    /// Returns the Twitter specific <see cref="string"/> representation of the <see cref="CardType"/>.
    /// </summary>
    /// <param name="twitterCardType">Type of the twitter card.</param>
    /// <returns>
    /// The Twitter specific <see cref="string"/> representation of the <see cref="CardType"/>.
    /// </returns>
    public static string ToTwitterString(this CardType twitterCardType) =>
        twitterCardType switch
        {
            CardType.App => "app",
            CardType.Player => "player",
            CardType.Summary => "summary",
            CardType.SummaryLargeImage => "summary_large_image",
            _ => string.Empty,
        };
}
