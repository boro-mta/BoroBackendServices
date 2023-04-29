using Microsoft.Extensions.Configuration;

namespace Boro.Facebook.Services;

internal class FacebookAuthSettings
{
    const string APP_ID_KEY = "Authentication:Facebook:AppId";
    const string APP_SECRET_KEY = "Authentication:Facebook:AppSecret";

    public FacebookAuthSettings(IConfiguration configuration)
    {
        AppId = configuration[APP_ID_KEY] ??
            throw new NullReferenceException($"configuration[{APP_ID_KEY}] is null");
        AppSecret = configuration[APP_SECRET_KEY] ??
            throw new NullReferenceException($"configuration[{APP_SECRET_KEY}] is null");
    }

    public string AppId { get; init; }
    public string AppSecret { get; init; }
}
