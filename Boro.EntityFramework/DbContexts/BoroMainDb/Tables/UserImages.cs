using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

public class UserImages
{
    [Key]
    public Guid ImageId { get; set; }
    public Guid UserId { get; set; }
    public string ImageMetaData { get; set; }
    public string ImageData { get; set; }

    [ForeignKey("UserId")]
    public Users User { get; set; }
}