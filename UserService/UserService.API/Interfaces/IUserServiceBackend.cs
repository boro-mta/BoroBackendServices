using UserService.API.Models.Input;
using UserService.API.Models.Output;

namespace UserService.API.Interfaces;

public interface IUserServiceBackend
{
    Task<UserProfileModel> GetUserProfileAsync(Guid userId);
    Task UpdateUserImageAsync(Guid userId, UserImageInput imageInput);
    Task UpdateUserInfoAsync(Guid userId, UpdateUserInput input);
}
