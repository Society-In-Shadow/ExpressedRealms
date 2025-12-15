using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using ExpressedRealms.Events.API.API.Events.Create;
using ExpressedRealms.Events.API.API.Events.Delete;
using ExpressedRealms.Events.API.API.Events.Get;
using ExpressedRealms.Events.API.API.Events.GetSummary;
using ExpressedRealms.Events.API.API.Events.Publish;
using ExpressedRealms.Events.API.API.EventScheduleItem.Create;
using ExpressedRealms.Events.API.API.EventScheduleItem.Delete;
using ExpressedRealms.Events.API.API.EventScheduleItem.Edit;
using ExpressedRealms.Events.API.API.EventScheduleItem.Get;
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
            .WithTags("Events");

        endpointGroup.MapGet("", GetAllEventsEndpoint.ExecuteAsync);
        endpointGroup.MapGet("summary", GetSummaryEventsEndpoint.ExecuteAsync);

        endpointGroup
            .MapPost("", CreateEventEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.Event.Create);

        endpointGroup
            .MapPut("{id}", EditEventEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.Event.Edit);

        endpointGroup
            .MapDelete("{id}", DeleteEventEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.Event.Delete);

        endpointGroup
            .MapPost("{id}/publish", PublishEventEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.Event.Publish);

        endpointGroup.MapGet(
            "{eventId}/scheduleItems",
            GetAllEventScheduleItemsEndpoint.ExecuteAsync
        );

        endpointGroup
            .MapPost("{eventId}/scheduleItems", CreateEventScheduleItemEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.EventScheduleItem.Create);

        endpointGroup
            .MapPut("{eventId}/scheduleItems/{id}", EditEventScheduleItemEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.EventScheduleItem.Edit);

        endpointGroup
            .MapDelete("{eventId}/scheduleItems/{id}", DeleteEventScheduleItemEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.EventScheduleItem.Delete);;
    }
}
