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
            .WithOpenApi();

        endpointGroup
            .MapGet("{blessingId}/level/{levelId}", GetBlessingLevelEndpoint.Execute)
            .WithSummary(
                "Returns three sets of blessings, advantages, disadvantages, and mixed blessings."
            );
        
        endpointGroup
            .MapPost("{blessingId}/level", CreateBlessingLevelEndpoint.Execute)
            .RequirePolicyAuthorization(Policies.ManageBlessings);

        endpointGroup
            .MapPut("{blessingId}/level/{levelId}", EditBlessingLevelEndpoint.Execute)
            .RequirePolicyAuthorization(Policies.ManageBlessings);

        endpointGroup
            .MapDelete("{blessingId}/level/{levelId}", DeleteBlessingLevelEndpoint.Execute)
            .RequirePolicyAuthorization(Policies.ManageBlessings);
    }
}
