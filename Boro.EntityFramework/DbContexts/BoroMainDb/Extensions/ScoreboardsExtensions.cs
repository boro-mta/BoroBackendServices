using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Extensions
{
    public static class ScoreboardsExtensions
    {
        public static Scoreboards UpsertScoresAndGetResult(this DbSet<Scoreboards> scoreboard, Guid userId)
        {
            return scoreboard.FromSqlRaw("EXEC dbo.UpsertAndCalculateTotalScore @userId",
                                         new SqlParameter("userId", userId))
                             .AsEnumerable()
                             .Single();
        }
    }
}
