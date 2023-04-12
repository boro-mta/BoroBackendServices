using System.ComponentModel.DataAnnotations;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

public class Users
{
    [Key]
    public string FacebookId { get; set; } = string.Empty;
    public string FacebookAccessToken { get; set; } = string.Empty;
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
