using System.Reflection;
using ExpressedRealms.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Global.API.Configuration;

public static class GlobalRepositoryConfiguration
{
    public static void ConfigureGlobalServices(this IServiceCollection services)
    {
        services.ImportGenericUseCases(Assembly.GetExecutingAssembly());
        services.ImportValidators(Assembly.GetExecutingAssembly());
        services.ImportRepositories(Assembly.GetExecutingAssembly());
    }
}
