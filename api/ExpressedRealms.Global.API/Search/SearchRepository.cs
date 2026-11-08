using ExpressedRealms.DB;
using ExpressedRealms.DB.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Global.API.Search;

internal sealed class SearchRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : ISearchRepository
{
    public async Task<List<SearchInformationDto>> GetExpressionSections(
        string search,
        List<int> allowedExpressionIds
    )
    {
        const string language = "english";
        return await context
            .ExpressionSections.Where(x =>
                x.SearchVector.Matches(EF.Functions.WebSearchToTsQuery(language, search))
                && allowedExpressionIds.Contains(x.ExpressionId)
            )
            .OrderByDescending(x => x.Name.ToLower() == search.ToLower())
            .ThenByDescending(x =>
                x.SearchVector.Rank(EF.Functions.WebSearchToTsQuery(language, search))
            )
            .Select(x => new SearchInformationDto()
            {
                EntryId = x.Id,
                Section = x.Expression.CmsType.Name,
                NavName = x.Expression.Name,
                SectionName = x.Name,
                SectionContent = EF
                    .Functions.WebSearchToTsQuery(language, search)
                    .GetResultHeadline(
                        language,
                        x.ProcessedPlainText ?? "",
                        "MaxWords=20, MinWords=10, MaxFragments=2, StartSel=<mark>, StopSel=</mark>"
                    ),
                Rank = x.SearchVector.Rank(EF.Functions.WebSearchToTsQuery(language, search)),
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<SearchInformationDto>> GetFactionSections(
        string search,
        List<int> allowedExpressionIds
    )
    {
        const string language = "english";
        return await context
            .Factions.Where(x =>
                x.SearchVector.Matches(EF.Functions.WebSearchToTsQuery(language, search))
                && allowedExpressionIds.Contains(x.ExpressionId)
            )
            .OrderByDescending(x => x.Name.ToLower() == search.ToLower())
            .ThenByDescending(x =>
                x.SearchVector.Rank(EF.Functions.WebSearchToTsQuery(language, search))
            )
            .Select(x => new SearchInformationDto()
            {
                EntryId = x.Id,
                Section = x.Expression.CmsType.Name,
                NavName = x.Expression.Name,
                SectionName = x.Name,
                SectionSubSection = "Faction",
                SectionContent = EF
                    .Functions.WebSearchToTsQuery(language, search)
                    .GetResultHeadline(
                        language,
                        x.ProcessedPlainText ?? "",
                        "MaxWords=20, MinWords=10, MaxFragments=2, StartSel=<mark>, StopSel=</mark>"
                    ),
                Rank = x.SearchVector.Rank(EF.Functions.WebSearchToTsQuery(language, search)),
            })
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<SearchInformationDto>> GetFactionPowerSections(
        string search,
        List<int> allowedExpressionIds
    )
    {
        const string language = "english";
        return await context
            .FactionLevels.Where(x =>
                x.Power != null && x.Power.SearchVector.Matches(EF.Functions.WebSearchToTsQuery(language, search))
                && allowedExpressionIds.Contains(x.Faction.ExpressionId)
            )
            .OrderByDescending(x => x.Power.Name.ToLower() == search.ToLower())
            .ThenByDescending(x =>
                x.Power.SearchVector.Rank(EF.Functions.WebSearchToTsQuery(language, search))
            )
            .Select(x => new SearchInformationDto()
            {
                EntryId = x.Id,
                Section = x.Faction.Expression.CmsType.Name,
                NavName = x.Faction.Expression.Name,
                SectionName = x.Power!.Name,
                SectionSubSection = "Faction Power",
                SectionContent = EF
                    .Functions.WebSearchToTsQuery(language, search)
                    .GetResultHeadline(
                        language,
                        x.Power.ProcessedPlainText ?? "",
                        "MaxWords=20, MinWords=10, MaxFragments=2, StartSel=<mark>, StopSel=</mark>"
                    ),
                Rank = x.Power.SearchVector.Rank(EF.Functions.WebSearchToTsQuery(language, search)),
            })
            .ToListAsync(cancellationToken);
    }

    public async Task EditAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await context.CommonSaveChanges(entity, cancellationToken);
    }
}
