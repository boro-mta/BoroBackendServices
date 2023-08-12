using Boro.SendBird.API;
using Boro.SendBird.Models;
using Boro.SendBird.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Boro.SendBird;

public static class DependencyInjection
{
    public static IServiceCollection AddSendBird(this IServiceCollection services)
    {
        services.AddTransient<SendBirdConfiguration>();
        services.AddHttpClient<ISendBirdClient, SendBirdClient>();
        return services;
    }
}
