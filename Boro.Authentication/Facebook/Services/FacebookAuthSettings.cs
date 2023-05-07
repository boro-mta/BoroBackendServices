using Microsoft.Extensions.Configuration;

namespace Boro.Authentication.Facebook.Services;

internal class FacebookAuthSettings
{
    //const string APP_ID_KEY = "Authentication:Facebook:AppId";
    //const string APP_SECRET_KEY = "Authentication:Facebook:AppSecret";

    public FacebookAuthSettings(IConfiguration configuration)
    {
        //AppId = configuration[APP_ID_KEY] ??
        //    throw new NullReferenceException($"configuration[{APP_ID_KEY}] is null");
        //AppSecret = configuration[APP_SECRET_KEY] ??
        //    throw new NullReferenceException($"configuration[{APP_SECRET_KEY}] is null");
        configuration.GetSection("Authentication:Facebook").Bind(this);
        if (AppId is null or "" || AppSecret is null or "")
        {
            throw new ArgumentException($"FacebookAuthSettings - didn't iniitalize properly. Missing configurations.");
        }
    }

    public string AppId { get; init; }
    public string AppSecret { get; init; }
}
