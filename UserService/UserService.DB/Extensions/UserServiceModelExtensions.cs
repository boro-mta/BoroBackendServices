using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using UserService.API.Models.Input;
using UserService.API.Models.Output;

namespace UserService.DB.Extensions;

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
            FacebookId = entry.FacebookId,
            Latitude = entry.Latitude,
            Longitude = entry.Longitude,
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
            FacebookId = input.FacebookId,
            Latitude = input.Latitude,
            Longitude = input.Longitude,
        };
    }

    internal static Users UpdateUser(this Users entry, UpdateUserInput input)
    {
        entry.About = input.About;
        entry.Email = input.Email;
        entry.Latitude = input.Latitude;
        entry.Longitude = input.Longitude;
        return entry;
    }
}