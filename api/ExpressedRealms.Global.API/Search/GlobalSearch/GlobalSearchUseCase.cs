using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.UseCases.Shared;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Slugify;

namespace ExpressedRealms.Global.API.Search.GlobalSearch;

internal sealed class GlobalSearchUseCase(
    ISearchRepository searchRepository,
    IExpressionRepository expressionRepository,
    GlobalSearchUseCaseModelValidator validator,
    CancellationToken cancellationToken
) : IGlobalSearchUseCase
{
    public async Task<Result<List<GlobalSearchUseCaseReturnModel>>> ExecuteAsync(
        GlobalSearchUseCaseModel useCaseModel
    )
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            useCaseModel,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var visibleExpressions = expressionRepository.GetAllowedExpressionsQuery();

        var availableExpressions = await visibleExpressions.Select(x => x.Id).ToListAsync();

        var expressionSectionResults = await searchRepository.GetExpressionSections(
            useCaseModel.Search,
            availableExpressions
        );

        var helper = new SlugHelper();

        return Result.Ok(
            expressionSectionResults
                .Select(x => new GlobalSearchUseCaseReturnModel()
                {
                    EntryId = x.EntryId,
                    NavName = x.NavName,
                    Section = x.Section,
                    SectionContent = x.SectionContent,
                    SectionName = x.SectionName,
                    Slug = helper.GenerateSlug(x.NavName),
                })
                .ToList()
        );
    }
}
