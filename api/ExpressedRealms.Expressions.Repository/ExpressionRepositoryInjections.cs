using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.Expressions.DTOs;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections.DTOs;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Expressions.Repository;

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
