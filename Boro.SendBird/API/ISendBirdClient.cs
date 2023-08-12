using Boro.SendBird.Models;

namespace Boro.SendBird.API;

public interface ISendBirdClient
{
    Task<SendBirdUser> CreateNewUserAsync(Guid boroUserId, string nickname);

    Task SendAnnouncementAsync(SendBirdUser from, SendBirdUser to, string message);
}
