using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ItemService.Controller.DependencyInjection;
using ReservationsService.Controller.DependencyInjection;
using UserService.Controller.DependencyInjection;
using Boro.EntityFramework.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Boro.Logging;
using Boro.Authentication;
using Microsoft.OpenApi.Models;

namespace Boro.AppBuilder;

public static class AppBuilder
{
    public static WebApplication BuildApp(string[] args)
    {
        var builder = GetMinimalAppBuilder(args);
        builder.Services.AddBoroAuthentication(builder.Configuration);
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(config =>
        {
            config.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "API Key to access and send requests to the server",
                Type = SecuritySchemeType.ApiKey,
                Name = "x-boro-api-key",
                In = ParameterLocation.Header,
                Scheme = "ApiKeyScheme",
            });

            var scheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey",
                },
                In = ParameterLocation.Header,
            };

            var requirement = new OpenApiSecurityRequirement
            {
                {scheme, new List<string>() }
            };

            config.AddSecurityRequirement(requirement);
        });

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

        app.UseBoroAuthentication();

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
        builder.Services.AddUserService();

        return builder;
    }
}