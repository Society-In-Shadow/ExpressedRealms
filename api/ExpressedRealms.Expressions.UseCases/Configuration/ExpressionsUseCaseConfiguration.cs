using System.Reflection;
using ExpressedRealms.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Expressions.UseCases.Configuration;

public static class ExpressionsUseCaseConfiguration
{
    public static IServiceCollection AddExpressionTextSectionInjections(
        this IServiceCollection services
    )
    {
        services.ImportGenericUseCases(Assembly.GetExecutingAssembly());
        services.ImportValidators(Assembly.GetExecutingAssembly());

        return services;
    }
}
