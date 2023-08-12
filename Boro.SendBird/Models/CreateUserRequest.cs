using System.Text.Json.Serialization;

namespace Boro.SendBird.Models;

internal class CreateUserRequest
{
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }
    [JsonPropertyName("issue_access_token")]
    public bool IssueAccessToken { get; set; }
    [JsonPropertyName("nickname")]
    public string Nickname { get; set; }
    [JsonPropertyName("profile_url")]
    public string ProfileUrl { get; set; } = "";

}
