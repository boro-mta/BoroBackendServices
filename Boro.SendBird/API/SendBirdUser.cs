namespace Boro.SendBird.API;

public record SendBirdUser(Guid BoroUserId, Guid SendBirdUserId, string AccessToken, string Nickname);
