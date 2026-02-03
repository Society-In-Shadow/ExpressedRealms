using ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup;
using ExpressedRealms.Events.API.Repositories.EventQuestions;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventQuestions.Create;

internal sealed class CreateEventQuestionUseCase(
    IEventQuestionRepository repository,
    CreateEventQuestionModelValidator validator,
    CancellationToken cancellationToken
) : ICreateEventQuestionUseCase
{
    public async Task<Result<int>> ExecuteAsync(CreateEventQuestionModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var eventId = await repository.CreateAsync(
            new EventQuestion()
            {
                EventId = model.EventId,
                Question = model.Question,
                QuestionTypeId = model.QuestionTypeId,
            }
        );

        return Result.Ok(eventId);
    }
}
