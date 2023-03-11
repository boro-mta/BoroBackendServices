using ItemService.DB.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace ItemService.Controller.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddItemService(this IServiceCollection services)
    {
        services.AddItemServiceBackend();
        services.AddControllers();

        return services;
    }
}