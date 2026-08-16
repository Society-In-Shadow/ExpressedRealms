using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.StatModifier;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.GetModifierTypes;

internal sealed class GetModifierTypesUseCase(
    IStatModifierRepository repository,
    StatModifierPermissionChecks permissionChecks,
    IExpressionRepository expressionRepository
) : IGetModifierTypesUseCase
{
    public async Task<Result<OptionsReturnModel>> ExecuteAsync(GetModifierTypesModel model)
    {
        if (permissionChecks.HasPermissionPolicyForStatModifiers(model.Source, out var fail))
            return fail;

        var groupMapping = await repository.GetModifierTypes();
        var expressions = await expressionRepository.GetAllEnabledExpressionAndSubpaths();

        return Result.Ok(
            new OptionsReturnModel()
            {
                ModifierTypes = groupMapping
                    .Select(x => new ModifierTypesReturnModel() { Id = x.Id, Name = x.Name })
                    .ToList(),
                Expressions = expressions
                    .Select(x => new ExpressionReturnModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ProgressionPaths = x
                            .ProgressionPaths.Select(y => new ProgressionPath()
                            {
                                Id = y.Id,
                                Name = y.Name,
                            })
                            .ToList(),
                    })
                    .ToList(),
            }
        );
    }
}
