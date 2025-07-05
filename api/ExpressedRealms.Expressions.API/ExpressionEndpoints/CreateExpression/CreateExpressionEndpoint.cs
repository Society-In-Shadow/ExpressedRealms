using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.Expressions.DTOs;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ExpressionEndpoints.CreateExpression;

internal static class CreateExpressionEndpoint
{
    public static async Task<Results<ValidationProblem, Created<int>>> CreateExpression(
        AddExpressionRequest request,
        IExpressionRepository repository
    )
    {
        var results = await repository.CreateExpressionAsync(
            new CreateExpressionDto()
            {
                Name = request.Name,
                ShortDescription = request.ShortDescription,
                NavMenuImage = request.NavMenuImage,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Created("/expressions", results.Value);
    }
}
