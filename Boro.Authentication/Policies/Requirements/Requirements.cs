using Microsoft.AspNetCore.Authorization;

namespace Boro.Authentication.Policies.Requirements;

public record ItemOwnerRequirement(string ItemIdParameterName = "itemId", bool Owner = true) : IAuthorizationRequirement;
public record ImageOwnerRequirement(string ImageIdParameterName = "imageId", bool Owner = true) : IAuthorizationRequirement;

