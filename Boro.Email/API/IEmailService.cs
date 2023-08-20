namespace Boro.Email.API;

public interface IEmailService
{
    Task SendHTMLEmailAsync(IEnumerable<string> recepients, string emailTitle, string HTMLEmailBody);
    Task SendTextEmailAsync(IEnumerable<string> recepients, string emailTitle, string textEmailBody);
}
