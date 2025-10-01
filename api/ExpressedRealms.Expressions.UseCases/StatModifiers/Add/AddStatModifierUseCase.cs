using ExpressedRealms.DB.Models.ModifierSystem.StatGroupMappings;
using ExpressedRealms.Expressions.Repository.StatModifier;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.Add;

internal sealed class AddStatModifierUseCase(
    IStatModifierRepository repository,
    AddStatModifierModelValidator validator,
    CancellationToken cancellationToken
) : IAddStatModifierUseCase
{
    public async Task<Result> ExecuteAsync(AddStatModifierModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        await repository.AddStatGroupMapping(
            new StatGroupMapping()
            {
                StatGroupId = groupId,
                Modifier = model.Modifier,
                ScaleWithLevel = model.ScaleWithLevel,
                CreationSpecificBonus = model.CreationSpecificBonus,
                StatModifierId = model.StatModifierId,
            }
        );

        return Result.Ok();
    }
}
