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

    public async Task<Guid> CreateUserAsync(UserInput userInput)
    {
        Guid userId = Guid.NewGuid();
        DateTime joined = DateTime.UtcNow;

        var entry = userInput.ToTableEntry(userId, joined);

        await _dbContext.Users.AddAsync(entry);

        await _dbContext.SaveChangesAsync();

        return userId;
    }

    public async Task<UserModel> GetUserAsync(Guid userId)
    {
        var usersQ = from u in _dbContext.Users
                     where u.UserId == userId
                     select u;
        var first = await usersQ.FirstAsync();
        return first.ToUserModel();
    }

    public async Task<UserProfileModel> GetUserProfileAsync(Guid userId)
    {
        var usersQ = from u in _dbContext.Users
                     where u.UserId == userId
                     select u;
        var first = await usersQ.FirstAsync();
        return first.ToUserProfileModel();
    }

    public async Task UpdateUserInfoAsync(Guid userId, UpdateUserInput input)
    {
        var entry = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId.Equals(userId))
            ?? throw new DoesNotExistException(userId.ToString());

        var updatedEntry = entry.UpdateUser(input);

        _dbContext.Users.Update(entry);
        await _dbContext.SaveChangesAsync();
    }
}