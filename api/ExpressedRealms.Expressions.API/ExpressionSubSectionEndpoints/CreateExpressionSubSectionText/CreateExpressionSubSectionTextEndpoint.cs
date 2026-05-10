using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.Requests;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections.DTOs;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.CreateExpressionSubSectionText;

internal static class CreateExpressionSubSectionTextEndpoint
{
    internal static async Task<Results<NotFound, ValidationProblem, Created<int>>> ExecuteAsync(
        int expressionId,
        CreateExpressionSubSectionTextRequest request,
        IExpressionTextSectionRepository repository
    )
    {
        var results = await repository.CreateExpressionTextSectionAsync(
            new CreateExpressionTextSectionDto()
            {
                ExpressionId = expressionId,
                Name = request.Name,
                Content = request.Content,
                SectionTypeId = request.SectionTypeId,
                ParentId = request.ParentId,
            }
        );

        if (results.HasNotFound(out var notFound))
            return notFound;
        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Created("/", results.Value);
    }
}
