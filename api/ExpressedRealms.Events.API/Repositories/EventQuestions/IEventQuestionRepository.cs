using ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup;
using ExpressedRealms.Shared;

namespace ExpressedRealms.Events.API.Repositories.EventQuestions;

public interface IEventQuestionRepository : IGenericRepository
{
    Task<int> CreateAsync(EventQuestion eventQuestion);
    Task<bool> IsExistingEventQuestion(int eventId, string question);
    Task<bool> IsExistingCustomizableQuestionType(int questionTypeId);
}
