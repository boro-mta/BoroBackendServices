using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserService.API.Interfaces;
using UserService.API.Models.Output;
using Boro.Common.Authentication;
using Boro.Authentication.Models;

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
    public ActionResult<TokenDetails> RefreshToken()
    {
        var userId = User.UserId();

        var tokenDetails = _backend.RefreshTokenAsync(userId).Result;

        return Ok(tokenDetails);
    }

    [HttpPost("LoginWithFacebook")]
    public ActionResult<UserLoginResults> LoginWithFacebook(string accessToken, string facebookId)
    {
        try
        {
            _logger.LogInformation("LoginWithFacebook was called with accessToken: [{accessToken}]",
                accessToken);

            var loginInfo = _backend.LoginWithFacebookAsync(accessToken, facebookId).Result;

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
}
