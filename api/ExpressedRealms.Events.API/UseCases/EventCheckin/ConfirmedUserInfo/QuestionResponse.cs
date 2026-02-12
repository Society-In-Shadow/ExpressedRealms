namespace ExpressedRealms.Events.API.UseCases.EventCheckin.ConfirmedUserInfo;

public class QuestionResponse
{
    public int QuestionId { get; set; }
    public required string Response { get; set; }
}