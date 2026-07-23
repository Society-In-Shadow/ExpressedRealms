using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Factions.FactionRankModels;
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

        var playerLevels = await characterFactionRepository.GetLatestPlayerFactionLevels(
            model.CharacterId
        );

        var factionLevels = await characterFactionRepository.GetFactionLevels(model.CharacterId);

        var knowledgeInfo = await knowledgeRepository.GetSimpleKnowledgesForCharacter(
            model.CharacterId
        );

        var currentFaction = await characterFactionRepository.GetPlayerFactionInfo(model.CharacterId);

        return Result.Ok(
            new FactionsReturnModel()
            {
                FactionLevels = factionLevels
                    .Select(x =>
                    {
                        var knowledge = knowledgeInfo.FirstOrDefault(y => y.Id == x.KnowledgeId);
                        var playerLevel = playerLevels.FirstOrDefault(y =>
                            y.FactionLevelId == x.Id
                        );

                        var factionLevel = new CharacterFactionLevelInfo()
                        {
                            FactionLevelId = x.Id,
                            HasKnowledge = knowledge is not null,
                            HasKnowledgeLevel =
                                knowledge?.Level >= x.KnowledgeLevel
                                && x.FactionRankId != FactionRankEnum.Basic,
                            HasSpecialization = x.Specialization is not null
                                ? knowledge?.Specializations.Contains(x.Specialization)
                                : false,
                        };

                        if (playerLevel is null)
                            return factionLevel;

                        factionLevel.Approver = playerLevel.Approver;
                        factionLevel.ApprovalReason = playerLevel.ApprovalReason;
                        factionLevel.RequestedPromotion = playerLevel.RequestedPromotion;
                        factionLevel.RequestedPromotionReason =
                            playerLevel.RequestedPromotionReason;
                        factionLevel.CharacterNotes = playerLevel.CharacterNotes;
                        factionLevel.ApprovalDate = playerLevel.ApprovalDate;

                        return factionLevel;
                    })
                    .ToList(),
                FactionId = currentFaction?.FactionId
            }
        );
    }
}
