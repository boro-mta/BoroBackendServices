namespace Boro.SendBird.API;

public interface ISendBirdClient
{
    Task<string> CreateGroupChannel(params SendBirdUser[] users);
    Task<SendBirdUser> CreateNewUserAsync(Guid boroUserId, string nickname);
    Task SendAnnouncementAsync(SendBirdUser from, SendBirdUser to, string message);
    Task SendMessageToChannelAsync(SendBirdUser from, string channelUrl, string message);
}
