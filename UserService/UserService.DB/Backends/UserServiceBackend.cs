using Boro.EntityFramework.DbContexts.BoroMainDb;
using Microsoft.Extensions.Logging;
using UserService.API.Interfaces;
using UserService.API.Models;
using UserService.DB.Extensions;

namespace UserService.DB.Backends
{
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

        public Guid CreateUser(UserInput userInput)
        {
            Guid userId = Guid.NewGuid();
            DateTime joined = DateTime.UtcNow;

            var entry = userInput.ToTableEntry(userId, joined);

            _dbContext.Users.Add(entry);

            _dbContext.SaveChanges();

            return userId;
        }

        public UserModel GetUser(Guid userId)
        {
            var usersQ = from u in _dbContext.Users
                        where u.UserId == userId
                        select u;

            return usersQ.First().ToUserModel();
        }

        public UserProfileModel GetUserProfile(Guid userId)
        {
            var usersQ = from u in _dbContext.Users
                         where u.UserId == userId
                         select u;

            return usersQ.First().ToUserProfileModel();
        }
    }
}