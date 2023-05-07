using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using Microsoft.EntityFrameworkCore;

namespace Boro.EntityFramework.DbContexts.BoroMainDb;

public class BoroMainDbContext : DbContext
{
    public BoroMainDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Items> Items { get; set; }
    public DbSet<ItemImages> ItemImages { get; set; }
    public DbSet<Reservations> Reservations { get; set; }
    public DbSet<BlockedDates> BlockedDates { get; set; }
    public DbSet<Users> Users { get; set; }

}

public static class DBContextExtensions
{
    public static async Task<bool> IsItemOwner(this BoroMainDbContext context, Guid userId, Guid itemId)
    {
        return await context.Items.AnyAsync(item => item.Id == itemId && item.OwnerId == userId);
    }

    public static async Task<bool> IsImageOwner(this BoroMainDbContext context, Guid userId, Guid imageId)
    {
        return await context.ItemImages.AnyAsync(image => image.ImageId == imageId && context.IsItemOwner(userId, image.ParentId).Result);
    }
}