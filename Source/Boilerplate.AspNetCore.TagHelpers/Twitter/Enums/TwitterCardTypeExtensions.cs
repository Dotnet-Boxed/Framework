namespace Boilerplate.AspNetCore.TagHelpers.Twitter
{
    /// <summary>
    /// <see cref="CardType"/> extension methods.
    /// </summary>
    internal static class TwitterCardTypeExtensions
    {
        /// <summary>
        /// Returns the Twitter specific <see cref="string"/> representation of the <see cref="CardType"/>.
        /// </summary>
        /// <param name="twitterCardType">Type of the twitter card.</param>
        /// <returns>
        /// The Twitter specific <see cref="string"/> representation of the <see cref="CardType"/>.
        /// </returns>
        public static string ToTwitterString(this CardType twitterCardType)
        {
            switch (twitterCardType)
            {
                case CardType.App:
                    return "app";

                case CardType.Player:
                    return "player";

                case CardType.Summary:
                    return "summary";

                case CardType.SummaryLargeImage:
                    return "summary_large_image";

                default:
                    return string.Empty;
            }
        }
    }
}