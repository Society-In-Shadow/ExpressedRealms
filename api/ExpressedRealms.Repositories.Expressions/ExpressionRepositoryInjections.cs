using ExpressedRealms.Repositories.Expressions.Expressions;
using ExpressedRealms.Repositories.Expressions.Expressions.DTOs;
using ExpressedRealms.Repositories.Expressions.ExpressionTextSections;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Repositories.Expressions;

public static class ExpressionRepositoryInjections
{
    public static IServiceCollection AddExpressionRepositoryInjections(
        this IServiceCollection services
    )
    {
        services.AddScoped<CreateExpressionDtoValidator>();
        services.AddScoped<EditExpressionDtoValidator>();

        services.AddScoped<IExpressionRepository, ExpressionRepository>();
        services.AddScoped<IExpressionTextSectionRepository, ExpressionTextSectionRepository>();
        return services;
    }
}
