using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

public class BlockedDates
{
    [Key]
    [ForeignKey(nameof(Items))]
    public Guid ItemId { get; set; }
    public DateTime Date { get; set; }
}
