using ExpressedRealms.Powers.API.PowerEndpoints;
using ExpressedRealms.Powers.API.PowerPathEndpoints;
using Microsoft.AspNetCore.Builder;

namespace ExpressedRealms.Powers.API.Configuration;

public static class PowersApiConfiguration
{
    public static void AddPowerEndPoints(this WebApplication app)
    {
        app.AddPowerApi();
        app.AddPowerPathApi();
    }
}
