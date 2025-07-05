using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.Expressions.DTOs;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ExpressionEndpoints.EditExpression;

internal static class EditExpressionEndpoint
{
    public static async Task<Results<NotFound, ValidationProblem, NoContent>> EditExpression(
        int expressionId,
        EditExpressionRequest editExpressionRequest,
        IExpressionRepository repository
    )
    {
        var results = await repository.EditExpressionAsync(
            new EditExpressionDto()
            {
                Id = editExpressionRequest.Id,
                Name = editExpressionRequest.Name,
                PublishStatus = editExpressionRequest.PublishStatus,
                ShortDescription = editExpressionRequest.ShortDescription,
                NavMenuImage = editExpressionRequest.NavMenuImage,
            }
        );

        if (results.HasNotFound(out var notFound))
            return notFound;
        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        results.ThrowIfErrorNotHandled();

        return TypedResults.NoContent();
    }
}
