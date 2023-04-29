using System.Text.Json.Serialization;

namespace Boro.Facebook.Models;

public class FacebookUserInfo
{
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string LastName { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }
}
