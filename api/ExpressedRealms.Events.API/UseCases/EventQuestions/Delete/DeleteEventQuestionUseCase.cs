using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Events.Questions.QuestionTypeSetup;
using ExpressedRealms.Events.API.Repositories.EventQuestions;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventQuestions.Delete;

internal sealed class DeleteEventQuestionUseCase(
    IEventQuestionRepository repository,
    DeleteEventQuestionModelValidator validator,
    CancellationToken cancellationToken
) : IDeleteEventQuestionUseCase
{
    public async Task<Result> ExecuteAsync(DeleteEventQuestionModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var question = await repository.GetEventQuestionForEdit(model.EventId, model.Id);

        if (question.QuestionTypeId == QuestionTypeEnum.IsMinorCheck)
        {
            return Result.Fail("Cannot delete the minor check question");
        }

        if (question.QuestionTypeId == QuestionTypeEnum.BroughtNewPlayer)
        {
            return Result.Fail("Cannot delete the new player question");
        }
        
        question.SoftDelete();

        await repository.EditAsync(question);

        return Result.Ok();
    }
}
