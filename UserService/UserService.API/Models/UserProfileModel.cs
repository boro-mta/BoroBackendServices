namespace UserService.API.Models
{
    public class UserProfileModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string About { get; set; } = string.Empty;
        public DateTime DateJoined { get; set; }
        public string Email { get; set; }
    }
}