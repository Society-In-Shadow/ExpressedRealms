using ExpressedRealms.DB;
using ExpressedRealms.DB.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Global.API.Search;

internal sealed class SearchRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : ISearchRepository
{
    public async Task<List<SearchInformationDto>> GetExpressionSections(string search)
    {
        const string language = "english";
        return await context.ExpressionSections
            .Where(x => x.SearchVector.Matches(
                EF.Functions.WebSearchToTsQuery(language, search)))
            .OrderByDescending(x => x.Name.ToLower() == search.ToLower())
            .ThenByDescending(x => x.SearchVector.Rank(
                EF.Functions.WebSearchToTsQuery(language, search)))
            .Select(x => new SearchInformationDto()
            {
                EntryId = x.Id,
                Section = x.Expression.CmsType.Name,
                NavName = x.Expression.Name,
                SectionName = x.Name,
                SectionContent = EF.Functions.WebSearchToTsQuery(language, search)
                    .GetResultHeadline(
                        language,
                        x.ProcessedPlainText,
                        "MaxWords=20, MinWords=10, MaxFragments=2, StartSel=<mark>, StopSel=</mark>")
            })
            .ToListAsync(cancellationToken);
    }

    public async Task EditAsync<TEntity>(TEntity entity) where TEntity : class
    {
        await context.CommonSaveChanges(entity, cancellationToken);
    }
}