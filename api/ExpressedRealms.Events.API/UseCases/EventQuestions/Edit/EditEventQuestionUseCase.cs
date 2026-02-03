using ExpressedRealms.Events.API.Repositories.EventQuestions;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventQuestions.Edit;

internal sealed class EditEventQuestionUseCase(
    IEventQuestionRepository repository,
    EditEventQuestionModelValidator validator,
    CancellationToken cancellationToken
) : IEditEventQuestionUseCase
{
    public async Task<Result> ExecuteAsync(EditEventQuestionModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var question = await repository.GetEventQuestionForEdit(model.EventId, model.Id);

        question.Question = model.Question;

        await repository.EditAsync(question);

        return Result.Ok();
    }
}
