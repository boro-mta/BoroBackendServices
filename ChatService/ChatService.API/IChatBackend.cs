namespace ChatService.API;

public interface IChatBackend
{
    Task SendMessage(Guid fromUser, Guid toUser, string message);
}