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

        var searchResults = new List<SearchInformationDto>();
        var expressionSectionResults = await searchRepository.GetExpressionSections(
            useCaseModel.Search,
            availableExpressions
        );

        var factions = await searchRepository.GetFactionSections(
            useCaseModel.Search,
            availableExpressions
        );

        var factionPowers = await searchRepository.GetFactionPowerSections(
            useCaseModel.Search,
            availableExpressions
        );

        var powerPathPowerSections = await searchRepository.GetPowerPathPowerSections(
            useCaseModel.Search,
            availableExpressions
        );

        var powerPathSections = await searchRepository.GetPowerPathSections(
            useCaseModel.Search,
            availableExpressions
        );
        
        var blessings = await searchRepository.GetBlessingSections(useCaseModel.Search);

        searchResults.AddRange(expressionSectionResults);
        searchResults.AddRange(factions);
        searchResults.AddRange(factionPowers);
        searchResults.AddRange(powerPathPowerSections);
        searchResults.AddRange(powerPathSections);
        searchResults.AddRange(blessings);

        var helper = new SlugHelper();

        return Result.Ok(
            searchResults
                .OrderByDescending(x => x.Rank)
                .Select(x => new GlobalSearchUseCaseReturnModel()
                {
                    EntryId = x.EntryId,
                    NavName = x.NavName,
                    Section = x.Section,
                    SectionSubSection = x.SectionSubSection,
                    SectionContent = x.SectionContent,
                    SectionName = x.SectionName,
                    Slug = helper.GenerateSlug(x.NavName),
                })
                .ToList()
        );
    }
}
