namespace Boxed.AspNetCore.TagHelpers.OpenGraph;

using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

/// <summary>
/// This object type represents a group of product items. This object type is not part of the Open Graph standard but is used by Facebook.
/// See https://developers.facebook.com/docs/reference/opengraph/object-type/product.group/.
/// </summary>
[HtmlTargetElement(
    "open-graph-product-group",
    Attributes = TitleAttributeName + "," + MainImageAttributeName,
    TagStructure = TagStructure.WithoutEndTag)]
public class OpenGraphProductGroup : OpenGraphMetadata
{
    private const string RetailerGroupIdAttributeName = "retailer-group-id";

    /// <summary>
    /// Gets the namespace of this open graph type.
    /// </summary>
    public override string Namespace => "product: http://ogp.me/ns/product#";

    /// <summary>
    /// Gets or sets the retailer's ID for the product group.
    /// </summary>
    [HtmlAttributeName(RetailerGroupIdAttributeName)]
    public string? RetailerGroupId { get; set; }

    /// <summary>
    /// Gets the type of your object. Depending on the type you specify, other properties may also be required.
    /// </summary>
    public override OpenGraphType Type => OpenGraphType.ProductGroup;

    /// <summary>
    /// Appends a HTML-encoded string representing this instance to the <paramref name="stringBuilder"/> containing the Open Graph meta tags.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    public override void ToString(StringBuilder stringBuilder)
    {
        base.ToString(stringBuilder);

        stringBuilder.AppendMetaPropertyContentIfNotNull("product:retailer_group_id", this.RetailerGroupId);
    }
}
