namespace ExpressedRealms.Events.API.Repositories.EventQuestions.Dtos;

public class QuestionResponseDto
{
    public int QuestionId { get; set; }
    public required string PlayerName { get; set; }
    public required string Approver { get; set; }
    public DateTime ApprovalDate { get; set; }
    public required string Answer { get; set; }
    public required string Question { get; set; }
}