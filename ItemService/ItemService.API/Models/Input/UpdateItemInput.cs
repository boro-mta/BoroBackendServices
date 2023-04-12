namespace ItemService.API.Models.Input;
public class UpdateItemInput
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? OwnerId { get; set; } = string.Empty;
    public Dictionary<string, bool>? IncludedExtras { get; set; }
}