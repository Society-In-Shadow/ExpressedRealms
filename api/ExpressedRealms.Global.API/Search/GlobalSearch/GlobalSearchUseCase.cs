using ExpressedRealms.UseCases.Shared;
using FluentResults;
using Slugify;

namespace ExpressedRealms.Global.API.Search.GlobalSearch;

internal sealed class GlobalSearchUseCase(
    ISearchRepository searchRepository,
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

        var expressionSectionResults = await searchRepository.GetExpressionSections(
            useCaseModel.Search
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
