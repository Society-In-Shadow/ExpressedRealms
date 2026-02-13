using ExpressedRealms.DB.Models.Checkins.CheckinQuestionResponseSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.AnswerQuestion;

internal sealed class AnswerQuestionUseCase(
    IEventCheckinRepository checkinRepository,
    AnswerQuestionModelValidator validator,
    CancellationToken cancellationToken
) : IAnswerQuestionUseCase
{
    public async Task<Result> ExecuteAsync(AnswerQuestionModel model)
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
        var checkin = await checkinRepository.GetCheckinAsync(eventId!.Value, playerId);

        var questionResponse = await checkinRepository.GetCheckinQuestionResponseAsync(
            checkin!.Id,
            model.QuestionId
        );

        if (questionResponse is null)
        {
            await checkinRepository.AddCheckinQuestionResponseAsync(
                new CheckinQuestionResponse()
                {
                    CheckinId = checkin!.Id,
                    EventQuestionId = model.QuestionId,
                    Response = model.Response,
                }
            );

            return Result.Ok();
        }

        questionResponse.Response = model.Response;
        await checkinRepository.EditAsync(questionResponse);

        return Result.Ok();
    }
}
