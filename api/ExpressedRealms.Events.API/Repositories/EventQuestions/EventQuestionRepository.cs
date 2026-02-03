using ExpressedRealms.DB;
using ExpressedRealms.DB.Helpers;
using ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Events.API.Repositories.EventQuestions;

internal sealed class EventQuestionRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IEventQuestionRepository
{
    public async Task<int> CreateAsync(EventQuestion eventQuestion)
    {
        context.EventQuestions.Add(eventQuestion);
        await context.SaveChangesAsync(cancellationToken);
        return eventQuestion.Id;
    }

    public async Task<bool> IsExistingEventQuestion(int eventId, string question)
    {
        return await context.EventQuestions.AnyAsync(x =>
            x.EventId == eventId && x.Question.ToLower() == question.ToLower()
        );
    }

    public async Task<bool> IsExistingCustomizableQuestionType(int questionTypeId)
    {
        return await context.QuestionTypes.AnyAsync(x =>
            x.Id == questionTypeId && x.IsCustomizable
        );
    }

    public async Task EditAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await context.CommonSaveChanges(entity, cancellationToken);
    }
}
