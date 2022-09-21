namespace Boxed.AspNetCore.TagHelpers.OpenGraph;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

/// <summary>
/// This object represents a single book or publication. This is an appropriate type for E-Books, as well as
/// traditional paperback or hardback books. This object type is not part of the Open Graph standard but is used by
/// Facebook.
/// See https://developers.facebook.com/docs/reference/opengraph/object-type/books.book/.
/// </summary>
[HtmlTargetElement(
    "open-graph-books-book",
    Attributes = TitleAttributeName + "," + MainImageAttributeName + "," + ISBNAttributeName,
    TagStructure = TagStructure.WithoutEndTag)]
public class OpenGraphBooksBook : OpenGraphMetadata
{
    private const string AuthorUrlsAttributeName = "author-urls";
    private const string GenreUrlsAttributeName = "genre-urls";
    private const string InitialReleaseDateAttributeName = "initial-release-date";
    private const string ISBNAttributeName = "isbn";
    private const string LanguageAttributeName = "language";
    private const string PageCountAttributeName = "page-count";
    private const string RatingAttributeName = "rating";
    private const string ReleaseDateAttributeName = "release-date";
    private const string SampleUrlAttributeName = "sample-url";

    /// <summary>
    /// Gets or sets the URL's to the pages about the authors of the book. This URL must contain books.author meta tags <see cref="OpenGraphBooksAuthor"/>.
    /// </summary>
    [HtmlAttributeName(AuthorUrlsAttributeName)]
    public IEnumerable<string>? AuthorUrls { get; set; }

    /// <summary>
    /// Gets or sets the URL's to the pages about the genres of the book. This URL must contain books.genre meta tags <see cref="OpenGraphBooksGenre"/>.
    /// </summary>
    [HtmlAttributeName(GenreUrlsAttributeName)]
    public IEnumerable<string>? GenreUrls { get; set; }

    /// <summary>
    /// Gets or sets the initial release date of the book.
    /// </summary>
    [HtmlAttributeName(InitialReleaseDateAttributeName)]
    public DateTime? InitialReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets the books unique ISBN number.
    /// </summary>
    [HtmlAttributeName(ISBNAttributeName)]
    public string? ISBN { get; set; }

    /// <summary>
    /// Gets or sets the language of the book in the format language_TERRITORY.
    /// </summary>
    [HtmlAttributeName(LanguageAttributeName)]
    public string? Language { get; set; }

    /// <summary>
    /// Gets the namespace of this open graph type.
    /// </summary>
    public override string Namespace => "books: http://ogp.me/ns/books#";

    /// <summary>
    /// Gets or sets the number of pages in the book.
    /// </summary>
    [HtmlAttributeName(PageCountAttributeName)]
    public int? PageCount { get; set; }

    /// <summary>
    /// Gets or sets the rating of the book.
    /// </summary>
    [HtmlAttributeName(RatingAttributeName)]
    public OpenGraphRating? Rating { get; set; }

    /// <summary>
    /// Gets or sets the release date of the book.
    /// </summary>
    [HtmlAttributeName(ReleaseDateAttributeName)]
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets the URL to a sample of the book.
    /// </summary>
    [HtmlAttributeName(SampleUrlAttributeName)]
    public Uri? SampleUrl { get; set; }

    /// <summary>
    /// Gets the type of your object. Depending on the type you specify, other properties may also be required.
    /// </summary>
    public override OpenGraphType Type => OpenGraphType.BooksBook;

    /// <summary>
    /// Appends a HTML-encoded string representing this instance to the <paramref name="stringBuilder"/> containing the Open Graph meta tags.
    /// </summary>
    /// <param name="stringBuilder">The string builder.</param>
    public override void ToString(StringBuilder stringBuilder)
    {
        ArgumentNullException.ThrowIfNull(stringBuilder);

        base.ToString(stringBuilder);

        stringBuilder.AppendMetaPropertyContentIfNotNull("books:author", this.AuthorUrls);
        stringBuilder.AppendMetaPropertyContentIfNotNull("books:genre", this.GenreUrls);
        stringBuilder.AppendMetaPropertyContentIfNotNull("books:initial_release_date", this.InitialReleaseDate);
        stringBuilder.AppendMetaPropertyContentIfNotNull("books:isbn", this.ISBN);
        stringBuilder.AppendMetaPropertyContentIfNotNull("books:language", this.Language);
        stringBuilder.AppendMetaPropertyContentIfNotNull("books:page_count", this.PageCount);

        if (this.Rating is not null)
        {
            stringBuilder.AppendMetaPropertyContent("books:rating:value", this.Rating.Value);
            stringBuilder.AppendMetaPropertyContent("books:rating:scale", this.Rating.Scale);
        }

        stringBuilder.AppendMetaPropertyContentIfNotNull("books:release_date", this.ReleaseDate);
        stringBuilder.AppendMetaPropertyContentIfNotNull("books:sample", this.SampleUrl);
    }

    /// <summary>
    /// Checks that this instance is valid and throws exceptions if not valid.
    /// </summary>
    protected override void Validate()
    {
        base.Validate();

        if (this.ISBN is null)
        {
            throw new ValidationException(string.Create(CultureInfo.InvariantCulture, $"{nameof(this.ISBN)} cannot be null."));
        }
    }
}
