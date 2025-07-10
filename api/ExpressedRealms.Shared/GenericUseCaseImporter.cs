using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Shared;

public static class GenericUseCaseImporter
{
    public static void ImportGenericUseCases(this IServiceCollection services, Assembly assembly)
    {
        services.Scan(scan =>
            scan.FromAssemblies(assembly)
                .AddClasses(
                    classes => classes.AssignableTo(typeof(IGenericUseCase<,>)),
                    publicOnly: false
                )
                .AddClasses(
                    classes => classes.AssignableTo(typeof(IGenericUseCase<>)),
                    publicOnly: false
                )
                .AsMatchingInterface()
                .WithScopedLifetime()
        );
    }

    public static void ImportValidators(this IServiceCollection services, Assembly assembly)
    {
        services.Scan(scan =>
            scan.FromAssemblies(assembly)
                .AddClasses(
                    classes => classes.AssignableTo(typeof(AbstractValidator<>)),
                    publicOnly: false
                )
                .AsSelf()
                .WithScopedLifetime()
        );
    }
}
