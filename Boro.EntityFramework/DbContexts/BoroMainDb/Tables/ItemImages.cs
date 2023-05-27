using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

public class ItemImages
{
    [Key]
    public Guid ImageId { get; set; }
    public Guid ItemId { get; set; }
    public string ImageMetaData { get; set; }
    public byte[] ImageData { get; set; }

    [ForeignKey("ItemId")]
    public Items Item { get; set; }
}
