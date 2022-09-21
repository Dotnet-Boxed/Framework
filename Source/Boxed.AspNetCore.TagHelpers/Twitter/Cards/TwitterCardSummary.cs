namespace Boxed.AspNetCore.TagHelpers.Twitter;

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

/// <summary>
/// The Summary Card can be used for many kinds of web content, from blog posts and news
/// articles, to products and restaurants. It is designed to give the reader a preview of the
/// content before clicking through to your website. See https://dev.twitter.com/cards/types/summary.
/// </summary>
[HtmlTargetElement("twitter-card-summary", Attributes = SiteUsernameAttributeName + "," + CreatorIdAttributeName, TagStructure = TagStructure.WithoutEndTag)]
public class TwitterCardSummary : TwitterCard
{
    private const string DescriptionAttributeName = "description";

    private const string ImageAttributeName = "image";

    private const string TitleAttributeName = "title";

    /// <summary>
    /// Gets or sets the description that concisely summarizes the content of the page, as
    /// appropriate for presentation within a Tweet. Do not re-use the title text as the
    /// description, or use this field to describe the general services provided by the website.
    /// Description text will be truncated at the word to 200 characters. If you are using
    /// Facebook's Open Graph og:description, do not use this unless you want a different description.
    /// </summary>
    [HtmlAttributeName(DescriptionAttributeName)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the URL to a unique image representing the content of the page. Do not use a
    /// generic image such as your website logo, author photo, or other image that spans multiple
    /// pages. The image must be a minimum size of 120px by 120px and must be less than 1MB in
    /// file size. For an expanded tweet and its detail page, the image will be cropped to a 4:3
    /// aspect ratio and resized to be displayed at 120px by 90px. The image will also be cropped
    /// and resized to 120px by 120px for use in embedded tweets. If you are using Facebook's
    /// Open Graph og:image, you do not have to use this unless you want a different image but it
    /// is recommended to get supply a smaller image that fits Twitters needs.
    /// </summary>
    [HtmlAttributeName(ImageAttributeName)]
    public TwitterImage? Image { get; set; }

    /// <summary>
    /// Gets or sets the title of the summary. Title should be concise and will be truncated at
    /// 70 characters. If you are using Facebook's Open Graph og:title, do not use this unless
    /// you want a different title.
    /// </summary>
    [HtmlAttributeName(TitleAttributeName)]
    public string? Title { get; set; }

    /// <summary>
    /// Gets the type of the Twitter card.
    /// </summary>
    public override CardType Type => CardType.Summary;

    /// <summary>
    /// Appends a HTML-encoded string representing this instance to the
    /// <paramref name="stringBuilder"/> containing the Twitter card meta tags.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    public override void ToString(StringBuilder stringBuilder)
    {
        ArgumentNullException.ThrowIfNull(stringBuilder);

        base.ToString(stringBuilder);

        stringBuilder.AppendMetaNameContentIfNotNull("twitter:site:id", this.SiteId);
        stringBuilder.AppendMetaNameContentIfNotNull("twitter:title", this.Title);
        stringBuilder.AppendMetaNameContentIfNotNull("twitter:description", this.Description);
        stringBuilder.AppendMetaNameContentIfNotNull("twitter:creator", this.CreatorUsername);
        stringBuilder.AppendMetaNameContentIfNotNull("twitter:creator:id", this.CreatorId);

        if (this.Image is not null)
        {
            stringBuilder.AppendMetaNameContent("twitter:image", this.Image.ImageUrl);

            if (this.Image.Height.HasValue)
            {
                stringBuilder.AppendMetaNameContentIfNotNull("twitter:image:height", this.Image.Height);
            }

            if (this.Image.Width.HasValue)
            {
                stringBuilder.AppendMetaNameContentIfNotNull("twitter:image:width", this.Image.Width);
            }
        }
    }

    /// <summary>
    /// Checks that this instance is valid and throws exceptions if not valid.
    /// </summary>
    protected override void Validate()
    {
        base.Validate();

        if (string.IsNullOrEmpty(this.Title))
        {
            throw new ValidationException(string.Create(CultureInfo.InvariantCulture, $"{nameof(this.Title)} cannot be null or empty."));
        }

        if (string.IsNullOrEmpty(this.Description))
        {
            throw new ValidationException(string.Create(CultureInfo.InvariantCulture, $"{nameof(this.Description)} cannot be null or empty."));
        }

        if (string.IsNullOrEmpty(this.SiteId) && string.IsNullOrEmpty(this.SiteUsername))
        {
            throw new ValidationException(string.Create(CultureInfo.InvariantCulture, $"{nameof(this.SiteUsername)} cannot be null or empty and either twitter:site or twitter:site:id is required."));
        }
    }
}
