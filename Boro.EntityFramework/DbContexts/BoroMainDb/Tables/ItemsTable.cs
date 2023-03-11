using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

[Table("Items")]
public class ItemsTable
{
    [Key]
    [Column("Item_id")]
    public Guid Id { get; set; }

    [Column("Description")]
    public string? Description { get; set; }
}