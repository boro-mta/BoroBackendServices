using Boro.Common.Authentication;
using Boro.Common.Exceptions;
using Boro.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserService.API.Interfaces;
using UserService.API.Models.Input;
using UserService.API.Models.Output;

namespace UserService.Controller.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
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

    [HttpPost("Me/Update")]
    public ActionResult UpdateUser(UpdateUserInput updateInput)
    {
        try
        {
            var userId = User.UserId();
            _backend.UpdateUserInfoAsync(userId, updateInput).Wait();
            return Ok();
        }
        catch (DoesNotExistException e)
        {
            return NotFound($"user [{e.Id}] was not found");
        }
    }

    [HttpPost("Me/Update/Image")]
    public ActionResult UpdateUserImage(UserImageInput imageInput)
    {
        try
        {
            var userId = User.UserId();
            _backend.UpdateUserImageAsync(userId, imageInput).Wait();
            return Ok();
        }
        catch (DoesNotExistException e)
        {
            return NotFound($"user [{e.Id}] was not found");
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

    [HttpGet("Me/Profile")]
    public ActionResult<UserProfileModel> GetUserProfile()
    {
        var userId = User.UserId();
        _logger.LogInformation("GetUserProfile was called with id from context: [{userId}]", userId);

        var userProfile = _backend.GetUserProfileAsync(userId).Result;

        _logger.LogInformation("GetUserProfile - Finished with: [{@userProfile}]", userProfile);

        return Ok(userProfile);
    }

}