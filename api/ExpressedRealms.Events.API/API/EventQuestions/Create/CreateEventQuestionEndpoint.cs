using ExpressedRealms.Events.API.UseCases.EventQuestions.Create;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventQuestions.Create;

public static class CreateEventQuestionEndpoint
{
    public static async Task<Results<Ok<int>, ValidationProblem>> ExecuteAsync(
        int eventId,
        [FromBody] CreateEventQuestionRequest request,
        [FromServices] ICreateEventQuestionUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new CreateEventQuestionModel()
            {
                EventId = eventId,
                Question = request.Question,
                QuestionTypeId = request.QuestionTypeId,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(results.Value);
    }
}
