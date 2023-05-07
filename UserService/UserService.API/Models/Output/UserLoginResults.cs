namespace UserService.API.Models.Output;

public class UserLoginResults
{
    public Guid UserId { get; set; }
    public bool FirstLogin { get; set; }
    public string JwtToken { get; set; }
}