namespace ItemService.API.Models;

public class ItemInput
{
    public string? Name { get; set; }
    public string? CoverImage { get; set; }
    public List<string>? Images { get; set; }
    public string? Description { get; set; }
}
