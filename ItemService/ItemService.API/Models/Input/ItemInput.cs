namespace ItemService.API.Models.Input;

public class ItemInput
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<ItemImageInput>? Images { get; set; }
    public string Condition { get; set; } = "";
    public string[] Categories { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
