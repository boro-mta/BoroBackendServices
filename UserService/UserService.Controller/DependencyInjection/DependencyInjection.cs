using Microsoft.Extensions.DependencyInjection;
using UserService.API.Interfaces;
using UserService.DB.Backends;

namespace UserService.Controller.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddUserService(this IServiceCollection services)
    {
        services.AddTransient<IUserServiceBackend, UserServiceBackend>();
        services.AddTransient<IIdentityServiceBackend, IdentityServiceBackend>();

        return services;
    }

}