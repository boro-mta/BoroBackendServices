using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserService.API.Interfaces;
using UserService.API.Models.Output;
using Boro.Common.Authentication;
using Boro.Authentication.Models;
using Boro.Common.Exceptions;

namespace UserService.Controller.Controllers;

[Route("[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IIdentityServiceBackend _backend;

    public IdentityController(ILoggerFactory loggerFactory,
        IIdentityServiceBackend backend)
    {
        _logger = loggerFactory.CreateLogger("UserService");
        _backend = backend;
    }

    [HttpPost("RefreshToken")]
    [Authorize]
    public async Task<ActionResult<TokenDetails>> RefreshToken()
    {
        var userId = User.UserId();

        var tokenDetails = await _backend.RefreshTokenAsync(userId);

        return Ok(tokenDetails);
    }

    [HttpPost("LoginWithFacebook")]
    public async Task<ActionResult<UserLoginResults>> LoginWithFacebook(string accessToken, string facebookId)
    {
        try
        {
            _logger.LogInformation("LoginWithFacebook was called with accessToken: [{accessToken}]",
                accessToken);

            var loginInfo = await _backend.LoginWithFacebookAsync(accessToken, facebookId);

            _logger.LogInformation("LoginWithFacebook - Finished with: [{@loginInfo}]",
                loginInfo);

            return Ok(loginInfo);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "LoginWithFacebook - could not authenticate {@accessToken}", accessToken);
            return Unauthorized();
        }
    }

    [HttpGet("GetUserToken/{userId}")]
    public async Task<ActionResult<TokenDetails>> GetUserToken(string userId)
    {
        try
        {
            _logger.LogInformation("GetUserToken was called with userId: [{userId}]",
                userId);
            var guid = Guid.Parse(userId);
            var tokenInfo = await _backend.RefreshTokenAsync(guid);

            _logger.LogInformation("GetUserToken - Finished with: [{@tokenInfo}]",
                tokenInfo);

            return Ok(tokenInfo);
        }
        catch (DoesNotExistException)
        {
            return NotFound($"user [{userId}] does not exist");
        }
    }
}
