using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.GetEdit;

internal sealed class GetCharacterMappingEditUseCase(
    IKnowledgeRepository knowledgeRepository,
    ICharacterKnowledgeRepository characterKnowledgeRepository,
    ICharacterFactionRepository characterFactionRepository,
    ICharacterRepository characterRepository,
    GetCharacterMappingEditModelValidator validator,
    CancellationToken cancellationToken
) : IGetCharacterMappingEditUseCase
{
    public async Task<Result<GetCharacterMappingEditReturnModel>> ExecuteAsync(
        GetCharacterMappingEditModel model
    )
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var hasAccess = await characterRepository.CharacterExistsAsync(model.CharacterId);
        if (!hasAccess)
            return ValidationHelper.AddSingleValidationFailure(
                nameof(model.CharacterId),
                "User does not have access."
            );

        var mapping = await characterKnowledgeRepository.GetCharacterKnowledgeMappingForViewing(
            model.MappingId
        );
        if (mapping is null)
            return ValidationHelper.AddSingleValidationFailure(
                nameof(model.MappingId),
                "Mapping does not exist"
            );

        var knowledgeLevels = await knowledgeRepository.GetLevelsForKnowledge(
            mapping.IsUnknownType
        );
        var characterFactionLevels = await characterFactionRepository.GetLatestPlayerFactionLevels(
            model.CharacterId
        );
        var specializations = await characterKnowledgeRepository.GetSpecializationsForKnowledge(
            model.MappingId
        );

        return Result.Ok(
            new GetCharacterMappingEditReturnModel()
            {
                Id = mapping.KnowledgeId,
                Description = mapping.Description,
                Name = mapping.Name,
                SelectedLevelId = mapping.SelectedLevelId,
                KnowledgeType = mapping.Type,
                Notes = mapping.Notes,
                KnowledgeLevels = knowledgeLevels
                    .Select(x => new KnowledgeLevel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Level = x.Level,
                        SpecializationCount = x.SpecializationCount,
                        Stones = x.Stones,
                        TotalXpCost = x.TotalXpCost,
                        IsSelected = x.Id == mapping.SelectedLevelId,
                    })
                    .ToList(),
                MinimumKnowledgeId = characterFactionLevels
                    .Where(y => y.KnowledgeId == mapping.KnowledgeId)
                    .Max(y => y.KnowledgeLevel?.Id),
                BlockFactionChanges = characterFactionLevels.All(y =>
                    y.KnowledgeId != mapping.KnowledgeId
                ),
                Specializations = specializations
                    .Select(y => new SpecializationReturnModel()
                    {
                        Name = y.Name,
                        Description = y.Description,
                        Id = y.Id,
                        Notes = y.Notes,
                        BlockFactionChanges = characterFactionLevels.Any(z =>
                            z.KnowledgeSpecialization == y.Name
                            && z.KnowledgeId == mapping.KnowledgeId
                        ),
                    })
                    .ToList(),
            }
        );
    }
}
