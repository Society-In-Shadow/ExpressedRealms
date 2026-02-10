using ExpressedRealms.Authentication;
using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using ExpressedRealms.Events.API.API.EventCheckin.GetBasicCheckDetails;
using ExpressedRealms.Events.API.API.EventCheckin.GetGoCheckinInfo;
using ExpressedRealms.Events.API.API.EventQuestions.Create;
using ExpressedRealms.Events.API.API.EventQuestions.Delete;
using ExpressedRealms.Events.API.API.EventQuestions.Edit;
using ExpressedRealms.Events.API.API.EventQuestions.Get;
using ExpressedRealms.Events.API.API.Events.Create;
using ExpressedRealms.Events.API.API.Events.Delete;
using ExpressedRealms.Events.API.API.Events.Get;
using ExpressedRealms.Events.API.API.Events.GetSummary;
using ExpressedRealms.Events.API.API.Events.Publish;
using ExpressedRealms.Events.API.API.EventScheduleItem.Create;
using ExpressedRealms.Events.API.API.EventScheduleItem.Delete;
using ExpressedRealms.Events.API.API.EventScheduleItem.Edit;
using ExpressedRealms.Events.API.API.EventScheduleItem.Get;
using ExpressedRealms.FeatureFlags;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using EditEventEndpoint = ExpressedRealms.Events.API.API.Events.Edit.EditEventEndpoint;
using GetEventCheckinInfoEndpoint = ExpressedRealms.Events.API.API.EventCheckin.GetCheckDetails.GetEventCheckinInfoEndpoint;

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
            .RequirePolicyAuthorization(Policies.ManageEvents)
            .RequirePermission(Permissions.Event.Create);

        endpointGroup
            .MapPut("{id}", EditEventEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManageEvents)
            .RequirePermission(Permissions.Event.Edit);

        endpointGroup
            .MapDelete("{id}", DeleteEventEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManageEvents)
            .RequirePermission(Permissions.Event.Delete);

        endpointGroup
            .MapPost("{id}/publish", PublishEventEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManageEvents)
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
            .RequirePermission(Permissions.EventScheduleItem.Delete);

        endpointGroup
            .MapPost("{eventId}/questions/", CreateEventQuestionEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.EventQuestion.Create);

        endpointGroup
            .MapPut("{eventId}/questions/{questionId}", EditEventQuestionEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.EventQuestion.Edit);

        endpointGroup
            .MapDelete("{eventId}/questions/{questionId}", DeleteEventQuestionEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.EventQuestion.Delete);

        endpointGroup
            .MapGet("{eventId}/questions/", GetEventQuestionsEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.EventQuestion.View);
        
        endpointGroup
            .MapGet("checkin/available", GetEventCheckinShowStatusEndpoint.ExecuteAsync);
        
        endpointGroup
            .MapGet("checkin/info", GetEventCheckinInfoEndpoint.ExecuteAsync);
        
        endpointGroup
            .MapGet("checkin/lookup/{lookupId}", GetGoCheckinInfoEndpoint.ExecuteAsync)
            .RequireFeatureToggle(ReleaseFlags.ShowEventCheckin)
            .RequirePermission(Permissions.Event.Checkin);
    }
}
