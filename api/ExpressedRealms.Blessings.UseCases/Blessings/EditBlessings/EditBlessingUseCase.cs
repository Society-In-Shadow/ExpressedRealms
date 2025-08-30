using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.Blessings.EditBlessings;

internal sealed class EditBlessingUseCase(
    IBlessingRepository blessingRepository,
    EditBlessingModelValidator validator,
    CancellationToken cancellationToken
) : IEditBlessingUseCase
{
    public async Task<Result> ExecuteAsync(EditBlessingModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var blessing = await blessingRepository.GetBlessing(model.Id);
        
        blessing.Name = model.Name;
        blessing.Description = model.Description;
        blessing.SubCategory = model.SubCategory;
        blessing.Type = model.Type;

        await blessingRepository.EditBlessingAsync(blessing);

        return Result.Ok();
    }
}
