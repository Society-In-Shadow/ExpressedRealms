using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpMappingModels;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.AssignedXp.Delete;

internal sealed class DeleteAssignedXpMappingUseCase(
    IAssignedXpMappingRepository mappingRepository,
    DeleteAssignedXpMappingModelValidator validator,
    CancellationToken cancellationToken
) : IDeleteAssignedXpMappingUseCase
{
    public async Task<Result> ExecuteAsync(DeleteAssignedXpMappingModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var mapping = await mappingRepository.FindAsync<AssignedXpMapping>(model.Id);

        mapping!.SoftDelete();

        await mappingRepository.EditAsync(mapping!);

        return Result.Ok();
    }
}
