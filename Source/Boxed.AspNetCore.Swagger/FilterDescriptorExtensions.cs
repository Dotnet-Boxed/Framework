namespace Boxed.AspNetCore.Swagger
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Mvc.Filters;

    internal static class FilterDescriptorExtensions
    {
        public static IList<IAuthorizationRequirement> GetPolicyRequirements(
            this IList<FilterDescriptor> filterDescriptors)
        {
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
                    if (authorizeFilter.Policy != null)
                    {
                        policyRequirements.AddRange(authorizeFilter.Policy.Requirements);
                    }
                }
            }

            return policyRequirements;
        }
    }
}
