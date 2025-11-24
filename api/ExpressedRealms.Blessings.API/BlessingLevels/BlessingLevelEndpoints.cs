using ExpressedRealms.Authentication;
using ExpressedRealms.Blessings.API.BlessingLevels.CreateBlessingLevel;
using ExpressedRealms.Blessings.API.BlessingLevels.DeleteBlessingLevel;
using ExpressedRealms.Blessings.API.BlessingLevels.EditBlessingLevel;
using ExpressedRealms.Blessings.API.BlessingLevels.GetAllBlessings;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Blessings.API.BlessingLevels;

internal static class BlessingLevelEndpoints
{
    internal static void AddBlessingLevelEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("blessings")
            .AddFluentValidationAutoValidation()
            .RequireAuthorization()
            .WithTags("Blessings")
            .RequirePolicyAuthorization(Policies.ManageBlessings);

        endpointGroup.MapGet("{blessingId}/level/{levelId}", GetBlessingLevelEndpoint.Execute);

        endpointGroup.MapPost("{blessingId}/level", CreateBlessingLevelEndpoint.Execute);

        endpointGroup.MapPut("{blessingId}/level/{levelId}", EditBlessingLevelEndpoint.Execute);

        endpointGroup.MapDelete(
            "{blessingId}/level/{levelId}",
            DeleteBlessingLevelEndpoint.Execute
        );
    }
}
