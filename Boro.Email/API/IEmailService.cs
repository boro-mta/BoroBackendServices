using Boro.Email.Models;

namespace Boro.Email.API;

public interface IEmailService
{
    Task<EmailResults> SendEmail(string email, string title, string message);
}
