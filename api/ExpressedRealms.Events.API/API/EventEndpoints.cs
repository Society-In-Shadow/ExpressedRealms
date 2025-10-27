using ExpressedRealms.Authentication;
using ExpressedRealms.Events.API.API.Events.Create;
using ExpressedRealms.Events.API.API.Events.Delete;
using ExpressedRealms.Events.API.API.Events.Get;
using ExpressedRealms.Events.API.API.Events.Publish;
using ExpressedRealms.Events.API.API.EventScheduleItem.Create;
using ExpressedRealms.Events.API.API.EventScheduleItem.Delete;
using ExpressedRealms.Events.API.API.EventScheduleItem.Edit;
using ExpressedRealms.Events.API.API.EventScheduleItem.Get;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using EditEventEndpoint = ExpressedRealms.Events.API.API.Events.Edit.EditEventEndpoint;

namespace ExpressedRealms.Events.API.API;

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

        endpointGroup
            .MapPost("{id}/publish", PublishEventEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManageEvents);

        endpointGroup.MapGet(
            "{eventId}/scheduleItem",
            GetAllEventScheduleItemsEndpoint.ExecuteAsync
        );

        endpointGroup
            .MapPost("{eventId}/scheduleItem", CreateEventScheduleItemEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManageEvents);

        endpointGroup
            .MapPut("{eventId}/scheduleItem/{id}", EditEventScheduleItemEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManageEvents);

        endpointGroup
            .MapDelete("{eventId}/scheduleItem/{id}", DeleteEventScheduleItemEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManageEvents);
    }
}
