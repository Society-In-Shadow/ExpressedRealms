using ExpressedRealms.Events.API.Repositories.EventQuestions;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventQuestions.Get;

internal sealed class GetEventQuestionsUseCase(
    IEventQuestionRepository repository,
    GetEventQuestionModelValidator validator,
    CancellationToken cancellationToken
) : IGetEventQuestionsUseCase
{
    public async Task<Result<List<QuestionReturnModel>>> ExecuteAsync(GetEventQuestionModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var question = await repository.GetEventQuestionsForEvent(model.EventId);

        return Result.Ok(question.Select(x => new QuestionReturnModel()
        {
            Id = x.Id,
            Question = x.Question,
            QuestionTypeId = x.QuestionTypeId
        }).ToList());
    }
}
