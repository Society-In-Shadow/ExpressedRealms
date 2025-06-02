using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathCreate;
using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathEdit;
using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerCreate;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerEdit;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Powers.Repository;

public static class PowersRepositoryInjections
{
    public static IServiceCollection AddPowerRepositoryInjections(this IServiceCollection services)
    {
        services.AddScoped<CreatePowerModelValidator>();
        services.AddScoped<EditPowerModelValidator>();
        services.AddScoped<CreatePowerPathModelValidator>();
        services.AddScoped<EditPowerPathModelValidator>();
        services.AddScoped<IPowerRepository, PowerRepository>();
        services.AddScoped<IPowerPathRepository, PowerPathRepository>();
        return services;
    }
}
