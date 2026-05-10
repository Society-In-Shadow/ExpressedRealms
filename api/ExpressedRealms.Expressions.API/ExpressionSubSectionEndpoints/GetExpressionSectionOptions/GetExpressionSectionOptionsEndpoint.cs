using ExpressedRealms.Expressions.API.Helpers;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections.DTOs;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.GetExpressionSectionOptions;

internal static class GetExpressionSectionOptionsEndpoint
{
    internal static async Task<Results<ValidationProblem, Ok<ExpressionSectionOptionsResponse>>> ExecuteAsync(
        int expressionId,
        int sectionId,
        IExpressionTextSectionRepository repository
    )
    {
        var optionsResult = await repository.GetExpressionTextSectionOptions(
            new GetExpressionTestSectionOptionsDto()
            {
                ExpressionId = expressionId,
                SectionId = sectionId == 0 ? null : sectionId,
            }
        );

        if (optionsResult.HasValidationError(out var validationProblem))
            return validationProblem;
        optionsResult.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new ExpressionSectionOptionsResponse()
            {
                SectionTypes = optionsResult
                    .Value.ExpressionSectionTypes.Select(x => new SectionTypeDto()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                    })
                    .ToList(),
                AvailableParents = ExpressionHelpers.BuildAvailableParentTree(
                    optionsResult.Value.AvailableParents
                ),
            }
        );
    }
}
