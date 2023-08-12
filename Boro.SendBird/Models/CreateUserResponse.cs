using System.Text.Json.Serialization;

namespace Boro.SendBird.Models;

internal class CreateUserResponse
{
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("nickname")]
    public string Nickname { get; set; }
}
