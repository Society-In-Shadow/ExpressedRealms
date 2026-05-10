using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.Responses;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.GetExpressionTextSection;

internal static class GetExpressionTextSectionEndpoint
{
    internal static async Task<Results<NotFound, Ok<EditExpressionSectionResponse>>> ExecuteAsync(
        int expressionId,
        int sectionId,
        IExpressionTextSectionRepository repository
    )
    {
        var sectionResult = await repository.GetExpressionTextSection(sectionId);

        if (sectionResult.HasNotFound(out var sectionNotFound))
            return sectionNotFound;
        sectionResult.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new EditExpressionSectionResponse()
            {
                Name = sectionResult.Value.Name,
                Content = sectionResult.Value.Content,
                ParentId = sectionResult.Value.ParentId,
                SectionTypeId = sectionResult.Value.SectionTypeId,
            }
        );
    }
}
