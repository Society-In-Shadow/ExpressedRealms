using ExpressedRealms.Characters.UseCases.ExperienceBreakdown;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetOverallStats;

internal static class GetOverallStatsEndpoint
{
    internal static async Task<Results<Ok<ExperienceBreakdownResponse>, NotFound, StatusCodeHttpResult, ValidationProblem>
    > Execute(int characterId, IGetCharacterExperienceBreakdownUseCase repository)
    {
        var status = await repository.ExecuteAsync(
            new () { CharacterId = characterId }
        );

        if (status.HasValidationError(out var validation))
            return validation;
        if (status.HasNotFound(out var notFound))
            return notFound;
        if (status.HasBeenDeletedAlready(out var deletedAlready))
            return deletedAlready;
        status.ThrowIfErrorNotHandled();

        return TypedResults.Ok(new ExperienceBreakdownResponse()
        {
            PowersXp = status.Value.PowersXp,
            StatsXp = status.Value.StatsXp,
            SkillsXp = status.Value.SkillsXp,
            KnowledgeXp = status.Value.KnowledgeXp,
            SetupPowersXp = status.Value.SetupPowersXp,
            SetupStatsXp = status.Value.SetupStatsXp,
            SetupSkillsXp = status.Value.SetupSkillsXp,
            SetupKnowledgeXp = status.Value.SetupKnowledgeXp,
            Total = status.Value.Total,
        });
    }
}
