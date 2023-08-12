using ChatService.API;
using ChatService.SendBird;
using Microsoft.Extensions.DependencyInjection;

namespace ChatService.Controller.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddChatService(this IServiceCollection services)
    {
        services.AddTransient<IChatBackend, SendBirdBackend>();
        return services;
    }
}
