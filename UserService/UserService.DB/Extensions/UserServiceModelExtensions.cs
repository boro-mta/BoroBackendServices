using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using UserService.API.Models;

namespace UserService.DB.Extensions
{
    internal static class UserServiceModelExtensions
    {
        internal static UserModel ToUserModel(this Users entry)
        {
            return new UserModel
            {
            };
        }

        internal static UserProfileModel ToUserProfileModel(this Users entry)
        {
            return new UserProfileModel
            {
                FirstName = entry.FirstName,
                LastName = entry.LastName,
                Email = entry.Email,
                About = entry.About,
                DateJoined = entry.DateJoined,
                UserId = entry.UserId,
            };
        }

        internal static Users ToTableEntry(this UserInput input, Guid userId, DateTime joined)
        {
            return new Users
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                Email = input.Email,
                About = input.About,
                DateJoined = joined,
                UserId = userId,
            };
        }

        internal static Users ToUserServicesTableEntry(this UserModel templateModel)
        {
            return new Users
            {
            };
        }
    }
}