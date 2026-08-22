using ExpressedRealms.Characters.UseCases.Characters.GetGoCheckinInfo;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetGoCheckinInfo;

internal static class GetGoCheckinInfoEndpoint
{
    internal static async Task<
        Results<Ok<GoCheckinChecks>, NotFound, StatusCodeHttpResult, ValidationProblem>
    > ExecuteAsync(int characterId, [FromServices] IGetGoCheckinInfoUseCase repository)
    {
        var status = await repository.ExecuteAsync(new() { Id = characterId });

        if (status.HasValidationError(out var validation))
            return validation;
        if (status.HasNotFound(out var notFound))
            return notFound;
        if (status.HasBeenDeletedAlready(out var deletedAlready))
            return deletedAlready;
        status.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new GoCheckinChecks()
            {
                Contacts = status
                    .Value.Contacts.Select(x => new ContactCheck() { Id = x.Id, Name = x.Name })
                    .ToList(),
                KnowledgeChecks = status
                    .Value.Knowledges.Select(x => new KnowledgeCheck()
                    {
                        Name = x.Name,
                        Id = x.Id,
                        IsDoctorateLevel = x.IsDoctorateLevel,
                        IsUnknownKnowledge = x.IsUnknownKnowledge,
                    })
                    .ToList(),
            }
        );
    }
}
