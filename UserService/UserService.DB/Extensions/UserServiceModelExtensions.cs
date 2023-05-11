using Boro.Authentication.Facebook.Models;
using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using UserService.API.Models.Input;
using UserService.API.Models.Output;

namespace UserService.DB.Extensions;

internal static class UserServiceModelExtensions
{
    internal static UserProfileModel ToUserProfileModel(this Users entry) => new()
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

    internal static Users ToTableEntry(this FacebookUserInfo facebookUserInfo, Guid userId, DateTime dateJoined) => new()
    {
        UserId = userId,
        FacebookId = facebookUserInfo.Id,
        FirstName = facebookUserInfo.FirstName,
        LastName = facebookUserInfo.LastName,
        DateJoined = dateJoined,
        Email = facebookUserInfo.Email ?? ""
    };

    internal static Users UpdateUser(this Users entry, UpdateUserInput input) => new()
    {
        UserId = entry.UserId,
        FirstName = entry.FirstName,
        LastName = entry.LastName,
        FacebookId = entry.FacebookId,
        DateJoined = entry.DateJoined,
        About = input.About,
        Email = input.Email,
        Latitude = input.Latitude,
        Longitude = input.Longitude,
    };
}