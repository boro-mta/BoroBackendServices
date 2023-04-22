using Boro.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserService.API.Interfaces;
using UserService.API.Models;

namespace UserService.Controller.Controllers
{
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
            var guid = _backend.CreateUser(userInput);

            return Ok(guid);
        }

        [HttpGet("{userId}/Profile")]
        [ValidatesGuid("userId")]
        public ActionResult<UserProfileModel> GetUserProfile(string userId)
        {
            _logger.LogInformation("GetUserProfile was called with id: [{userId}]", userId);

            var guid = Guid.Parse(userId);
            var userProfile = _backend.GetUserProfile(guid);

            _logger.LogInformation("GetUserProfile - Finished with: [{@userProfile}]", userProfile);

            return Ok(userProfile);
        }
    }
}