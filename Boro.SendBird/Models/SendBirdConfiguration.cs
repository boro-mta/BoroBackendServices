using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boro.SendBird.Models;

internal class SendBirdConfiguration
{
    public SendBirdConfiguration(IConfiguration configuration)
    {
        configuration.GetSection("SendBirdConfiguration").Bind(this);
        if (AppId is null or "" || ApiToken is null or "")
        {
            throw new ArgumentException("Send Bird configuration error.");
        }
    }

    public string AppId { get; init; }
    public string ApiToken { get; init; }
}
