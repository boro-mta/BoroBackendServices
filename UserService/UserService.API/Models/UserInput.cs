using System.ComponentModel.DataAnnotations;

namespace UserService.API.Models;

public class UserInput
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string About { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; }

}
