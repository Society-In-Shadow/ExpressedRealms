using ExpressedRealms.Characters.Repository;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Characters.EditCharacterGoFields;

internal sealed class EditCharacterGoFieldsUseCase(
    ICharacterRepository repository,
    EditCharacterGoFieldsModelValidator validator,
    CancellationToken cancellationToken
) : IEditCharacterGoFieldsUseCase
{
    public async Task<Result> ExecuteAsync(EditCharacterGoFieldsModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var character = await repository.FindCharacterAsync(model.Id);

        if (model.PrimaFragments >= 5)
        {
            model.Motes += Math.DivRem(model.PrimaFragments, 5, out var remainder);
            model.PrimaFragments -= remainder;
        }
        
        if (model.VoidFragments >= 5)
        {
            model.Motes += Math.DivRem(model.PrimaFragments, 5, out var remainder);
            model.VoidFragments -= remainder;
        }
        
        character!.PrimaFragments = model.PrimaFragments;
        character.WealthLevel = model.WealthLevel;
        character.VoidFragments = model.VoidFragments;
        character.Motes = model.Motes;

        await repository.EditAsync(character);

        return Result.Ok();
    }
}
