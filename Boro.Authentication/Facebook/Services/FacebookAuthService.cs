using Boro.Authentication.Facebook.Interfaces;
using Boro.Authentication.Facebook.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Boro.Authentication.Facebook.Services;

internal class FacebookAuthService : IFacebookAuthService
{
    private readonly ILogger _logger;
    private readonly FacebookAuthSettings _facebookAuthSettings;
    private readonly IHttpClientFactory _httpClientFactory;
    private const string TOKEN_VALIDATION_URL = @"https://graph.facebook.com/debug_token?input_token={0}&access_token={1}|{2}";
    private const string GET_USER_INFO_URL = @"https://graph.facebook.com/me?fields=first_name,last_name,email&access_token={0}";
    public FacebookAuthService(FacebookAuthSettings facebookAuthSettings,
        IHttpClientFactory httpClientFactory,
        ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger("AuthService");
        _facebookAuthSettings = facebookAuthSettings;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<(bool valid, FacebookUserInfo?)> ValidateAccessTokenAsync(string accessToken)
    {
        _logger.LogInformation("ValidateAccessTokenAsync - Attempting to validate [{accessToken}]", accessToken);
        var formattedUrl = string.Format(TOKEN_VALIDATION_URL, accessToken, _facebookAuthSettings.AppId, _facebookAuthSettings.AppSecret);
        try
        {
            var response = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var resultData = JsonSerializer.Deserialize<FacebookTokenValidationResult>(content)?.Data
                ?? throw new Exception($"content from token validation request was deserialized as null - content: {content}");
            bool valid = resultData.IsValid;

            var userInfo = valid ? await GetUserInfo(accessToken) : null;

            return (valid, userInfo);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "ValidateAccessTokenAsync - failed to authenticate [{url}]", formattedUrl);
            return (false, null);
        }
    }

    public async Task<FacebookUserInfo?> GetUserInfo(string accessToken)
    {
        _logger.LogInformation("GetUserInfo - Attempting to validate [{accessToken}]", accessToken);
        var formattedUrl = string.Format(GET_USER_INFO_URL, accessToken);
        try
        {
            var response = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<FacebookUserInfo>(content)
                ?? throw new Exception($"content from token validation request was deserialized as null - content: {content}");
            return result;
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "GetUserInfo - failed to get [{url}]", formattedUrl);
            return null;
        }
    }
}
