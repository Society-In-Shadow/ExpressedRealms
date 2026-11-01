using ExpressedRealms.FeatureFlags;
using ExpressedRealms.Global.API.Search.Endpoints.GlobalSearch;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ExpressedRealms.Global.API.Search.Endpoints;

internal static class SearchEndpoints
{
    internal static void AddSearchEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("").WithTags("Search");

        endpointGroup
            .MapGet("search", GlobalSearchEndpoint.ExecuteAsync)
            .RequireFeatureToggle(ReleaseFlags.ShowGlobalSearch);
    }
}
