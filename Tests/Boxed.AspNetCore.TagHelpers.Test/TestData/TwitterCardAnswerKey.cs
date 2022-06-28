namespace Boxed.AspNetCore.TagHelpers.Test.TestData;

using System;

/// <summary>
/// Answer Key For the Twitter Card Testing.
/// </summary>
public static class TwitterCardAnswerKey
{
    /// <summary>
    /// Twitter Id for the content creator. Currently: "191059018" (Id for : @RehanSaeedUK).
    /// </summary>
    public static readonly string CreatorId = "191059018";

    /// <summary>
    /// Twitter @username for the content creator. Currently: "@RehanSaeedUK".
    /// </summary>
    public static readonly string CreatorUsernameValue = "@RehanSaeedUK";

    /// <summary>
    /// Description. Currently: "This is my Description of the page content.".
    /// </summary>
    public static readonly string DescriptionValue = "This is my Description of the page content.";

    /// <summary>
    /// Image height. Currently: 50.
    /// </summary>
    public static readonly int ImageHeightValue = 50;

    /// <summary>
    /// Image URL. Currently: "http://www.aspnetboilerplate.com/images/abp_logo.png".
    /// </summary>
    public static readonly Uri ImageUrlValue = new("http://www.aspnetboilerplate.com/images/abp_logo.png");

    /// <summary>
    /// Image width. Currently: 300.
    /// </summary>
    public static readonly int ImageWidthValue = 300;

    /// <summary>
    /// Player height. Currently: 50.
    /// </summary>
    public static readonly int PlayerHeightValue = 50;

    /// <summary>
    /// Player URL (HTTPS Req). Currently: "https://www.aspnetboilerplate.com/images/abp_logo.png".
    /// </summary>
    public static readonly Uri PlayerUrlValue = new("https://www.aspnetboilerplate.com/images/abp_logo.png");

    /// <summary>
    /// Player width. Currently: 300.
    /// </summary>
    public static readonly int PlayerWidthValue = 300;

    /// <summary>
    /// Twitter Id for the site. Currently: "2396051970" (Id for : @aspboilerplate).
    /// </summary>
    public static readonly string SiteIdValue = "2396051970";

    /// <summary>
    /// Twitter @username for the site. Currently: "@aspboilerplate".
    /// </summary>
    public static readonly string SiteUsernameValue = "@aspboilerplate";

    /// <summary>
    /// Title. Currently: "This is my title for the Twitter Card";.
    /// </summary>
    public static readonly string TitleValue = "This is my title for the Twitter Card";
}
