using ExpressedRealms.Powers.API.CharacterPowerEndpoints.GetAll.Responses;
using ExpressedRealms.Powers.UseCases.CharacterPower.GetPickablePowers.ReturnModels;
using ExpressedRealms.Powers.UseCases.CharacterPower.GetPowers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Powers.API.CharacterPowerEndpoints.GetAll;

public static class GetCharacterPowersEndpoint
{
    public static async Task<Ok<CharacterPowerBaseResponse>> GetPowers(
        int characterId,
        IGetCharacterPowersUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(new() { CharacterId = characterId });

        return TypedResults.Ok(
            new CharacterPowerBaseResponse()
            {
                Powers = results
                    .Value.Select(x => new PowerPathResponse()
                    {
                        Name = x.Name,
                        Powers = x
                            .Powers.Select(y => new PowerResponse()
                            {
                                Id = y.Id,
                                Name = y.Name,
                                Category =
                                    y.Category?.Select(z => new DetailedInformationResponse(z))
                                        .ToList() ?? new List<DetailedInformationResponse>(),
                                Description = y.Description,
                                GameMechanicEffect = y.GameMechanicEffect,
                                Limitation = y.Limitation,
                                PowerDuration = new DetailedInformationResponse(y.PowerDuration),
                                AreaOfEffect = new DetailedInformationResponse(y.AreaOfEffect),
                                PowerLevel = new DetailedInformationResponse(y.PowerLevel),
                                PowerActivationType = new DetailedInformationResponse(
                                    y.PowerActivationType
                                ),
                                Other = y.Other,
                                IsPowerUse = y.IsPowerUse,
                                Cost = y.Cost,
                                Prerequisites = y.Prerequisites is not null
                                    ? new PrerequisiteDetailsResponse()
                                    {
                                        RequiredAmount = y.Prerequisites.RequiredAmount,
                                        Powers = y.Prerequisites.Powers,
                                    }
                                    : null,
                            })
                            .ToList(),
                    })
                    .ToList(),
            }
        );
    }
}
