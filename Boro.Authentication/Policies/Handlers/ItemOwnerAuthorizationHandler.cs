using Boro.Authentication.Policies.Requirements;
using Boro.Common.Authentication;
using Boro.EntityFramework.DbContexts.BoroMainDb;
using Boro.EntityFramework.DbContexts.BoroMainDb.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Boro.Authentication.Policies.Handlers;

public class ItemOwnerAuthorizationHandler : AuthorizationHandler<ItemOwnerRequirement>
{
    private readonly BoroMainDbContext _dbContext;

    public ItemOwnerAuthorizationHandler(BoroMainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ItemOwnerRequirement requirement)
    {
        var userId = context.User.UserId();
        var itemId = context.GetFromRoute<string>(requirement.ItemIdParameterName);

        if (itemId is not null
            && Guid.TryParse(itemId, out var itemIdGuid)
            && requirement.Owner == await _dbContext.IsItemOwner(userId, itemIdGuid))
        {
            context.Succeed(requirement);
        }
    }
}

