using ExpressedRealms.Characters.Repository.Stats;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetStatsForCharacter;

internal static class GetStatsForCharacterEndpoint
{
    internal static async Task<Results<NotFound, Ok<GetAllStatsResponse>>> ExecuteAsync(
        int characterId,
        [FromServices] ICharacterStatRepository repository
    )
    {
        var results = await repository.GetAllStats(characterId);

        if (results.HasNotFound(out var notFound))
            return notFound;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new GetAllStatsResponse()
            {
                HasExtraMortis = results.Value.ExtraMortis,
                Stats = [.. results.Value.StatInfos.Select(x => new SmallStatInfo(x))],
            }
        );
    }
}
