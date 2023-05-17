using Boro.Authentication.Facebook;
using Boro.Authentication.Interfaces;
using Boro.Authentication.Policies.Handlers;
using Boro.Authentication.Policies.Requirements;
using Boro.Authentication.Services;
using Boro.Common.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Boro.Authentication;

public static class BoroAuthentication
{
    public static IServiceCollection AddBoroAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<JwtSettings>();
        services.AddTransient<IBoroAuthService, BoroAuthService>();
        services.AddControllers(c => c.Filters.Add<ApiKeyAuthFilter>());

        services.AddAuthentication(config =>
        {
            config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(config =>
        {
            var jwtSettings = new JwtSettings(configuration);
            config.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };
        });

        services.AddFacebook();
        services.AddPolicies();

        return services;
    }

    private static IServiceCollection AddPolicies(this IServiceCollection services)
    {
        services.AddTransient<IAuthorizationHandler, ItemOwnerAuthorizationHandler>();
        services.AddTransient<IAuthorizationHandler, ImageOwnerAuthorizationHandler>();
        services.AddTransient<IAuthorizationHandler, ReservationParticipantAuthorizationHandler>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthPolicies.ItemOwner, policy =>
            {
                policy.Requirements.Add(new ItemOwnerRequirement());
                policy.Build();
            });
            options.AddPolicy(AuthPolicies.NotItemOwner, policy =>
            {
                policy.Requirements.Add(new ItemOwnerRequirement(Owner: false));
                policy.Build();
            });
            options.AddPolicy(AuthPolicies.ImageOwner, policy =>
            {
                policy.Requirements.Add(new ImageOwnerRequirement());
                policy.Build();
            });
            options.AddPolicy(AuthPolicies.ReservationLender, policy =>
            {
                policy.Requirements.Add(new ReservationParticipantRequirement(Lender: true));
                policy.Build();
            });
            options.AddPolicy(AuthPolicies.ReservationBorrower, policy =>
            {
                policy.Requirements.Add(new ReservationParticipantRequirement(Borrower: true));
                policy.Build();
            });
            options.AddPolicy(AuthPolicies.ReservationLenderOrBorrower, policy =>
            {
                policy.Requirements.Add(new ReservationParticipantRequirement(Lender: true, Borrower: true));
                policy.Build();
            });
        });

        return services;
    }

    public static WebApplication UseBoroAuthentication(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}