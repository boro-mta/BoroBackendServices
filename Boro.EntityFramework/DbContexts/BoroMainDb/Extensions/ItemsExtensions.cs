using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using Microsoft.EntityFrameworkCore;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Extensions;

public static class ItemsExtensions
{
    public static IEnumerable<Items> FilterByRadius(this DbSet<Items> items, double latitude, double longitude, double radius)
    {
        const string query = 
        """
            DECLARE @latitude FLOAT, @longitude FLOAT, @radius FLOAT;
            SET @latitude = @p0;
            SET @longitude = @p1;
            SET @radius = @p2;

            SELECT * FROM dbo.Items item
            WHERE dbo.IsLocationInRadius(@longitude, @latitude, item.Longitude, item.Latitude, @radius) = 1
            """;

        return items.FromSqlRaw(query, latitude, longitude, radius).AsEnumerable();
    }
}