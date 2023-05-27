using Boro.Authentication.Facebook.Models;
using Boro.EntityFramework.DbContexts.BoroMainDb.Extensions;
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
        Image = entry.Image?.ToUserImage(),
    };

    internal static UserImage ToUserImage(this UserImages entry)
    {
        return new UserImage
        {
            ImageId = entry.ImageId,
            Base64ImageData = entry.ImageData.ToBase64String(),
            Base64ImageMetaData = entry.ImageMetaData,
        };
    }

    internal static Users ToTableEntry(this FacebookUserInfo facebookUserInfo, Guid userId, DateTime dateJoined) => new()
    {
        UserId = userId,
        FacebookId = facebookUserInfo.Id,
        FirstName = facebookUserInfo.FirstName,
        LastName = facebookUserInfo.LastName,
        DateJoined = dateJoined,
        Email = facebookUserInfo.Email ?? "",
        About = "",
    };

    internal static UserImages ToTableEntry(this UserImageInput input, Guid imageId) => new()
    {
        ImageId = imageId,
        ImageData = input.Base64ImageData.FromBase64String(),
        ImageMetaData = input.Base64ImageMetaData
    };

    internal static void UpdateImage(this UserImages entry, UserImageInput input) 
    {
        entry.ImageData = input.Base64ImageData.FromBase64String();
        entry.ImageMetaData = input.Base64ImageMetaData;
    }

    internal static void UpdateUser(this Users entry, UpdateUserInput input)
    {
        entry.About = input.About;
        entry.Email = input.Email;
        entry.Latitude = input.Latitude;
        entry.Longitude = input.Longitude;
    }
}