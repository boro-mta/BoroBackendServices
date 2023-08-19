using System.Text.Json.Serialization;

namespace Boro.SendBird.Models;

internal class SendMessageRequest
{
    [JsonPropertyName("message_type")]
    public string MessageType { get; set; } = "MESG";

    [JsonPropertyName("user_id")]
    public Guid UserId { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}
