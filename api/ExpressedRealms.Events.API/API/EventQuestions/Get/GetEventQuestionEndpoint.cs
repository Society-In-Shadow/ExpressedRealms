using ExpressedRealms.Events.API.UseCases.EventQuestions.Get;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventQuestions.Get;

public static class GetEventQuestionEndpoint
{
    public static async Task<Results<Ok<GetEventQuestionResponse>, ValidationProblem, NotFound>> ExecuteAsync(
        int eventId,
        [FromServices] IGetEventQuestionUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new GetEventQuestionModel()
            {
                EventId = eventId
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;

        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(new GetEventQuestionResponse()
        {
            Questions = results.Value.Select(x => new EventQuestion()
            {
                Id = x.Id,
                QuestionTypeId = x.QuestionTypeId,
                Question = x.Question
            }).ToList()
        });
    }
}
