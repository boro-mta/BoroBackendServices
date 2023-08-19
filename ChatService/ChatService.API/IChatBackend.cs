namespace ChatService.API;

public interface IChatBackend
{
    Task SendMessageAsync(Guid fromUser, Guid toUser, string message);
}