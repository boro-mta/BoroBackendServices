using Microsoft.EntityFrameworkCore;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

[PrimaryKey("ItemId", "From", "To")]
public class Reservations
{
    public Guid ItemId { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}
