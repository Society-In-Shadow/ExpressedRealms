using ExpressedRealms.Events.API.UseCases.EventCheckin.AnswerQuestion;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.AnswerQuestion;

public static class AnswerQuestionEndpoint
{
    public static async Task<Results<Ok, ValidationProblem, NotFound>> ExecuteAsync(
        string lookupId,
        int questionId,
        [FromBody] AnswerQuestionRequest request,
        [FromServices] IAnswerQuestionUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new AnswerQuestionModel()
            {
                LookupId = lookupId,
                Response = request.Response,
                QuestionId = questionId,
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
