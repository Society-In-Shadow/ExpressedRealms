using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Models.Characters.XpTables;
using ExpressedRealms.UseCases.Shared;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Characters.ToggleExtraMortisUseCase;

internal sealed class ToggleExtraMortisUseCase(
    IXpRepository xpRepository,
    ICharacterRepository repository,
    ToggleExtraMortisModelValidator validator,
    CancellationToken cancellationToken
) : IToggleExtraMortisUseCase
{
    public async Task<Result> ExecuteAsync(ToggleExtraMortisModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var character = await repository.FindCharacterAsync(model.Id);
        
        var xpInfo = await xpRepository.GetAvailableXpForSection(
            model.Id,
            XpSectionTypes.Stats
        );

        const int extraMortisCost = 12;
        if (model.HasExtraMortis && xpInfo.AvailableXp - xpInfo.SpentXp < extraMortisCost)
        {
            return Result.Fail(
                new NotEnoughXPFailure(xpInfo.AvailableXp - extraMortisCost, extraMortisCost)
            );
        }
        
        if (character!.IsInCharacterCreation)
            return Result.Fail("You cannot modify extra mortis during character creation.");

        character.ExtraMortis = model.HasExtraMortis;

        await repository.EditAsync(character);

        return Result.Ok();
    }
}
