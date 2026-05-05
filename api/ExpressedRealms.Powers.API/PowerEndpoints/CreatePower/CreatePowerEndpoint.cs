using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerCreate;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Powers.API.PowerEndpoints.CreatePower;

public static class CreatePowerEndpoint
{
    public static async Task<Results<ValidationProblem, NotFound, Created<int>>> Execute(
        CreatePowerRequest request,
        IPowerRepository repository
    )
    {
        var results = await repository.CreatePower(
            new CreatePowerModel()
            {
                Name = request.Name,
                Category = request.CategoryIds,
                Description = request.Description,
                GameMechanicEffect = request.GameMechanicEffect,
                Limitation = request.Limitation,
                PowerDuration = request.PowerDuration,
                AreaOfEffect = request.AreaOfEffect,
                PowerLevel = request.PowerLevel,
                PowerActivationType = request.PowerActivationType,
                Other = request.Other,
                PowerPathId = request.PowerPathId,
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

        return TypedResults.Created("/powers", results.Value);
    }
}
