using ExpressedRealms.Expressions.UseCases.CharacterFactionMapping.GetFactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.CharacterFactionEndpoints.GetFactions;

public static class GetCharacterFactionsEndpoint
{
    public static async Task<Ok<FactionResponse>> ExecuteAsync(
        int characterId,
        IGetCharacterFactionLevelsUseCase createFactionUseCase
    )
    {
        var results = await createFactionUseCase.ExecuteAsync(
            new () { CharacterId = characterId }
        );

        return TypedResults.Ok(
            new FactionResponse()
            {
                FactionLevels = results.Value.FactionLevels.Select(x => new FactionLevelModel()
                    {
                        FactionLevelId = x.FactionLevelId,
                        Approver = x.Approver,
                        ApprovalReason = x.ApprovalReason,
                        RequestedPromotion = x.RequestedPromotion,
                        HasKnowledge = x.HasKnowledge,
                        HasKnowledgeLevel = x.HasKnowledgeLevel,
                        HasSpecialization = x.HasSpecialization,
                        RequestedPromotionReason = x.RequestedPromotionReason,
                        ApprovalDate = x.ApprovalDate,
                    })
                    .ToList(),
            }
        );
    }
}
