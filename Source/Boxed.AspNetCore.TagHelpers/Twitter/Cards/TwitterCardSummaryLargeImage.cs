namespace Boxed.AspNetCore.TagHelpers.Twitter
{
    using System;
    using System.Text;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    /// <summary>
    /// The Summary Card with Large Image features a large, full-width prominent image alongside a
    /// tweet. It is designed to give the reader a rich photo experience, and clicking on the image
    /// brings the user to your website. On twitter.com and the mobile clients, the image appears
    /// below the tweet text. See https://dev.twitter.com/cards/types/summary-large-image.
    /// </summary>
    [HtmlTargetElement("twitter-card-summary-large-image", Attributes = SiteUsernameAttributeName + "," + CreatorUsernameAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class TwitterCardSummaryLargeImage : TwitterCardSummary
    {
        /// <summary>
        /// Gets the type of the Twitter card.
        /// </summary>
        public override CardType Type => CardType.SummaryLargeImage;

        /// <summary>
        /// Appends a HTML-encoded string representing this instance to the
        /// <paramref name="stringBuilder"/> containing the Twitter card meta tags.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        public override void ToString(StringBuilder stringBuilder)
        {
            if (stringBuilder is null)
            {
                throw new ArgumentNullException(nameof(stringBuilder));
            }

            base.ToString(stringBuilder);

            stringBuilder.AppendMetaNameContentIfNotNull("twitter:site:Id", this.SiteId);
            stringBuilder.AppendMetaNameContentIfNotNull("twitter:title", this.Title);
            stringBuilder.AppendMetaNameContentIfNotNull("twitter:description", this.Description);
            stringBuilder.AppendMetaNameContentIfNotNull("twitter:creator", this.CreatorUsername);
            stringBuilder.AppendMetaNameContentIfNotNull("twitter:creator:id", this.CreatorId);

            if (this.Image is not null)
            {
                stringBuilder.AppendMetaNameContent("twitter:image", this.Image.ImageUrl);
                stringBuilder.AppendMetaNameContentIfNotNull("twitter:image:height", this.Image.Height);
                stringBuilder.AppendMetaNameContentIfNotNull("twitter:image:width", this.Image.Width);
            }
        }
    }
}
