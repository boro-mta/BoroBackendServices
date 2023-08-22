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

    [HttpPost("Me/Update/Location")]
    public async Task<ActionResult> UpdateUserLocation(double latitude, double longitude)
    {
        try
        {
            var userId = User.UserId();
            await _backend.UpateUserLocationAsync(userId, latitude, longitude);
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
        try
        {
            _logger.LogInformation("GetUserProfile was called with id: [{userId}]", userId);

            var guid = Guid.Parse(userId);
            var userProfile = await _backend.GetUserProfileAsync(guid);

            _logger.LogInformation("GetUserProfile - Finished");

            return Ok(userProfile);
        }
        catch (DoesNotExistException)
        {
            return NotFound($"user with id: {userId} was not found");
        }
    }

    [HttpGet("Me/Profile")]
    public async Task<ActionResult<UserProfileModel>> GetUserProfile()
    {
        try
        {
            var userId = User.UserId();
            _logger.LogInformation("GetUserProfile was called with id from context: [{userId}]", userId);

            var userProfile = await _backend.GetUserProfileAsync(userId);

            _logger.LogInformation("GetUserProfile - Finished");

            return Ok(userProfile);
        }
        catch (DoesNotExistException)
        {
            return NotFound($"user with id: {User.UserId()} was not found");
        }
    }

    [HttpGet("Me/Location")]
    public async Task<ActionResult<LocationDetails>> GetUserLocation()
    {
        try
        {
            var userId = User.UserId();
            _logger.LogInformation("GetUserLocation was called with id from context: [{userId}]", userId);

            var userLocation = await _backend.GetUserLocationAsync(userId);

            _logger.LogInformation("GetUserLocation - Finished with {@location}", userLocation);

            return Ok(userLocation);
        }
        catch (DoesNotExistException)
        {
            return NotFound($"no profile picture found for user {User.UserId()}");
        }
    }

    [HttpGet("{userId}/Location")]
    [ValidatesGuid("userId")]
    public async Task<ActionResult<LocationDetails>> GetUserLocation(string userId)
    {
        try
        {
            var guid = Guid.Parse(userId);
            _logger.LogInformation("GetUserLocation was called with id: [{userId}]", userId);

            var userLocation = await _backend.GetUserLocationAsync(guid);

            _logger.LogInformation("GetUserLocation - Finished with {@location}", userLocation);

            return Ok(userLocation);
        }
        catch (DoesNotExistException)
        {
            return NotFound($"no profile picture found for user {User.UserId()}");
        }
    }

    [HttpGet("Me/ProfilePicture")]
    public async Task<ActionResult<UserImage>> GetUserPicture()
    {
        try
        {
            var userId = User.UserId();
            _logger.LogInformation("GetUserPicture was called with id from context: [{userId}]", userId);

            var userPicture = await _backend.GetUserPictureAsync(userId);

            _logger.LogInformation("GetUserPicture - Finished");

            return Ok(userPicture);
        }
        catch (DoesNotExistException)
        {
            return NotFound($"no profile picture found for user {User.UserId()}");
        }
    }

    [HttpGet("{userId}/ProfilePicture")]
    [ValidatesGuid("userId")]
    public async Task<ActionResult<UserImage>> GetUserPicture(string userId)
    {
        try
        {
            var guid = Guid.Parse(userId);
            _logger.LogInformation("GetUserPicture was called with id: [{userId}]", userId);

            var userLocation = await _backend.GetUserPictureAsync(guid);

            _logger.LogInformation("GetUserPicture - Finished");

            return Ok(userLocation);
        }
        catch (DoesNotExistException) 
        {
            return NotFound($"no profile picture found for user {userId}");
        }
    }

    [HttpGet("{userId}/Statistics")]
    [ValidatesGuid("userId")]
    public async Task<ActionResult<UserStatistics>> GetUserStatistics(string userId)
    {
        try
        {
            var guid = Guid.Parse(userId);
            _logger.LogInformation("GetUserStatistics was called with id: [{userId}]", userId);

            var statistics = await _backend.GetUserStatisticsAsync(guid);

            _logger.LogInformation("GetUserStatistics - Finished");

            return Ok(statistics);
        }
        catch (DoesNotExistException)
        {
            return NotFound($"no user was found with id {userId}");
        }
    }

    [HttpGet("Leaderboard")]
    public async Task<ActionResult<List<UserStatistics>>> GetLeaderboard()
    {
        try
        {
            var leaderboard = await _backend.GetTop10UserStatistics();

            return Ok(leaderboard);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "GetLeaderboard - Error");

            return BadRequest();
        }
    }

}