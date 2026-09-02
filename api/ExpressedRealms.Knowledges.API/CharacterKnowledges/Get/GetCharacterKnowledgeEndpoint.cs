using ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.GetEdit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Knowledges.API.CharacterKnowledges.Get;

public static class GetCharacterKnowledgeEndpoint
{
    public static async Task<Ok<GetCharacterMappingEditResponse>> ExecuteAsync(
        int characterId,
        int mappingId,
        IGetCharacterMappingEditUseCase createKnowledgeUseCase
    )
    {
        var results = await createKnowledgeUseCase.ExecuteAsync(
            new () { MappingId = mappingId, CharacterId = characterId }
        );

        return TypedResults.Ok(
            new GetCharacterMappingEditResponse()
            {
                Id = results.Value.Id,
                Description = results.Value.Description,
                KnowledgeType = results.Value.KnowledgeType,
                Name = results.Value.Name,
                SelectedLevelId = results.Value.SelectedLevelId,
                Notes = results.Value.Notes,
                KnowledgeLevels = results.Value.KnowledgeLevels.Select(x => new KnowledgeLevel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Level = x.Level,
                    SpecializationCount = x.SpecializationCount,
                    Stones = x.Stones,
                    TotalXpCost = x.TotalXpCost,
                    IsSelected = x.IsSelected
                }).ToList(),
                MinimumKnowledgeId = results.Value.MinimumKnowledgeId,
                BlockFactionChanges = results.Value.BlockFactionChanges,
                Specializations = results.Value.Specializations.Select(y => new Specialization()
                    {
                        Name = y.Name,
                        Description = y.Description,
                        Id = y.Id,
                        Notes = y.Notes,
                        BlockFactionChanges = y.BlockFactionChanges
                    })
                    .ToList()
            }
        );
    }
}
