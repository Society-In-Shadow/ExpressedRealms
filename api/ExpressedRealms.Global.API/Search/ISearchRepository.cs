namespace ExpressedRealms.Global.API.Search;

internal interface ISearchRepository
{
    Task<List<SearchInformationDto>> GetExpressionSections(string search);
}