namespace ItemService.API.Models.Input;

public class SearchRequestData
{
    public (double lat, double lon) Location { get; set; }
    public int RadiusInMeters { get; set; } = 5000;
    public int ResultsLengthLimit { get; set; } = 100;
}
