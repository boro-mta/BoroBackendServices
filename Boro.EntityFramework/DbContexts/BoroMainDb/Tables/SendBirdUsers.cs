using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

public class SendBirdUsers
{
    [Key]
    public Guid SendBirdUserId { get; set; }
    public Guid BoroUserId { get; set;}
    public string AccessToken { get; set; }
    public string Nickname { get; set; }
    [ForeignKey("BoroUserId")]
    public Users User { get; set; }
}
