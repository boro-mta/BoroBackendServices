using Boro.EntityFramework.DbContexts.BoroMainDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boro.EntityFramework.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddBoroMainDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BoroMainDbContext>(options =>
        {
#if DEBUG
            options.UseInMemoryDatabase("BoroMainDB");
#else
            const string CONNECTION_STRING_NAME = "BoroMainDB";

            var connectionString = configuration?.GetConnectionString(CONNECTION_STRING_NAME)
                ?? throw new Exception($"AddBoroMainDbContext - Failed to obtain {CONNECTION_STRING_NAME} from configuration");

            options.UseSqlServer(connectionString);
#endif
        });

        return services;
    }
}
