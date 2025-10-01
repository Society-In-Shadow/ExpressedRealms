using ExpressedRealms.Expressions.Repository.StatModifier;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.GetModifierTypes;

internal sealed class GetModifierTypesUseCase(
    IStatModifierRepository repository,
    CancellationToken cancellationToken
) : IGetModifierTypesUseCase
{
    public async Task<Result<List<ModifierTypesReturnModel>>> ExecuteAsync()
    {
        var groupMapping = await repository.GetModifierTypes();

        return Result.Ok(
            groupMapping
                .Select(x => new ModifierTypesReturnModel() { Id = x.Id, Name = x.Name })
                .ToList()
        );
    }
}
