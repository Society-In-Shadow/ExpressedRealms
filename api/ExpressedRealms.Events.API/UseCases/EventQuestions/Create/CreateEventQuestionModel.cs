namespace ExpressedRealms.Events.API.UseCases.EventQuestions.Create;

public class CreateEventQuestionModel
{
    public int EventId { get; set; }
    public required string Question { get; set; }
    public int QuestionTypeId { get; set; }
}
