using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

[PrimaryKey("ParentId", "IsCover")]
public class ItemImages
{
    [Key]
    public Guid ImageId { get; set; }
    [ForeignKey(nameof(Items))]
    public Guid ParentId { get; set; }
    public bool IsCover { get; set; }
    public string ImageMetaData { get; set; } = string.Empty;
    public byte[] ImageData { get; set; } = Array.Empty<byte>();
}