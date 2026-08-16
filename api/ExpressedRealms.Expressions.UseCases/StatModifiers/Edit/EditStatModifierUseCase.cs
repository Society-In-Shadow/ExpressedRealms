using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.StatModifier;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.Edit;

internal sealed class EditStatModifierUseCase(
    IStatModifierRepository repository,
    IExpressionRepository expressionRepository,
    StatModifierPermissionChecks permissionChecks,
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

        if (model.TargetExpressionId is not null)
        {
            var expressionOptions = await expressionRepository.GetAllEnabledExpressionAndSubpaths();
            var expression = expressionOptions.FirstOrDefault(x =>
                x.Id == model.TargetExpressionId
            );

            if (expression is null)
                return ValidationHelper.AddSingleValidationFailure(
                    nameof(model.TargetExpressionId),
                    "Expression does not exist"
                );

            if (
                model.TargetProgressionPathId is not null
                && expression.ProgressionPaths.All(x => x.Id != model.TargetProgressionPathId)
            )
                return ValidationHelper.AddSingleValidationFailure(
                    nameof(model.TargetProgressionPathId),
                    "This is not a valid progression path for the expression"
                );
        }

        if (permissionChecks.HasPermissionPolicyForStatModifiers(model.Source, out var fail))
            return fail;

        var groupMapping = await repository.GetGroupMappingForEditing(model.Id);

        groupMapping.ScaleWithLevel = model.ScaleWithLevel;
        groupMapping.Modifier = model.Modifier;
        groupMapping.CreationSpecificBonus = model.CreationSpecificBonus;
        groupMapping.StatModifierId = model.StatModifierId;
        groupMapping.TargetExpressionId = model.TargetExpressionId;
        groupMapping.TargetProgressionPathId = model.TargetProgressionPathId;

        await repository.UpdateGroupMapping(groupMapping);

        return Result.Ok();
    }
}
