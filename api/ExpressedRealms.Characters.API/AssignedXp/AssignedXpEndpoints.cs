using ExpressedRealms.Authentication;
using ExpressedRealms.Characters.API.AssignedXp.Create;
using ExpressedRealms.Characters.API.AssignedXp.Delete;
using ExpressedRealms.Characters.API.AssignedXp.Edit;
using ExpressedRealms.Characters.API.AssignedXp.Get;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Characters.API.AssignedXp;

internal static class AssignedXpEndpoints
{
    internal static void AddAssignedXpEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("characters")
            .AddFluentValidationAutoValidation()
            .WithTags("Character Assigned Xp")
            .WithOpenApi()
            .RequireAuthorization();

        endpointGroup.MapGet("{characterId}/assignedXp", GetEndpoint.ExecuteAsync);

        endpointGroup
            .MapPost("{characterId}/assignedXp", CreateEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManagePlayerExperience);

        endpointGroup
            .MapPut("{characterId}/assignedXp/{mappingId}", EditEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManagePlayerExperience);

        endpointGroup
            .MapDelete("{characterId}/assignedXp/{mappingId}", DeleteEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManagePlayerExperience);

        var eventEndpointGroup = app.MapGroup("events")
            .AddFluentValidationAutoValidation()
            .WithTags("Events")
            .WithOpenApi();

        eventEndpointGroup
            .MapGet("{eventId}/assignedXp", GetEventEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManageEvents);
    }
}
