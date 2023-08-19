using Microsoft.Extensions.Configuration;

namespace Boro.Email.Services;

internal class ElasticEmailSettings
{
    public ElasticEmailSettings(IConfiguration configuration)
    {
        ApiKey = "DF53B6EB890BFB8957087C75CB0981FBA0A8ED989BD6241D0BF89A77E206877EE3E3D506EA4B669FB8B38D888989489D";
        //configuration.GetSection("ElasticEmailSettings").Bind(this);
        //if (ApiKey is null or "")
        //{
        //    throw new ArgumentException("Elastic Email configuration error.");
        //}
    }

    public string ApiKey { get; init; }
}
