namespace Boxed.AspNetCore.TagHelpers.Twitter;

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

/// <summary>
/// The App Card is a great way to represent mobile applications on Twitter and to drive
/// installs. The app card is designed to allow for a name, description and icon, and also to
/// highlight attributes such as the rating and the price. This Card type is currently available
/// on the twitter.com website, as well as iOS and Android mobile clients. It is not yet
/// available on mobile web. See https://dev.twitter.com/cards/types/app.
/// </summary>
[HtmlTargetElement("twitter-card-app", Attributes = SiteUsernameAttributeName, TagStructure = TagStructure.WithoutEndTag)]
public class TwitterCardApp : TwitterCard
{
    private const string CountryAttributeName = "country";
    private const string DescriptionAttributeName = "description";
    private const string GooglePlayAttributeName = "google-play";
    private const string GooglePlayCustomUrlSchemeAttributeName = "google-play-custom-url-scheme";
    private const string IPadAttributeName = "ipad";
    private const string IPadCustomUrlSchemeAttributeName = "iphone-custom-url-scheme";
    private const string IPhoneAttributeName = "ipad";
    private const string IPhoneCustomUrlSchemeAttributeName = "iphone-custom-url-scheme";

    /// <summary>
    /// Gets or sets the country. If your application is not available in the US App Store, you
    /// must set this value to the two-letter country code for the App Store that contains your application.
    /// </summary>
    [HtmlAttributeName(CountryAttributeName)]
    public string? Country { get; set; }

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
    /// Gets or sets the numeric representation of your app ID in Google Play (.i.e. "com.android.app").
    /// </summary>
    [HtmlAttributeName(GooglePlayAttributeName)]
    public string? GooglePlay { get; set; }

    /// <summary>
    /// Gets or sets your google play app’s custom URL scheme (you must include "://" after your
    /// scheme name).
    /// </summary>
    [HtmlAttributeName(GooglePlayCustomUrlSchemeAttributeName)]
#pragma warning disable CA1056 // Uri properties should not be strings
    public string? GooglePlayCustomUrlScheme { get; set; }
#pragma warning restore CA1056 // Uri properties should not be strings

    /// <summary>
    /// Gets or sets numeric representation of your iPad app ID in the App Store (.i.e. "307234931").
    /// </summary>
    [HtmlAttributeName(IPadAttributeName)]
    public string? IPad { get; set; }

    /// <summary>
    /// Gets or sets your iPad app’s custom URL scheme (you must include "://" after your scheme name).
    /// </summary>
    [HtmlAttributeName(IPadCustomUrlSchemeAttributeName)]
#pragma warning disable CA1056 // Uri properties should not be strings
    public string? IPadCustomUrlScheme { get; set; }
#pragma warning restore CA1056 // Uri properties should not be strings

    /// <summary>
    /// Gets or sets numeric representation of your iPhone app ID in the App Store (.i.e. “307234931”).
    /// </summary>
    [HtmlAttributeName(IPhoneAttributeName)]
    public string? IPhone { get; set; }

    /// <summary>
    /// Gets or sets your iPhone app’s custom URL scheme (you must include "://" after your
    /// scheme name).
    /// </summary>
    [HtmlAttributeName(IPhoneCustomUrlSchemeAttributeName)]
#pragma warning disable CA1056 // Uri properties should not be strings
    public string? IPhoneCustomUrlScheme { get; set; }
#pragma warning restore CA1056 // Uri properties should not be strings

    /// <summary>
    /// Gets the type of the Twitter card.
    /// </summary>
    public override CardType Type => CardType.App;

    /// <summary>
    /// Appends a HTML-encoded string representing this instance to the
    /// <paramref name="stringBuilder"/> containing the Twitter card meta tags.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    public override void ToString(StringBuilder stringBuilder)
    {
        base.ToString(stringBuilder);

        stringBuilder.AppendMetaNameContentIfNotNull("twitter:description", this.Description);
        stringBuilder.AppendMetaNameContentIfNotNull("twitter:app:id:iphone", this.IPhone);
        stringBuilder.AppendMetaNameContentIfNotNull("twitter:app:url:iphone", this.IPhoneCustomUrlScheme);
        stringBuilder.AppendMetaNameContentIfNotNull("twitter:app:id:ipad", this.IPad);
        stringBuilder.AppendMetaNameContentIfNotNull("twitter:app:url:ipad", this.IPadCustomUrlScheme);
        stringBuilder.AppendMetaNameContentIfNotNull("twitter:app:id:googleplay", this.GooglePlay);
        stringBuilder.AppendMetaNameContentIfNotNull("twitter:app:url:googleplay", this.GooglePlayCustomUrlScheme);
        stringBuilder.AppendMetaNameContentIfNotNull("twitter:app:country", this.Country);
    }

    /// <summary>
    /// Checks that this instance is valid and throws exceptions if not valid.
    /// </summary>
    protected override void Validate()
    {
        base.Validate();

        if (string.IsNullOrEmpty(this.SiteUsername))
        {
            throw new ValidationException(string.Create(CultureInfo.InvariantCulture, $"{nameof(this.SiteUsername)} cannot be null or empty."));
        }

        if (string.IsNullOrEmpty(this.IPhone))
        {
            throw new ValidationException(string.Create(CultureInfo.InvariantCulture, $"{nameof(this.IPhone)} cannot be null or empty."));
        }

        if (string.IsNullOrEmpty(this.IPad))
        {
            throw new ValidationException(string.Create(CultureInfo.InvariantCulture, $"{nameof(this.IPad)} cannot be null or empty."));
        }

        if (string.IsNullOrEmpty(this.GooglePlay))
        {
            throw new ValidationException(string.Create(CultureInfo.InvariantCulture, $"{nameof(this.GooglePlay)} cannot be null or empty."));
        }
    }
}
