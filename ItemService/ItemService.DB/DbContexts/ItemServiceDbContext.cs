using Boro.EntityFramework.DbContexts.BoroMainDb;
using Microsoft.EntityFrameworkCore;

namespace ItemService.DB.DbContexts;

public class ItemServiceDbContext : BoroMainDbContext<ItemServiceDbContext>
{
    public ItemServiceDbContext(DbContextOptions<ItemServiceDbContext> options) : base(options)
    {
    }
}
