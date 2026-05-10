using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.Responses;
using ExpressedRealms.Expressions.API.Helpers;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.GetExpressionTextSections;

internal static class GetExpressionTextSectionsEndpoint
{
    internal static async Task<Results<NotFound, Ok<ExpressionBaseResponse>>> ExecuteAsync(
        int id,
        IExpressionTextSectionRepository repository
    )
    {
        var sections = await repository.GetExpressionTextSections(id);

        return TypedResults.Ok(
            new ExpressionBaseResponse()
            {
                ExpressionSections = ExpressionHelpers.BuildExpressionPage(sections),
            }
        );
    }
}
