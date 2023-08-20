using Microsoft.Extensions.DependencyInjection;
using ReservationsService.API.Interfaces;
using ReservationsService.DB.Backends;
using ReservationsService.DB.Services;

namespace ReservationsService.DB.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddReservationsServiceDB(this IServiceCollection services)
    {
        services.AddTransient<IReservationsServiceBackend, ReservationsServiceBackend>();
        services.AddTransient<IReservationsDashboardBackend, ReservationsDashboardBackend>();
        services.AddTransient<IReservationsOperationsBackend, ReservationsOperationsBackend>();
        services.AddTransient<ReservationsEmailNotifier>();
        return services;
    }

}