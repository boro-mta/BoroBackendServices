using System.Text.Json.Serialization;

namespace Boro.SendBird.Models;

internal class CreateGroupChannelRequest
{
    [JsonPropertyName("user_ids")]
    public IEnumerable<Guid> UserIds { get; set; }

    [JsonPropertyName("name")]
    public string ChannelName { get; set; }

    [JsonPropertyName("cover_url")]
    public string CoverUrl { get; set; } = "https://img.freepik.com/premium-vector/8-bit-pixel-coffee-cup-logo-image-drink-vector-illustration-game-icon_614713-358.jpg?w=740";

    [JsonPropertyName("is_distinct")]
    public bool IsDistinct { get; set; } = true;
}
