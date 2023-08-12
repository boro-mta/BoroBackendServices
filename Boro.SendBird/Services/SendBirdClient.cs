using Boro.SendBird.API;
using Boro.SendBird.Extensions;
using Boro.SendBird.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Boro.SendBird.Services;

internal class SendBirdClient : ISendBirdClient
{
    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;
    public SendBirdClient(ILoggerFactory loggerFactory,
                          HttpClient httpClient,
                          SendBirdConfiguration sendBirdConfiguration)
    {
        _logger = loggerFactory.CreateLogger("ChatService");
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri($@"https://api-{sendBirdConfiguration.AppId}.sendbird.com");
        _httpClient.DefaultRequestHeaders.Add("Api-Token", sendBirdConfiguration.ApiToken);
    }

    public async Task<SendBirdUser> CreateNewUserAsync(Guid boroUserId, string nickname)
    {
        const string endpoint = "/v3/users";

        var sendBirdUserId = Guid.NewGuid();

        var requestBody = sendBirdUserId.ToCreateUserRequest(nickname);

        var response = await _httpClient.PostAsJsonAsync(endpoint, requestBody);

        response.EnsureSuccessStatusCode();

        _logger.LogInformation("CreateNewUserAsync - created a new user with id: [{sendBirdId}] for boro user: [{boroUserId}] Successfuly.",
            sendBirdUserId, boroUserId);

        var responseObject = await response.Content.ReadFromJsonAsync<CreateUserResponse>();

        return responseObject?.ToSendBirdUser(boroUserId) ?? throw new Exception("Error when trying to convert a CreateUserResponse object to a SendBirdUser object");

    }

    public async Task SendAnnouncementAsync(SendBirdUser from, SendBirdUser to, string message)
    {
        const string endpoint = "/v3/announcements";

        var requestBody = from.CreateAnnouncementRequest(to, message);

        var content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(endpoint, content);

        response.EnsureSuccessStatusCode();

        _logger.LogInformation("SendAnnouncementAsync - sent an announcement from [{fromUser}] to [{toUser}] successfully.",
            from.BoroUserId, to.BoroUserId);
    }
}
