using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.Delete;

internal sealed class DeletePowerFromCharacterUseCase(
    ICharacterPowerRepository mappingRepository,
    DeletePowerFromCharacterModelValidator validator,
    CancellationToken cancellationToken
) : IDeletePowerFromCharacterUseCase
{
    public async Task<Result> ExecuteAsync(DeletePowerFromCharacterModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var mapping = await mappingRepository.GetCharacterPowerMapping(
            model.CharacterId,
            model.PowerId
        );

        mapping.SoftDelete();

        await mappingRepository.UpdateCharacterPowerMapping(mapping);

        return Result.Ok();
    }
}
