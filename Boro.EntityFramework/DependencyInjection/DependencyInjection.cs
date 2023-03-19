using Boro.EntityFramework.DbContexts.BoroMainDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Boro.EntityFramework.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddBoroMainDbContext(this IServiceCollection services)
    {
        services.AddDbContext<BoroMainDbContext>(options =>
        {
#if DEBUG
            options.UseInMemoryDatabase("BoroMainDB");
#else
            const string methodName = "AddBoroMainDbContext";

            const string CONNECTION_STRING_NAME = "BoroMainDB";

            var configuration = services?.BuildServiceProvider()?.GetService<IConfiguration>()
                ?? throw new Exception($"{methodName} - Failed to get configuration");

            var connectionString = configuration?.GetConnectionString(CONNECTION_STRING_NAME)
                ?? throw new Exception($"{methodName} - Failed to obtain {CONNECTION_STRING_NAME} from configuration");

            options.UseSqlServer(connectionString);
#endif
        });

        return services;
    }
//    public static IServiceCollection AddBoroMainDbContext<T>(this IServiceCollection services)
//        where T : BoroMainDbContext<T>
//    {
//        services.AddDbContext<T>(options =>
//        {
//#if DEBUG
//            options.UseInMemoryDatabase("BoroMainDB");
//#else
//            const string methodName = "AddBoroMainDbContext";

//            const string CONNECTION_STRING_NAME = "BoroMainDB";

//            var configuration = services?.BuildServiceProvider()?.GetService<IConfiguration>()
//                ?? throw new Exception($"{methodName} - Failed to get configuration");

//            var connectionString = configuration?.GetConnectionString(CONNECTION_STRING_NAME)
//                ?? throw new Exception($"{methodName} - Failed to obtain {CONNECTION_STRING_NAME} from configuration");

//            options.UseSqlServer(connectionString);
//#endif
//        });

//        return services;
//    }
}
