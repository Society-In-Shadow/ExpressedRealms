using ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup;
using ExpressedRealms.Shared;

namespace ExpressedRealms.Events.API.Repositories.EventQuestions;

public interface IEventQuestionRepository : IGenericRepository
{
    Task<int> CreateAsync(EventQuestion eventQuestion);
    Task<bool> IsDuplicateEventQuestionQuestion(int eventId, string question);
    Task<bool> IsDuplicateEventQuestionQuestion(int eventId, int eventQuestionId, string question);
    Task<bool> IsExistingCustomizableQuestionType(int questionTypeId);
    Task<EventQuestion> GetEventQuestionForEdit(int eventId, int id);
    Task<bool> IsExistingEventQuestion(int eventId, int eventQuestionId);
    Task<List<EventQuestion>> GetEventQuestionsForEvent(int modelEventId);
}
