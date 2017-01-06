namespace Boilerplate.AspNetCore.TagHelpers.Test.Twitter.Cards
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Boilerplate.AspNetCore.TagHelpers.Twitter;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using TestData;
    using Xunit;

    /// <summary>
    /// TwitterCard App Tests
    /// </summary>
    public class TwitterCard_App_Test
    {
        /// <summary>
        /// Renders the meta tags with no value for google play. (exception thrown)
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_NoValueForGooglePlay_ExceptionThrown")]
        public void RenderMetaTags_NoValueForGooglePlay_ExceptionThrown()
        {
            var expected = typeof(System.ArgumentNullException);
            Exception throwenException = null;

            TwitterCardApp myTagHelper = new TwitterCardApp()
            {
                TwitterSiteUsername = TwitterCardAnswerKey.TwitterSiteUsernameValue,
                IPhone = "307234931",
                IPad = "307234931",
                GooglePlay = string.Empty
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
            Assert.Equal("GooglePlay", ((System.ArgumentException)throwenException).ParamName.ToString());
        }

        /// <summary>
        /// Renders the meta tags with no value for iPad. (exception thrown)
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_NoValueForIPad_ExceptionThrown")]
        public void RenderMetaTags_NoValueForIPad_ExceptionThrown()
        {
            var expected = typeof(System.ArgumentNullException);
            Exception throwenException = null;

            TwitterCardApp myTagHelper = new TwitterCardApp()
            {
                TwitterSiteUsername = TwitterCardAnswerKey.TwitterSiteUsernameValue,
                IPhone = "307234931",
                IPad = string.Empty,
                GooglePlay = "com.android.app"
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
            Assert.Equal("IPad", ((System.ArgumentException)throwenException).ParamName.ToString());
        }

        /// <summary>
        /// Renders the meta tags with no value for twitter site username. (exception thrown)
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_NoValueForTwitterSiteUsername_ExceptionThrown")]
        public void RenderMetaTags_NoValueForTwitterSiteUsername_ExceptionThrown()
        {
            var expected = typeof(System.ArgumentNullException);
            Exception throwenException = null;

            TwitterCardApp myTagHelper = new TwitterCardApp()
            {
                TwitterSiteUsername = string.Empty,
                IPhone = "307234931",
                IPad = "307234931",
                GooglePlay = "com.android.app"
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
            Assert.Equal("TwitterSiteUsername", ((System.ArgumentException)throwenException).ParamName.ToString());
        }

        /// <summary>
        /// Renders the correct twitter card type tag.
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_RenderedCorrectTwitterCardTypeTag_Match")]
        public void RenderMetaTags_RenderedCorrectTwitterCardTypeTag_Match()
        {
            TwitterCardApp myTagHelper = new TwitterCardApp()
            {
                TwitterSiteUsername = TwitterCardAnswerKey.TwitterSiteUsernameValue,
                IPhone = "307234931",
                IPad = "307234931",
                GooglePlay = "com.android.app"
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
            Assert.Contains("name=\"twitter:card\" content=\"app\"", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags for the card application validation fails missing iPhone.
        /// (exception thrown)
        /// </summary>
        [Fact(DisplayName = "RenderMetaTags_NoValueForIPhone_ExceptionThrown")]
        public void TwitterCard_App_Validation_Fails_Missing_IPhone()
        {
            var expected = typeof(System.ArgumentNullException);
            Exception throwenException = null;

            TwitterCardApp myTagHelper = new TwitterCardApp()
            {
                TwitterSiteUsername = TwitterCardAnswerKey.TwitterSiteUsernameValue,
                IPhone = string.Empty,
                IPad = "307234931",
                GooglePlay = "com.android.app"
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
            Assert.Equal("IPhone", ((System.ArgumentException)throwenException).ParamName.ToString());
        }
    }
}