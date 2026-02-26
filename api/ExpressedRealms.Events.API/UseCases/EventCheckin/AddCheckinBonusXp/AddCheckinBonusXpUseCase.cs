using ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpMappingModels;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.AddCheckinBonusXp;

internal sealed class AddCheckinBonusXpUseCase(
    IEventCheckinRepository checkinRepository,
    IUserContext userContext,
    TimeProvider timeProvider,
    AddCheckinBonusXpModelValidator validator,
    CancellationToken cancellationToken
) : IAddCheckinBonusXpUseCase
{
    public async Task<Result> ExecuteAsync(AddCheckinBonusXpModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var eventId = await checkinRepository.GetActiveEventId();
        var playerId = await checkinRepository.GetPlayerId(model.LookupId);
        var primaryCharacter = await checkinRepository.GetPrimaryCharacterInformation(playerId);

        if (eventId is null)
            return Result.Fail("There are no active events to assign xp to");

        // Need to add a check to make sure to only add one of these three types to the event
        if (await checkinRepository.HasPreAssignedXpTypes(eventId.Value, playerId))
            return Result.Fail("Player already has a preassigned xp type");

        // Force full xp for First Time player and brought new player
        if (model.AssignedXpTypeId == 4 || model.AssignedXpTypeId == 5)
            model.Amount = 5;

        await checkinRepository.AddAssignedXpAsync(
            new AssignedXpMapping()
            {
                AssignedByUserId = userContext.CurrentUserId(),
                EventId = eventId.Value,
                AssignedXpTypeId = model.AssignedXpTypeId,
                CharacterId = primaryCharacter?.CharacterId,
                PlayerId = playerId,
                Amount = model.Amount,
                Timestamp = timeProvider.GetUtcNow(),
            }
        );

        return Result.Ok();
    }
}
