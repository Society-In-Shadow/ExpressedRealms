using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.GetReadOnly;

internal sealed class GetKnowledgesForCharacterUseCase(
    ICharacterKnowledgeRepository mappingRepository,
    ICharacterFactionRepository characterFactionRepository,
    GetKnowledgesForCharacterModelValidator validator,
    CancellationToken cancellationToken
) : IGetKnowledgesForCharacterUseCase
{
    public async Task<Result<List<CharacterKnowledgeReturnModel>>> ExecuteAsync(
        GetKnowledgesForCharacterModel model
    )
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var knowledges = await mappingRepository.GetKnowledgesForCharacter(model.CharacterId);
        var characterFactionLevels = await characterFactionRepository.GetLatestPlayerFactionLevels(model.CharacterId);

        var items = knowledges
            .Select(x => new CharacterKnowledgeReturnModel()
            {
                MappingId = x.MappingId,
                Knowledge = new KnowledgeReturnModel()
                {
                    Id = x.Knowledge.Id,
                    Name = x.Knowledge.Name,
                    Description = x.Knowledge.Description,
                    Type = x.Knowledge.Type,
                    BlockFactionChanges = characterFactionLevels.All(y => y.KnowledgeId != x.Knowledge.Id)
                },
                StoneModifier = x.StoneModifier,
                LevelName = x.LevelName,
                Level = x.Level,
                LevelId = x.LevelId,
                Notes = x.Notes,
                SpecializationCount = x.SpecializationCount,
                MinimumKnowledgeId = characterFactionLevels.Where(y => y.KnowledgeId == x.Knowledge.Id).Max(y => y.KnowledgeLevel?.Id),
                Specializations = x
                    .Specializations.Select(y => new SpecializationReturnModel()
                    {
                        Name = y.Name,
                        Description = y.Description,
                        Id = y.Id,
                        Notes = y.Notes,
                        BlockFactionChanges = characterFactionLevels.Any(z => z.KnowledgeSpecialization == y.Name && z.KnowledgeId == x.Knowledge.Id)
                    })
                    .ToList(),
            })
            .ToList();
        
        

        return Result.Ok(items.ToList());
    }
}
