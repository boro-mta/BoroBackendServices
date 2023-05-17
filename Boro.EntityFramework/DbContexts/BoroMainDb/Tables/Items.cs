using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

public class Items
{
    [Key]
    public Guid ItemId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid OwnerId { get; set; }
    public string Condition { get; set; } = "";
    public string Categories { get; set; } = "";
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public IEnumerable<ItemImages>? Images { get; set; }
    [ForeignKey("OwnerId")]
    public Users User { get; set; }
}
