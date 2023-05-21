using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

public class UserImages
{
    [Key]
    public Guid ImageId { get; set; }
    public Guid UserId { get; set; }
    public string ImageMetaData { get; set; } = string.Empty;
    public byte[] ImageData { get; set; } = Array.Empty<byte>();

    [ForeignKey("UserId")]
    public Users User { get; set; }
}