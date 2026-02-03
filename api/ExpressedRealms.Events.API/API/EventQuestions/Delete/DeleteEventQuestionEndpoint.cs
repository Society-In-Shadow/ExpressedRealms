using ExpressedRealms.Events.API.UseCases.EventQuestions.Delete;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventQuestions.Delete;

public static class DeleteEventQuestionEndpoint
{
    public static async Task<Results<Ok, ValidationProblem, NotFound>> ExecuteAsync(
        int eventId,
        int questionId,
        [FromServices] IDeleteEventQuestionUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new DeleteEventQuestionModel() { EventId = eventId, Id = questionId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;

        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
