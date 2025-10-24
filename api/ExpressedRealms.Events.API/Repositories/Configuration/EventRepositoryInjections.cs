using ExpressedRealms.Events.API.Repositories.Events;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Events.API.Repositories.Configuration;

public static class EventRepositoryInjections
{
    public static IServiceCollection AddEventRepositoryInjections(
        this IServiceCollection services
    )
    {
        services.AddScoped<IEventRepository, EventRepository>();
        return services;
    }
}
