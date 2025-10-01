using ExpressedRealms.Expressions.Repository.StatModifier;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.GetModifiers;

internal sealed class GetModifiersUseCase(
    IStatModifierRepository repository,
    GetModifiersModelValidator validator,
    CancellationToken cancellationToken
) : IGetModifiersUseCase
{
    public async Task<Result<List<StatMappingReturnModel>>> ExecuteAsync(GetModifiersModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var groupMapping = await repository.GetGroupMappings(model.GroupId);

        return Result.Ok(
            groupMapping
                .Select(x => new StatMappingReturnModel()
                {
                    Modifier = x.Modifier,
                    ScaleWithLevel = x.ScaleWithLevel,
                    CreationSpecificBonus = x.CreationSpecificBonus,
                    Id = x.Id,
                    StatModifierId = x.StatModifierId,
                })
                .ToList()
        );
    }
}
