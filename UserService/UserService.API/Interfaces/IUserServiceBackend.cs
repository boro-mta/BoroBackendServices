using UserService.API.Models.Input;
using UserService.API.Models.Output;

namespace UserService.API.Interfaces;

public interface IUserServiceBackend
{
    Task<LocationDetails> GetUserLocationAsync(Guid userId);
    Task<UserImage> GetUserPictureAsync(Guid userId);
    Task<UserProfileModel> GetUserProfileAsync(Guid userId);
    Task UpateUserLocationAsync(Guid userId, double latitude, double longitude);
    Task UpdateUserImageAsync(Guid userId, UserImageInput imageInput);
    Task UpdateUserInfoAsync(Guid userId, UpdateUserInput input);
}
