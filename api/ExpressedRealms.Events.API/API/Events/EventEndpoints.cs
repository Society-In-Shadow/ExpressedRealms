using ExpressedRealms.Authentication;
using ExpressedRealms.Events.API.API.Events.Create;
using ExpressedRealms.Events.API.API.Events.Edit;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Events.API.API.Events;

internal static class EventEndpoints
{
    internal static void AddEventEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("events")
            .AddFluentValidationAutoValidation()
            .WithTags("Events")
            .WithOpenApi();

        endpointGroup
            .MapPost("", CreateEventEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManageEvents);

        endpointGroup
            .MapPut("{id}", EditEventEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManageEvents);
    }
}
