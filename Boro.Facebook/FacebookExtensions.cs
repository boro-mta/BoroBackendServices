using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boro.Facebook;

public static class FacebookExtensions
{

    public static IServiceCollection AddFacebook(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        string facebookAppId = configuration["Authentication:Facebook:AppId"] ?? throw new NullReferenceException("configuration[\"Authentication:Facebook:AppId\"] is null");
        string facebookAppSecret = configuration["Authentication:Facebook:AppSecret"] ?? throw new NullReferenceException("\"Authentication:Facebook:AppSecret\" is null");

        services.AddAuthentication().AddFacebook(facebookOptions =>
        {
            facebookOptions.AppId = facebookAppId;
            facebookOptions.AppSecret = facebookAppSecret;
        });

        return services;
    }
}
