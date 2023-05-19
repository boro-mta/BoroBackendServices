using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

[PrimaryKey("ItemId", "Date")]
public class BlockedDates
{
    public Guid ItemId { get; set; }
    public DateTime Date { get; set; }

    [ForeignKey("ItemId")]
    public Items Item { get; set; }
}
