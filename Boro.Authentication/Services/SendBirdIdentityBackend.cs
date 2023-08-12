using Boro.EntityFramework.DbContexts.BoroMainDb;
using Boro.SendBird.API;
using Boro.SendBird.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Boro.SendBird.Extensions;
using Boro.Authentication.Interfaces;

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
            var entry = newUser.ToTableEntry();
            await _dbContext.SendBirdUsers.AddAsync(entry);
            await _dbContext.SaveChangesAsync();
            return newUser;
        }
        else
        {
            return sendBirdUserEntry.ToSendBirdUser();
        }
    }
}
