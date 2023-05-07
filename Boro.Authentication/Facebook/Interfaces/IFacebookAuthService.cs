using Boro.Authentication.Facebook.Models;

namespace Boro.Authentication.Facebook.Interfaces;

public interface IFacebookAuthService
{
    Task<FacebookUserInfo?> GetUserInfo(string accessToken);
    Task<(bool valid, FacebookUserInfo?)> ValidateAccessTokenAsync(string accessToken);
}
