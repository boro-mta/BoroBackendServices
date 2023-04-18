using System.ComponentModel.DataAnnotations;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

public class Items
{
    [Key]
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? OwnerId { get; set; }
    public IEnumerable<ItemImages>? Images { get; set; }
    public string Condition { get; set; } = "";
    public string Categories { get; set; } = "";
}
