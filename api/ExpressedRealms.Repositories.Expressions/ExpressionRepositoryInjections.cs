using ExpressedRealms.Repositories.Expressions.Expressions;
using ExpressedRealms.Repositories.Expressions.Expressions.DTOs;
using ExpressedRealms.Repositories.Expressions.ExpressionTextSections;
using ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;
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

        services.AddScoped<GetExpressionTestSectionOptionsValidator>();
        services.AddScoped<CreateExpressionTextSectionDtoValidator>();
        services.AddScoped<EditExpressionTextSectionDtoValidator>();
        services.AddScoped<EditExpressionHierarchyDtoValidator>();

        services.AddScoped<IExpressionRepository, ExpressionRepository>();
        services.AddScoped<IExpressionTextSectionRepository, ExpressionTextSectionRepository>();
        return services;
    }
}
