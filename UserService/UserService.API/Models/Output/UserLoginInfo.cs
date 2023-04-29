namespace UserService.API.Models.Output;

public class UserLoginInfo
{
    public Guid UserId { get; set; }
    public bool FirstLogin { get; set; }
}