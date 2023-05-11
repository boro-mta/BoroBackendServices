using Boro.Authentication.Facebook.Interfaces;
using Boro.Authentication.Facebook.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Boro.Authentication.Facebook;

public static class DependencyInjection
{
    public static IServiceCollection AddFacebook(this IServiceCollection services)
    {
        services.AddTransient<IFacebookAuthService, FacebookAuthService>();
        services.AddTransient<FacebookAuthSettings>();
        services.AddHttpClient();

        return services;
    }
}
