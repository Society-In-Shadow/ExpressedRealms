using ExpressedRealms.Admin.API.AdminEndpoints;
using Microsoft.AspNetCore.Builder;

namespace ExpressedRealms.Admin.API.Configuration;

public static class AdminAPIConfiguration
{
    public static void ConfigureAdminEndPoints(this WebApplication app)
    {
        app.AddAdminEndPoints();
    }
}