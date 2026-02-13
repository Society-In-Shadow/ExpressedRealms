namespace ExpressedRealms.Events.API.UseCases.EventCheckin.AnswerQuestion;

public class AnswerQuestionModel
{
    public required string LookupId { get; set; }
    public int QuestionId { get; set; }
    public required string Response { get; set; }
}
