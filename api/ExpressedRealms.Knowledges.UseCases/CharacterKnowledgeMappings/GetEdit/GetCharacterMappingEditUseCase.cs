using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.GetEdit;

internal sealed class GetCharacterMappingEditUseCase(
    IKnowledgeRepository knowledgeRepository,
    ICharacterKnowledgeRepository characterKnowledgeRepository,
    ICharacterRepository characterRepository,
    GetCharacterMappingEditModelValidator validator,
    CancellationToken cancellationToken
) : IGetCharacterMappingEditUseCase
{
    public async Task<Result<GetCharacterMappingEditReturnModel>> ExecuteAsync(GetCharacterMappingEditModel model)
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
            return ValidationHelper.AddSingleValidationFailure(nameof(model.CharacterId), "User does not have access.");
        
        var mapping = await characterKnowledgeRepository.GetCharacterKnowledgeMappingForViewing(model.MappingId);
        if(mapping is null)
            return ValidationHelper.AddSingleValidationFailure(nameof(model.MappingId), "Mapping does not exist");
        
        var knowledgeLevels = await knowledgeRepository.GetLevelsForKnowledge(mapping.IsUnknownType);

        return Result.Ok(
            new GetCharacterMappingEditReturnModel()
            {
                Id = mapping.KnowledgeId,
                Description = mapping.Description,
                Name = mapping.Name,
                SelectedLevelId = mapping.SelectedLevelId,
                Notes = mapping.Notes,
                KnowledgeLevels = knowledgeLevels.Select(x => new KnowledgeLevel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Level = x.Level,
                    SpecializationCount = x.SpecializationCount,
                    Stones = x.Stones,
                    TotalXpCost = x.TotalXpCost,
                    IsSelected = x.Id == mapping.SelectedLevelId
                }).ToList()
            }
        );
    }
}
