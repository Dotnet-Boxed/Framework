namespace Boxed.AspNetCore.Swagger.Test;

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Xunit;

public class FilterDescriptorExtensionsTest
{
    private readonly List<FilterDescriptor> filterDescriptors;

    public FilterDescriptorExtensionsTest() =>
        this.filterDescriptors = new List<FilterDescriptor>();

    [Fact]
    public void GetPolicyRequirements_HasNoFilters_ReturnsNull()
    {
        var policyRequirements = FilterDescriptorExtensions.GetPolicyRequirements(this.filterDescriptors);

        Assert.Empty(policyRequirements);
    }

    [Fact]
    public void GetPolicyRequirements_HasAllowAnonymousFilter_ReturnsNull()
    {
        this.filterDescriptors.Add(new FilterDescriptor(new AllowAnonymousFilter(), 30));

        var policyRequirements = FilterDescriptorExtensions.GetPolicyRequirements(this.filterDescriptors);

        Assert.Empty(policyRequirements);
    }

    [Fact]
    public void GetPolicyRequirements_HasAllowAnonymousFilterWithHighestPriority_ReturnsNull()
    {
        var requirement = new DenyAnonymousAuthorizationRequirement();
        var requirements = new List<IAuthorizationRequirement>() { requirement };
        var policy = new AuthorizationPolicy(requirements, new List<string>());
        this.filterDescriptors.Add(new FilterDescriptor(new AuthorizeFilter(policy), 20));
        this.filterDescriptors.Add(new FilterDescriptor(new AllowAnonymousFilter(), 30));

        var policyRequirements = FilterDescriptorExtensions.GetPolicyRequirements(this.filterDescriptors);

        Assert.Empty(policyRequirements);
    }

    [Fact]
    public void GetPolicyRequirements_HasAuthorizeFilterWithHighestPriorityAndRequirements_ReturnsRequirements()
    {
        this.filterDescriptors.Add(new FilterDescriptor(new AllowAnonymousFilter(), 20));
        var requirement = new DenyAnonymousAuthorizationRequirement();
        var requirements = new List<IAuthorizationRequirement>() { requirement };
        var policy = new AuthorizationPolicy(requirements, new List<string>());
        this.filterDescriptors.Add(new FilterDescriptor(new AuthorizeFilter(policy), 30));

        var policyRequirements = FilterDescriptorExtensions.GetPolicyRequirements(this.filterDescriptors);

        Assert.Equal(requirements, policyRequirements);
    }

    [Fact]
    public void GetPolicyRequirements_HasMultipleAuthorizeFilterRequirements_ReturnsAllRequirements()
    {
        this.filterDescriptors.Add(new FilterDescriptor(new AllowAnonymousFilter(), 10));
        var requirement1 = new DenyAnonymousAuthorizationRequirement();
        var requirements1 = new List<IAuthorizationRequirement>() { requirement1 };
        var policy1 = new AuthorizationPolicy(requirements1, new List<string>());
        this.filterDescriptors.Add(new FilterDescriptor(new AuthorizeFilter(policy1), 20));
        var requirement2 = new DenyAnonymousAuthorizationRequirement();
        var requirements2 = new List<IAuthorizationRequirement>() { requirement2 };
        var policy2 = new AuthorizationPolicy(requirements2, new List<string>());
        this.filterDescriptors.Add(new FilterDescriptor(new AuthorizeFilter(policy2), 30));

        var policyRequirements = FilterDescriptorExtensions.GetPolicyRequirements(this.filterDescriptors);

        Assert.Equal(2, policyRequirements.Count);
        Assert.Same(requirement2, policyRequirements.First());
        Assert.Same(requirement1, policyRequirements.Last());
    }
}
