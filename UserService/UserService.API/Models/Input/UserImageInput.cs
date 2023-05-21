namespace UserService.API.Models.Input;

public class UserImageInput
{
    public string Base64ImageMetaData { get; set; } = string.Empty;
    public string Base64ImageData { get; set; } = string.Empty;
}
