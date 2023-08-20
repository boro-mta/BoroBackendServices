using Boro.Email.API;
using Microsoft.Extensions.DependencyInjection;

namespace Boro.UnitTests;

[TestClass]
public class EmailServiceTests
{
    private IEmailService? _emailService;

    [TestInitialize]
    public void Initialize()
    {
        var app = TestUtilities.GenerateApp();
        _emailService = app?.Services.GetService<IEmailService>();
    }

    [TestMethod]
    public void TrySendEmail()
    {
        string[] emails = { "alonmore28@gmail.com" };
        _emailService.SendTextEmailAsync(emails, "test email title", "this is a test email content").Wait();


    }
}
