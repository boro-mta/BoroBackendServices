using Boro.EntityFramework.DependencyInjection;
using ItemService.DB.DbContexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ItemService.DB.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddItemServiceDbContext(this IServiceCollection services)
        {
            string methodName = MethodBase.GetCurrentMethod()?.Name
                ?? throw new Exception($"Error getting current method name");

            const string CONNECTION_STRING_NAME = "BoroMainDB";

            var configuration = services?.BuildServiceProvider()?.GetService<IConfiguration>()
                ?? throw new Exception($"{methodName} - Failed to get configuration");

            var connectionString = configuration?.GetConnectionString(CONNECTION_STRING_NAME)
                ?? throw new Exception($"{methodName} - Failed to obtain {CONNECTION_STRING_NAME} from configuration");

            services.AddBoroMainDbContext<ItemServiceDbContext>(connectionString);

            return services;
        }

    }
}