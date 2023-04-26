using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

public class Items
{
    [Key]
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    [ForeignKey(nameof(Users))]
    public Guid? OwnerId { get; set; }
    public IEnumerable<ItemImages>? Images { get; set; }
    public string Condition { get; set; } = "";
    public string Categories { get; set; } = "";
}
