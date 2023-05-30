using Boro.Common.Exceptions;
using Boro.EntityFramework.DbContexts.BoroMainDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.API.Interfaces;
using UserService.API.Models.Input;
using UserService.API.Models.Output;
using UserService.DB.Extensions;

namespace UserService.DB.Backends;

public class UserServiceBackend : IUserServiceBackend
{
    private readonly ILogger _logger;
    private readonly BoroMainDbContext _dbContext;

    public UserServiceBackend(ILoggerFactory loggerFactory,
        BoroMainDbContext dbContext)
    {
        _logger = loggerFactory.CreateLogger("UserService");
        _dbContext = dbContext;
    }

    public async Task<UserProfileModel> GetUserProfileAsync(Guid userId)
    {
        _logger.LogInformation("GetUserProfileAsync - getting user profile for {userId}", userId);

        var entry = await _dbContext.Users
            .Include(user => user.Image)
            .FirstOrDefaultAsync(u => u.UserId.Equals(userId))
            ?? throw new DoesNotExistException(userId.ToString());
        
        return entry.ToUserProfileModel();
    }

    public async Task UpdateUserInfoAsync(Guid userId, UpdateUserInput input)
    {
        _logger.LogInformation("UpdateUserInfoAsync - updating user {userId} with following info: {about}, {email}, {latitude}, {longitude}, Image Data:  size: {imageSize}, metaData: {metaData}" ,
            userId, input.About, input.Email, input.Latitude, input.Longitude, input.Image?.Base64ImageData.Length, input.Image?.Base64ImageMetaData);

        var entry = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId.Equals(userId))
            ?? throw new DoesNotExistException(userId.ToString());

        entry.UpdateUser(input);
        
        if (input?.Image is not null)
        {
            await UpdateUserImageAsync(userId, input.Image);
        }
        else
        {
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task UpdateUserImageAsync(Guid userId, UserImageInput imageInput)
    {
        _logger.LogInformation("UpdateUserInfoAsync - updating user image for {userId} with image data: size: {imageSize}, metaData: {metaData}", 
            userId, imageInput.Base64ImageData.Length, imageInput.Base64ImageMetaData);

        

        var imageEntry = await _dbContext.UserImages.SingleOrDefaultAsync(u => u.UserId.Equals(userId));
        if (imageEntry is null)
        {
            var imageId = Guid.NewGuid();
            imageEntry = imageInput.ToTableEntry(imageId, userId);
            await _dbContext.UserImages.AddAsync(imageEntry);

            var userEntry = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId.Equals(userId))
                ?? throw new DoesNotExistException(userId.ToString());

            userEntry.ImageId = imageId;
            _dbContext.Update(userEntry);
        }
        else
        {
            imageEntry.UpdateImage(imageInput);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<LocationDetails> GetUserLocationAsync(Guid userId)
    {
        var locationDetailsQ = from user in _dbContext.Users
                              where user.UserId == userId
                              select new LocationDetails(user.Latitude, user.Longitude);

        var locationDetails = await locationDetailsQ.FirstOrDefaultAsync()
            ?? throw new DoesNotExistException(userId.ToString());

        return locationDetails;
    }

    public async Task<UserImage> GetUserPictureAsync(Guid userId)
    {
        var userImageQ = from image in _dbContext.UserImages
                         where image.UserId == userId
                         select image;
        var firstImage = await userImageQ.FirstOrDefaultAsync()
            ?? throw new DoesNotExistException(userId.ToString());

        return firstImage.ToUserImage();
    }

    public async Task UpateUserLocationAsync(Guid userId, double latitude, double longitude)
    {
        var entry = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId.Equals(userId))
            ?? throw new DoesNotExistException(userId.ToString());

        entry.Latitude = latitude;
        entry.Longitude = longitude;

        await _dbContext.SaveChangesAsync();
    }
}