using ExpressedRealms.Characters.Repository;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.UpdateCharacterXp;

internal sealed class UpdateCharacterXpUseCase(
    ICharacterRepository characterRepository,
    UpdateCharacterXpModelValidator validator,
    CancellationToken cancellationToken
) : IUpdateCharacterXpUseCase
{
    public async Task<Result> ExecuteAsync(UpdateCharacterXpModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var character = await characterRepository.GetCharacterForEdit(model.Id);

        character.AssignedXp = model.Xp;

        await characterRepository.UpdateCharacter(character);

        return Result.Ok();
    }
}
