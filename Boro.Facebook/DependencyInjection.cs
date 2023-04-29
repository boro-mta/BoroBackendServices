using Boro.Facebook.Interfaces;
using Boro.Facebook.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boro.Facebook;

public static class DependencyInjection
{
    public static IServiceCollection AddFacebook(this IServiceCollection services, IConfiguration configuration)
    {
        const string APP_ID_KEY = "Authentication:Facebook:AppId";
        const string APP_SECRET_KEY = "Authentication:Facebook:AppSecret";

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        string facebookAppId = configuration[APP_ID_KEY] ?? 
            throw new NullReferenceException($"configuration[{APP_ID_KEY}] is null");
        string facebookAppSecret = configuration[APP_SECRET_KEY] ?? 
            throw new NullReferenceException($"configuration[{APP_SECRET_KEY}] is null");

        //services.AddAuthentication(options =>
        //{
        //    options.DefaultAuthenticateScheme = FacebookDefaults.AuthenticationScheme;
        //    options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;

        //}).AddFacebook(facebookOptions =>
        //{
        //    facebookOptions.AppId = facebookAppId;
        //    facebookOptions.AppSecret = facebookAppSecret;
        //});

        services.AddSingleton<IFacebookAuthService, FacebookAuthService>();
        services.AddTransient<FacebookAuthSettings>();
        services.AddHttpClient();

        return services;
    }
}
