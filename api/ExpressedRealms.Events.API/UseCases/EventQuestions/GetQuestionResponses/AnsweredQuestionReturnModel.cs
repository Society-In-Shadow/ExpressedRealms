namespace ExpressedRealms.Events.API.UseCases.EventQuestions.GetQuestionResponses;

public class AnsweredQuestionReturnModel
{
    public int QuestionId { get; set; }
    public required string PlayerName { get; set; }
    public required string Approver { get; set; }
    public DateTime ApprovalDate { get; set; }
    public required string Answer { get; set; }
    public required string Question { get; set; }
}
