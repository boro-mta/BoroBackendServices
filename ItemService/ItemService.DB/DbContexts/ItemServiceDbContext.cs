using Boro.EntityFramework.DbContexts.BoroMainDb;
using ItemService.DB.DbContexts.Tables;
using Microsoft.EntityFrameworkCore;

namespace ItemService.DB.DbContexts
{
    public class ItemServiceDbContext : BoroMainDbContext<ItemServiceDbContext>
    {
        public ItemServiceDbContext(DbContextOptions<ItemServiceDbContext> options) : base(options)
        {

        }

        public DbSet<ItemServices> ItemServices { get; set; }

    }
}