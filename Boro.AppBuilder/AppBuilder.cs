using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ItemService.Controller.DependencyInjection;
using ReservationsService.Controller.DependencyInjection;
using Boro.EntityFramework.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Boro.Logging;

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
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        var configuration = builder.Configuration;

        var app = builder.Build();
        app.UseCors("AllowAll");
        app.UseBoroLogging();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        //app.UseAuthentication();
        app.MapControllers();

        return app;
    }

    public static WebApplicationBuilder GetMinimalAppBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddBoroLogging(Path.Combine(Environment.CurrentDirectory, "Logs"));
        // Add services to the container.
        builder.Services.AddBoroMainDbContext(builder.Configuration);

        builder.Services.AddItemService();
        builder.Services.AddReservationsService();
        return builder;
    }
}