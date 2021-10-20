namespace Boxed.AspNetCore.Swagger
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// <see cref="IList{FilterDescriptor}"/> extension methods.
    /// </summary>
    internal static class FilterDescriptorExtensions
    {
        /// <summary>
        /// Gets the authorization policy requirements.
        /// </summary>
        /// <param name="filterDescriptors">The filter descriptors.</param>
        /// <returns>A collection of authorization policy requirements.</returns>
        public static IList<IAuthorizationRequirement> GetPolicyRequirements(
            this IList<FilterDescriptor> filterDescriptors)
        {
            ArgumentNullException.ThrowIfNull(filterDescriptors);

            var policyRequirements = new List<IAuthorizationRequirement>();

            for (var i = filterDescriptors.Count - 1; i >= 0; --i)
            {
                var filterDescriptor = filterDescriptors[i];
                if (filterDescriptor.Filter is AllowAnonymousFilter)
                {
                    break;
                }

                if (filterDescriptor.Filter is AuthorizeFilter authorizeFilter)
                {
                    if (authorizeFilter.Policy is not null)
                    {
                        policyRequirements.AddRange(authorizeFilter.Policy.Requirements);
                    }
                }
            }

            return policyRequirements;
        }
    }
}
