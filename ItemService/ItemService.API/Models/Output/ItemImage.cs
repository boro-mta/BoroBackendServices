namespace ItemService.API.Models.Output;

public class ItemImage
{
    public string FileName { get; set; } = string.Empty;
    public string ImageFormat { get; set; } = string.Empty;
    public string Base64ImageData { get; set; } = string.Empty;
    public bool IsCover { get; set; }
}
