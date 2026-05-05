using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerEdit;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Powers.API.PowerEndpoints.EditPower;

public static class EditPowerEndpoint
{
    public static async Task<Results<ValidationProblem, NotFound, NoContent>> Execute(
        int id,
        EditPowerRequest request,
        IPowerRepository repository
    )
    {
        var results = await repository.EditPower(
            new EditPowerModel()
            {
                Id = request.Id,
                Name = request.Name,
                Category = request.CategoryIds,
                Description = request.Description,
                GameMechanicEffect = request.GameMechanicEffect,
                Limitation = request.Limitation,
                PowerDuration = request.PowerDurationId,
                AreaOfEffect = request.AreaOfEffectId,
                PowerLevel = request.PowerLevelId,
                PowerActivationType = request.PowerActivationTypeId,
                Other = request.Other,
                IsPowerUse = request.IsPowerUse,
                Cost = request.Cost,
            }
        );

        if (results.HasNotFound(out var notFound))
        {
            return notFound;
        }

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;

        results.ThrowIfErrorNotHandled();

        return TypedResults.NoContent();
    }
}
