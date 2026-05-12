using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using ExpressedRealms.Blessings.API.BlessingLevels.CreateBlessingLevel;
using ExpressedRealms.Blessings.API.BlessingLevels.DeleteBlessingLevel;
using ExpressedRealms.Blessings.API.BlessingLevels.EditBlessingLevel;
using ExpressedRealms.Blessings.API.BlessingLevels.GetAllBlessings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ExpressedRealms.Blessings.API.BlessingLevels;

internal static class BlessingLevelEndpoints
{
    internal static void AddBlessingLevelEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("blessings").WithTags("Blessings");

        endpointGroup
            .MapGet("{blessingId}/level/{levelId}", GetBlessingLevelEndpoint.Execute)
            .RequirePermission(Permissions.Blessings.View);

        endpointGroup
            .MapPost("{blessingId}/level", CreateBlessingLevelEndpoint.Execute)
            .RequirePermission(Permissions.Blessings.Create);

        endpointGroup
            .MapPut("{blessingId}/level/{levelId}", EditBlessingLevelEndpoint.Execute)
            .RequirePermission(Permissions.Blessings.Edit);

        endpointGroup
            .MapDelete("{blessingId}/level/{levelId}", DeleteBlessingLevelEndpoint.Execute)
            .RequirePermission(Permissions.Blessings.Delete);
    }
}
