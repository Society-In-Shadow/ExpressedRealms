using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.Repositories.EventQuestions;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.ConfirmedUserInfo;

internal sealed class ConfirmedUserInfoUseCase(
    IEventCheckinRepository checkinRepository,
    IEventQuestionRepository questionRepository,
    ConfirmedUserInfoModelValidator validator,
    CancellationToken cancellationToken
) : IConfirmedUserInfoUseCase
{
    public async Task<Result<ConfirmedUserInfoReturnModel>> ExecuteAsync(
        ConfirmedUserInfoModel model
    )
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
        var checkinId = await GetCheckinId(eventId, playerId);

        var playerName = await checkinRepository.GetUserName(model.LookupId);
        var isFirstTimePlayer = await checkinRepository.IsFirstTimePlayer(model.LookupId);
        var playerNumber = checkinRepository.GetPlayerNumber(model.LookupId);

        var questions = await questionRepository.GetEventQuestionsForEvent(eventId!.Value);
        var answeredQuestions = await checkinRepository.GetAnsweredQuestions(checkinId);
        var primaryCharacterInformation = await checkinRepository.GetPrimaryCharacterInformation(
            playerId
        );
        var assignedXp = await checkinRepository.GetAssignedXp(playerId, eventId!.Value);

        PrimaryCharacterInfo? characterInfo = null;
        if (primaryCharacterInformation is not null)
        {
            characterInfo = new PrimaryCharacterInfo()
            {
                CharacterId = primaryCharacterInformation.CharacterId,
                CharacterName = primaryCharacterInformation.CharacterName,
            };
        }

        var currentStage = await checkinRepository.GetCurrentStage(checkinId);

        return Result.Ok(
            new ConfirmedUserInfoReturnModel()
            {
                PlayerName = playerName,
                IsFirstTimeUser = isFirstTimePlayer,
                PlayerNumber = playerNumber,
                CheckinId = checkinId,
                Questions = questions
                    .Select(x => new QuestionResponse()
                    {
                        QuestionId = x.Id,
                        Question = x.Question,
                        QuestionTypeId = x.QuestionTypeId,
                        Response = answeredQuestions
                            .FirstOrDefault(y => y.EventQuestionId == x.Id)
                            ?.Response,
                    })
                    .ToList(),
                PrimaryCharacterInfo = characterInfo,
                AssignedXp = assignedXp is null
                    ? null
                    : new AssignedXpType()
                    {
                        TypeId = assignedXp.TypeId,
                        Amount = assignedXp.Amount,
                    },
                CurrentStage = currentStage,
            }
        );
    }

    private async Task<int> GetCheckinId(int? eventId, Guid playerId)
    {
        int? checkinId = null;

        var checkin = await checkinRepository.GetCheckinAsync(eventId!.Value, playerId);
        checkinId = checkin?.Id;
        if (checkin is null)
        {
            checkinId = await checkinRepository.CreateCheckinAsync(
                new Checkin() { PlayerId = playerId, EventId = eventId!.Value }
            );
        }

        return checkinId!.Value;
    }
}
