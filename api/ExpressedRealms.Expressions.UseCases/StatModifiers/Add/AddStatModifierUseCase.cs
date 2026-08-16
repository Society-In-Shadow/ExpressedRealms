using System.ComponentModel;
using ExpressedRealms.DB.Models.ModifierSystem.StatGroupMappings;
using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.StatModifier;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.Add;

internal sealed class AddStatModifierUseCase(
    IStatModifierRepository repository,
    IExpressionRepository expressionRepository,
    StatModifierPermissionChecks permissionChecks,
    AddStatModifierModelValidator validator,
    CancellationToken cancellationToken
) : IAddStatModifierUseCase
{
    public async Task<Result<ReturnIds>> ExecuteAsync(AddStatModifierModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        if (permissionChecks.HasPermissionPolicyForStatModifiers(model.SourceTable, out var fail))
            return fail;

        if (model.TargetExpressionId is not null)
        {
            var expressionOptions = await expressionRepository.GetAllEnabledExpressionAndSubpaths();
            var expression = expressionOptions.FirstOrDefault(x => x.Id == model.TargetExpressionId);
            
            if (expression is null)
                return ValidationHelper.AddSingleValidationFailure(nameof(model.TargetExpressionId),
                    "Expression does not exist");

            if (model.TargetProgressionPathId is not null && expression.ProgressionPaths.All(x => x.Id != model.TargetProgressionPathId))
                return ValidationHelper.AddSingleValidationFailure(nameof(model.TargetProgressionPathId),
                    "This is not a valid progression path for the expression");
        }

        var groupId = model.StatModifierGroupId ?? 0;
        if (!model.StatModifierGroupId.HasValue)
        {
            // Create new group Id
            groupId = await repository.AddGroup();

            switch (model.SourceTable)
            {
                case SourceTableEnum.ProgressionLevels:
                    await repository.UpdateProgressionPathGroupId(model.SourceId, groupId);
                    break;
                case SourceTableEnum.Blessings:
                    await repository.UpdateBlessingGroupId(model.SourceId, groupId);
                    break;
                case SourceTableEnum.Powers:
                    await repository.UpdatePowerGroupId(model.SourceId, groupId);
                    break;
                case SourceTableEnum.Characters:
                    await repository.UpdateCharacterGroupId(model.SourceId, groupId);
                    break;
                default:
                    throw new InvalidEnumArgumentException(
                        nameof(model.SourceTable),
                        (int)model.SourceTable,
                        typeof(SourceTableEnum)
                    );
            }
        }

        var mappingId = await repository.AddStatGroupMapping(
            new StatGroupMapping()
            {
                StatGroupId = groupId,
                Modifier = model.Modifier,
                ScaleWithLevel = model.ScaleWithLevel,
                CreationSpecificBonus = model.CreationSpecificBonus,
                StatModifierId = model.StatModifierId,
                TargetExpressionId = model.TargetExpressionId,
                TargetProgressionPathId = model.TargetProgressionPathId
            }
        );

        return Result.Ok(new ReturnIds() { GroupId = groupId, ModifierMappingId = mappingId });
    }
}
