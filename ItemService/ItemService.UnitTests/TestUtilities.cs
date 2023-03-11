using Boro.AppBuilder;
using Boro.Logging.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace ItemService.UnitTests;

internal static class TestUtilities
{
    internal static WebApplication GenerateApp()
    {
        var builder = AppBuilder.GetMinimalAppBuilder(Array.Empty<string>());
        var app = builder.Build();
        app.UseBoroLogging();
        return app;
    }
}
