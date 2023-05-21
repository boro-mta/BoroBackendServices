namespace UserService.API.Models.Output;

public class UserProfileModel
{
    public Guid UserId { get; set; }
    public string FacebookId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string About { get; set; }
    public DateTime DateJoined { get; set; }
    public string Email { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public UserImage? Image { get; set; }
}
