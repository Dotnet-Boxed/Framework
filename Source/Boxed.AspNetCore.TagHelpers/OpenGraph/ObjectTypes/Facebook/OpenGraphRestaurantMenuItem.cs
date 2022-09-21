namespace Boxed.AspNetCore.TagHelpers.OpenGraph;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

/// <summary>
/// This object type represents a single item on a restaurant's menu. Every item belongs within a menu section.
/// This object type is not part of the Open Graph standard but is used by Facebook.
/// See https://developers.facebook.com/docs/reference/opengraph/object-type/restaurant.menu_item/.
/// </summary>
[HtmlTargetElement(
    "open-graph-restaurant-menu-item",
    Attributes = TitleAttributeName + "," + MainImageAttributeName + "," + SectionUrlAttributeName,
    TagStructure = TagStructure.WithoutEndTag)]
public class OpenGraphRestaurantMenuItem : OpenGraphMetadata
{
    private const string SectionUrlAttributeName = "section-url";
    private const string VariationsAttributeName = "variations";

    /// <summary>
    /// Gets the namespace of this open graph type.
    /// </summary>
    public override string Namespace => "restaurant: http://ogp.me/ns/restaurant#";

    /// <summary>
    /// Gets or sets the URL to the page about the section this menu item is from. This URL must contain profile meta tags <see cref="OpenGraphRestaurantMenuSection"/>.
    /// </summary>
    [HtmlAttributeName(SectionUrlAttributeName)]
    public Uri? SectionUrl { get; set; }

    /// <summary>
    /// Gets the type of your object. Depending on the type you specify, other properties may also be required.
    /// </summary>
    public override OpenGraphType Type => OpenGraphType.RestaurantMenuItem;

    /// <summary>
    /// Gets or sets the variations of this menu item.
    /// </summary>
    [HtmlAttributeName(VariationsAttributeName)]
    public IEnumerable<OpenGraphMenuItemVariation>? Variations { get; set; }

    /// <summary>
    /// Appends a HTML-encoded string representing this instance to the <paramref name="stringBuilder"/> containing the Open Graph meta tags.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    public override void ToString(StringBuilder stringBuilder)
    {
        ArgumentNullException.ThrowIfNull(stringBuilder);

        base.ToString(stringBuilder);

        stringBuilder.AppendMetaPropertyContent("restaurant:section", this.SectionUrl);

        if (this.Variations is not null)
        {
            foreach (var variation in this.Variations)
            {
                stringBuilder.AppendMetaPropertyContent("restaurant:variation:name", variation.Name);
                stringBuilder.AppendMetaPropertyContent("restaurant:variation:price:amount", variation.Price.Amount);
                stringBuilder.AppendMetaPropertyContent("restaurant:variation:price:currency", variation.Price.Currency);
            }
        }
    }

    /// <summary>
    /// Checks that this instance is valid and throws exceptions if not valid.
    /// </summary>
    protected override void Validate()
    {
        base.Validate();

        if (this.SectionUrl is null)
        {
            throw new ValidationException(string.Create(CultureInfo.InvariantCulture, $"{nameof(this.SectionUrl)} cannot be null."));
        }
    }
}
