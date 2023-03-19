using Boro.EntityFramework.DbContexts.BoroMainDb;
using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using Microsoft.EntityFrameworkCore;

namespace ItemService.DB.DbContexts;

public class ItemServiceDbContext : BoroMainDbContext<ItemServiceDbContext>
{
    public ItemServiceDbContext(DbContextOptions<ItemServiceDbContext> options) : base(options)
    {
    }
}
