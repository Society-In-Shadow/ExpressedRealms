using ExpressedRealms.Shared;

namespace ExpressedRealms.Global.API.Search;

internal interface ISearchRepository : IGenericRepository
{
    Task<List<SearchInformationDto>> GetExpressionSections(
        string search,
        List<int> allowedExpressionIds
    );
    Task<List<SearchInformationDto>> GetFactionSections(
        string search,
        List<int> allowedExpressionIds
    );

    Task<List<SearchInformationDto>> GetFactionPowerSections(
        string search,
        List<int> allowedExpressionIds
    );

    Task<List<SearchInformationDto>> GetPowerPathPowerSections(
        string search,
        List<int> allowedExpressionIds
    );

    Task<List<SearchInformationDto>> GetPowerPathSections(
        string search,
        List<int> allowedExpressionIds
    );
}
