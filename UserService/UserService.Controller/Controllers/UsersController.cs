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
    public async Task<ActionResult> UpdateUser(UpdateUserInput updateInput)
    {
        try
        {
            var userId = User.UserId();
            await _backend.UpdateUserInfoAsync(userId, updateInput);
            return Ok();
        }
        catch (DoesNotExistException e)
        {
            return NotFound($"user [{e.Id}] was not found");
        }
    }

    [HttpPost("Me/Update/Image")]
    public async Task<ActionResult> UpdateUserImage(UserImageInput imageInput)
    {
        try
        {
            var userId = User.UserId();
            await _backend.UpdateUserImageAsync(userId, imageInput);
            return Ok();
        }
        catch (DoesNotExistException e)
        {
            return NotFound($"user [{e.Id}] was not found");
        }

    }

    [HttpGet("{userId}/Profile")]
    [ValidatesGuid("userId")]
    public async Task<ActionResult<UserProfileModel>> GetUserProfile(string userId)
    {
        _logger.LogInformation("GetUserProfile was called with id: [{userId}]", userId);

        var guid = Guid.Parse(userId);
        var userProfile = await _backend.GetUserProfileAsync(guid);

        _logger.LogInformation("GetUserProfile - Finished");

        return Ok(userProfile);
    }

    [HttpGet("Me/Profile")]
    public async Task<ActionResult<UserProfileModel>> GetUserProfile()
    {
        var userId = User.UserId();
        _logger.LogInformation("GetUserProfile was called with id from context: [{userId}]", userId);

        var userProfile = await _backend.GetUserProfileAsync(userId);

        _logger.LogInformation("GetUserProfile - Finished");

        return Ok(userProfile);
    }

}