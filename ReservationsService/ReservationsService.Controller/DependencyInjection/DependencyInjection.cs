using Microsoft.Extensions.DependencyInjection;
using ReservationsService.DB.DependencyInjection;

namespace ReservationsService.Controller.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddReservationsService(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddReservationsServiceBackend();

        return services;
    }

}