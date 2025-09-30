using ExpressedRealms.Expressions.Repository.StatModifier;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.Delete;

internal sealed class DeleteStatModifierUseCase(
    IStatModifierRepository repository,
    DeleteStatModifierModelValidator validator,
    CancellationToken cancellationToken
) : IDeleteStatModifierUseCase
{
    public async Task<Result> ExecuteAsync(DeleteStatModifierModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var groupMapping = await repository.GetGroupMappingForEditing(model.Id);
        await repository.HardDeleteGroupMapping(groupMapping);

        return Result.Ok();
    }
}
