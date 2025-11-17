using ExpressedRealms.Characters.UseCases.AssignedXp.Get;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.AssignedXp.Get;

public static class GetEndpoint
{
    public static async Task<
        Results<Ok<GetAssignedXpResponse>, NotFound, ValidationProblem>
    > ExecuteAsync(
        int characterId,
        [FromServices] IGetAssignedXpMappingUseCase createKnowledgeUseCase
    )
    {
        var results = await createKnowledgeUseCase.ExecuteAsync(
            new GetAssignedXpMappingModel() { CharacterId = characterId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new GetAssignedXpResponse()
            {
                AssignedXpInfo = results
                    .Value.Select(x => new AssignedXpInfo()
                    {
                        Id = x.Id,
                        DateAssigned = x.DateAssigned,
                        Notes = x.Notes,
                        Amount = x.Amount,
                        Player = new BasicInfo() { Id = x.Player.Id, Name = x.Player.Name },
                        Character = new BasicInfo()
                        {
                            Id = x.Character.Id,
                            Name = x.Character.Name,
                        },
                        Event = new BasicInfo() { Id = x.Event.Id, Name = x.Event.Name },
                        XpType = new BasicInfo() { Id = x.XpType.Id, Name = x.XpType.Name },
                        Assigner = new BasicInfo() { Id = x.Assigner.Id, Name = x.Assigner.Name },
                    })
                    .ToList(),
            }
        );
    }
}
