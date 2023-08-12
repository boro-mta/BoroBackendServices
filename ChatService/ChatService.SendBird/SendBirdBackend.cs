using Boro.Authentication.Interfaces;
using Boro.EntityFramework.DbContexts.BoroMainDb;
using Boro.SendBird.API;
using ChatService.API;
using Microsoft.Extensions.Logging;

namespace ChatService.SendBird;

public class SendBirdBackend : IChatBackend
{
    private readonly ILogger _logger;
    private readonly BoroMainDbContext _dbContext;
    private readonly ISendBirdClient _sendBirdClient;
    private readonly ISendBirdIdentityBackend _sendBirdIdentity;

    public SendBirdBackend(ILoggerFactory loggerFactory,
                           BoroMainDbContext dbContext,
                           ISendBirdClient sendBirdClient,
                           ISendBirdIdentityBackend sendBirdIdentity)
    {
        _logger = loggerFactory.CreateLogger("ChatService");
        _dbContext = dbContext;
        _sendBirdClient = sendBirdClient;
        _sendBirdIdentity = sendBirdIdentity;
    }

    public async Task SendMessage(Guid fromUser, Guid toUser, string message)
    {
        var sendBirdFrom = await _sendBirdIdentity.GetSendBirdUserAsync(fromUser);
        var sendBirdTo = await _sendBirdIdentity.GetSendBirdUserAsync(toUser);

        await _sendBirdClient.SendAnnouncementAsync(sendBirdFrom, sendBirdTo, message);

    }
}