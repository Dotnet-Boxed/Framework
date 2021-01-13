namespace Boxed.AspNetCore.TagHelpers.OpenGraph
{
    /// <summary>
    /// <see cref="OpenGraphGender"/> extension methods.
    /// </summary>
    internal static class OpenGraphGenderExtensions
    {
        /// <summary>
        /// Returns the lower-case <see cref="string" /> representation of the <see cref="OpenGraphGender" />.
        /// </summary>
        /// <param name="gender">The gender.</param>
        /// <returns>
        /// The lower-case <see cref="string" /> representation of the <see cref="OpenGraphGender" />.
        /// </returns>
        public static string ToLowercaseString(this OpenGraphGender gender) =>
            gender switch
            {
                OpenGraphGender.Male => "male",
                OpenGraphGender.Female => "female",
                _ => string.Empty,
            };
    }
}
