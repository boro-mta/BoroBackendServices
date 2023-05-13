using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using Microsoft.EntityFrameworkCore;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Extensions;

public static class DBContextExtensions
{
    public static async Task<bool> IsItemOwner(this BoroMainDbContext context, Guid userId, Guid itemId)
    {
        return await context.Items.AnyAsync(item => item.ItemId == itemId && item.OwnerId == userId);
    }

    public static async Task<bool> IsImageOwner(this BoroMainDbContext context, Guid userId, Guid imageId)
    {
        return await context.ItemImages.AnyAsync(image => image.ImageId == imageId && context.IsItemOwner(userId, image.ItemId).Result);
    }

    public static IQueryable<DateTime> GetBlockedDates(this DbSet<BlockedDates> blockedDates, Guid itemId, DateTime startDate, DateTime endDate)
    {
        return from bd in blockedDates
               where bd.ItemId.Equals(itemId)
                     && startDate <= bd.Date
                     && bd.Date <= endDate
               select bd.Date;
    }
}
