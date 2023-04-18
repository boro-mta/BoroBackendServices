namespace ItemService.API.Models.Output;

public class ItemModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<ItemImage> Images { get; set; } = Enumerable.Empty<ItemImage>().ToList();
    public string? OwnerId { get; set; }
    public string[] Categories { get; set; }
    public string Condition { get; set; }

}