using Microsoft.Extensions.DependencyInjection;
using UserService.API.Interfaces;
using UserService.DB.Backends;

namespace UserService.Controller.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUserService(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<IUserServiceBackend, UserServiceBackend>();

            services.AddControllers();

            return services;
        }

    }
}