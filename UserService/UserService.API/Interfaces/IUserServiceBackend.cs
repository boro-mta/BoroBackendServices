using UserService.API.Models;

namespace UserService.API.Interfaces;

public interface IUserServiceBackend
{
    UserModel GetUser(Guid userId);

    UserProfileModel GetUserProfile(Guid userId);

    Guid CreateUser(UserInput userInput);
}