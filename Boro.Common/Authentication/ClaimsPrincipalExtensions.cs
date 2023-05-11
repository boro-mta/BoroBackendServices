using System.Security.Claims;

namespace Boro.Common.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static Guid? UserIdOrDefault(this ClaimsPrincipal user)
    {
        var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(id, out var guid))
        {
            return guid;
        }
        return default;
    }

    public static Guid UserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(id, out var guid))
        {
            return guid;
        }
        throw new InvalidDataException($"{id} is not a valid guid");
    }
}
