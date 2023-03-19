namespace ItemService.API.Models.Input;

public class ItemInput
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<ItemImageInput>? Images { get; set; }
    public string OwnerId { get; set; } = string.Empty;
    public Dictionary<string, bool>? IncludedExtras { get; set; }
}
