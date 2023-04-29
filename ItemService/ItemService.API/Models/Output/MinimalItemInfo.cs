namespace ItemService.API.Models.Output;

public class MinimalItemInfo
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
}
