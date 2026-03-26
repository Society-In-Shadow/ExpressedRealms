using ExpressedRealms.Characters.Repository;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Characters.GetArchetypesForExpression;

internal sealed class GetArchetypesForExpressionUseCase(
    ICharacterRepository repository,
    GetArchetypesForExpressionModelValidator validator,
    CancellationToken cancellationToken
) : IGetArchetypesForExpressionUseCase
{
    public async Task<Result<GetArchetypesForExpressionDto>> ExecuteAsync(GetArchetypesForExpressionModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var archetypes = await repository.GetArchetypesForExpression(model.Id);
        return Result.Ok(
            new GetArchetypesForExpressionDto()
            {
                Archetypes = archetypes.Select(x => new ArchetypeCharacterInfoDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Background = x.Background,
                }).ToList(),
            }
        );
    }
}
