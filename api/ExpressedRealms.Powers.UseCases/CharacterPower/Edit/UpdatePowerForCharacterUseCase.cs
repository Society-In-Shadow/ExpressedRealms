using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.Edit;

internal sealed class UpdatePowerForCharacterUseCase(
    ICharacterPowerRepository mappingRepository,
    UpdatePowerForCharacterModelValidator validator,
    CancellationToken cancellationToken
) : IUpdatePowerForCharacterUseCase
{
    public async Task<Result> ExecuteAsync(UpdatePowerForCharacterModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var mapping = await mappingRepository.GetCharacterPowerMapping(model.MappingId);

        mapping.Notes = model.Notes?.Trim() == string.Empty ? null : model.Notes?.Trim();

        await mappingRepository.UpdateCharacterPowerMapping(mapping);

        return Result.Ok();
    }
}
