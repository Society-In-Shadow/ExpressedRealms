using ExpressedRealms.Blessings.API.Blessings;
using Microsoft.AspNetCore.Builder;

namespace ExpressedRealms.Blessings.API.Configuration;

public static class BlessingsApiConfiguration
{
    public static void ConfigureBlessingEndPoints(this WebApplication app)
    {
        app.AddBlessingEndpoints();
    }
}