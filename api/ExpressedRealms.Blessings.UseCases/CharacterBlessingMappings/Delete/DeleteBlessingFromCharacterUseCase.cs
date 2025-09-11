using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Edit;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Delete;

internal sealed class DeleteBlessingFromCharacterUseCase(
    ICharacterBlessingRepository mappingRepository,
    UpdateBlessingForCharacterModelValidator validator,
    CancellationToken cancellationToken
) : IDeleteBlessingFromCharacterUseCase
{
    public async Task<Result> ExecuteAsync(UpdateBlessingForCharacterModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var mapping = await mappingRepository.GetCharacterBlessingMappingForEditing(
            model.MappingId
        );

        mapping.SoftDelete();

        await mappingRepository.UpdateMapping(mapping);

        return Result.Ok();
    }
}
