using ExpressedRealms.Events.API.API.Events.Create;
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
            .MapPost("", CreateEventEndpoint.ExecuteAsync);
    }
}
