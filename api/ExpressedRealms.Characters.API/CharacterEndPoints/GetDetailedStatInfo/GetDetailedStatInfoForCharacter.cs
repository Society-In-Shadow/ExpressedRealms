using ExpressedRealms.Characters.Repository.Stats;
using ExpressedRealms.Characters.Repository.Stats.DTOs;
using ExpressedRealms.Characters.Repository.Stats.Enums;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SingleStatInfo = ExpressedRealms.Characters.API.StatEndPoints.Responses.SingleStatInfo;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetDetailedStatInfo;

internal static class GetDetailedStatInfoForCharacter
{
    internal static async Task<Results<NotFound, ValidationProblem, Ok<SingleStatInfo>>> ExecuteAsync(
        int characterId,
        StatType statTypeId,
        [FromServices]ICharacterStatRepository repository
        )
    {
        var results = await repository.GetDetailedStatInfo(
            new GetDetailedStatInfoDto(characterId, statTypeId)
        );

        if (results.HasNotFound(out var notFound))
            return notFound;
        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(new SingleStatInfo(results.Value));
    }
}
