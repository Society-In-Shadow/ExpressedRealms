using ExpressedRealms.Characters.API.StatEndPoints.Requests;
using ExpressedRealms.Characters.Repository.Stats;
using ExpressedRealms.Characters.Repository.Stats.DTOs;
using ExpressedRealms.Characters.Repository.Stats.Enums;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.EditStatInfo;

internal static class EditStatInfoForCharacterEndpoint
{
    internal static async Task<Results<NotFound, NoContent, ValidationProblem, BadRequest<string>>> ExecuteAsync(
        int characterId,
        StatType statTypeId,
        EditStatRequest request,
        ICharacterStatRepository repository
    )
    {
        var results = await repository.UpdateCharacterStat(
            new EditStatDto()
            {
                CharacterId = characterId,
                StatTypeId = statTypeId,
                LevelTypeId = request.LevelTypeId,
            }
        );

        if (results.HasNotFound(out var notFound))
            return notFound;
        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasInsufficientXP(out var insufficientXPMessage))
            return insufficientXPMessage;
        results.ThrowIfErrorNotHandled();

        return TypedResults.NoContent();
    }
}
