using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Boro.Logging.DependencyInjection;
using ItemService.Controller.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Boro.AppBuilder;

public static class AppBuilder
{
    public static WebApplication BuildApp(string[] args)
    {
        var builder = GetMinimalAppBuilder(args);

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseBoroLogging();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }

    public static WebApplicationBuilder GetMinimalAppBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddBoroLogging(Path.Combine(Environment.CurrentDirectory, "Logs"));

        // Add services to the container.
        builder.Services.AddItemService();

        return builder;
    }
}