using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.Expressions.Repository.Factions;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMapping.JoinFaction;

internal sealed class JoinFactionUseCase(
    IFactionRepository factionRepository,
    ICharacterRepository characterRepository,
    ICharacterFactionRepository characterFactionRepository,
    JoinFactionModelValidator validator,
    CancellationToken cancellationToken
) : IJoinFactionUseCase
{
    public async Task<Result<int>> ExecuteAsync(JoinFactionModel model)
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
                "Character Id is not of an expression type."
            );

        var factionRankId = await factionRepository.GetBasicFactionRankId(
            model.FactionId,
            character.ExpressionId
        );
        if (factionRankId is null)
            return ValidationHelper.AddSingleValidationFailure(
                nameof(model.FactionId),
                "This faction does not exist for the character's expression."
            );

        var factionId = await characterFactionRepository.JoinFaction(
            new DB.Models.Factions.CharacterFactionMappingModels.CharacterFactionMapping()
            {
                CharacterId = model.CharacterId,
                FactionLevelId = factionRankId.Value,
                RequestPromotion = false,
                ApprovalDate = DateTimeOffset.UtcNow,
            }
        );

        return Result.Ok(factionId);
    }
}
