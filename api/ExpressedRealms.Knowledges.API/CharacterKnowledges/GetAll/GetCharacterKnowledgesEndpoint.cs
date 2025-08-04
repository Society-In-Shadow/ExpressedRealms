using ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.GetReadOnly;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Knowledges.API.CharacterKnowledges.GetAll;

public static class GetCharacterKnowledgesEndpoint
{
    public static async Task<Ok<CharacterKnowledgeBaseResponse>> GetKnowledges(
        int characterId,
        IGetKnowledgesForCharacterUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(new GetKnowledgesForCharacterModel()
        {
            CharacterId = characterId
        });

        return TypedResults.Ok(
            new CharacterKnowledgeBaseResponse()
            {
                Knowledges = results.Value
                    .Select(x => new CharacterKnowledgeResponse()
                    {
                        MappingId = x.MappingId,
                        Knowledge = new KnowledgeModel()
                        {
                            Name = x.Knowledge.Name,
                            Description = x.Knowledge.Description,
                            Type = x.Knowledge.Type,
                        },
                        StoneModifier = x.StoneModifier,
                        LevelName = x.LevelName,
                        Level = x.Level,
                        Specializations = x
                            .Specializations.Select(y => new SpecializationModel()
                            {
                                Name = y.Name,
                                Description = y.Description,
                                Id = y.Id,
                                Notes = y.Notes,
                            })
                            .ToList(),
                    }).ToList()
            }
        );
    }
}
