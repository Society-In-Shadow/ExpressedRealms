using ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup;
using ExpressedRealms.Events.API.Repositories.EventQuestions;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventQuestions.PopulateDefaults;

internal sealed class PopulateDefaultQuestionsUseCase(
    IEventQuestionRepository repository,
    PopulateDefaultQuestionsModelValidator validator,
    CancellationToken cancellationToken
) : IPopulateDefaultQuestionsUseCase
{
    public async Task<Result> ExecuteAsync(PopulateDefaultQuestionsModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var defaultQuestions = new List<EventQuestion>()
        {
            new()
            {
                EventId = model.EventId,
                Question = "What is your badge number / name on your badge?",
                QuestionTypeId = 2
            },
            new()
            {
                EventId = model.EventId,
                Question = "Are you under the age of 18?",
                QuestionTypeId = 1
            },
            new()
            {
                EventId = model.EventId,
                Question = "Have you brought in a new player? If so, what is their name?",
                QuestionTypeId = 6
            },
        };

        await repository.AddDefaultQuestionsToEvent(defaultQuestions);
        
        return Result.Ok();
    }
}
