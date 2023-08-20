using Microsoft.Extensions.Configuration;

namespace Boro.Email.Services;

internal class ElasticEmailSettings
{
    public ElasticEmailSettings(IConfiguration configuration)
    {
        configuration.GetSection("ElasticEmailSettings").Bind(this);
        if (ApiKey is null or "")
        {
            throw new ArgumentException("Elastic Email configuration error.");
        }
    }

    public string ApiKey { get; init; }
    public bool Active { get; set; }
}
