using Boro.Authentication.Models;

namespace Boro.Authentication.Interfaces;

public interface IBoroAuthService
{
    TokenDetails GenerateJwtToken(Guid userId, params (AdditionalClaims claim, string? value)[] additionalClaims);
}
