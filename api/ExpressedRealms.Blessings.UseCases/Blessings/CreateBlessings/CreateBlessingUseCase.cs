using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.Blessings.CreateBlessings;

internal sealed class CreateBlessingUseCase(
    IBlessingRepository blessingRepository,
    CreateBlessingModelValidator validator,
    CancellationToken cancellationToken
) : ICreateBlessingUseCase
{
    public async Task<Result<int>> ExecuteAsync(CreateBlessingModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var returnedId = await blessingRepository.CreateBlessingAsync(
            new Blessing()
            {
                Name = model.Name,
                Description = model.Description,
                SubCategory = model.SubCategory,
                Type = model.Type,
            }
        );

        return Result.Ok(returnedId);
    }
}
