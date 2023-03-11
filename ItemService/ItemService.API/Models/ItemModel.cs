namespace ItemService.API.Models;

public class ItemModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? CoverImage { get; set; }
    public List<string>? Images { get; set; }
    public string OwnerId { get; set; } = string.Empty;
}