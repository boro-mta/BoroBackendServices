using Boro.EntityFramework.DbContexts.BoroMainDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Boro.EntityFramework.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddBoroMainDbContext<T>(this IServiceCollection services, string connectionString)
        where T : BoroMainDbContext<T>
    {
        services.AddDbContext<T>(options =>
        {
#if DEBUG
            options.UseInMemoryDatabase("BoroMainDB");
#else
            options.UseSqlServer(connectionString);
#endif
        });

        return services;
    }
}
