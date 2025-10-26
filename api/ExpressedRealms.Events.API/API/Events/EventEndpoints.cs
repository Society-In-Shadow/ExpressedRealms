using ExpressedRealms.Authentication;
using ExpressedRealms.Events.API.API.Events.Create;
using ExpressedRealms.Events.API.API.Events.Delete;
using ExpressedRealms.Events.API.API.Events.Get;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using EditEventEndpoint = ExpressedRealms.Events.API.API.Events.Edit.EditEventEndpoint;

namespace ExpressedRealms.Events.API.API.Events;

internal static class EventEndpoints
{
    internal static void AddEventEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("events")
            .AddFluentValidationAutoValidation()
            .WithTags("Events")
            .WithOpenApi();

        endpointGroup.MapGet("", GetAllEventsEndpoint.ExecuteAsync);

        endpointGroup
            .MapPost("", CreateEventEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManageEvents);

        endpointGroup
            .MapPut("{id}", EditEventEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManageEvents);

        endpointGroup
            .MapDelete("{id}", DeleteEventEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManageEvents);
    }
}
