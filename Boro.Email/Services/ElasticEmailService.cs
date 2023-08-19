using Boro.Email.API;
using Boro.Email.Models;
using ElasticEmail.Api;
using ElasticEmail.Client;
using ElasticEmail.Model;
using Microsoft.Extensions.Logging;

namespace Boro.Email.Services;

internal class ElasticEmailService : IEmailService
{
    private readonly EmailsApi _elasticEmailClient;
    private readonly ILogger _logger;

    public ElasticEmailService(ILoggerFactory loggerFactory,
                               ElasticEmailSettings settings)
    {
        _logger = loggerFactory.CreateLogger("ChatService");
        Configuration config = new();
        config.ApiKey.Add("X-ElasticEmail-ApiKey", settings.ApiKey);
        
        //config.Password = "CAFAB9C0AC8E5F2F34883389005395EBCA60";
        //config.Username = "alonmo@mta.ac.il";
        _elasticEmailClient = new EmailsApi(config);
    }

    public async Task<EmailResults> SendEmail(string email, string title, string message)
    {
        var to = new List<string>
        {
            email
        };

        var recipients = new TransactionalRecipient(to: to);

        var emailData = new EmailTransactionalMessageData(recipients: recipients)
        {
            Content = new EmailContent()
        };
        emailData.Content.Body = new List<BodyPart>();
        //var htmlBodyPart = new BodyPart
        //{
        //    ContentType = BodyContentType.HTML,
        //    Charset = "utf-8",
        //    Content = $"<h1>{message}</h1>"
        //};
        var plainTextBodyPart = new BodyPart
        {
            ContentType = BodyContentType.PlainText,
            Charset = "utf-8",
            Content = message
        };
        //emailData.Content.Body.Add(htmlBodyPart);
        emailData.Content.Body.Add(plainTextBodyPart);
        emailData.Content.From = "alonmore28@gmail.com";
        emailData.Content.Subject = title;

        try
        {
            var result = await _elasticEmailClient.EmailsTransactionalPostAsync(emailData);
            _logger.LogInformation("{result}", result.ToJson());
            return new()
            {
                ResultMessage = result.ToJson()
            };
        }
        catch (ApiException e)
        {
            Console.WriteLine("Exception when calling EmailsApi.EmailsTransactionalPost: " + e.Message);
            Console.WriteLine("Status Code: " + e.ErrorCode);
            Console.WriteLine(e.StackTrace);
            throw;
        }
        
    }
}
