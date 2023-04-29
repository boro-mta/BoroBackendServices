using Boro.Facebook.Models;

namespace Boro.Facebook.Interfaces;

public interface IFacebookAuthService
{
    Task<FacebookUserInfo?> GetUserInfo(string accessToken);
    Task<(bool valid, FacebookUserInfo?)> ValidateAccessTokenAsync(string accessToken);
}
