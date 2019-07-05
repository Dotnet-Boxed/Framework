namespace Boxed.AspNetCore.TagHelpers.Test.Twitter.Cards
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Boxed.AspNetCore.TagHelpers.Test.TestData;
    using Boxed.AspNetCore.TagHelpers.Twitter;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Xunit;

    public class TwitterCardAppTest
    {
        /// <summary>
        /// Renders the meta tags with no value for google play. (exception thrown).
        /// </summary>
        [Fact]
        public void RenderMetaTags_NoValueForGooglePlay_ExceptionThrown()
        {
            var tagHelper = new TwitterCardApp()
            {
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                IPhone = "307234931",
                IPad = "307234931",
                GooglePlay = string.Empty
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

            Assert.Throws<ArgumentNullException>("GooglePlay", () => tagHelper.Process(context, output));
        }

        /// <summary>
        /// Renders the meta tags with no value for iPad. (exception thrown).
        /// </summary>
        [Fact]
        public void RenderMetaTags_NoValueForIPad_ExceptionThrown()
        {
            var expected = typeof(System.ArgumentNullException);
            Exception thrownException = null;

            var tagHelper = new TwitterCardApp()
            {
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                IPhone = "307234931",
                IPad = string.Empty,
                GooglePlay = "com.android.app"
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
            Assert.Equal("IPad", ((System.ArgumentException)thrownException).ParamName.ToString());
        }

        /// <summary>
        /// Renders the meta tags with no value for twitter site username. (exception thrown).
        /// </summary>
        [Fact]
        public void RenderMetaTags_NoValueForSiteUsername_ExceptionThrown()
        {
            var expected = typeof(System.ArgumentNullException);
            Exception thrownException = null;

            var tagHelper = new TwitterCardApp()
            {
                SiteUsername = string.Empty,
                IPhone = "307234931",
                IPad = "307234931",
                GooglePlay = "com.android.app"
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
            Assert.Equal("SiteUsername", ((System.ArgumentException)thrownException).ParamName.ToString());
        }

        /// <summary>
        /// Renders the correct twitter card type tag.
        /// </summary>
        [Fact]
        public void RenderMetaTags_RenderedCorrectTwitterCardTypeTag_Match()
        {
            var tagHelper = new TwitterCardApp()
            {
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                IPhone = "307234931",
                IPad = "307234931",
                GooglePlay = "com.android.app"
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
            Assert.Contains("name=\"twitter:card\" content=\"app\"", output.Content.GetContent());
        }

        /// <summary>
        /// Renders the meta tags for the card application validation fails missing iPhone.
        /// (exception thrown).
        /// </summary>
        [Fact]
        public void TwitterCard_App_Validation_Fails_Missing_IPhone()
        {
            var expected = typeof(System.ArgumentNullException);
            Exception thrownException = null;

            var tagHelper = new TwitterCardApp()
            {
                SiteUsername = TwitterCardAnswerKey.SiteUsernameValue,
                IPhone = string.Empty,
                IPad = "307234931",
                GooglePlay = "com.android.app"
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
            Assert.Equal("IPhone", ((System.ArgumentException)thrownException).ParamName.ToString());
        }
    }
}