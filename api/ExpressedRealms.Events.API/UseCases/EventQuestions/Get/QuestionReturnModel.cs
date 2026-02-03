namespace ExpressedRealms.Events.API.UseCases.EventQuestions.Get;

public class QuestionReturnModel
{
    public int Id { get; set; }
    public required string Question { get; set; }
    public int QuestionTypeId { get; set; }
}
