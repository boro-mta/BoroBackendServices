using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Nodes;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

public class Items
{
    [Key]
    [Column("Item_id")]
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Column("Owner_id")]
    public string OwnerId { get; set; } = string.Empty;
    public IEnumerable<ItemImages>? Images { get; set; }
    [Column("Included_extras")]
    public string? IncludedExtras { get; set; }
}
