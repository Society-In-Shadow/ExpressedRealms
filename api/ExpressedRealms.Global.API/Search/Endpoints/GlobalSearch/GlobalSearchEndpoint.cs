using ExpressedRealms.Global.API.Search.GlobalSearch;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Global.API.Search.Endpoints.GlobalSearch;

public static class GlobalSearchEndpoint
{
    public static async Task<Results<Ok<GlobalSearchResponse>, ValidationProblem, NotFound>> ExecuteAsync(
        [FromQuery]string query,
        [FromServices] IGlobalSearchUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new()
            {
                Search = query,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(new GlobalSearchResponse()
        {
            Items = results.Value.Select(x => new SearchResultDto()
            {
                EntryId = x.EntryId,
                NavName = x.NavName,
                Section = x.Section,
                SectionContent = x.SectionContent,
                SectionName = x.SectionName,
                Slug = x.Slug
            }).ToList()
        });
    }
}
