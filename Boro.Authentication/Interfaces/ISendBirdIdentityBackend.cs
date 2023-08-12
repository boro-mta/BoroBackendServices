using Boro.SendBird.Models;

namespace Boro.Authentication.Interfaces;

public interface ISendBirdIdentityBackend
{
    Task<SendBirdUser> GetSendBirdUserAsync(Guid boroUserId);
}
