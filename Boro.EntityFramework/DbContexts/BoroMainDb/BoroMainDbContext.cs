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
    public DbSet<Users> Users { get; set; }


}