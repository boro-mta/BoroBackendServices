using Boro.Authentication.Facebook.Interfaces;
using Boro.Authentication.Facebook.Models;
using Boro.Authentication.Interfaces;
using Boro.Common.Exceptions;
using Boro.EntityFramework.DbContexts.BoroMainDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.API.Interfaces;
using UserService.API.Models.Output;

namespace UserService.DB.Backends;

public class IdentityServiceBackend : IIdentityServiceBackend
{
    private readonly ILogger _logger;
    private readonly BoroMainDbContext _dbContext;
    private readonly IFacebookAuthService _facebookAuthService;
    private readonly IBoroAuthService _boroAuthService;

    public IdentityServiceBackend(ILoggerFactory loggerFactory,
        BoroMainDbContext dbContext,
        IFacebookAuthService facebookAuthService,
        IBoroAuthService boroAuthService)
    {
        _logger = loggerFactory.CreateLogger("AuthService");
        _dbContext = dbContext;
        _facebookAuthService = facebookAuthService;
        _boroAuthService = boroAuthService;
    }

    public async Task<UserLoginResults> LoginWithFacebookAsync(string accessToken, string facebookId)
    {
        var facebookUser = await ValidateAccessToken(accessToken, facebookId);

        var additionalClaims = new List<(AdditionalClaims claim, string value)>
        {
            (AdditionalClaims.FacebookId, facebookId),
            (AdditionalClaims.FullName, $"{facebookUser.FirstName}_{facebookUser.LastName}")
        };

        var userInfo = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.FacebookId.Equals(facebookId));
        if (userInfo is not null)
        {
            additionalClaims.Add((AdditionalClaims.Email, userInfo.Email));

            return new UserLoginResults
            {
                UserId = userInfo.UserId,
                FirstLogin = false,
                JwtToken = _boroAuthService.GenerateJwtToken(userInfo.UserId, additionalClaims.ToArray())
            };
        }

        _logger.LogInformation("LoginWithFacebook - user with facebookId: [{facebookId}] does not exist in db. Creating a new user.",
             facebookId);

        if (facebookUser.Email is not null and not "")
        {
            additionalClaims.Add((AdditionalClaims.Email, facebookUser.Email));
        }

        var userId = Guid.NewGuid();
        var dateJoined = DateTime.UtcNow;
        await _dbContext.Users.AddAsync(new()
        {
            UserId = userId,
            FacebookId = facebookId,
            FirstName = facebookUser.FirstName,
            LastName = facebookUser.LastName,
            DateJoined = dateJoined,
            Email = facebookUser.Email ?? ""
        });

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("LoginWithFacebook - new user created with user id: [{userId}]",
             userId);

        var jwtToken = _boroAuthService.GenerateJwtToken(userId, additionalClaims.ToArray());

        return new UserLoginResults
        {
            UserId = userId,
            FirstLogin = true,
            JwtToken = jwtToken
        };
    }

    private async Task<FacebookUserInfo> ValidateAccessToken(string accessToken, string facebookId)
    {
        var (valid, facebookUser) = await _facebookAuthService.ValidateAccessTokenAsync(accessToken);
        if (!valid || facebookUser is null)
        {
            throw new Exception("Could not authenticate user");
        }
        if (!facebookUser.Id.Equals(facebookId))
        {
            throw new Exception("provided facebook id does not match the access token");
        }

        _logger.LogInformation("LoginWithFacebookAsync - user with {@facebookId} was authenticated. {@facebookUser}",
            facebookId, facebookUser);

        return facebookUser;
    }

    public Task<string> RefreshTokenAsync(Guid userId)
    {
        var user = _dbContext.Users.FirstOrDefault(x => x.UserId == userId)
            ?? throw new DoesNotExistException(userId.ToString());

        return Task.FromResult(_boroAuthService.GenerateJwtToken(userId, (AdditionalClaims.FacebookId ,user.FacebookId),
            (AdditionalClaims.FullName, $"{user.FirstName}_{user.LastName}"), (AdditionalClaims.Email, user.Email)));
    }
}
