namespace Boilerplate.AspNetCore.Filters
{
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// Require the User-Agent HTTP header to be specified in a request.
    /// </summary>
    /// <seealso cref="HttpHeaderAttribute" />
    public class UserAgentHttpHeaderAttribute : HttpHeaderAttribute
    {
        private const string UserAgentHttpHeaderName = "User-Agent";

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgentHttpHeaderAttribute"/> class.
        /// </summary>
        public UserAgentHttpHeaderAttribute()
            : base(UserAgentHttpHeaderName)
        {
        }

        /// <summary>
        /// Returns <c>true</c> if the User-Agent HTTP header value is valid, otherwise returns <c>false</c>.
        /// </summary>
        /// <param name="headerValues">The header values.</param>
        /// <returns><c>true</c> if the User-Agent HTTP header values are valid; otherwise, <c>false</c>.</returns>
        public override bool IsValid(StringValues headerValues) => !StringValues.IsNullOrEmpty(headerValues);
    }
}