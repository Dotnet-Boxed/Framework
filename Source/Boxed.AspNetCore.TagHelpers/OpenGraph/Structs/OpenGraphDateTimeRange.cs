namespace Boxed.AspNetCore.TagHelpers.OpenGraph;

using System;

/// <summary>
/// A date and time range.
/// </summary>
public class OpenGraphDateTimeRange
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenGraphDateTimeRange"/> class.
    /// </summary>
    /// <param name="start">The start date and time.</param>
    /// <param name="end">The end date and time.</param>
    public OpenGraphDateTimeRange(DateTime start, DateTime end)
    {
        this.Start = start;
        this.End = end;
    }

    /// <summary>
    /// Gets the end date and time.
    /// </summary>
    public DateTime End { get; }

    /// <summary>
    /// Gets the start date and time.
    /// </summary>
    public DateTime Start { get; }
}
