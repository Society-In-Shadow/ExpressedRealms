namespace ExpressedRealms.Events.API.API.EventCheckin.ConfirmUserCheckinInfo;

public class QuestionResponse
{
    public int Id { get; set; }
    public string? Response { get; set; }
    public required string Question { get; set; }
    public int TypeId { get; set; }
}
