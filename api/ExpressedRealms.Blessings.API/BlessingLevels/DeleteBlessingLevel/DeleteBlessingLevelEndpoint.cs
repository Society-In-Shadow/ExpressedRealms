using ExpressedRealms.Blessings.UseCases.BlessingLevels.DeleteBlessingLevel;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Blessings.API.BlessingLevels.DeleteBlessingLevel;

public static class DeleteBlessingLevelEndpoint
{
    public static async Task<Results<Ok, NotFound, ValidationProblem>> Execute(
        int blessingId,
        int levelId,
        [FromServices] IDeleteBlessingLevelUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new DeleteBlessingLevelModel() { BlessingId = blessingId, LevelId = levelId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
