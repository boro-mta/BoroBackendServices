using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Boro.Authentication.Policies.Handlers;

public static class AuthorizationHandlerContextExtensions
{
    public static HttpContext? GetHttpContext(this AuthorizationHandlerContext context)
    {
        return context.Resource as HttpContext;
    }

    public static T? GetFromRoute<T>(this AuthorizationHandlerContext context, string parameterName)
        where T : class
    {
        var httpContext = context.GetHttpContext();
        if (httpContext is not null)
        {
            return httpContext.Request.RouteValues[parameterName] as T;
        }
        return null;
    }
}

