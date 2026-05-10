using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ExpressionSectionDto = ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.DTOs.ExpressionSectionDto;

namespace ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.GetExpressionSection;

internal static class GetExpressionSectionEndpoint
{
    internal static async Task<Results<NotFound, Ok<ExpressionSectionDto>>> ExecuteAsync(
        int id,
        IExpressionTextSectionRepository repository
    )
    {
        var section = await repository.GetExpressionSection(id);

        if (section is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(
            new ExpressionSectionDto()
            {
                Name = section.Name,
                Id = section.Id,
                Content = section.Content,
                SectionTypeName = section.SectionTypeName,
                SubSections = new List<ExpressionSectionDto>(),
            }
        );
    }
}
