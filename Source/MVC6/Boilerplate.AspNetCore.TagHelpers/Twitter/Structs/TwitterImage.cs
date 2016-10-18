namespace Boilerplate.AspNetCore.TagHelpers.Twitter
{
    using System;

    /// <summary>
    /// An image used in a Twitter card. The Image must be less than 1MB in size.
    /// </summary>
    public class TwitterImage
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitterImage"/> class.
        /// </summary>
        /// <param name="imageUrl">
        /// The image URL. The Image must be less than 1MB in size.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// imageUrl
        /// </exception>
        public TwitterImage(string imageUrl)
        {
            if (imageUrl == null)
            {
                throw new ArgumentNullException(nameof(imageUrl));
            }

            this.ImageUrl = imageUrl;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitterImage"/> class.
        /// </summary>
        /// <param name="imageUrl">
        /// The image URL. The Image must be less than 1MB in size.
        /// </param>
        /// <param name="width">
        /// The width of the image in pixels to help Twitter accurately preserve the aspect ratio of the image when resizing. This property is optional.
        /// </param>
        /// <param name="height">
        /// The height of the image in pixels to help Twitter accurately preserve the aspect ratio of the image when resizing. This property is optional.
        /// </param>
        /// <param name="imageAlt">
        /// The text description of the image conveying the essential nature of an image to users who are visually impaired.
        /// </param>
        public TwitterImage(string imageUrl, int width, int height, string imageAlt)
            : this(imageUrl)
        {
            this.Height = height;
            this.Width = width;
            this.ImageAlt = imageAlt;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the height of the image in pixels to help Twitter accurately preserve the aspect ratio of the image when resizing. This property is optional.
        /// </summary>
        /// <value>
        /// The height of the image in pixels.
        /// </value>
        public int? Height { get; set; }

        /// <summary>
        /// Gets the image alt.
        /// </summary>
        /// <value>
        /// A text description of the image conveying the essential nature of an image to users who are visually impaired.
        /// </value>
        public string ImageAlt { get; }

        /// <summary>
        /// Gets the image URL. The Image must be less than 1MB in size.
        /// </summary>
        /// <value>
        /// The image URL. The Image must be less than 1MB in size.
        /// </value>
        public string ImageUrl { get; }

        /// <summary>
        /// Gets or sets the width of the image in pixels to help Twitter accurately preserve the aspect ratio of the image when resizing. This property is optional.
        /// </summary>
        /// <value>
        /// The width of the image in pixels.
        /// </value>
        public int? Width { get; set; }

        #endregion Public Properties
    }
}