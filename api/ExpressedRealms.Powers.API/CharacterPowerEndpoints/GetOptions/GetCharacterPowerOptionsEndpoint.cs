using ExpressedRealms.Powers.UseCases.CharacterPower.GetOptions;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Powers.API.CharacterPowerEndpoints.GetOptions;

public static class GetCharacterPowerOptionsEndpoint
{
    public static async Task<
        Results<Ok<CharacterPickablePowerBaseResponse>, NotFound, ValidationProblem>
    > GetOptions(int characterId, int powerId, IGetCharacterPowerOptionsUseCase useCase)
    {
        var results = await useCase.ExecuteAsync(
            new() { CharacterId = characterId, PowerId = powerId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new CharacterPickablePowerBaseResponse()
            {
                AvailableXp = results.Value.AvailableExperience,
                PowerXp = results.Value.PowerLevelExperience,
            }
        );
    }
}
