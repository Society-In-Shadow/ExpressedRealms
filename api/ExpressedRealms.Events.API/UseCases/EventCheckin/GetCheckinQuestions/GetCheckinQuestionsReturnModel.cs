namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetCheckinQuestions;

public class GetCheckinQuestionsReturnModel
{
    public List<QuestionResponse> Questions { get; set; } = new List<QuestionResponse>();
}
