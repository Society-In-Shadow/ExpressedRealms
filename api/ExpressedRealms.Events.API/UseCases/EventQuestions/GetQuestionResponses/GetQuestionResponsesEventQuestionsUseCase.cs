using ExpressedRealms.Events.API.Repositories.EventQuestions;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventQuestions.GetQuestionResponses;

internal sealed class GetEventQuestionResponsesUseCase(
    IEventQuestionRepository repository,
    GetEventQuestionResponsesModelValidator validator,
    CancellationToken cancellationToken
) : IGetEventQuestionResponsesUseCase
{
    public async Task<Result<List<AnsweredQuestionReturnModel>>> ExecuteAsync(GetEventQuestionResponsesModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var question = await repository.GetAllQuestionResponsesForEvent(model.EventId);

        return Result.Ok(
            question
                .Select(x => new AnsweredQuestionReturnModel()
                {
                    QuestionId = x.QuestionId,
                    PlayerName = x.PlayerName,
                    Approver = x.Approver,
                    ApprovalDate = x.ApprovalDate,
                    Question = x.Question,
                    Answer = x.Answer
                })
                .ToList()
        );
    }
}
