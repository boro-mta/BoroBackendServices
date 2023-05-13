using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

public class ItemImages
{
    [Key]
    public Guid ImageId { get; set; }
    [ForeignKey(nameof(Items))]
    public Guid ItemId { get; set; }
    public string ImageMetaData { get; set; } = string.Empty;
    public byte[] ImageData { get; set; } = Array.Empty<byte>();
}