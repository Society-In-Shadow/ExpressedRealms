namespace ExpressedRealms.Events.API.UseCases.EventQuestions.Edit;

public class EditEventQuestionModel
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public required string Question { get; set; }
}
