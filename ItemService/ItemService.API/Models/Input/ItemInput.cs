namespace ItemService.API.Models.Input;

public class ItemInput
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<ItemImageInput>? Images { get; set; }
    public Guid? OwnerId { get; set; }
    public string Condition { get; set; } = "";
    public string[] Categories { get; set; }
}
