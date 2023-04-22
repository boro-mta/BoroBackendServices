using System.ComponentModel.DataAnnotations;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

public class Users
{
    [Key]
    public Guid UserId { get; set; }
    //[Key]
    //public string FacebookId { get; set; } = string.Empty;
    //public string FacebookAccessToken { get; set; } = string.Empty;
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string About { get; set; } = string.Empty;
    public DateTime DateJoined { get; set; }
}
