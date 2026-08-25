using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetCharacterStorageOptins;

internal sealed class GetCharacterStorageOptinsUseCase(
    IEventCheckinRepository checkinRepository,
    GetCharacterStorageOptinsModelValidator validator,
    CancellationToken cancellationToken
) : IGetCharacterStorageOptinsUseCase
{
    public async Task<Result<GetCharacterStorageOptinsReturnModel>> ExecuteAsync(
        GetCharacterStorageOptinsModel model
    )
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var activeEvent = await checkinRepository.GetActiveEventInfoOrDefaultAsync();

        var characterStorageOptins = await checkinRepository.GetCharacterStorageUsersForEvent(activeEvent!.Id);


        return Result.Ok(
            new GetCharacterStorageOptinsReturnModel()
            {
                CharacterStorageOptins = characterStorageOptins
                    .Select(x => new CharacterStorageOptin()
                    {
                        Id = x.Id,
                        Timestamp = x.Timestamp,
                        ApproverName = x.ApproverName,
                        PlayerName = x.PlayerName,
                        Amount = x.Amount
                    })
                    .ToList(),
            }
        );
    }
}
