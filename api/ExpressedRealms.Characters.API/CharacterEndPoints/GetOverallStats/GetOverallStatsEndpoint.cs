using ExpressedRealms.Characters.UseCases.ExperienceBreakdown;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetOverallStats;

internal static class GetOverallStatsEndpoint
{
    internal static async Task<
        Results<Ok<ExperienceBreakdownResponse>, NotFound, StatusCodeHttpResult, ValidationProblem>
    > Execute(int id, [FromServices] IGetCharacterExperienceBreakdownUseCase repository)
    {
        var status = await repository.ExecuteAsync(new() { CharacterId = id });

        if (status.HasValidationError(out var validation))
            return validation;
        if (status.HasNotFound(out var notFound))
            return notFound;
        if (status.HasBeenDeletedAlready(out var deletedAlready))
            return deletedAlready;
        status.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new ExperienceBreakdownResponse()
            {
                Experience = status.Value.ExperienceSections.Select(x => new ExperienceSection()
                {
                    Name = x.Name,
                    Total = x.Total,
                    CharacterCreateMax = x.Max
                }).ToList()
            }
        );
    }
}
