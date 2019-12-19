namespace Boxed.AspNetCore.TagHelpers.Test
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Xunit;

    public class VisibilityTagHelperTest
    {
        [Fact]
        public void Process_Visible_DoesNotSuppressOutput()
        {
            var tagHelper = new VisibilityTagHelper();
            var context = GetContext();
            var output = GetOutput();

            tagHelper.Process(context, output);

            Assert.True(tagHelper.IsVisible);
            Assert.False(output.IsContentModified);
        }

        [Fact]
        public void Process_NotVisible_SuppressesOutput()
        {
            var tagHelper = new VisibilityTagHelper()
            {
                IsVisible = false,
            };
            var context = GetContext();
            var output = GetOutput();

            tagHelper.Process(context, output);

            Assert.True(output.IsContentModified);
        }

        private static TagHelperContext GetContext() =>
            new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test");

        private static TagHelperOutput GetOutput()
        {
            var output = new TagHelperOutput(
                "test",
                new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent("Hello World");
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });
            return output;
        }
    }
}
