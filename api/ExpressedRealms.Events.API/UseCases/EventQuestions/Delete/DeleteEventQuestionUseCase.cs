using ExpressedRealms.DB.Interceptors;
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

        question.SoftDelete();

        await repository.EditAsync(question);

        return Result.Ok();
    }
}
