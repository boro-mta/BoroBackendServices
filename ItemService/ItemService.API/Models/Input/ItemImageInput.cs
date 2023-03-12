namespace ItemService.API.Models.Input;

public class ItemImageInput
{
    public string FileName { get; set; } = string.Empty;
    public string ImageFormat { get; set; } = string.Empty;
    public string Base64ImageData { get; set; } = string.Empty;
    public bool IsCover { get; set; } = false;
}
