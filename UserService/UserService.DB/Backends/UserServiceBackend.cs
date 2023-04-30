using Boro.Common.Exceptions;
using Boro.EntityFramework.DbContexts.BoroMainDb;
using Boro.Facebook.Interfaces;
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
    private readonly IFacebookAuthService _facebookAuthService;

    public UserServiceBackend(ILoggerFactory loggerFactory,
        BoroMainDbContext dbContext,
        IFacebookAuthService facebookAuthService)
    {
        _logger = loggerFactory.CreateLogger("UserService");
        _dbContext = dbContext;
        _facebookAuthService = facebookAuthService;
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

    public async Task<UserLoginInfo> LoginWithFacebookAsync(string accessToken, string facebookId)
    {
        var (valid, facebookUser) = await _facebookAuthService.ValidateAccessTokenAsync(accessToken);
        if (!valid || facebookUser is null)
        {
            throw new Exception("Could not authenticate user");
        }
        if (!facebookUser.Id.Equals(facebookId)) 
        {
            throw new Exception("provided facebook id is different than the facebook id that was returned from facebook");
        }
        _logger.LogInformation("LoginWithFacebookAsync - user with {@facebookId} was authenticated. {@facebookUser}",
            facebookId, facebookUser);

        var userInfo = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.FacebookId.Equals(facebookId));
        if (userInfo is not null)
        {
            return new UserLoginInfo
            {
                UserId = userInfo.UserId,
                FirstLogin = false
            };
        }
        _logger.LogInformation("LoginWithFacebook - user with facebookId: [{facebookId}] does not exist in db. Creating a new user.",
             facebookId);

        var userId = Guid.NewGuid();
        var dateJoined = DateTime.UtcNow;
        await _dbContext.Users.AddAsync(new()
        {
            UserId = userId,
            FacebookId = facebookId,
            FirstName = facebookUser.FirstName,
            LastName = facebookUser.LastName,
            DateJoined = dateJoined
        });

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("LoginWithFacebook - new user created with user id: [{userId}]",
             userId);

        return new UserLoginInfo
        {
            UserId = userId,
            FirstLogin = true
        };
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