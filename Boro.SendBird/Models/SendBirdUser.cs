namespace Boro.SendBird.Models;

public record SendBirdUser(Guid BoroUserId, Guid SendBirdUserId, string AccessToken, string Nickname);
