using ExpressedRealms.Events.API.UseCases.EventQuestions.Edit;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventQuestions.Edit;

public static class EditEventQuestionEndpoint
{
    public static async Task<Results<Ok, ValidationProblem, NotFound>> ExecuteAsync(
        int eventId,
        int questionId,
        [FromBody] EditEventQuestionRequest request,
        [FromServices] IEditEventQuestionUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new EditEventQuestionModel()
            {
                EventId = eventId,
                Question = request.Question,
                Id = questionId,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;

        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
