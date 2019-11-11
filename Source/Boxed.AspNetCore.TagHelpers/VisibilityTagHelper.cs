namespace Boxed.AspNetCore.TagHelpers
{
    using System;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    /// <summary>
    /// Determine whether a target element should be visible or not based on a conditional.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    [HtmlTargetElement(Attributes = IsVisibleAttributeName)]
    public class VisibilityTagHelper : TagHelper
    {
        private const string IsVisibleAttributeName = "asp-is-visible";

        /// <summary>
        /// Gets or sets a value indicating whether the target instance is visible.
        /// </summary>
        /// <value>
        /// <c>true</c> if the target instance is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Synchronously executes the <see cref="TagHelper" /> with the given
        /// <paramref name="context" /> and <paramref name="output" />.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output is null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            if (!this.IsVisible)
            {
                output.SuppressOutput();
            }

            base.Process(context, output);
        }
    }
}
