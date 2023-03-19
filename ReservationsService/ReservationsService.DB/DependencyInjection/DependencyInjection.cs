using Boro.EntityFramework.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ReservationsService.API.Interfaces;
using ReservationsService.DB.Backends;
using ReservationsService.DB.DbContexts;

namespace ReservationsService.DB.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddReservationsServiceBackend(this IServiceCollection services)
    {
        //services.AddBoroMainDbContext<ReservationsServiceDbContext>();
        services.AddTransient<IReservationsServiceBackend, ReservationsServiceBackend>();

        return services;
    }
}