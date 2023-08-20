using Boro.Email.API;
using ElasticEmail.Api;
using ElasticEmail.Client;
using ElasticEmail.Model;
using Microsoft.Extensions.Logging;

namespace Boro.Email.Services;

internal class ElasticEmailService : IEmailService
{
    private readonly EmailsApi _elasticEmailClient;
    private readonly bool _active;
    private readonly ILogger _logger;


    public ElasticEmailService(ILoggerFactory loggerFactory,
                               ElasticEmailSettings settings)
    {
        _active = settings.Active;
        _logger = loggerFactory.CreateLogger("EmailService");
        Configuration config = new();
        config.ApiKey.Add("X-ElasticEmail-ApiKey", settings.ApiKey);
        _elasticEmailClient = new EmailsApi(config);
    }

    private async Task SendEmailAsync(EmailTransactionalMessageData emailData)
    {
        if (_active)
        {
            try
            {
                _logger.LogDebug("attempting to send {@emailData}", emailData);
                var result = await _elasticEmailClient.EmailsTransactionalPostAsync(emailData);
                _logger.LogInformation("{result}", result.ToJson());
            }
            catch (ApiException e)
            {
                _logger.LogError(e, "An error occured while trying to send an email. Email data: [{@emailData}]", emailData);
            }
        }
    }

    private static EmailTransactionalMessageData GetEmailData(IEnumerable<string> recipients, string emailTitle, BodyContentType contentType, string content)
    {
        var emailData = new EmailTransactionalMessageData(recipients: new TransactionalRecipient(to: recipients.ToList()))
        {
            Content = new()
            {
                Body = new()
                {
                    new()
                    {
                        ContentType = contentType,
                        Charset = "utf-8",
                        Content = content
                    }
                },
                From = "boro.sharing@gmail.com",
                Subject = $"A Message From boro: {emailTitle}"
            }
        };

        return emailData;
    }

    public async Task SendHTMLEmailAsync(IEnumerable<string> recipients, string emailTitle, string htmlEmailBody)
    {
        var emailData = GetEmailData(recipients, emailTitle, BodyContentType.HTML, htmlEmailBody);

        await SendEmailAsync(emailData);
    }

    public async Task SendTextEmailAsync(IEnumerable<string> recepients, string emailTitle, string textEmailBody)
    {
        var emailData = GetEmailData(recepients, emailTitle, BodyContentType.PlainText, 
            $$"""
            ||-----------------------------
            ||
            ||      {{emailTitle}}
            ||
            ||-----------------------------
            ||
            ||    {{textEmailBody}}
            ||
            ||-----------------------------
            """);

        await SendEmailAsync(emailData);
    }
}
