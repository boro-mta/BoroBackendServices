using Boro.Authentication.Interfaces;
using Boro.EntityFramework.DbContexts.BoroMainDb;
using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using Boro.SendBird.API;
using ChatService.API;
using Microsoft.EntityFrameworkCore;
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

    public async Task SendMessageAsync(Guid fromUser, Guid toUser, string message)
    {
        var sendBirdFrom = await _sendBirdIdentity.GetSendBirdUserAsync(fromUser);
        var sendBirdTo = await _sendBirdIdentity.GetSendBirdUserAsync(toUser);

        var sendBirdChannel = await _dbContext.SendBirdChannels.FirstOrDefaultAsync(c => (c.UserA.Equals(sendBirdFrom.SendBirdUserId) && c.UserB.Equals(sendBirdTo.SendBirdUserId))
                                                                                         || (c.UserA.Equals(sendBirdTo.SendBirdUserId) && c.UserB.Equals(sendBirdFrom.SendBirdUserId)));
        string channelUrl = sendBirdChannel?.ChannelUrl ?? "";                           
        if (channelUrl is null or "")
        {
            channelUrl = await _sendBirdClient.CreateGroupChannel(sendBirdFrom, sendBirdTo);
            var newChannelEntry = new SendBirdChannels
            {
                UserA = sendBirdFrom.SendBirdUserId,
                UserB = sendBirdTo.SendBirdUserId,
                ChannelUrl = channelUrl
            };
            await _dbContext.SendBirdChannels.AddAsync(newChannelEntry);
            await _dbContext.SaveChangesAsync();
        }

        await _sendBirdClient.SendMessageToChannelAsync(sendBirdFrom, channelUrl, message);
        
    }
}