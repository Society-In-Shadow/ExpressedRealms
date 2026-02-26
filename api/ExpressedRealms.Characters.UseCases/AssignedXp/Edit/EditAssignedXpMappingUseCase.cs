using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpMappingModels;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.AssignedXp.Edit;

internal sealed class EditAssignedXpMappingUseCase(
    IAssignedXpMappingRepository mappingRepository,
    EditAssignedXpMappingModelValidator validator,
    CancellationToken cancellationToken
) : IEditAssignedXpMappingUseCase
{
    public async Task<Result> ExecuteAsync(EditAssignedXpMappingModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var mapping = await mappingRepository.FindAsync<AssignedXpMapping>(model.Id);

        mapping!.Reason = model.Reason;
        mapping.AssignedXpTypeId = model.AssignedXpTypeId;
        mapping.Amount = model.Amount;
        mapping.EventId = model.EventId;

        await mappingRepository.EditAsync(mapping);

        return Result.Ok();
    }
}
