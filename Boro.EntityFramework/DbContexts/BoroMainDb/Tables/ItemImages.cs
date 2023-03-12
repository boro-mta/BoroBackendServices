using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

[PrimaryKey("ItemId", "IsCover")]
public class ItemImages
{
    [ForeignKey(nameof(Items))]
    [Column("Item_id")]
    public Guid ItemId { get; set; }
    [Column("Is_cover")]
    public bool IsCover { get; set; }
    public byte[] ImageData { get; set; } = Array.Empty<byte>();
    public string FileName { get; set; } = string.Empty;
    public string ImageFormat { get; set; } = string.Empty;
}