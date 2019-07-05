namespace Boxed.AspNetCore.TagHelpers.Test.Twitter.Cards
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Boxed.AspNetCore.TagHelpers.Test.TestData;
    using Boxed.AspNetCore.TagHelpers.Twitter;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Xunit;

    public class TwitterCardSummaryLargeImageTest
    {
        /// <summary>
        /// Renders the meta tags with no value for description. (exception thrown).
        /// </summary>
        [Fact]
        public void RenderMetaTags_NoValueForDescription_ExceptionThrown()
        {
            var expected = typeof(System.ArgumentNullException);
            Exception thrownException = null;

            var tagHelper = new TwitterCardSummaryLargeImage()
            {
                Title = TwitterCardAnswerKey.TitleValue,
                Description = string.Empty,
                SiteUsername = TwitterCardAnswerKey.SiteIdValue,
                Image = null
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
            Assert.Equal("Description", ((ArgumentException)thrownException).ParamName.ToString());
        }

        /// <summary>
        /// Renders the meta tags with no value for image rendered.
        /// </summary>
        [Fact]
        public void RenderMetaTags_NoValueForImage_Rendered()
        {
            var tagHelper = new TwitterCardSummaryLargeImage()
            {
                Title = TwitterCardAnswerKey.TitleValue,
                Description = TwitterCardAnswerKey.DescriptionValue,
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                Image = null
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
            Assert.DoesNotContain("twitter:image", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags with no value for image height or width rendered.
        /// </summary>
        [Fact]
        public void RenderMetaTags_NoValueForImageHeightOrWidth_Rendered()
        {
            var tagHelper = new TwitterCardSummaryLargeImage()
            {
                Title = TwitterCardAnswerKey.TitleValue,
                Description = TwitterCardAnswerKey.DescriptionValue,
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                Image = new TwitterImage(TwitterCardAnswerKey.ImageUrlValue)
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
            Assert.DoesNotContain("twitter:image:height", output.Content.GetContent());
            Assert.DoesNotContain("twitter:image:width", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags with no value for title. (exception thrown).
        /// </summary>
        [Fact]
        public void RenderMetaTags_NoValueForTitle_ExceptionThrown()
        {
            var expected = typeof(System.ArgumentNullException);
            Exception thrownException = null;

            var tagHelper = new TwitterCardSummaryLargeImage()
            {
                Title = string.Empty,
                Description = TwitterCardAnswerKey.DescriptionValue,
                SiteUsername = TwitterCardAnswerKey.SiteIdValue,
                Image = null
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
        /// Renders the meta tags with no value for twitter site username. (exception thrown).
        /// </summary>
        [Fact]
        public void RenderMetaTags_NoValueForTwitterSiteUsername_ExceptionThrown()
        {
            var expected = typeof(System.ArgumentNullException);
            Exception thrownException = null;

            try
            {
                var tagHelper = new TwitterCardSummaryLargeImage()
                {
                    Title = TwitterCardAnswerKey.TitleValue,
                    Description = TwitterCardAnswerKey.DescriptionValue,
                    SiteUsername = string.Empty,
                    Image = null
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
            Assert.Contains("Either twitter:site or twitter:site:id is required.", thrownException.Message.ToString());
        }

        /// <summary>
        /// Renders the meta tags with no value for twitter site username using twitter site.
        /// </summary>
        [Fact]
        public void RenderMetaTags_NoValueForTwitterSiteUsernameUsingTwitterSiteId_Rendered()
        {
            var tagHelper = new TwitterCardSummaryLargeImage()
            {
                Title = TwitterCardAnswerKey.TitleValue,
                Description = TwitterCardAnswerKey.DescriptionValue,
                SiteUsername = string.Empty,
                SiteId = TwitterCardAnswerKey.SiteIdValue,
                Image = null
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
            Assert.Contains("name=\"twitter:site:id\" content=\"" + TwitterCardAnswerKey.SiteIdValue + "\"", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags rendered correct twitter card type tag match.
        /// </summary>
        [Fact]
        public void RenderMetaTags_RenderedCorrectTwitterCardTypeTag_Match()
        {
            var tagHelper = new TwitterCardSummaryLargeImage()
            {
                Title = TwitterCardAnswerKey.TitleValue,
                Description = TwitterCardAnswerKey.DescriptionValue,
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                Image = null
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));

            var output = new TagHelperOutput(
                "div",
                new TagHelperAttributeList(),
                (cache, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent(string.Empty);
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            tagHelper.Process(context, output);
            Assert.Contains("name=\"twitter:card\" content=\"summary_large_image\"", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags rendered with optional tag creator twitter identifier creator
        /// username rendered.
        /// </summary>
        [Fact]
        public void RenderMetaTags_RenderedOptionalTagCreatorTwitterIdCreatorUsername_Rendered()
        {
            var tagHelper = new TwitterCardSummaryLargeImage()
            {
                Title = TwitterCardAnswerKey.TitleValue,
                Description = TwitterCardAnswerKey.DescriptionValue,
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                CreatorUsername = TwitterCardAnswerKey.CreatorUsernameValue,
                CreatorId = TwitterCardAnswerKey.CreatorId
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
            Assert.Contains("name=\"twitter:creator:id\" content=\"" + TwitterCardAnswerKey.CreatorId + "\"", output.Content.GetContent());
            Assert.Contains("name=\"twitter:creator\" content=\"" + TwitterCardAnswerKey.CreatorUsernameValue + "\"", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags rendered tag description rendered.
        /// </summary>
        [Fact]
        public void RenderMetaTags_RenderedTagDescription_Rendered()
        {
            var tagHelper = new TwitterCardSummaryLargeImage()
            {
                Title = TwitterCardAnswerKey.TitleValue,
                Description = TwitterCardAnswerKey.DescriptionValue,
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                Image = null
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
            Assert.Contains("name=\"twitter:description\" content=\"" + TwitterCardAnswerKey.DescriptionValue + "\"", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags for image with image URL height and width rendered.
        /// </summary>
        [Fact]
        public void RenderMetaTags_RenderedTagImageWithImageUrlHeightAndWidth_Rendered()
        {
            var tagHelper = new TwitterCardSummaryLargeImage()
            {
                Title = TwitterCardAnswerKey.TitleValue,
                Description = TwitterCardAnswerKey.DescriptionValue,
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                Image = new TwitterImage(TwitterCardAnswerKey.ImageUrlValue, TwitterCardAnswerKey.ImageWidthValue, TwitterCardAnswerKey.ImageHeightValue)
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
            Assert.Contains("name=\"twitter:image\" content=\"" + TwitterCardAnswerKey.ImageUrlValue + "\"", output.Content.GetContent());
            Assert.Contains("name=\"twitter:image:height\" content=\"" + TwitterCardAnswerKey.ImageHeightValue.ToString() + "\"", output.Content.GetContent());
            Assert.Contains("name=\"twitter:image:width\" content=\"" + TwitterCardAnswerKey.ImageWidthValue.ToString() + "\"", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags for tag title rendered.
        /// </summary>
        [Fact]
        public void RenderMetaTags_RenderedTagTitle_Rendered()
        {
            var tagHelper = new TwitterCardSummaryLargeImage()
            {
                Title = TwitterCardAnswerKey.TitleValue,
                Description = TwitterCardAnswerKey.DescriptionValue,
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                Image = null
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
            Assert.Contains("name=\"twitter:title\" content=\"" + TwitterCardAnswerKey.TitleValue + "\"", output.Content.GetContent());
        }
    }
}