using ExpressedRealms.DB;
using ExpressedRealms.DB.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Global.API.Search;

internal sealed class SearchRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : ISearchRepository
{
    private const string Language = "english";

    private const string SearchConfiguration =
        "MaxWords=20, MinWords=10, MaxFragments=2, StartSel=<mark>, StopSel=</mark>";

    public async Task<List<SearchInformationDto>> GetExpressionSections(
        string search,
        List<int> allowedExpressionIds
    )
    {
        return await context
            .ExpressionSections.Where(x =>
                x.SearchVector.Matches(EF.Functions.WebSearchToTsQuery(Language, search))
                && allowedExpressionIds.Contains(x.ExpressionId)
            )
            .OrderByDescending(x => x.Name.ToLower() == search.ToLower())
            .ThenByDescending(x =>
                x.SearchVector.Rank(EF.Functions.WebSearchToTsQuery(Language, search))
            )
            .Select(x => new SearchInformationDto()
            {
                EntryId = x.Id,
                Section = x.Expression.CmsType.Name,
                NavName = x.Expression.Name,
                SectionName = x.Name,
                SectionContent = EF
                    .Functions.WebSearchToTsQuery(Language, search)
                    .GetResultHeadline(Language, x.ProcessedPlainText ?? "", SearchConfiguration),
                Rank = x.SearchVector.Rank(EF.Functions.WebSearchToTsQuery(Language, search)),
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<SearchInformationDto>> GetFactionSections(
        string search,
        List<int> allowedExpressionIds
    )
    {
        return await context
            .Factions.Where(x =>
                x.SearchVector.Matches(EF.Functions.WebSearchToTsQuery(Language, search))
                && allowedExpressionIds.Contains(x.ExpressionId)
            )
            .OrderByDescending(x => x.Name.ToLower() == search.ToLower())
            .ThenByDescending(x =>
                x.SearchVector.Rank(EF.Functions.WebSearchToTsQuery(Language, search))
            )
            .Select(x => new SearchInformationDto()
            {
                EntryId = x.Id,
                Section = x.Expression.CmsType.Name,
                NavName = x.Expression.Name,
                SectionName = x.Name,
                SectionSubSection = "Faction",
                SectionContent = EF
                    .Functions.WebSearchToTsQuery(Language, search)
                    .GetResultHeadline(Language, x.ProcessedPlainText ?? "", SearchConfiguration),
                Rank = x.SearchVector.Rank(EF.Functions.WebSearchToTsQuery(Language, search)),
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<SearchInformationDto>> GetFactionPowerSections(
        string search,
        List<int> allowedExpressionIds
    )
    {
        return await context
            .FactionLevels.Where(x =>
                x.Power != null
                && x.Power.SearchVector.Matches(EF.Functions.WebSearchToTsQuery(Language, search))
                && allowedExpressionIds.Contains(x.Faction.ExpressionId)
            )
            .OrderByDescending(x => x.Power.Name.ToLower() == search.ToLower())
            .ThenByDescending(x =>
                x.Power.SearchVector.Rank(EF.Functions.WebSearchToTsQuery(Language, search))
            )
            .Select(x => new SearchInformationDto()
            {
                EntryId = x.Id,
                Section = x.Faction.Expression.CmsType.Name,
                NavName = x.Faction.Expression.Name,
                SectionName = x.Power!.Name,
                SectionSubSection = "Faction Power",
                SectionContent = EF
                    .Functions.WebSearchToTsQuery(Language, search)
                    .GetResultHeadline(
                        Language,
                        x.Power.ProcessedPlainText ?? "",
                        SearchConfiguration
                    ),
                Rank = x.Power.SearchVector.Rank(EF.Functions.WebSearchToTsQuery(Language, search)),
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<SearchInformationDto>> GetPowerPathPowerSections(
        string search,
        List<int> allowedExpressionIds
    )
    {
        return await context
            .PowerPathPowerMappings.Where(x =>
                x.Power != null
                && x.Power.SearchVector.Matches(EF.Functions.WebSearchToTsQuery(Language, search))
                && allowedExpressionIds.Contains(x.PowerPath.ExpressionId)
            )
            .OrderByDescending(x => x.Power.Name.ToLower() == search.ToLower())
            .ThenByDescending(x =>
                x.Power.SearchVector.Rank(EF.Functions.WebSearchToTsQuery(Language, search))
            )
            .Select(x => new SearchInformationDto()
            {
                EntryId = x.Id,
                Section = x.PowerPath.Expression.CmsType.Name,
                NavName = x.PowerPath.Expression.Name,
                SectionName = x.Power.Name,
                SectionSubSection = "Power Path Power",
                SectionContent = EF
                    .Functions.WebSearchToTsQuery(Language, search)
                    .GetResultHeadline(
                        Language,
                        x.Power.ProcessedPlainText ?? "",
                        SearchConfiguration
                    ),
                Rank = x.Power.SearchVector.Rank(EF.Functions.WebSearchToTsQuery(Language, search)),
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<SearchInformationDto>> GetPowerPathSections(
        string search,
        List<int> allowedExpressionIds
    )
    {
        return await context
            .PowerPaths.Where(x =>
                x.SearchVector.Matches(EF.Functions.WebSearchToTsQuery(Language, search))
                && allowedExpressionIds.Contains(x.ExpressionId)
            )
            .OrderByDescending(x => x.Name.ToLower() == search.ToLower())
            .ThenByDescending(x =>
                x.SearchVector.Rank(EF.Functions.WebSearchToTsQuery(Language, search))
            )
            .Select(x => new SearchInformationDto()
            {
                EntryId = x.Id,
                Section = x.Expression.CmsType.Name,
                NavName = x.Expression.Name,
                SectionName = x.Name,
                SectionSubSection = "Power Path",
                SectionContent = EF
                    .Functions.WebSearchToTsQuery(Language, search)
                    .GetResultHeadline(Language, x.ProcessedPlainText ?? "", SearchConfiguration),
                Rank = x.SearchVector.Rank(EF.Functions.WebSearchToTsQuery(Language, search)),
            })
            .ToListAsync(cancellationToken);
    }

    public async Task EditAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await context.CommonSaveChanges(entity, cancellationToken);
    }
}
