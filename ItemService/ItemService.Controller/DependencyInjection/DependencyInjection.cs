using ItemService.API.Interfaces;
using ItemService.DB.Backends;
using ItemService.DB.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace ItemService.Controller.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddItemService(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddItemServiceDbContext();
            services.AddTransient<IItemServiceBackend, ItemServiceBackend>();

            return services;
        }

    }
}