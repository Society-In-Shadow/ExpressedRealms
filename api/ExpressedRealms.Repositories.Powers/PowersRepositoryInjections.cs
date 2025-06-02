using ExpressedRealms.Repositories.Powers.PowerPaths;
using ExpressedRealms.Repositories.Powers.Powers;
using ExpressedRealms.Repositories.Powers.Powers.DTOs.PowerCreate;
using ExpressedRealms.Repositories.Powers.Powers.DTOs.PowerEdit;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Repositories.Powers;

public static class PowersRepositoryInjections
{
    public static IServiceCollection AddPowerRepositoryInjections(this IServiceCollection services)
    {
        services.AddScoped<CreatePowerModelValidator>();
        services.AddScoped<EditPowerModelValidator>();
        services.AddScoped<IPowerRepository, PowerRepository>();
        services.AddScoped<IPowerPathRepository, PowerPathRepository>();
        return services;
    }
}
