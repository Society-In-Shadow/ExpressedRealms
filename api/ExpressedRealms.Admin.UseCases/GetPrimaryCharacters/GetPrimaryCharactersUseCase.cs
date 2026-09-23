using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.Events.API.UseCases.EventCheckin.ApproveStageAndSendMessages;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.GetPrimaryCharacters;

internal sealed class GetPrimaryCharactersUseCase(ICharacterRepository characterRepository)
    : IGetPrimaryCharactersUseCase
{
    public async Task<Result<List<PrimaryCharacterReturnInfo>>> ExecuteAsync()
    {
        var primaryCharacters = await characterRepository.GetPrimaryCharactersAsync();

        return Result.Ok(
            primaryCharacters
                .Select(x => new PrimaryCharacterReturnInfo()
                {
                    Expression = x.Expression,
                    Id = x.Id,
                    Name = x.Name,
                    PlayerName = x.PlayerName,
                    PlayerNumber = x.PlayerNumber,
                    ActiveStages = GetActiveStatus(x.ActiveStages),
                    HasPromotionRequest = x.HasPromotionRequest,
                })
                .ToList()
        );
    }

    private static List<int> GetActiveStatus(List<int> completedStages)
    {
        var playerList = new List<int>();

        // This will always be the next step needed to be completed
        var earliestIncomplete = CheckinWorkflows.InitialCheckinSequence.FirstOrDefault(x =>
            !completedStages.Contains(x)
        );

        // Ignore Age Check Approval - list default behavior is to show as awaiting checkin
        if (earliestIncomplete is not null)
        {
            if (earliestIncomplete == CheckinStageEnum.AgeCheckApproval)
            {
                playerList.Add(CheckinStageEnum.AwaitingInitialCheckin);
            }
            else
            {
                playerList.Add(earliestIncomplete);
            }
        }

        var latestCompleted = CheckinWorkflows.InitialCheckinSequence.LastOrDefault(x =>
            completedStages.Contains(x)
        );

        if (latestCompleted is not null)
        {
            var latestIndex = CheckinWorkflows.InitialCheckinSequence.IndexOf(latestCompleted);
            var nextStep = CheckinWorkflows.InitialCheckinSequence[
                Math.Min(latestIndex + 1, CheckinWorkflows.InitialCheckinSequence.Count - 1)
            ];
            if (!playerList.Contains(nextStep))
                playerList.Add(nextStep);
        }

        return playerList;
    }
}
