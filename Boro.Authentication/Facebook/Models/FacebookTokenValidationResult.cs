using System.Text.Json.Serialization;

namespace Boro.Authentication.Facebook.Models;

internal class FacebookTokenValidationResult
{
    [JsonPropertyName("data")]
    public FacebookTokenValidationResultData? Data { get; set; }
}
