using ExpressedRealms.Characters.Repository;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Characters.GetEditOptions;

internal sealed class GetEditOptionsUseCase(
    ICharacterRepository repository,
    GetEditOptionsModelValidator validator,
    CancellationToken cancellationToken
) : IGetEditOptionsUseCase
{
    public async Task<Result<EditCharacterOptionDto>> ExecuteAsync(GetEditOptionsModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        return Result.Ok(
            new EditCharacterOptionDto()
            {
                CanModifyPrimaryCharacter = await repository.CanUpdatePrimaryCharacterStatus(
                    model.Id
                ),
            }
        );
    }
}
