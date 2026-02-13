namespace ExpressedRealms.Events.API.UseCases.EventCheckin.ConfirmedUserInfo;

public class QuestionResponse
{
    public int QuestionId { get; set; }
    public string? Response { get; set; }
    public required string Question { get; set; }
    public int QuestionTypeId { get; set; }
}
