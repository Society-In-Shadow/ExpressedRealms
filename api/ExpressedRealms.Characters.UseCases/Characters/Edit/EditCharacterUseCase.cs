using ExpressedRealms.Characters.Repository;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Characters.Edit;

internal sealed class EditCharacterUseCase(
    ICharacterRepository repository,
    EditCharacterModelValidator validator,
    CancellationToken cancellationToken
) : IEditCharacterUseCase
{
    public async Task<Result> ExecuteAsync(EditCharacterModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var character = await repository.FindCharacterAsync(model.Id);

        var validExpressionsForProgressions = new List<int> { 3, 4, 8, 9 }; // Adepts, Shammas, Sorcerer, Vampyres
        if (
            model.PrimaryProgressionId is not null
            && validExpressionsForProgressions.Contains(character!.ExpressionId)
        )
        {
            character.PrimaryProgressionId = model.PrimaryProgressionId;
        }

        const int SorcererId = 8;
        if (model.SecondaryProgressionId is not null && character!.ExpressionId == SorcererId)
        {
            character.SecondaryProgressionId = model.SecondaryProgressionId;
        }

        character!.Name = model.Name;
        character.Background = model.Background;
        character.FactionId = model.FactionId;
        character.IsPrimaryCharacter = model.IsPrimaryCharacter;

        await repository.EditAsync(character);

        return Result.Ok();
    }
}
