namespace Boxed.AspNetCore.TagHelpers.OpenGraph
{
    using System;

    /// <summary>
    /// <see cref="DayOfWeek"/> extension methods.
    /// </summary>
    internal static class DayOfWeekExtensions
    {
        /// <summary>
        /// Returns the lower-case <see cref="string" /> representation of the <see cref="DayOfWeek" />.
        /// </summary>
        /// <param name="dayOfWeek">The day of week.</param>
        /// <returns>
        /// The lower-case <see cref="string" /> representation of the <see cref="DayOfWeek" />.
        /// </returns>
        public static string ToLowercaseString(this DayOfWeek dayOfWeek) =>
            dayOfWeek switch
            {
                DayOfWeek.Friday => "friday",
                DayOfWeek.Monday => "monday",
                DayOfWeek.Saturday => "saturday",
                DayOfWeek.Sunday => "sunday",
                DayOfWeek.Thursday => "thursday",
                DayOfWeek.Tuesday => "tuesday",
                DayOfWeek.Wednesday => "wednesday",
                _ => string.Empty,
            };
    }
}
