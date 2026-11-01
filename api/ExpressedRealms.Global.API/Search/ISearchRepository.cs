using ExpressedRealms.Shared;

namespace ExpressedRealms.Global.API.Search;

internal interface ISearchRepository : IGenericRepository
{
    Task<List<SearchInformationDto>> GetExpressionSections(string search);
}