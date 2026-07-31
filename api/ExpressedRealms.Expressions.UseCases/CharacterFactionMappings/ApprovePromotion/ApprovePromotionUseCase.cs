using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Factions.CharacterFactionMappingModels;
using ExpressedRealms.DB.Models.Factions.FactionRankModels;
using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.Expressions.Repository.Factions;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMappings.ApprovePromotion;

internal sealed class ApprovePromotionUseCase(
    IFactionRepository factionRepository,
    ICharacterRepository characterRepository,
    ICharacterFactionRepository characterFactionRepository,
    ICharacterKnowledgeRepository knowledgeLevelRepository,
    TimeProvider timeProvider,
    IUserContext userContext,
    ApprovePromotionModelValidator validator,
    CancellationToken cancellationToken
) : IApprovePromotionUseCase
{
    public async Task<Result<int>> ExecuteAsync(ApprovePromotionModel model)
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

        var factionLevel = await factionRepository.GetFactionLevelAsync(model.FactionLevelId);
        if (factionLevel is null)
            return ValidationHelper.AddSingleValidationFailure(
                nameof(model.FactionLevelId),
                "This faction level does not exist."
            );

        var characterFactionInfo = await characterFactionRepository.GetPlayerFactionInfo(
            model.CharacterId
        );
        if (characterFactionInfo is null)
            return ValidationHelper.AddSingleValidationFailure(
                nameof(model.CharacterId),
                "Character does not have a faction."
            );

        if (factionLevel.FactionId != characterFactionInfo.FactionId)
            return ValidationHelper.AddSingleValidationFailure(
                nameof(model.FactionLevelId),
                "This faction level does not belong to the character's faction."
            );

        if (factionLevel.FactionRankId == FactionRankEnum.Basic)
            return ValidationHelper.AddSingleValidationFailure(
                nameof(model.FactionLevelId),
                "Basic faction levels are automatically approved upon joining."
            );

        if (factionLevel.FactionRankId <= characterFactionInfo.FactionRankId)
            return ValidationHelper.AddSingleValidationFailure(
                nameof(model.FactionLevelId),
                "GO already approved this rank."
            );

        if (characterFactionInfo.FactionRankId != factionLevel.FactionRankId - 1)
            return ValidationHelper.AddSingleValidationFailure(
                nameof(model.FactionLevelId),
                "Character does not have a previous rank approved."
            );

        var hasKnowledgePrerequisites = await knowledgeLevelRepository.HasFactionPrerequisites(
            model.CharacterId,
            factionLevel.KnowledgeId!.Value,
            factionLevel.KnowledgeLevelId!.Value,
            factionLevel.Specialization!
        );
        if (!hasKnowledgePrerequisites)
            return ValidationHelper.AddSingleValidationFailure(
                nameof(model.FactionLevelId),
                "Character does not have one or more of the required knowledge, knowledge level, or specialization for this faction level."
            );

        var existingFactionLevel = await 
            characterFactionRepository.GetCharacterFactionMapping(model.CharacterId, model.FactionLevelId);

        if (existingFactionLevel != null)
        {
            existingFactionLevel.ApprovalDate = timeProvider.GetUtcNow();
            existingFactionLevel.ApprovedByUserId = userContext.CurrentUserId();
            existingFactionLevel.ApprovalReason = model.ApprovalReason;
            
            await characterFactionRepository.EditAsync(existingFactionLevel);
            
            return Result.Ok();
        }
        
        await characterFactionRepository.AddCharacterFactionMapping(
            new CharacterFactionMapping()
            {
                CharacterId = model.CharacterId,
                FactionLevelId = model.FactionLevelId,
                ApprovalDate = timeProvider.GetUtcNow(),
                ApprovalReason = model.ApprovalReason,
                ApprovedByUserId = userContext.CurrentUserId()
            }
        );

        return Result.Ok();
    }
}
