namespace Boilerplate.AspNetCore.TagHelpers.OpenGraph
{
    using System;

    /// <summary>
    /// A period of time on the specified day.
    /// </summary>
    public class OpenGraphHours
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGraphHours"/> class.
        /// </summary>
        /// <param name="day">The day in the week.</param>
        /// <param name="start">The start time of the day. This can be a value from 00:00 to 24:00 hours.</param>
        /// <param name="end">The end time of the day. This can be a value from 00:00 to 24:00 hours.</param>
        public OpenGraphHours(DayOfWeek day, TimeSpan start, TimeSpan end)
        {
            this.Day = day;
            this.End = end;
            this.Start = start;
        }

        /// <summary>
        /// Gets the day in the week.
        /// </summary>
        public DayOfWeek Day { get; }

        /// <summary>
        /// Gets the end time of the day. This can be a value from 00:00 to 24:00 hours.
        /// </summary>
        public TimeSpan End { get; }

        /// <summary>
        /// Gets the start time of the day. This can be a value from 00:00 to 24:00 hours.
        /// </summary>
        public TimeSpan Start { get; }
    }
}
