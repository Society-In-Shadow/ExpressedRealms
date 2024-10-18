using ExpressedRealms.Repositories.Expressions.ExpressionTextSections;
using ExpressedRealms.Server.EndPoints.CharacterEndPoints;
using ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Helpers;
using ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Responses;
using ExpressedRealms.Server.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Server.EndPoints.ExpressionEndpoints;

internal static class ExpectedSubSectionsEndpoints
{
    internal static void AddExpressionSubsectionEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("expressionSubSections")
            .AddFluentValidationAutoValidation()
            .WithTags("Expressions")
            .WithOpenApi();

        endpointGroup
            .MapGet(
                "{name}",
                async Task<Results<NotFound, Ok<ExpressionBaseResponse>>>
                    (string name, HttpContext httpContext, IExpressionTextSectionRepository repository) =>
                {
                    var expressionIdResult = await repository.GetExpressionId(name);
                    if (expressionIdResult.HasNotFound(out var notFound))
                        return notFound;
                    expressionIdResult.ThrowIfErrorNotHandled();
                    
                    var sections = await repository.GetExpressionTextSections(expressionIdResult.Value);
                    
                    var hasEditPolicy = await httpContext.UserHasPolicyAsync(
                        Policies.ExpressionEditorPolicy
                    );
                    
                    return TypedResults.Ok( new ExpressionBaseResponse()
                    {
                        ExpressionId = expressionIdResult.Value,
                        ExpressionSections = ExpressionHelpers.BuildExpressionPage(sections),
                        CanEditPolicy = hasEditPolicy
                    });
                }
            )
            .RequireAuthorization();
    }
}
