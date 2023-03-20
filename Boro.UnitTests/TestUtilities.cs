using static Boro.AppBuilder.AppBuilder;
using Microsoft.AspNetCore.Builder;

namespace Boro.UnitTests;

internal static class TestUtilities
{
    internal static WebApplication GenerateApp()
    {
        var builder = GetMinimalAppBuilder(Array.Empty<string>());
        var app = builder.Build();
        return app;
    }
}
