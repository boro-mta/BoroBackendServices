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

    public async Task<Statistics> GetUpdatedStatisticsAsync(Guid userId)
    {
        const string rawSql =
            """
            DECLARE @uid UNIQUEIDENTIFIER;
            SET @uid = @p0;

            SELECT
                @uid as UserId,
                (SELECT COUNT(*) FROM Reservations WHERE BorrowerId = @uid AND Status = 10) AS AmountOfBorrowings,
                (SELECT COUNT(*) FROM Reservations WHERE LenderId = @uid AND Status = 10) AS AmountOfLendings,
                (SELECT COUNT(*) FROM Items WHERE OwnerId = @uid) AS AmountOfItems
            """
        ;

        var newStats = Set<Statistics>().FromSqlRaw(rawSql, userId)
                                        .AsEnumerable()
                                        .First();

        return await Task.FromResult(newStats);
    }

    public virtual DbSet<Items> Items { get; set; }
    public virtual DbSet<ItemImages> ItemImages { get; set; }
    public virtual DbSet<Reservations> Reservations { get; set; }
    public virtual DbSet<BlockedDates> BlockedDates { get; set; }
    public virtual DbSet<Users> Users { get; set; }
    public virtual DbSet<SendBirdUsers> SendBirdUsers { get; set; }
    public virtual DbSet<SendBirdChannels> SendBirdChannels { get; set; }
    public virtual DbSet<UserImages> UserImages { get; set; }
    public virtual DbSet<Statistics> Statistics { get; set; }

}
