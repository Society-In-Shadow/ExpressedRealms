using System.Reflection;
using ExpressedRealms.Events.API.Discord;
using ExpressedRealms.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Events.API.UseCases.Configuration;

public static class EventUseCaseConfiguration
{
    public static IServiceCollection AddEventUseCaseInjections(this IServiceCollection services)
    {
        services.ImportGenericUseCases(Assembly.GetExecutingAssembly());
        services.ImportValidators(Assembly.GetExecutingAssembly());
        services.ImportRepositories(Assembly.GetExecutingAssembly());
        services.AddSingleton<IDiscordService, DiscordService>();
        
        return services;
    }
}
