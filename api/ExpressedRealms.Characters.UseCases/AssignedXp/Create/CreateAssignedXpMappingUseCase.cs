using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Characters.AssignedXp.AssignedXpMappingModels;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.AssignedXp.Create;

internal sealed class CreateAssignedXpMappingUseCase(
    IAssignedXpMappingRepository mappingRepository,
    CreateAssignedXpMappingModelValidator validator,
    IUserContext userContext,
    TimeProvider timeProvider,
    CancellationToken cancellationToken
) : ICreateAssignedXpMappingUseCase
{
    public async Task<Result<int>> ExecuteAsync(CreateAssignedXpMappingModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var id = await mappingRepository.AddAsync(
            new AssignedXpMapping()
            {
                AssignedByUserId = userContext.CurrentUserId(),
                EventId = model.EventId,
                AssignedXpTypeId = model.AssignedXpTypeId,
                CharacterId = model.CharacterId,
                PlayerId = model.PlayerId,
                Reason = model.Reason,
                Timestamp = timeProvider.GetUtcNow(),
            }
        );

        return Result.Ok(id);
    }
}
