namespace Boro.Authentication.Interfaces;

public interface IBoroAuthService
{
    string GenerateJwtToken(Guid userId, params (AdditionalClaims claim, string value)[] additionalClaims);
}
