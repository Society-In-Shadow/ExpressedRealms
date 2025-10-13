using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.StatModifier;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.GetModifierTypes;

internal sealed class GetModifierTypesUseCase(
    IStatModifierRepository repository,
    IExpressionRepository expressionRepository
) : IGetModifierTypesUseCase
{
    public async Task<Result<OptionsReturnModel>> ExecuteAsync()
    {
        var groupMapping = await repository.GetModifierTypes();
        var expressions = await expressionRepository.GetAllEnabledExpressions();

        return Result.Ok(
            new OptionsReturnModel()
            {
                ModifierTypes = groupMapping
                    .Select(x => new ModifierTypesReturnModel() { Id = x.Id, Name = x.Name })
                    .ToList(),
                Expressions = expressions
                    .Select(x => new KeyValuePair<int, string>(x.Id, x.Name))
                    .ToList(),
            }
        );
    }
}
