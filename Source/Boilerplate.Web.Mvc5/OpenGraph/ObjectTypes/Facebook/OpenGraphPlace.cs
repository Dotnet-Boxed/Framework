namespace Boilerplate.Web.Mvc.OpenGraph
{
    using System;
    using System.Text;

    /// <summary>
    /// This object type represents a place - such as a venue, a business, a landmark, or any other location which can be identified by longitude and
    /// latitude. This object type is not part of the Open Graph standard but is used by Facebook.
    /// See https://developers.facebook.com/docs/reference/opengraph/object-type/place/
    /// </summary>
    public class OpenGraphPlace : OpenGraphMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGraphPlace" /> class.
        /// </summary>
        /// <param name="title">The title of the object as it should appear in the graph.</param>
        /// <param name="image">The default image.</param>
        /// <param name="location">The location of the business.</param>
        /// <param name="url">The canonical URL of the object, used as its ID in the graph.</param>
        /// <exception cref="System.ArgumentNullException">location is <c>null</c>.</exception>
        public OpenGraphPlace(string title, OpenGraphImage image, OpenGraphLocation location, string url = null)
            : base(title, image, url)
        {
            if (location == null)
            {
                throw new ArgumentNullException("location");
            }

            this.Location = location;
        }

        /// <summary>
        /// Gets the location of the place.
        /// </summary>
        public OpenGraphLocation Location { get; }

        /// <summary>
        /// Gets the namespace of this open graph type.
        /// </summary>
        public override string Namespace => "place: http://ogp.me/ns/place#";

        /// <summary>
        /// Gets the type of your object. Depending on the type you specify, other properties may also be required.
        /// </summary>
        public override OpenGraphType Type => OpenGraphType.Place;

        /// <summary>
        /// Appends a HTML-encoded string representing this instance to the <paramref name="stringBuilder"/> containing the Open Graph meta tags.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        public override void ToString(StringBuilder stringBuilder)
        {
            base.ToString(stringBuilder);

            stringBuilder.AppendMetaPropertyContent("place:location:latitude", this.Location.Latitude);
            stringBuilder.AppendMetaPropertyContent("place:location:longitude", this.Location.Longitude);
            stringBuilder.AppendMetaPropertyContentIfNotNull("place:location:altitude", this.Location.Altitude);
        }
    }
}
