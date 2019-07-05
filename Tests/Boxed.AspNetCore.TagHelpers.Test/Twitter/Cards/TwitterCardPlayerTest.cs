namespace Boxed.AspNetCore.TagHelpers.Test.Twitter.Cards
{
    using System;
    using System.Collections.Generic;
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
            var expected = typeof(System.ArgumentNullException);
            Exception thrownException = null;

            var tagHelper = new TwitterCardPlayer()
            {
                Title = TwitterCardAnswerKey.TitleValue,
                Description = TwitterCardAnswerKey.DescriptionValue,
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                Image = null,
                Player = new TwitterPlayer(TwitterCardAnswerKey.ImageUrlValue, TwitterCardAnswerKey.ImageWidthValue, TwitterCardAnswerKey.ImageHeightValue)
            };

            try
            {
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
            }
            catch (Exception e)
            {
                thrownException = e;
            }

            Assert.Equal(expected, thrownException.GetType());
            Assert.Equal("Image", ((ArgumentException)thrownException).ParamName.ToString());
        }

        /// <summary>
        /// Renders the meta tags with no value for player. (exception thrown).
        /// </summary>
        [Fact]
        public void RenderMetaTags_NoValueForPlayer_ExceptionThrown()
        {
            var expected = typeof(System.ArgumentNullException);
            Exception thrownException = null;

            var tagHelper = new TwitterCardPlayer()
            {
                Title = TwitterCardAnswerKey.TitleValue,
                Description = TwitterCardAnswerKey.DescriptionValue,
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                Image = new TwitterImage(TwitterCardAnswerKey.ImageUrlValue, TwitterCardAnswerKey.ImageWidthValue, TwitterCardAnswerKey.ImageHeightValue),
                Player = null
            };

            try
            {
                var context = new TagHelperContext(
                     new TagHelperAttributeList(),
                     new Dictionary<object, object>(),
                     Guid.NewGuid().ToString("N"));

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
            }
            catch (Exception e)
            {
                thrownException = e;
            }

            Assert.Equal(expected, thrownException.GetType());
            Assert.Equal("Player", ((ArgumentException)thrownException).ParamName.ToString());
        }

        /// <summary>
        /// Renders the meta tags with no value for twitter site username. (exception thrown).
        /// </summary>
        [Fact]
        public void RenderMetaTags_NoValueForSiteUsername_ExceptionThrown()
        {
            var expected = typeof(ArgumentNullException);
            Exception thrownException = null;

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
                    TwitterCardAnswerKey.ImageHeightValue)
            };

            try
            {
                var context = new TagHelperContext(
                     new TagHelperAttributeList(),
                     new Dictionary<object, object>(),
                     Guid.NewGuid().ToString("N"));

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
            }
            catch (Exception e)
            {
                thrownException = e;
            }

            Assert.Equal(expected, thrownException.GetType());
            Assert.Equal("SiteUsername", ((ArgumentException)thrownException).ParamName.ToString());
        }

        /// <summary>
        /// Renders the meta tags with no value for title. (exception thrown).
        /// </summary>
        [Fact]
        public void RenderMetaTags_NoValueForTitle_ExceptionThrown()
        {
            var expected = typeof(System.ArgumentNullException);
            Exception thrownException = null;

            var tagHelper = new TwitterCardPlayer()
            {
                Title = string.Empty,
                Description = TwitterCardAnswerKey.DescriptionValue,
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                Image = new TwitterImage(TwitterCardAnswerKey.ImageUrlValue, TwitterCardAnswerKey.ImageWidthValue, TwitterCardAnswerKey.ImageHeightValue),
                Player = new TwitterPlayer(TwitterCardAnswerKey.ImageUrlValue, TwitterCardAnswerKey.ImageWidthValue, TwitterCardAnswerKey.ImageHeightValue)
            };

            try
            {
                var context = new TagHelperContext(
                     new TagHelperAttributeList(),
                     new Dictionary<object, object>(),
                     Guid.NewGuid().ToString("N"));

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
            }
            catch (Exception e)
            {
                thrownException = e;
            }

            Assert.Equal(expected, thrownException.GetType());
            Assert.Equal("Title", ((ArgumentException)thrownException).ParamName.ToString());
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
                    TwitterCardAnswerKey.ImageHeightValue)
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));

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
            Assert.Contains("name=\"twitter:card\" content=\"player\"", output.Content.GetContent());
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
                    TwitterCardAnswerKey.PlayerHeightValue)
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));

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
            Assert.Contains("HTTPS:".ToLower(), output.Content.GetContent().ToLower());
            Assert.Contains("name=\"twitter:player\"", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags for the player without an HTTPS URI (exception thrown).
        /// </summary>
        [Fact]
        public void RenderMetaTags_RenderedTagPlayerWithoutHTTPS_ExceptionThrown()
        {
            var expected = typeof(ArgumentNullException);
            Exception thrownException = null;

            try
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
                        TwitterCardAnswerKey.PlayerHeightValue)
                };

                var context = new TagHelperContext(
                    new TagHelperAttributeList(),
                    new Dictionary<object, object>(),
                    Guid.NewGuid().ToString("N"));

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
            }
            catch (Exception e)
            {
                thrownException = e;
            }

            Assert.Equal(expected, thrownException.GetType());
            Assert.Equal("PlayerUrl", ((ArgumentException)thrownException).ParamName.ToString());
        }

        /// <summary>
        /// Renders the meta tags with a zero value for player height. (exception thrown).
        /// </summary>
        [Fact]
        public void RenderMetaTags_ZeroValueForPlayerHeight_ExceptionThrown()
        {
            var expected = typeof(ArgumentOutOfRangeException);
            Exception thrownException = null;

            try
            {
                var tagHelper = new TwitterCardPlayer()
                {
                    Title = TwitterCardAnswerKey.TitleValue,
                    Description = TwitterCardAnswerKey.DescriptionValue,
                    SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                    Image = null,
                    Player = new TwitterPlayer(
                        TwitterCardAnswerKey.PlayerUrlValue,
                        TwitterCardAnswerKey.PlayerWidthValue,
                        0)
                };

                var context = new TagHelperContext(
                     new TagHelperAttributeList(),
                     new Dictionary<object, object>(),
                     Guid.NewGuid().ToString("N"));

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
            }
            catch (Exception e)
            {
                thrownException = e;
            }

            Assert.Equal(expected, thrownException.GetType());
            Assert.Equal("height", ((ArgumentException)thrownException).ParamName.ToString());
        }

        /// <summary>
        /// Renders the meta tags with a zero value for player width. (exception thrown).
        /// </summary>
        [Fact]
        public void RenderMetaTags_ZeroValueForPlayerWidth_ExceptionThrown()
        {
            var expected = typeof(ArgumentOutOfRangeException);
            Exception thrownException = null;

            try
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
                        0,
                        TwitterCardAnswerKey.PlayerHeightValue)
                };

                var context = new TagHelperContext(
                     new TagHelperAttributeList(),
                     new Dictionary<object, object>(),
                     Guid.NewGuid().ToString("N"));

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
            }
            catch (Exception e)
            {
                thrownException = e;
            }

            Assert.Equal(expected, thrownException.GetType());
            Assert.Equal("width", ((ArgumentException)thrownException).ParamName.ToString());
        }
    }
}