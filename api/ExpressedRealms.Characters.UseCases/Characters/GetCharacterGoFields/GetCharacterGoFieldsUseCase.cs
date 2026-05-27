using ExpressedRealms.Characters.Repository;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Characters.GetCharacterGoFields;

internal sealed class GetCharacterGoFieldsUseCase(
    ICharacterRepository repository,
    GetCharacterGoFieldsModelValidator validator,
    CancellationToken cancellationToken
) : IGetCharacterGoFieldsUseCase
{
    public async Task<Result<GetCharacterGoFieldReturnModel>> ExecuteAsync(GetCharacterGoFieldsModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var character = await repository.FindCharacterAsync(model.Id);

        return Result.Ok(new GetCharacterGoFieldReturnModel()
        {
            WealthLevel = character!.WealthLevel,
            VoidFragments = character.VoidFragments,
            Motes = character.Motes,
            PrimaFragments = character.PrimaFragments
        });
    }
}
