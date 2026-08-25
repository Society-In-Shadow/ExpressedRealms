using ExpressedRealms.Characters.Repository;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Characters.ToggleExtraMortisUseCase;

internal sealed class ToggleExtraMortisUseCase(
    ICharacterRepository repository,
    ToggleExtraMortisModelValidator validator,
    CancellationToken cancellationToken
) : IToggleExtraMortisUseCase
{
    public async Task<Result> ExecuteAsync(ToggleExtraMortisModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var character = await repository.FindCharacterAsync(model.Id);
        
        character!.ExtraMortis = model.HasExtraMortis;

        await repository.EditAsync(character);

        return Result.Ok();
    }
}
