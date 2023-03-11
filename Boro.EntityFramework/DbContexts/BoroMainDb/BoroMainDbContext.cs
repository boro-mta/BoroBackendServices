﻿using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using Microsoft.EntityFrameworkCore;

namespace Boro.EntityFramework.DbContexts.BoroMainDb;

public class BoroMainDbContext<T> : DbContext
    where T : BoroMainDbContext<T>
{
    public BoroMainDbContext(DbContextOptions<T> options) : base(options)
    {
        
    }

    public DbSet<Items> Items { get; set; }
    public DbSet<ItemImages> ItemImages { get; set; }
}