using ExpressedRealms.Global.API.Search.Endpoints;
using Microsoft.AspNetCore.Builder;

namespace ExpressedRealms.Global.API.Configuration;

public static class GlobalApiConfiguration
{
    public static void ConfigureGlobalEndPoints(this WebApplication app)
    {
        app.AddSearchEndpoints();
    }
}
