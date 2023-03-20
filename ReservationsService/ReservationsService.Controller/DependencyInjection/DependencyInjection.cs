using Microsoft.Extensions.DependencyInjection;
using ReservationsService.API.Interfaces;
using ReservationsService.DB.Backends;

namespace ReservationsService.Controller.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddReservationsService(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddTransient<IReservationsServiceBackend, ReservationsServiceBackend>();

        return services;
    }

}