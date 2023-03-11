using Boro.EntityFramework.DependencyInjection;
using ItemService.API.Interfaces;
using ItemService.DB.Backends;
using ItemService.DB.DbContexts;
using Microsoft.Extensions.DependencyInjection;

namespace ItemService.DB.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddItemServiceBackend(this IServiceCollection services)
    {
        services.AddBoroMainDbContext<ItemServiceDbContext>();
        services.AddTransient<IItemServiceBackend, ItemServiceBackend>();

        return services;
    }
}
