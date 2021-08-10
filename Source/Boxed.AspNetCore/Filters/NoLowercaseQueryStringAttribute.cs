namespace Boxed.AspNetCore.Filters
{
    using System;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// Ensures that a HTTP request URL can contain query string parameters with both upper-case and lower-case
    /// characters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class NoLowercaseQueryStringAttribute : Attribute, IFilterMetadata
    {
    }
}
