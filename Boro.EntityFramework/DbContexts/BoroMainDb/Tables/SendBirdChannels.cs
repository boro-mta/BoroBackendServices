using System.ComponentModel.DataAnnotations;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

public class SendBirdChannels
{
    public Guid UserA { get; set; }
    public Guid UserB { get; set; }
    [Key]
    public string ChannelUrl { get; set; }
}