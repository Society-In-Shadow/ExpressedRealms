using ExpressedRealms.Expressions.Repository.StatModifier;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.Edit;

internal sealed class EditStatModifierUseCase(
    IStatModifierRepository repository,
    EditStatModifierModelValidator validator,
    CancellationToken cancellationToken
) : IEditStatModifierUseCase
{
    public async Task<Result> ExecuteAsync(EditStatModifierModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var groupMapping = await repository.GetGroupMappingForEditing(model.Id);

        groupMapping.ScaleWithLevel = model.ScaleWithLevel;
        groupMapping.Modifier = model.Modifier;
        groupMapping.CreationSpecificBonus = model.CreationSpecificBonus;
        groupMapping.StatModifierId = model.StatModifierId;
        groupMapping.TargetExpressionId = model.TargetExpressionId;

        await repository.UpdateGroupMapping(groupMapping);

        return Result.Ok();
    }
}
