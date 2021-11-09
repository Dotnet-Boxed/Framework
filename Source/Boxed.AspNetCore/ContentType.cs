namespace Boxed.AspNetCore;

/// <summary>
/// A list of internet media types, which are a standard identifier used on the Internet to indicate the type of
/// data that a file contains. Web browsers use them to determine how to display, output or handle files and search
/// engines use them to classify data files on the web.
/// </summary>
public static class ContentType
{
    /// <summary>Atom feeds.</summary>
    public const string Atom = "application/atom+xml";

    /// <summary>HTML; Defined in RFC 2854.</summary>
    public const string Html = "text/html";

    /// <summary>Form URL Encoded.</summary>
    public const string FormUrlEncoded = "application/x-www-form-urlencoded";

    /// <summary>GIF image; Defined in RFC 2045 and RFC 2046.</summary>
    public const string Gif = "image/gif";

    /// <summary>JPEG JFIF image; Defined in RFC 2045 and RFC 2046.</summary>
    public const string Jpg = "image/jpeg";

    /// <summary>JavaScript Object Notation JSON; Defined in RFC 4627.</summary>
    public const string Json = "application/json";

    /// <summary>JSON Patch; Defined at http://jsonpatch.com/.</summary>
    public const string JsonPatch = "application/json-patch+json";

    /// <summary>Web App Manifest.</summary>
    public const string Manifest = "application/manifest+json";

    /// <summary>Multi-part form daata; Defined in RFC 2388.</summary>
    public const string MultipartFormData = "multipart/form-data";

    /// <summary>Portable Network Graphics; Registered,[8] Defined in RFC 2083.</summary>
    public const string Png = "image/png";

    /// <summary>Problem Details JavaScript Object Notation (JSON); Defined at https://tools.ietf.org/html/rfc7807.</summary>
    public const string ProblemJson = "application/problem+json";

    /// <summary>Problem Details Extensible Markup Language (XML); Defined at https://tools.ietf.org/html/rfc7807.</summary>
    public const string ProblemXml = "application/problem+xml";

    /// <summary>REST'ful JavaScript Object Notation (JSON); Defined at http://restfuljson.org/.</summary>
    public const string RestfulJson = "application/vnd.restful+json";

    /// <summary>Rich Site Summary; Defined by Harvard Law.</summary>
    public const string Rss = "application/rss+xml";

    /// <summary>Textual data; Defined in RFC 2046 and RFC 3676.</summary>
    public const string Text = "text/plain";

    /// <summary>Extensible Markup Language; Defined in RFC 3023.</summary>
    public const string Xml = "application/xml";

    /// <summary>Compressed ZIP.</summary>
    public const string Zip = "application/zip";
}
