namespace ExpressedRealms.Events.API.API.EventQuestions.Create;

public class CreateEventQuestionRequest
{
    public required string Question { get; set; }
    public int QuestionTypeId { get; set; }
}
