using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.Requests;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections.DTOs;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.EditExpressionSubSectionText;

internal static class EditExpressionSubSectionTextEndpoint
{
    internal static async Task<Results<NotFound, ValidationProblem, NoContent>> ExecuteAsync(
        int expressionId,
        int sectionId,
        EditExpressionSubSectionTextRequest request,
        IExpressionTextSectionRepository repository
    )
    {
        var results = await repository.EditExpressionTextSectionAsync(
            new EditExpressionTextSectionDto()
            {
                Id = sectionId,
                ExpressionId = expressionId,
                Name = request.Name,
                Content = request.Content,
                SectionTypeId = request.SectionTypeId,
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
