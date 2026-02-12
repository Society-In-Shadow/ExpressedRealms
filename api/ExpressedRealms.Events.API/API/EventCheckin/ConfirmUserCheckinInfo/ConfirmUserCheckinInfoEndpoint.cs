using ExpressedRealms.Events.API.UseCases.EventCheckin.ConfirmedUserInfo;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.ConfirmUserCheckinInfo;

public static class ConfirmUserCheckinInfoEndpoint
{
    public static async Task<
        Results<Ok<GetGoCheckinInfoResponse>, ValidationProblem, NotFound>
    > ExecuteAsync(string lookupId, [FromServices] IConfirmedUserInfoUseCase useCase)
    {
        var results = await useCase.ExecuteAsync(
            new ()
            {
                LookupId = lookupId
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new GetGoCheckinInfoResponse()
            {
                PlayerName = results.Value.PlayerName,
                IsFirstTimeUser = results.Value.IsFirstTimeUser,
                CheckinId = results.Value.CheckinId,
                PlayerNumber = results.Value.PlayerNumber,
                AssignedXp = results.Value.AssignedXp,
                PrimaryCharacterInfo = results.Value.PrimaryCharacterInfo is null ? null : new PrimaryCharacterInfo()
                {
                    CharacterId = results.Value.PrimaryCharacterInfo.CharacterId,
                    CharacterName = results.Value.PrimaryCharacterInfo.CharacterName
                },
                QuestionResponses = results.Value.QuestionAnswers.Select(x => new QuestionResponse()
                {
                    Response = x.Response,
                    QuestionId = x.QuestionId
                }).ToList()
            }
        );
    }
}
