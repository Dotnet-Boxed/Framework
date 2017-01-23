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
    /// TwitterCard Summary Card Tests
    /// </summary>
    public class TwitterCard_Summary_Test
    {
        /// <summary>
        /// Renders the meta tags with no value for description. (exception thrown)
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_NoValueForDescription_ExceptionThrown")]
        public void RenderMetaTags_NoValueForDescription_ExceptionThrown()
        {
            var expected = typeof(System.ArgumentNullException);
            Exception throwenException = null;

            TwitterCardSummary myTagHelper = new TwitterCardSummary()
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

                myTagHelper.Process(context, output);
            }
            catch (Exception e)
            {
                throwenException = e;
            }

            Assert.Equal(expected, throwenException.GetType());
            Assert.Equal("Description", ((System.ArgumentException)throwenException).ParamName.ToString());
        }

        /// <summary>
        /// Renders the meta tags with no value for image.
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_NoValueForImage_Rendered")]
        public void RenderMetaTags_NoValueForImage_Rendered()
        {
            TwitterCardSummary myTagHelper = new TwitterCardSummary()
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

            myTagHelper.Process(context, output);
            Assert.DoesNotContain("twitter:image", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags with no value for image height or width.
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_NoValueForImageHeightOrWidth_Rendered")]
        public void RenderMetaTags_NoValueForImageHeightOrWidth_Rendered()
        {
            TwitterCardSummary myTagHelper = new TwitterCardSummary()
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

            myTagHelper.Process(context, output);
            Assert.DoesNotContain("twitter:image:height", output.Content.GetContent());
            Assert.DoesNotContain("twitter:image:width", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags with no value for twitter site username. (exception thrown)
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_NoValueForSiteUsername_ExceptionThrown")]
        public void RenderMetaTags_NoValueForSiteUsername_ExceptionThrown()
        {
            var expected = typeof(System.ArgumentNullException);
            Exception throwenException = null;

            TwitterCardSummary myTagHelper = new TwitterCardSummary()
            {
                Title = TwitterCardAnswerKey.TitleValue,
                Description = TwitterCardAnswerKey.DescriptionValue,
                SiteUsername = string.Empty,
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
        /// Renders the meta tags with no value for twitter site username using twitter site identifier.
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_NoValueForSiteUsernameUsingSiteId_Rendered")]
        public void RenderMetaTags_NoValueForSiteUsernameUsingSiteId_Rendered()
        {
            TwitterCardSummary myTagHelper = new TwitterCardSummary()
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

            myTagHelper.Process(context, output);
            Assert.Contains("name=\"twitter:site:id\" content=\"" + TwitterCardAnswerKey.SiteIdValue + "\"", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags with no value for title. (exception thrown)
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_NoValueForTitle_ExceptionThrown")]
        public void RenderMetaTags_NoValueForTitle_ExceptionThrown()
        {
            var expected = typeof(System.ArgumentNullException);
            Exception throwenException = null;

            TwitterCardSummary myTagHelper = new TwitterCardSummary()
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

                myTagHelper.Process(context, output);
            }
            catch (Exception e)
            {
                throwenException = e;
            }

            Assert.Equal(expected, throwenException.GetType());
            Assert.Equal("Title", ((System.ArgumentException)throwenException).ParamName.ToString());
        }

        /// <summary>
        /// Renders the meta tags with the correct twitter card type tag.
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_RenderedCorrectCardTypeTag_Match")]
        public void RenderMetaTags_RenderedCorrectCardTypeTag_Match()
        {
            TwitterCardSummary myTagHelper = new TwitterCardSummary()
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
            myTagHelper.Process(context, output);
            Assert.Contains("name=\"twitter:card\" content=\"summary\"", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the optional meta tags; twitter identifier, creator username.
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_RenderedOptionalTagCreatorIdCreatorUsername_Rendered")]
        public void RenderMetaTags_RenderedOptionalTagCreatorIdCreatorUsername_Rendered()
        {
            TwitterCardSummary myTagHelper = new TwitterCardSummary()
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
            myTagHelper.Process(context, output);
            Assert.Contains("name=\"twitter:creator\" content=\"" + TwitterCardAnswerKey.CreatorUsernameValue + "\"", output.Content.GetContent());
            Assert.Contains("name=\"twitter:creator:id\" content=\"" + TwitterCardAnswerKey.CreatorId + "\"", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tag description.
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_RenderedTagDescription_Rendered")]
        public void RenderMetaTags_RenderedTagDescription_Rendered()
        {
            TwitterCardSummary myTagHelper = new TwitterCardSummary()
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
            myTagHelper.Process(context, output);
            Assert.Contains("name=\"twitter:description\" content=\"" + TwitterCardAnswerKey.DescriptionValue + "\"", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tag image with image URL height and width.
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_RenderedTagImageWithImageUrlHeightAndWidth_Rendered")]
        public void RenderMetaTags_RenderedTagImageWithImageUrlHeightAndWidth_Rendered()
        {
            TwitterCardSummary myTagHelper = new TwitterCardSummary()
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
            myTagHelper.Process(context, output);

            Assert.Contains("name=\"twitter:image\"", output.Content.GetContent());
            Assert.Contains("name=\"twitter:image:width\" content=\"" + TwitterCardAnswerKey.ImageWidthValue.ToString() + "\"", output.Content.GetContent());
            Assert.Contains("name=\"twitter:image:height\" content=\"" + TwitterCardAnswerKey.ImageHeightValue.ToString() + "\"", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the title meta tag.
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_RenderedTagTitle_Rendered")]
        public void RenderMetaTags_RenderedTagTitle_Rendered()
        {
            TwitterCardSummary myTagHelper = new TwitterCardSummary()
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
            myTagHelper.Process(context, output);
            Assert.Contains("name=\"twitter:title\" content=\"" + TwitterCardAnswerKey.TitleValue + "\"", output.Content.GetContent());
        }
    }
}