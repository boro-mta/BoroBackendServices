using System.Text.Json.Serialization;

namespace Boro.Facebook.Models;

internal class FacebookTokenValidationResultData
{
    [JsonPropertyName("app_id")]
    public string AppId { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("application")]
    public string Application { get; set; }

    [JsonPropertyName("data_access_expires_at")]
    public int DataAccessExpiresAt { get; set; }

    [JsonPropertyName("expires_at")]
    public int ExpiresAt { get; set; }

    [JsonPropertyName("is_valid")]
    public bool IsValid { get; set; }

    [JsonPropertyName("issued_at")]
    public int IssuedAt { get; set; }

    [JsonPropertyName("scopes")]
    public List<string> Scopes { get; set; }

    [JsonPropertyName("user_id")]
    public string UserId { get; set; }
}
