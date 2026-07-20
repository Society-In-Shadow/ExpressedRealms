using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMapping.GetFactions;

internal sealed class GetCharacterFactionLevelsUseCase(
    ICharacterFactionRepository characterFactionRepository,
    ICharacterKnowledgeRepository knowledgeRepository,
    ICharacterRepository characterRepository,
    GetCharacterFactionLevelsModelValidator validator,
    CancellationToken cancellationToken
) : IGetCharacterFactionLevelsUseCase
{
    public async Task<Result<FactionsReturnModel>> ExecuteAsync(
        GetCharacterFactionLevelsModel model
    )
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var character = await characterRepository.FindCharacterAsync(model.CharacterId);
        if (character is null)
            return ValidationHelper.AddSingleValidationFailure(
                nameof(model.CharacterId),
                "Character Id does not exist."
            );

        var factionLevels = await characterFactionRepository.GetLatestPlayerFactionLevels(
            model.CharacterId
        );
        var knowledgeInfo = await knowledgeRepository.GetSimpleKnowledgesForCharacter(
            model.CharacterId
        );

        return Result.Ok(
            new FactionsReturnModel()
            {
                FactionLevels = factionLevels
                    .Select(x =>
                    {
                        var knowledge = knowledgeInfo.FirstOrDefault(y => y.Id == x.KnowledgeId);

                        return new CharacterFactionLevelInfo()
                        {
                            FactionLevelId = x.FactionLevelId,
                            Approver = x.Approver,
                            ApprovalReason = x.ApprovalReason,
                            RequestedPromotion = x.RequestedPromotion,
                            HasKnowledge = knowledge is not null,
                            HasKnowledgeLevel = knowledge?.LevelId == x.KnowledgeLevel?.Id,
                            HasSpecialization = x.KnowledgeSpecialization is not null
                                ? knowledge?.Specializations.Contains(x.KnowledgeSpecialization)
                                : false,
                            RequestedPromotionReason = x.RequestedPromotionReason,
                            CharacterNotes = x.CharacterNotes,
                            ApprovalDate = x.ApprovalDate,
                        };
                    })
                    .ToList(),
            }
        );
    }
}
