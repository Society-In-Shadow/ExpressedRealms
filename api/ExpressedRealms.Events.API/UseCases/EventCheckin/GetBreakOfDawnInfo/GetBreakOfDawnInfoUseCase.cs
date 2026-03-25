using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetBreakOfDawnInfo;

internal sealed class GetBreakOfDawnInfoUseCase(
    IEventCheckinRepository checkinRepository,
    GetBreakOfDawnInfoModelValidator validator,
    CancellationToken cancellationToken
) : IGetBreakOfDawnInfoUseCase
{
    public async Task<Result<GetBreakOfDawnInfoDto>> ExecuteAsync(GetBreakOfDawnInfoModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var player = await checkinRepository.GetPlayerId(model.LookupId);
        var currentEventId = await checkinRepository.GetActiveEventId();
        var checkin = await checkinRepository.GetCheckinAsync(currentEventId!.Value, player);

        var proficiencies = await checkinRepository.GetSecondaryProficiencies(checkin!.Id);

        return Result.Ok(
            new GetBreakOfDawnInfoDto()
            {
                Blood = proficiencies!.Blood,
                Rwp = proficiencies.Rwp,
                Mortis = proficiencies.Mortis,
                Health = proficiencies.Health,
                Vitality = proficiencies.Vitality,
                Psyche = proficiencies.Psyche,
                CharacterLevel = proficiencies.PlayerLevel,
                ExpressionId = proficiencies.ExpressionId,
            }
        );
    }
}
