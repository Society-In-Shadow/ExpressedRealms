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
            .WithTags("Blessings")
            .WithOpenApi();

        endpointGroup
            .MapGet("", GetAllBlessingsEndpoint.GetBlessings)
            .WithSummary(
                "Returns three sets of blessings, advantages, disadvantages, and mixed blessings."
            );
    }
}
