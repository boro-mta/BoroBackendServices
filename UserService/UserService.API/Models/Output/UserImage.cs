namespace UserService.API.Models.Output;

public class UserImage
{
    public Guid ImageId { get; set; }
    public string Base64ImageMetaData { get; set; } = string.Empty;
    public string Base64ImageData { get; set; } = string.Empty;
}