namespace Boilerplate.AspNetCore.TagHelpers.Test.Twitter.Cards
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Boilerplate.AspNetCore.TagHelpers.Twitter;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using TestData;
    using Xunit;

    /// <summary>
    /// TwitterCard Summary Large Image Tests
    /// </summary>
    public class TwitterCard_SummaryLargeImage_Test
    {
        /// <summary>
        /// Renders the meta tags with no value for description. (exception thrown)
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_NoValueForDescription_ExceptionThrown")]
        public void RenderMetaTags_NoValueForDescription_ExceptionThrown()
        {
            var expected = typeof(System.ArgumentNullException);
            Exception throwenException = null;

            TwitterCardSummaryLargeImage myTagHelper = new TwitterCardSummaryLargeImage()
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
                     Guid.NewGuid().ToString("N")
                );

                var output = new TagHelperOutput(
                    "meta",
                    new TagHelperAttributeList(),
                        (cache, encoder) =>
                        {
                            var tagHelperContent = new DefaultTagHelperContent();
                            tagHelperContent.SetContent(string.Empty);
                            return Task.FromResult<TagHelperContent>(tagHelperContent);
                        }
                );

                myTagHelper.Process(context, output);
            }
            catch (Exception e)
            {
                throwenException = e;
            }
            Assert.Equal(expected, throwenException.GetType());
            Assert.Equal("Description", ((ArgumentException)throwenException).ParamName.ToString());
        }

        /// <summary>
        /// Renders the meta tags with no value for image rendered.
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_NoValueForImage_Rendered")]
        public void RenderMetaTags_NoValueForImage_Rendered()
        {
            TwitterCardSummaryLargeImage myTagHelper = new TwitterCardSummaryLargeImage()
            {
                Title = TwitterCardAnswerKey.TitleValue,
                Description = TwitterCardAnswerKey.DescriptionValue,
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                Image = null
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N")
            );

            var output = new TagHelperOutput(
                "meta",
                new TagHelperAttributeList(),
                    (cache, encoder) =>
                    {
                        var tagHelperContent = new DefaultTagHelperContent();
                        tagHelperContent.SetContent(string.Empty);
                        return Task.FromResult<TagHelperContent>(tagHelperContent);
                    }
            );

            myTagHelper.Process(context, output);
            Assert.DoesNotContain("twitter:image", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags with no value for image height or width rendered.
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_NoValueForImageHeightOrWidth_Rendered")]
        public void RenderMetaTags_NoValueForImageHeightOrWidth_Rendered()
        {
            TwitterCardSummaryLargeImage myTagHelper = new TwitterCardSummaryLargeImage()
            {
                Title = TwitterCardAnswerKey.TitleValue,
                Description = TwitterCardAnswerKey.DescriptionValue,
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                Image = new TwitterImage(TwitterCardAnswerKey.ImageUrlValue)
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N")
            );

            var output = new TagHelperOutput(
                "meta",
                new TagHelperAttributeList(),
                    (cache, encoder) =>
                    {
                        var tagHelperContent = new DefaultTagHelperContent();
                        tagHelperContent.SetContent(string.Empty);
                        return Task.FromResult<TagHelperContent>(tagHelperContent);
                    }
            );

            myTagHelper.Process(context, output);
            Assert.DoesNotContain("twitter:image:height", output.Content.GetContent());
            Assert.DoesNotContain("twitter:image:width", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags with no value for title. (exception thrown)
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_NoValueForTitle_ExceptionThrown")]
        public void RenderMetaTags_NoValueForTitle_ExceptionThrown()
        {
            var expected = typeof(System.ArgumentNullException);
            Exception throwenException = null;

            TwitterCardSummaryLargeImage myTagHelper = new TwitterCardSummaryLargeImage()
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
                     Guid.NewGuid().ToString("N")
                );

                var output = new TagHelperOutput(
                    "meta",
                    new TagHelperAttributeList(),
                        (cache, encoder) =>
                        {
                            var tagHelperContent = new DefaultTagHelperContent();
                            tagHelperContent.SetContent(string.Empty);
                            return Task.FromResult<TagHelperContent>(tagHelperContent);
                        }
                );

                myTagHelper.Process(context, output);
            }
            catch (Exception e)
            {
                throwenException = e;
            }
            Assert.Equal(expected, throwenException.GetType());
            Assert.Equal("Title", ((ArgumentException)throwenException).ParamName.ToString());
        }

        /// <summary>
        /// Renders the meta tags with no value for twitter site username. (exception thrown)
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_NoValueForTwitterSiteUsername_ExceptionThrown")]
        public void RenderMetaTags_NoValueForTwitterSiteUsername_ExceptionThrown()
        {
            var expected = typeof(System.ArgumentNullException);
            Exception throwenException = null;

            try
            {
                TwitterCardSummaryLargeImage myTagHelper = new TwitterCardSummaryLargeImage()
                {
                    Title = TwitterCardAnswerKey.TitleValue,
                    Description = TwitterCardAnswerKey.DescriptionValue,
                    SiteUsername = string.Empty,
                    Image = null
                };

                var context = new TagHelperContext(
                    new TagHelperAttributeList(),
                    new Dictionary<object, object>(),
                    Guid.NewGuid().ToString("N")
                );

                var output = new TagHelperOutput(
                    "meta",
                    new TagHelperAttributeList(),
                        (cache, encoder) =>
                        {
                            var tagHelperContent = new DefaultTagHelperContent();
                            tagHelperContent.SetContent(string.Empty);
                            return Task.FromResult<TagHelperContent>(tagHelperContent);
                        }
                );

                myTagHelper.Process(context, output);
            }
            catch (Exception e)
            {
                throwenException = e;
            }

            Assert.Equal(expected, throwenException.GetType());
            Assert.Contains("Either twitter:site or twitter:site:id is required.", throwenException.Message.ToString());
        }

        /// <summary>
        /// Renders the meta tags with no value for twitter site username using twitter site
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_NoValueForTwitterSiteUsernameUsingTwitterSiteId_Rendered")]
        public void RenderMetaTags_NoValueForTwitterSiteUsernameUsingTwitterSiteId_Rendered()
        {
            TwitterCardSummaryLargeImage myTagHelper = new TwitterCardSummaryLargeImage()
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
                Guid.NewGuid().ToString("N")
            );

            var output = new TagHelperOutput(
                "meta",
                new TagHelperAttributeList(),
                    (cache, encoder) =>
                    {
                        var tagHelperContent = new DefaultTagHelperContent();
                        tagHelperContent.SetContent(string.Empty);
                        return Task.FromResult<TagHelperContent>(tagHelperContent);
                    }
            );

            myTagHelper.Process(context, output);
            Assert.Contains("name=\"twitter:site:id\" content=\"" + TwitterCardAnswerKey.SiteIdValue + "\"", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags rendered correct twitter card type tag match.
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_RenderedCorrectTwitterCardTypeTag_Match")]
        public void RenderMetaTags_RenderedCorrectTwitterCardTypeTag_Match()
        {
            TwitterCardSummaryLargeImage myTagHelper = new TwitterCardSummaryLargeImage()
            {
                Title = TwitterCardAnswerKey.TitleValue,
                Description = TwitterCardAnswerKey.DescriptionValue,
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                Image = null
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N")
            );

            var output = new TagHelperOutput(
                "div",
                new TagHelperAttributeList(),
                    (cache, encoder) =>
                    {
                        var tagHelperContent = new DefaultTagHelperContent();
                        tagHelperContent.SetContent(string.Empty);
                        return Task.FromResult<TagHelperContent>(tagHelperContent);
                    }
            );

            myTagHelper.Process(context, output);
            Assert.Contains("name=\"twitter:card\" content=\"summary_large_image\"", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags rendered with optional tag creator twitter identifier creator
        /// username rendered.
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_RenderedOptionalTagCreatorTwitterIdCreatorUsername_Rendered")]
        public void RenderMetaTags_RenderedOptionalTagCreatorTwitterIdCreatorUsername_Rendered()
        {
            TwitterCardSummaryLargeImage myTagHelper = new TwitterCardSummaryLargeImage()
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
                Guid.NewGuid().ToString("N")
            );

            var output = new TagHelperOutput(
                "meta",
                new TagHelperAttributeList(),
                    (cache, encoder) =>
                    {
                        var tagHelperContent = new DefaultTagHelperContent();
                        tagHelperContent.SetContent(string.Empty);
                        return Task.FromResult<TagHelperContent>(tagHelperContent);
                    }
            );

            myTagHelper.Process(context, output);
            Assert.Contains("name=\"twitter:creator:id\" content=\"" + TwitterCardAnswerKey.CreatorId + "\"", output.Content.GetContent());
            Assert.Contains("name=\"twitter:creator\" content=\"" + TwitterCardAnswerKey.CreatorUsernameValue + "\"", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags rendered tag description rendered.
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_RenderedTagDescription_Rendered")]
        public void RenderMetaTags_RenderedTagDescription_Rendered()
        {
            TwitterCardSummaryLargeImage myTagHelper = new TwitterCardSummaryLargeImage()
            {
                Title = TwitterCardAnswerKey.TitleValue,
                Description = TwitterCardAnswerKey.DescriptionValue,
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                Image = null
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N")
            );

            var output = new TagHelperOutput(
                "meta",
                new TagHelperAttributeList(),
                    (cache, encoder) =>
                    {
                        var tagHelperContent = new DefaultTagHelperContent();
                        tagHelperContent.SetContent(string.Empty);
                        return Task.FromResult<TagHelperContent>(tagHelperContent);
                    }
            );

            myTagHelper.Process(context, output);
            Assert.Contains("name=\"twitter:description\" content=\"" + TwitterCardAnswerKey.DescriptionValue + "\"", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags for image with image URL height and width rendered.
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_RenderedTagImageWithImageUrlHeightAndWidth_Rendered")]
        public void RenderMetaTags_RenderedTagImageWithImageUrlHeightAndWidth_Rendered()
        {
            TwitterCardSummaryLargeImage myTagHelper = new TwitterCardSummaryLargeImage()
            {
                Title = TwitterCardAnswerKey.TitleValue,
                Description = TwitterCardAnswerKey.DescriptionValue,
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                Image = new TwitterImage(TwitterCardAnswerKey.ImageUrlValue, TwitterCardAnswerKey.ImageWidthValue, TwitterCardAnswerKey.ImageHeightValue)
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N")
            );

            var output = new TagHelperOutput(
                "meta",
                new TagHelperAttributeList(),
                    (cache, encoder) =>
                    {
                        var tagHelperContent = new DefaultTagHelperContent();
                        tagHelperContent.SetContent(string.Empty);
                        return Task.FromResult<TagHelperContent>(tagHelperContent);
                    }
            );

            myTagHelper.Process(context, output);
            Assert.Contains("name=\"twitter:image\" content=\"" + TwitterCardAnswerKey.ImageUrlValue + "\"", output.Content.GetContent());
            Assert.Contains("name=\"twitter:image:height\" content=\"" + TwitterCardAnswerKey.ImageHeightValue.ToString() + "\"", output.Content.GetContent());
            Assert.Contains("name=\"twitter:image:width\" content=\"" + TwitterCardAnswerKey.ImageWidthValue.ToString() + "\"", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags for tag title rendered.
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_RenderedTagTitle_Rendered")]
        public void RenderMetaTags_RenderedTagTitle_Rendered()
        {
            TwitterCardSummaryLargeImage myTagHelper = new TwitterCardSummaryLargeImage()
            {
                Title = TwitterCardAnswerKey.TitleValue,
                Description = TwitterCardAnswerKey.DescriptionValue,
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                Image = null
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N")
            );

            var output = new TagHelperOutput(
                "meta",
                new TagHelperAttributeList(),
                    (cache, encoder) =>
                    {
                        var tagHelperContent = new DefaultTagHelperContent();
                        tagHelperContent.SetContent(string.Empty);
                        return Task.FromResult<TagHelperContent>(tagHelperContent);
                    }
            );

            myTagHelper.Process(context, output);
            Assert.Contains("name=\"twitter:title\" content=\"" + TwitterCardAnswerKey.TitleValue + "\"", output.Content.GetContent());
        }
    }
}