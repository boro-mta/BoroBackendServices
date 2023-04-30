using System.ComponentModel.DataAnnotations;

namespace UserService.API.Models.Input;

public class UserInput
{
    public string FacebookId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string About { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

}
