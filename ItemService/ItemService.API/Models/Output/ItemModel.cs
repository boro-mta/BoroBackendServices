namespace ItemService.API.Models.Output;

public class ItemModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<ItemImage> Images { get; set; } = Enumerable.Empty<ItemImage>().ToList();
    public string OwnerId { get; set; } = string.Empty;
    public Dictionary<string, bool>? IncludedExtras { get; set; }

}