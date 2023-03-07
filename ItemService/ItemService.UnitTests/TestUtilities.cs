using ItemService.Controller.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ItemService.UnitTests
{
    internal static class TestUtilities
    {
        internal static (IServiceProvider serviceProvider, WebApplication app) GenerateApp()
        {
            var builder = WebApplication.CreateBuilder();
            var serviceProvider = builder.Services
            .AddItemService()
            .BuildServiceProvider();
            var app = builder.Build();

            return (serviceProvider, app);
        }
    }
}
