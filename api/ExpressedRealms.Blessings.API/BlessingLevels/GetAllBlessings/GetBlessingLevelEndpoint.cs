using ExpressedRealms.Blessings.UseCases.BlessingLevels.GetBlessingLevel;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Blessings.API.BlessingLevels.GetAllBlessings;

public static class GetBlessingLevelEndpoint
{
    public static async Task<
        Results<Ok<BlessingLevelResponse>, ValidationProblem, NotFound>
    > Execute(int blessingId, int levelId, IGetBlessingLevelUseCase useCase)
    {
        var results = await useCase.ExecuteAsync(
            new GetBlessingLevelModel() { BlessingId = blessingId, LevelId = levelId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new BlessingLevelResponse()
            {
                Description = results.Value.Description,
                Level = results.Value.Level,
                XpCost = results.Value.XpCost,
                XpGain = results.Value.XpGain,
                Id = results.Value.Id,
            }
        );
    }
}
