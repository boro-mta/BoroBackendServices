using Boro.SendBird.API;

namespace Boro.Authentication.Interfaces;

public interface ISendBirdIdentityBackend
{
    Task<SendBirdUser> GetSendBirdUserAsync(Guid boroUserId);
}
