using ExpressedRealms.Characters.UseCases.Contacts.GetContacts;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.ContactEndpoints.GetContacts;

public static class GetContactsEndpoint
{
    public static async Task<
        Results<Ok<GetContactsResponse>, NotFound, ValidationProblem>
    > ExecuteAsync(int characterId, [FromServices] IGetContactsUseCase useCase)
    {
        var results = await useCase.ExecuteAsync(
            new GetContactsModel() { CharacterId = characterId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new GetContactsResponse()
            {
                Contacts = results
                    .Value.Select(x => new Contact()
                    {
                        KnowledgeLevel = x.KnowledgeLevel,
                        Name = x.Name,
                        Knowledge = x.Knowledge,
                        Id = x.Id,
                        IsApproved = x.IsApproved,
                        UsesPerWeek = x.UsesPerWeek,
                    })
                    .ToList(),
            }
        );
    }
}
