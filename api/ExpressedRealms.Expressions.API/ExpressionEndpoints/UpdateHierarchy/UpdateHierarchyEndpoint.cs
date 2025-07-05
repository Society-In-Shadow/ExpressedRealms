using ExpressedRealms.Expressions.API.Helpers;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections.DTOs;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ExpressionEndpoints.UpdateHierarchy;

internal static class UpdateHierarchyEndpoint
{
    public static async Task<Results<NotFound, ValidationProblem, NoContent>> UpdateHierarchy(
        int expressionId,
        EditExpressionHierarchyItemRequest editExpressionRequest,
        IExpressionTextSectionRepository repository
    )
    {
        var results = await repository.UpdateSectionHierarchyAndSorting(
            new EditExpressionHierarchyDto()
            {
                ExpressionId = expressionId,
                Items = ExpressionHelpers.FlattenHierarchy(editExpressionRequest.Items),
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
