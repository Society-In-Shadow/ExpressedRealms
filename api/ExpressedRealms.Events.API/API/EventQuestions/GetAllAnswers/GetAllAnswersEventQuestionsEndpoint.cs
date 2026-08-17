using ExpressedRealms.Events.API.UseCases.EventQuestions.GetQuestionResponses;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventQuestions.GetAllAnswers;

public static class GetEventQuestionResponsesEndpoint
{
    public static async Task<
        Results<Ok<GetEventQuestionResponsesResponse>, ValidationProblem, NotFound>
    > ExecuteAsync(int id, [FromServices] IGetEventQuestionResponsesUseCase useCase)
    {
        var results = await useCase.ExecuteAsync(new () { EventId = id });

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;

        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new GetEventQuestionResponsesResponse()
            {
                Responses = results
                    .Value.Select(x => new EventQuestionResponse()
                    {
                        QuestionId = x.QuestionId,
                        PlayerName = x.PlayerName,
                        Approver = x.Approver,
                        ApprovalDate = x.ApprovalDate,
                        Question = x.Question,
                        Answer = x.Answer
                    })
                    .ToList(),
            }
        );
    }
}
