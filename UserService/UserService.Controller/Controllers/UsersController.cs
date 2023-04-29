using Boro.Common.Exceptions;
using Boro.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserService.API.Interfaces;
using UserService.API.Models.Input;
using UserService.API.Models.Output;

namespace UserService.Controller.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IUserServiceBackend _backend;

    public UsersController(ILoggerFactory loggerFactory,
        IUserServiceBackend backend)
    {
        _logger = loggerFactory.CreateLogger("UserService");
        _backend = backend;
    }

    [HttpGet("{userId}")]
    [ValidatesGuid("userId")]
    public ActionResult<UserModel> GetUser(string userId)
    {
        //_logger.LogInformation("GetUserService was called with id: [{id}]", id);
        //var guid = Guid.Parse(userId);
        //var template = _backend.GetUser(guid);

        //_logger.LogInformation("GetUserService - Finished with: [{@template}]", template);

        return Ok();
    }

    [HttpPost("Create")]
    public ActionResult<Guid> CreateUser(UserInput userInput)
    {
        var guid = _backend.CreateUserAsync(userInput).Result;

        return Ok(guid);
    }

    [HttpPost("{userId}/Update")]
    [ValidatesGuid("userId")]
    public ActionResult UpdateUser(string userId, UpdateUserInput updateInput)
    {
        try
        {
            var guid = Guid.Parse(userId);
            _backend.UpdateUserInfoAsync(guid, updateInput).Wait();
            return Ok();
        }
        catch (DoesNotExistException)
        {
            return NotFound($"user [{userId}] was not found");
        }

    }

    [HttpGet("{userId}/Profile")]
    [ValidatesGuid("userId")]
    public ActionResult<UserProfileModel> GetUserProfile(string userId)
    {
        _logger.LogInformation("GetUserProfile was called with id: [{userId}]", userId);

        var guid = Guid.Parse(userId);
        var userProfile = _backend.GetUserProfileAsync(guid).Result;

        _logger.LogInformation("GetUserProfile - Finished with: [{@userProfile}]", userProfile);

        return Ok(userProfile);
    }

    [HttpPost("LoginWithFacebook")]
    public ActionResult<UserLoginInfo> LoginWithFacebook(string accessToken, string facebookId)
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