using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace Boro.Authentication.Services;

internal class ApiKeyAuthFilter : IAuthorizationFilter
{
    private static class AuthDefinitions
    {
        public const string ApiKeySection = "Authentication:ApiKey";
        public const string ApiKeyHeaderName = "x-boro-api-key";
    }

    private readonly IConfiguration _configuration;

    public ApiKeyAuthFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var request = context.HttpContext.Request;

        if (!request.Headers.TryGetValue(AuthDefinitions.ApiKeyHeaderName, out var apiHeader))
        {
            context.Result = new UnauthorizedObjectResult("API key is missing");
            return;
        }

        var apiKey = _configuration.GetValue<string>(AuthDefinitions.ApiKeySection);
        if (!(apiKey?.Equals(apiHeader) ?? false))
        {
            context.Result = new UnauthorizedObjectResult("Invalid API key");
            return;
        }
    }
}