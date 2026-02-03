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

    public async Task<bool> IsDuplicateEventQuestionQuestion(int eventId, string question)
    {
        return await context.EventQuestions.AnyAsync(x =>
            x.EventId == eventId && x.Question.ToLower() == question.ToLower()
        );
    }

    public async Task<bool> IsDuplicateEventQuestionQuestion(
        int eventId,
        int eventQuestionId,
        string question
    )
    {
        return await context.EventQuestions.AnyAsync(x =>
            x.EventId == eventId
            && x.Id != eventQuestionId
            && x.Question.ToLower() == question.ToLower()
        );
    }

    public async Task<bool> IsExistingEventQuestion(int eventId, int eventQuestionId)
    {
        return await context.EventQuestions.AnyAsync(x =>
            x.EventId == eventId && x.Id == eventQuestionId
        );
    }

    public Task<List<EventQuestion>> GetEventQuestionsForEvent(int modelEventId)
    {
        return context.EventQuestions.Where(x => x.EventId == modelEventId).ToListAsync();
    }

    public async Task<bool> IsExistingCustomizableQuestionType(int questionTypeId)
    {
        return await context.QuestionTypes.AnyAsync(x =>
            x.Id == questionTypeId && x.IsCustomizable
        );
    }

    public async Task<EventQuestion> GetEventQuestionForEdit(int eventId, int id)
    {
        return await context.EventQuestions.FirstAsync(x => x.Id == id && x.EventId == eventId);
    }

    public async Task EditAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await context.CommonSaveChanges(entity, cancellationToken);
    }
}
