namespace ExpressedRealms.Events.API.API.EventQuestions.GetAllAnswers;

public class EventQuestionResponse
{
    public int QuestionId { get; set; }
    public required string PlayerName { get; set; }
    public required string Approver { get; set; }
    public DateTime ApprovalDate { get; set; }
    public required string Answer { get; set; }
    public required string Question { get; set; }
}
