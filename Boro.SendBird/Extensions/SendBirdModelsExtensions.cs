using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using Boro.SendBird.Models;

namespace Boro.SendBird.Extensions;

public static class SendBirdModelsExtensions
{
    public static SendBirdUsers ToTableEntry(this SendBirdUser user)
    {
        return new()
        {
            BoroUserId = user.BoroUserId,
            SendBirdUserId = user.SendBirdUserId,
            AccessToken = user.AccessToken,
            Nickname = user.Nickname,
        };
    }

    public static SendBirdUser ToSendBirdUser(this SendBirdUsers entry)
    {
        return new(entry.BoroUserId, entry.SendBirdUserId, entry.AccessToken, entry.Nickname);
    }

}
