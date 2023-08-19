using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using Microsoft.EntityFrameworkCore;

namespace Boro.EntityFramework.DbContexts.BoroMainDb;

public class BoroMainDbContext : DbContext
{
    public BoroMainDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);
    }

    public virtual DbSet<Items> Items { get; set; }
    public virtual DbSet<ItemImages> ItemImages { get; set; }
    public virtual DbSet<Reservations> Reservations { get; set; }
    public virtual DbSet<BlockedDates> BlockedDates { get; set; }
    public virtual DbSet<Users> Users { get; set; }
    public virtual DbSet<SendBirdUsers> SendBirdUsers { get; set; }
    public virtual DbSet<SendBirdChannels> SendBirdChannels { get; set; }
    public virtual DbSet<UserImages> UserImages { get; set; }

}
