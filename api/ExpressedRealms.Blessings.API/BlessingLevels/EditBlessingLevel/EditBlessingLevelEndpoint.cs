using ExpressedRealms.Blessings.UseCases.BlessingLevels.EditBlessingLevel;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Blessings.API.BlessingLevels.EditBlessingLevel;

public static class EditBlessingLevelEndpoint
{
    public static async Task<Results<Ok, NotFound, ValidationProblem>> Execute(
        int blessingId,
        int levelId,
        [FromBody] EditBlessingLevelRequest request,
        [FromServices] IEditBlessingLevelUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new EditBlessingLevelModel()
            {
                Level = request.Level,
                Description = request.Description,
                BlessingId = blessingId,
                LevelId = levelId,
                XpCost = request.XpCost,
                XpGain = request.XpGain,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
