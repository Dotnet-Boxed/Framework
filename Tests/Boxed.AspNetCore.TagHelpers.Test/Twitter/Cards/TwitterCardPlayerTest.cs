namespace Boxed.AspNetCore.TagHelpers.Test.Twitter.Cards;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading.Tasks;
using Boxed.AspNetCore.TagHelpers.Test.TestData;
using Boxed.AspNetCore.TagHelpers.Twitter;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

public class TwitterCardPlayerTest
{
    /// <summary>
    /// Renders the meta tags with no value for image. (exception thrown).
    /// </summary>
    [Fact]
    public void RenderMetaTags_NoValueForImage_ExceptionThrown()
    {
        var tagHelper = new TwitterCardPlayer()
        {
            Title = TwitterCardAnswerKey.TitleValue,
            Description = TwitterCardAnswerKey.DescriptionValue,
            SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
            Image = null,
            Player = new TwitterPlayer(TwitterCardAnswerKey.ImageUrlValue, TwitterCardAnswerKey.ImageWidthValue, TwitterCardAnswerKey.ImageHeightValue),
        };
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
        var output = new TagHelperOutput(
            "meta",
            new TagHelperAttributeList(),
            (cache, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(string.Empty);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var validationException = Assert.Throws<ValidationException>(() => tagHelper.Process(context, output));
        Assert.Contains(nameof(TwitterCardPlayer.Image), validationException.Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Renders the meta tags with no value for player. (exception thrown).
    /// </summary>
    [Fact]
    public void RenderMetaTags_NoValueForPlayer_ExceptionThrown()
    {
        var tagHelper = new TwitterCardPlayer()
        {
            Title = TwitterCardAnswerKey.TitleValue,
            Description = TwitterCardAnswerKey.DescriptionValue,
            SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
            Image = new TwitterImage(TwitterCardAnswerKey.ImageUrlValue, TwitterCardAnswerKey.ImageWidthValue, TwitterCardAnswerKey.ImageHeightValue),
            Player = null,
        };
        var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
        var output = new TagHelperOutput(
            "meta",
            new TagHelperAttributeList(),
            (cache, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(string.Empty);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var validationException = Assert.Throws<ValidationException>(() => tagHelper.Process(context, output));
        Assert.Contains(nameof(TwitterCardPlayer.Player), validationException.Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Renders the meta tags with no value for twitter site username. (exception thrown).
    /// </summary>
    [Fact]
    public void RenderMetaTags_NoValueForSiteUsername_ExceptionThrown()
    {
        var tagHelper = new TwitterCardPlayer()
        {
            Title = TwitterCardAnswerKey.TitleValue,
            Description = TwitterCardAnswerKey.DescriptionValue,
            SiteUsername = string.Empty,
            Image = new TwitterImage(
                TwitterCardAnswerKey.ImageUrlValue,
                TwitterCardAnswerKey.ImageWidthValue,
                TwitterCardAnswerKey.ImageHeightValue),
            Player = new TwitterPlayer(
                TwitterCardAnswerKey.ImageUrlValue,
                TwitterCardAnswerKey.ImageWidthValue,
                TwitterCardAnswerKey.ImageHeightValue),
        };
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
        var output = new TagHelperOutput(
            "meta",
            new TagHelperAttributeList(),
            (cache, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(string.Empty);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var validationException = Assert.Throws<ValidationException>(() => tagHelper.Process(context, output));
        Assert.Contains(nameof(TwitterCardPlayer.SiteUsername), validationException.Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Renders the meta tags with no value for title. (exception thrown).
    /// </summary>
    [Fact]
    public void RenderMetaTags_NoValueForTitle_ExceptionThrown()
    {
        var tagHelper = new TwitterCardPlayer()
        {
            Title = string.Empty,
            Description = TwitterCardAnswerKey.DescriptionValue,
            SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
            Image = new TwitterImage(TwitterCardAnswerKey.ImageUrlValue, TwitterCardAnswerKey.ImageWidthValue, TwitterCardAnswerKey.ImageHeightValue),
            Player = new TwitterPlayer(TwitterCardAnswerKey.ImageUrlValue, TwitterCardAnswerKey.ImageWidthValue, TwitterCardAnswerKey.ImageHeightValue),
        };
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
        var output = new TagHelperOutput(
            "meta",
            new TagHelperAttributeList(),
            (cache, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(string.Empty);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var validationException = Assert.Throws<ValidationException>(() => tagHelper.Process(context, output));
        Assert.Contains(nameof(TwitterCardPlayer.Title), validationException.Message, StringComparison.Ordinal);
    }

    /// <summary>
    /// Renders the meta tags with the correct twitter card type tag.
    /// </summary>
    [Fact]
    public void RenderMetaTags_RenderedCorrectTwitterCardTypeTag_Match()
    {
        var tagHelper = new TwitterCardPlayer()
        {
            Title = TwitterCardAnswerKey.TitleValue,
            Description = TwitterCardAnswerKey.DescriptionValue,
            SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
            Image = new TwitterImage(
                TwitterCardAnswerKey.ImageUrlValue,
                TwitterCardAnswerKey.ImageWidthValue,
                TwitterCardAnswerKey.ImageHeightValue),
            Player = new TwitterPlayer(
                TwitterCardAnswerKey.PlayerUrlValue,
                TwitterCardAnswerKey.ImageWidthValue,
                TwitterCardAnswerKey.ImageHeightValue),
        };
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
        var output = new TagHelperOutput(
            "meta",
            new TagHelperAttributeList(),
            (cache, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(string.Empty);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        tagHelper.Process(context, output);
        Assert.Contains(
            "name=\"twitter:card\" content=\"player\"",
            output.Content.GetContent(),
            StringComparison.Ordinal);
    }

    /// <summary>
    /// Renders the meta tags rendered tag player with an HTTPS URI.
    /// </summary>
    [Fact]
    public void RenderMetaTags_RenderedTagPlayerWithHTTPS_Rendered()
    {
        var tagHelper = new TwitterCardPlayer()
        {
            Title = TwitterCardAnswerKey.TitleValue,
            Description = TwitterCardAnswerKey.DescriptionValue,
            SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
            Image = new TwitterImage(
                TwitterCardAnswerKey.ImageUrlValue,
                TwitterCardAnswerKey.ImageWidthValue,
                TwitterCardAnswerKey.ImageHeightValue),
            Player = new TwitterPlayer(
                TwitterCardAnswerKey.PlayerUrlValue,
                TwitterCardAnswerKey.PlayerWidthValue,
                TwitterCardAnswerKey.PlayerHeightValue),
        };
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
        var output = new TagHelperOutput(
            "meta",
            new TagHelperAttributeList(),
            (cache, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(string.Empty);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        tagHelper.Process(context, output);
        Assert.Contains(
            "HTTPS:".ToUpperInvariant(),
            output.Content.GetContent().ToUpperInvariant(),
            StringComparison.Ordinal);
        Assert.Contains("name=\"twitter:player\"", output.Content.GetContent(), StringComparison.Ordinal);
    }

    /// <summary>
    /// Renders the meta tags for the player without an HTTPS URI (exception thrown).
    /// </summary>
    [Fact]
    public void RenderMetaTags_RenderedTagPlayerWithoutHTTPS_ExceptionThrown()
    {
        var tagHelper = new TwitterCardPlayer()
        {
            Title = TwitterCardAnswerKey.TitleValue,
            Description = TwitterCardAnswerKey.DescriptionValue,
            SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
            Image = new TwitterImage(
                TwitterCardAnswerKey.ImageUrlValue,
                TwitterCardAnswerKey.ImageWidthValue,
                TwitterCardAnswerKey.ImageHeightValue),
            Player = new TwitterPlayer(
                TwitterCardAnswerKey.ImageUrlValue,
                TwitterCardAnswerKey.PlayerWidthValue,
                TwitterCardAnswerKey.PlayerHeightValue),
        };
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture));
        var output = new TagHelperOutput(
            "meta",
            new TagHelperAttributeList(),
            (cache, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(string.Empty);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        var validationException = Assert.Throws<ValidationException>(() => tagHelper.Process(context, output));
        Assert.Contains(
            nameof(TwitterCardPlayer.Player.PlayerUrl),
            validationException.Message,
            StringComparison.Ordinal);
    }
}
