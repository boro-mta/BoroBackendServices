using Boro.EntityFramework.DbContexts.BoroMainDb;
using Boro.SendBird.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Boro.Authentication.Interfaces;
using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

namespace Boro.Authentication.Services;

internal class SendBirdIdentityBackend : ISendBirdIdentityBackend
{
    private readonly ILogger _logger;
    private readonly BoroMainDbContext _dbContext;
    private readonly ISendBirdClient _sendBirdClient;

    public SendBirdIdentityBackend(ILoggerFactory loggerFactory,
                                   BoroMainDbContext dbContext,
                                   ISendBirdClient sendBirdClient)
    {
        _logger = loggerFactory.CreateLogger("AuthService");
        _dbContext = dbContext;
        _sendBirdClient = sendBirdClient;
    }

    public async Task<SendBirdUser> GetSendBirdUserAsync(Guid boroUserId)
    {
        var sendBirdUserEntry = await _dbContext.SendBirdUsers.SingleOrDefaultAsync(u => u.BoroUserId == boroUserId);

        if (sendBirdUserEntry is null)
        {
            var boroUser = await _dbContext.Users.SingleAsync(u => u.UserId.Equals(boroUserId));
            var newUser = await _sendBirdClient.CreateNewUserAsync(boroUserId, $"{boroUser.LastName}, {boroUser.FirstName}");
            
            var entry = new SendBirdUsers()
            {
                BoroUserId = newUser.BoroUserId,
                SendBirdUserId = newUser.SendBirdUserId,
                AccessToken = newUser.AccessToken,
                Nickname = newUser.Nickname,
            };

            await _dbContext.SendBirdUsers.AddAsync(entry);
            await _dbContext.SaveChangesAsync();
            return newUser;
        }
        else
        {
            return new(sendBirdUserEntry.BoroUserId, sendBirdUserEntry.SendBirdUserId, sendBirdUserEntry.AccessToken, sendBirdUserEntry.Nickname);
        }
    }
}
