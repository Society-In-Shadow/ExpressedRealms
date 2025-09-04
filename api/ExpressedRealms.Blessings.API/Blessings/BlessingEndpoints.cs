using ExpressedRealms.Authentication;
using ExpressedRealms.Blessings.API.Blessings.CreateBlessing;
using ExpressedRealms.Blessings.API.Blessings.DeleteBlessing;
using ExpressedRealms.Blessings.API.Blessings.EditBlessing;
using ExpressedRealms.Blessings.API.Blessings.GetAllBlessings;
using ExpressedRealms.Server.Shared;
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
            .WithTags("Blessings")
            .WithOpenApi();

        endpointGroup
            .MapGet("", GetAllBlessingsEndpoint.GetBlessings)
            .WithSummary(
                "Returns three sets of blessings, advantages, disadvantages, and mixed blessings."
            );

        endpointGroup
            .MapPost("", CreateBlessingEndpoint.Execute)
            .RequirePolicyAuthorization(Policies.ManageBlessings);

        endpointGroup
            .MapPut("{id}", EditBlessingEndpoint.Execute)
            .RequirePolicyAuthorization(Policies.ManageBlessings);

        endpointGroup
            .MapDelete("{id}", DeleteBlessingEndpoint.Execute)
            .RequirePolicyAuthorization(Policies.ManageBlessings);
    }
}
