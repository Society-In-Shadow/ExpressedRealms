using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Expressions.ExpressionSubTypeSetup;
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
        var subExpressionTypeId = await repository.GetExpressionSubTypeId(character!.ExpressionId);

        var validExpressionsForProgressions = new List<int>
        {
            ExpressionSubTypeEnum.Adepts,
            ExpressionSubTypeEnum.Shammas,
            ExpressionSubTypeEnum.Sorcerers,
            ExpressionSubTypeEnum.Vampyre,
        };
        if (
            model.PrimaryProgressionId is not null
            && validExpressionsForProgressions.Contains(subExpressionTypeId)
        )
        {
            character.PrimaryProgressionId = model.PrimaryProgressionId;
        }

        if (
            model.SecondaryProgressionId is not null
            && subExpressionTypeId == ExpressionSubTypeEnum.Sorcerers
        )
        {
            character.SecondaryProgressionId = model.SecondaryProgressionId;
        }

        character!.Name = model.Name;
        character.Background = model.Background;
        character.IsPrimaryCharacter = model.IsPrimaryCharacter;

        await repository.EditAsync(character);

        return Result.Ok();
    }
}
