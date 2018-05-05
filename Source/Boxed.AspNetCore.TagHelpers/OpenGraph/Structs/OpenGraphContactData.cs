namespace Boxed.AspNetCore.TagHelpers.OpenGraph
{
    using System;

    /// <summary>
    /// A set of contact information, including address, phone, email, fax and website.
    /// </summary>
    public class OpenGraphContactData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGraphContactData" /> class.
        /// </summary>
        /// <param name="streetAddress">The number and street of the postal address for this business.</param>
        /// <param name="locality">The city (or locality) line of the postal address for this business.</param>
        /// <param name="postalCode">The postcode (or ZIP code) of the postal address for this business</param>
        /// <param name="country">The country of the postal address for this business.</param>
        /// <exception cref="System.ArgumentNullException">streetAddress or locality or postalCode or country is <c>null.</c>.</exception>
        public OpenGraphContactData(string streetAddress, string locality, string postalCode, string country)
        {
            this.Country = country ?? throw new ArgumentNullException(nameof(country));
            this.Locality = locality ?? throw new ArgumentNullException(nameof(locality));
            this.PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode));
            this.StreetAddress = streetAddress ?? throw new ArgumentNullException(nameof(streetAddress));
        }

        /// <summary>
        /// Gets the country of the postal address for this business.
        /// </summary>
        public string Country { get; }

        /// <summary>
        /// Gets or sets the email address to contact this business.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a fax number to contact this business.
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Gets the city (or locality) line of the postal address for this business.
        /// </summary>
        public string Locality { get; }

        /// <summary>
        /// Gets or sets a telephone number to contact this business.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets the postcode (or ZIP code) of the postal address for this business.
        /// </summary>
        public string PostalCode { get; }

        /// <summary>
        /// Gets or sets the state (or region) line of the postal address for this business.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets the number and street of the postal address for this business.
        /// </summary>
        public string StreetAddress { get; }

        /// <summary>
        /// Gets or sets a website for this business.
        /// </summary>
        public string Website { get; set; }
    }
}
