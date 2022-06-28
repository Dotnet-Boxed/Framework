namespace Boxed.AspNetCore.TagHelpers.OpenGraph;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

/// <summary>
/// This object type represents a restaurant's menu. A restaurant can have multiple menus, and each menu has multiple sections.
/// This object type is not part of the Open Graph standard but is used by Facebook.
/// See https://developers.facebook.com/docs/reference/opengraph/object-type/restaurant.menu/.
/// </summary>
[HtmlTargetElement(
    "open-graph-restaurant-menu",
    Attributes = TitleAttributeName + "," + MainImageAttributeName + "," + RestaurantUrlAttributeName,
    TagStructure = TagStructure.WithoutEndTag)]
public class OpenGraphRestaurantMenu : OpenGraphMetadata
{
    private const string RestaurantUrlAttributeName = "restaurant-url";
    private const string SectionUrlsAttributeName = "section-urls";

    /// <summary>
    /// Gets the namespace of this open graph type.
    /// </summary>
    public override string Namespace => "restaurant: http://ogp.me/ns/restaurant#";

    /// <summary>
    /// Gets or sets the URL to the page about the restaurant who wrote the menu. This URL must contain profile meta tags <see cref="OpenGraphRestaurant"/>.
    /// </summary>
    [HtmlAttributeName(RestaurantUrlAttributeName)]
    public Uri? RestaurantUrl { get; set; }

    /// <summary>
    /// Gets or sets the URL's to the pages about the menu sections. This URL must contain restaurant.section meta tags <see cref="OpenGraphRestaurantMenuSection"/>.
    /// </summary>
    [HtmlAttributeName(SectionUrlsAttributeName)]
    public IEnumerable<string>? SectionUrls { get; set; }

    /// <summary>
    /// Gets the type of your object. Depending on the type you specify, other properties may also be required.
    /// </summary>
    public override OpenGraphType Type => OpenGraphType.RestaurantMenu;

    /// <summary>
    /// Appends a HTML-encoded string representing this instance to the <paramref name="stringBuilder"/> containing the Open Graph meta tags.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    public override void ToString(StringBuilder stringBuilder)
    {
        ArgumentNullException.ThrowIfNull(stringBuilder);

        base.ToString(stringBuilder);

        stringBuilder.AppendMetaPropertyContent("restaurant:restaurant", this.RestaurantUrl);

        if (this.SectionUrls is not null)
        {
            foreach (var sectionUrl in this.SectionUrls)
            {
                stringBuilder.AppendMetaPropertyContent("restaurant:section", sectionUrl);
            }
        }
    }

    /// <summary>
    /// Checks that this instance is valid and throws exceptions if not valid.
    /// </summary>
    protected override void Validate()
    {
        base.Validate();

        if (this.RestaurantUrl is null)
        {
            throw new ValidationException(FormattableString.Invariant($"{nameof(this.RestaurantUrl)} cannot be null."));
        }
    }
}
