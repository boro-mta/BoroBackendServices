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
        var results = _emailService.SendEmail("alonmore28@gmail.com", "test email title", "this is a test email content").Result;

        Assert.IsNotNull(results);


    }
}
