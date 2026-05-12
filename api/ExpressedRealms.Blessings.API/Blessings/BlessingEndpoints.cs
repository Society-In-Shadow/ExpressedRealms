using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using ExpressedRealms.Blessings.API.Blessings.CreateBlessing;
using ExpressedRealms.Blessings.API.Blessings.DeleteBlessing;
using ExpressedRealms.Blessings.API.Blessings.EditBlessing;
using ExpressedRealms.Blessings.API.Blessings.GetAllBlessings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Blessings.API.Blessings;

internal static class BlessingEndpoints
{
    internal static void AddBlessingEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("blessings")
            .AddFluentValidationAutoValidation()
            .RequireAuthorization()
            .WithTags("Blessings");

        endpointGroup
            .MapGet("", GetAllBlessingsEndpoint.GetBlessings)
            .WithSummary(
                "Returns three sets of blessings, advantages, disadvantages, and mixed blessings."
            );

        endpointGroup
            .MapPost("", CreateBlessingEndpoint.Execute)
            .RequirePermission(Permissions.Blessings.Create);

        endpointGroup
            .MapPut("{id}", EditBlessingEndpoint.Execute)
            .RequirePermission(Permissions.Blessings.Edit);

        endpointGroup
            .MapDelete("{id}", DeleteBlessingEndpoint.Execute)
            .RequirePermission(Permissions.Blessings.Delete);
    }
}
