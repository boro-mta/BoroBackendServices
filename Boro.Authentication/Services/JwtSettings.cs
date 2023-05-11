using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boro.Authentication.Services;

internal class JwtSettings
{
    public JwtSettings(IConfiguration configuration)
    {
        configuration.GetSection("JwtSettings").Bind(this);
        if (Issuer is null or "" || Audience is null or "" || Key is null or "")
        {
            throw new ArgumentException($"JwtSettings - didn't iniitalize properly. Missing configurations.");
        }
    }

    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
}
