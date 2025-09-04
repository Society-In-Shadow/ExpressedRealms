using ExpressedRealms.Blessings.UseCases.BlessingLevels.GetBlessingLevel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Blessings.API.BlessingLevels.GetAllBlessings;

public static class GetBlessingLevelEndpoint
{
    public static async Task<Ok<BlessingLevelResponse>> Execute(
        int blessingId,
        int levelId,
        IGetBlessingLevelUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(new GetBlessingLevelModel()
        {
            BlessingId = blessingId,
            LevelId = levelId
        });

        return TypedResults.Ok(
            new BlessingLevelResponse()
            {
                Description = results.Value.Description,
                Level = results.Value.Level,
                XpCost = results.Value.XpCost,
                XpGain = results.Value.XpGain,
                Id = results.Value.Id
            }
        );
    }
}
