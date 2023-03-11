using System.Text.Json.Serialization;

namespace ItemService.API.Models;

public class ItemInput
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    [JsonIgnore]
    public string? CoverImage { get; set; }
    [JsonIgnore]
    public List<string>? Images { get; set; }
    public string OwnerId { get; set; } = string.Empty;
}
