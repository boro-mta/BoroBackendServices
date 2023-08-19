using Boro.SendBird.API;
using System.Security.Claims;

namespace Boro.Authentication.Extensions;

internal static class SendBirdExtensions
{
    public static IEnumerable<Claim> ToJwtClaims(this SendBirdUser sendBirdUser)
    {
        return new List<Claim>
        {
            new Claim("sbuid", sendBirdUser.SendBirdUserId.ToString()),
            new Claim("sbat", sendBirdUser.AccessToken.ToString()),
        };
    }
}
