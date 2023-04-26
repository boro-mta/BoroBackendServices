using ItemService.API.Interfaces;
using ItemService.DB.Backends;
using Microsoft.Extensions.DependencyInjection;

namespace ItemService.Controller.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddItemService(this IServiceCollection services)
    {
        services.AddTransient<IItemServiceBackend, ItemServiceBackend>();
        services.AddTransient<IImagesBackend, ImagesBackend>();
        services.AddControllers();

        return services;
    }
}