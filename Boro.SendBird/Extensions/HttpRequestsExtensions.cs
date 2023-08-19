using Boro.SendBird.API;
using Boro.SendBird.Models;

namespace Boro.SendBird.Extensions;

internal static class HttpRequestsExtensions
{
    public static CreateUserRequest ToCreateUserRequest(this Guid sendBirdUserId, string nickname)
    {
        return new CreateUserRequest()
        {
            UserId = sendBirdUserId.ToString(),
            IssueAccessToken = true,
            Nickname = nickname
        };
    }

    public static SendBirdUser ToSendBirdUser(this CreateUserResponse createUserResponse, Guid boroUserId)
    {
        if (!Guid.TryParse(createUserResponse.UserId, out var sendBirdUserId))
        {
            throw new Exception($"error parsing [{createUserResponse.UserId}] to a Guid");
        }

        return new SendBirdUser(boroUserId, sendBirdUserId, createUserResponse.AccessToken, createUserResponse.Nickname);
    }

    public static string CreateAnnouncementRequest(this SendBirdUser from, SendBirdUser to, string message)
    {
        return 
            $$"""
            {
                "unique_id": "{{Guid.NewGuid()}}",
                "message": {
                    "type": "MESG",
                    "user_id": "{{from.SendBirdUserId}}",
                    "content": "{{message}}"
                },
                "enable_push": true,
                "target_at": "target_users_only_channels",
                "target_list": ["{{to.SendBirdUserId}}"],
                "target_channel_type": "distinct",
                "create_channel": true,
                "create_channel_options": {
                    "cover_url": "https://img.freepik.com/premium-vector/8-bit-pixel-coffee-cup-logo-image-drink-vector-illustration-game-icon_614713-358.jpg?w=740",
                    "distinct": true
                },
                "scheduled_at": "{{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}}"
            }
            """;
    }

    public static SendMessageRequest CreateSendMessageRequest(this SendBirdUser from, string message) => new()
    {
        UserId = from.SendBirdUserId,
        Message = message
    };

    public static CreateGroupChannelRequest CreateGroupChannelRequest(this IEnumerable<SendBirdUser> users) => new()
    {
        UserIds = users.Select(u => u.SendBirdUserId),
        ChannelName = string.Join("; ", users.Select(user => user.Nickname))
    };

}
