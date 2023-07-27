namespace ItemService.API.Models.Input;

public class UpdateItemInfoInput
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public string Condition { get; set; } = "";
    public string[] Categories { get; set; }
    public List<Guid>? ImagesToRemove { get; set; }
}
