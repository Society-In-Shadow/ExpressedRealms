using ExpressedRealms.Events.API.API.Events;
using Microsoft.AspNetCore.Builder;

namespace ExpressedRealms.Events.API.API.Configuration;

public static class EventsApiConfiguration
{
    public static void ConfigureEventsEndPoints(this WebApplication app)
    {
        app.AddEventEndpoints();
    }
}
