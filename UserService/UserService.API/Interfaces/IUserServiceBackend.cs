using UserService.API.Models.Input;
using UserService.API.Models.Output;

namespace UserService.API.Interfaces;

public interface IUserServiceBackend
{
    Task<UserModel> GetUserAsync(Guid userId);

    Task<UserProfileModel> GetUserProfileAsync(Guid userId);

    Task<Guid> CreateUserAsync(UserInput userInput);

    Task<UserLoginInfo> LoginWithFacebookAsync(string accessToken, string facebookId);

    Task UpdateUserInfoAsync(Guid userId, UpdateUserInput input);
}