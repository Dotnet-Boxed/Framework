namespace Boilerplate.AspNetCore.TagHelpers.OpenGraph
{
    /// <summary>
    /// A location specified by latitude and longitude.
    /// </summary>
    public class OpenGraphLocation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGraphLocation"/> class.
        /// </summary>
        /// <param name="latitude">The latitude of the location.</param>
        /// <param name="longitude">The longitude of the location.</param>
        public OpenGraphLocation(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGraphLocation"/> class.
        /// </summary>
        /// <param name="latitude">The latitude of the location.</param>
        /// <param name="longitude">The longitude of the location.</param>
        /// <param name="altitude">The altitude of the location.</param>
        public OpenGraphLocation(double latitude, double longitude, double altitude)
            : this(latitude, longitude)
        {
            this.Altitude = altitude;
        }

        /// <summary>
        /// Gets the altitude of the location.
        /// </summary>
        public double? Altitude { get; }

        /// <summary>
        /// Gets the latitude of the location.
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        /// Gets the longitude of the location.
        /// </summary>
        public double Longitude { get; }
    }
}
