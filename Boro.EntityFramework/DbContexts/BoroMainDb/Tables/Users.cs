using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Boro.EntityFramework.DbContexts.BoroMainDb.Tables;

[PrimaryKey("UserId", "FacebookId")]
public class Users
{
    public Guid UserId { get; set; }
    public string FacebookId { get; set; }
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string About { get; set; } = string.Empty;
    public DateTime DateJoined { get; set; } = DateTime.UtcNow;
    public double Latitude { get; set; } = 0;
    public double Longitude { get; set; } = 0;
}
