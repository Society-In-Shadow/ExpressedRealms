using ExpressedRealms.DB;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Global.API.Search;

internal sealed class SearchRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : ISearchRepository
{
    public async Task<List<SearchInformationDto>> GetExpressionSections(string search)
    {
        return await context.ExpressionSections
            .Where(x => x.SearchVector.Matches(
                EF.Functions.WebSearchToTsQuery("english", search)))
            .OrderByDescending(x => x.Name.ToLower() == search.ToLower())
            .ThenByDescending(x => x.SearchVector.Rank(
                EF.Functions.WebSearchToTsQuery("english", search)))
            .Select(x => new SearchInformationDto()
            {
                EntryId = x.Id,
                Section = x.Expression.CmsType.Name,
                NavName = x.Expression.Name,
                SectionName = x.Name,
                SectionContent = EF.Functions.WebSearchToTsQuery("english", search)
                    .GetResultHeadline(
                        "english",
                        x.ProcessedPlainText,
                        "MaxWords=20, MinWords=10, MaxFragments=2, StartSel=<mark>, StopSel=</mark>")
            })
            .ToListAsync(cancellationToken);
    }
}