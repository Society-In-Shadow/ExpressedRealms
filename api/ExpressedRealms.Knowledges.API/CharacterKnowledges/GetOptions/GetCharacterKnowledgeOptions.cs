using ExpressedRealms.Knowledges.UseCases.KnowledgeLevels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Knowledges.API.CharacterKnowledges.GetOptions;

public static class GetCharacterKnowledgeOptions
{
    public static async Task<Ok<KnowledgeOptionsResponse>> CharacterKnowledgeOptions(
        int characterId,
        IGetKnowledgeLevelsUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new GetKnowledgeLevelsModel() { CharacterId = characterId }
        );

        return TypedResults.Ok(
            new KnowledgeOptionsResponse()
            {
                AvailableExperience = results.Value.AvailableExperience,
                KnowledgeLevels = results
                    .Value.KnowledgeLevels.Select(x => new KnowledgeOptions()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Level = x.Level,
                        SpecializationCount = x.SpecializationCount,
                        StoneModifier = x.StoneModifier,
                        GeneralXpCost = x.GeneralXpCost,
                        TotalGeneralXpCost = x.TotalGeneralXpCost,
                        UnknownXpCost = x.UnknownXpCost,
                        TotalUnknownXpCost = x.TotalUnknownXpCost,
                    })
                    .ToList(),
            }
        );
    }
}
