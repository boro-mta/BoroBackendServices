using Boro.Authentication.Policies.Requirements;
using Boro.Common.Authentication;
using Boro.EntityFramework.DbContexts.BoroMainDb;
using Microsoft.AspNetCore.Authorization;

namespace Boro.Authentication.Policies.Handlers;

public class ImageOwnerAuthorizationHandler : AuthorizationHandler<ImageOwnerRequirement>
{
    private readonly BoroMainDbContext _dbContext;

    public ImageOwnerAuthorizationHandler(BoroMainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ImageOwnerRequirement requirement)
    {
        var userId = context.User.UserId();
        var imageId = context.GetFromRoute<string>(requirement.ImageIdParameterName);

        if (imageId is not null
            && Guid.TryParse(imageId, out var imageIdGuid)
            && requirement.Owner == await _dbContext.IsImageOwner(userId, imageIdGuid))
        {
            context.Succeed(requirement);
        }
    }
}

