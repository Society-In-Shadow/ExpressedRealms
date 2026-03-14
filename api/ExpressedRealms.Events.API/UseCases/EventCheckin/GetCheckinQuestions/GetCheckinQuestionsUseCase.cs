using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.Repositories.EventQuestions;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetCheckinQuestions;

internal sealed class GetCheckinQuestionsUseCase(
    IEventCheckinRepository checkinRepository,
    IEventQuestionRepository questionRepository,
    GetCheckinQuestionsModelValidator validator,
    CancellationToken cancellationToken
) : IGetCheckinQuestionsUseCase
{
    public async Task<Result<GetCheckinQuestionsReturnModel>> ExecuteAsync(
        GetCheckinQuestionsModel model
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
        var checkin = await checkinRepository.GetCheckinAsync(eventId!.Value, playerId);

        var questions = await questionRepository.GetEventQuestionsForEvent(eventId!.Value);
        var answeredQuestions = await checkinRepository.GetAnsweredQuestions(checkin!.Id);

        return Result.Ok(
            new GetCheckinQuestionsReturnModel()
            {
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
            }
        );
    }
}
