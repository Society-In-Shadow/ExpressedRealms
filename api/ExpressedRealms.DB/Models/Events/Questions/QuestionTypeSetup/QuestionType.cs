using ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup;

namespace ExpressedRealms.DB.Models.Events.Questions.QuestionTypeSetup;

public class QuestionType
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsDefault { get; set; }
    public bool IsCustomizable { get; set; }

    public virtual ICollection<EventQuestion> EventQuestions { get; set; } = new HashSet<EventQuestion>();
}
