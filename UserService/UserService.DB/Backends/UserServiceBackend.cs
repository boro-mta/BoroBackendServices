﻿using Boro.Common.Exceptions;
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

        var entry = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId.Equals(userId))
            ?? throw new DoesNotExistException(userId.ToString());
        
        return entry.ToUserProfileModel();
    }

    public async Task UpdateUserInfoAsync(Guid userId, UpdateUserInput input)
    {
        _logger.LogInformation("UpdateUserInfoAsync - updating user {userId} with following info: {@updateInfo}", userId, input);

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
        _logger.LogInformation("UpdateUserInfoAsync - updating user image for {userId}", userId);

        var imageEntry = await _dbContext.UserImages.SingleOrDefaultAsync(u => u.UserId.Equals(userId));
        if (imageEntry is null)
        {
            var imageId = Guid.NewGuid();
            imageEntry = imageInput.ToTableEntry(imageId);
            _dbContext.UserImages.Add(imageEntry);
        }
        else
        {
            imageEntry.UpdateImage(imageInput);
        }

        await _dbContext.SaveChangesAsync();
    }
}