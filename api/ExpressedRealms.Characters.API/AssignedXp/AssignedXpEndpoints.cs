using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using ExpressedRealms.Characters.API.AssignedXp.Create;
using ExpressedRealms.Characters.API.AssignedXp.Delete;
using ExpressedRealms.Characters.API.AssignedXp.Edit;
using ExpressedRealms.Characters.API.AssignedXp.Get;
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
            .RequireAuthorization();

        endpointGroup.MapGet("{characterId}/assignedXp", GetEndpoint.ExecuteAsync);

        endpointGroup
            .MapPost("{characterId}/assignedXp", CreateEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.PlayerExperience.Create);

        endpointGroup
            .MapPut("{characterId}/assignedXp/{mappingId}", EditEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.PlayerExperience.Edit);

        endpointGroup
            .MapDelete("{characterId}/assignedXp/{mappingId}", DeleteEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.PlayerExperience.Delete);

        var eventEndpointGroup = app.MapGroup("events")
            .AddFluentValidationAutoValidation()
            .WithTags("Events");

        eventEndpointGroup
            .MapGet("{eventId}/assignedXp", GetEventEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.Event.Checkin);
    }
}
