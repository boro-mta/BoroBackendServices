using System.Text.Json.Serialization;

namespace Boro.SendBird.Models;

internal class CreateGroupChannelResponse
{
    [JsonPropertyName("channel_url")]
    public string ChannelUrl { get; set; }
}
