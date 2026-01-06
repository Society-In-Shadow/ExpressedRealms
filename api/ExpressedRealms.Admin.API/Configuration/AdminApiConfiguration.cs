using ExpressedRealms.Admin.API.AdminCharacterList;
using ExpressedRealms.Admin.API.AdminEndpoints;
using ExpressedRealms.Admin.API.RolesEndpoints;
using Microsoft.AspNetCore.Builder;

namespace ExpressedRealms.Admin.API.Configuration;

public static class AdminApiConfiguration
{
    public static void ConfigureAdminEndPoints(this WebApplication app)
    {
        app.AddAdminEndPoints();
        app.AddAdminCharacterListEndPoints();
        app.AddRolesEndpoints();
    }
}
