namespace ItemService.API.Models.Output;

public class SearchResult
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public ItemImage? Image { get; set; }
}
