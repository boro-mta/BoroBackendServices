using Boro.Email.API;
using Boro.Email.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Boro.Email;

public static class DependencyInjection
{
    public static IServiceCollection AddEmailServices(this IServiceCollection services)
    {
        services.AddTransient<IEmailService, ElasticEmailService>();
        services.AddTransient<ElasticEmailSettings>();
        
        return services;
    }
}
